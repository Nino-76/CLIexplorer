using System.Drawing;

string current_directory = Directory.GetCurrentDirectory();
string[] parts = [];

string ResolvePath(string item)
{
    if (Path.IsPathFullyQualified(item))
    {
        return item;
    }
    else
    {
        return Path.Combine(current_directory, item);
    }
}

while (true)
{
    Console.Write($"{current_directory} > ");
    string user_input = Console.ReadLine()!;

    if (user_input == "list")
    {
        string[] files = Directory.GetFiles(current_directory);
        
        foreach (string item in files)
        {
            long fileSize = new FileInfo(item).Length;
            Console.WriteLine($"{item} | {fileSize / 1000} KB");
        }
        string[] folders = Directory.GetDirectories(current_directory);
        foreach (string folder in folders)
        {
            Console.WriteLine(folder);
        }

        if (files.Length == 0 && folders.Length == 0)
        {
            Console.WriteLine("Current directory doesn't contain any item to display.");
        }
    }

    else if (user_input.StartsWith("go "))
    {
        parts = user_input.Split(" ", 2);
        if (parts.Length < 2 || parts[1] == "")
        {
            Console.WriteLine("Error : missing arguments in the command.");
            continue;
        }
        if (parts[1].StartsWith(".."))
        {
            var directory_parent = Directory.GetParent(current_directory);
            current_directory = directory_parent.FullName;
        }
        else
        {
            string target = ResolvePath(parts[1]);
            if (Directory.Exists(target))
            {
                current_directory = target;
            }
        }
    }

    else if (user_input.StartsWith("delete "))
    {
        parts = user_input.Split(" ", 2);
        if (parts.Length < 2 || parts[1] == "")
        {
            Console.WriteLine("Error : missing arguments in the command.");
            continue;
        }
        string path = ResolvePath(parts[1]);
        if (File.Exists(path))
        {
            Console.Write("Are you sure ? (y/n) > ");
            user_input = Console.ReadLine()!;
            if (user_input == "y")
            {
                File.Delete(path);   
                Console.WriteLine("Deleted file.");
            }   
        }
        else if (Directory.Exists(path))
        {
            Console.Write("Are you sure ? (y/n) > ");
            user_input = Console.ReadLine()!;
            if (user_input == "y")
            {
                Directory.Delete(path);
                Console.WriteLine("Deleted folder.");
            }
        }

    }

    else if (user_input.StartsWith("makefile "))
    {
        parts = user_input.Split(" ", 2);
        if (parts.Length < 2 || parts[1] == "")
        {
            Console.WriteLine("Error : missing arguments in the command.");
            continue;
        }
        string path = ResolvePath(parts[1]);
        File.Create(path).Dispose();
        Console.WriteLine("Created new file.");
    }

    else if (user_input.StartsWith("makedir "))
    {
        parts = user_input.Split(" ", 2);
        if (parts.Length < 2 || parts[1] == "")
        {
            Console.WriteLine("Error : missing arguments in the command.");
            continue;
        }
        string path = ResolvePath(parts[1]);
        Directory.CreateDirectory(path);
        Console.WriteLine("Created new folder.");
    }

    else if (user_input.StartsWith("copy "))
    {
        parts = user_input.Split(" ", 3);
        if (parts.Length < 3 || parts[1] == "" || parts[2] == "")
        {
            Console.WriteLine("Error : missing arguments in the command.");
            continue;
        }
        string path = ResolvePath(parts[1]);
        string target = ResolvePath(parts[2]);
        string final_target = Path.Combine(target, Path.GetFileName(path));
    
        if (File.Exists(final_target))
        {
            Console.WriteLine("Error : a file with the same name already exists in the target directory.");
        }
        else if (File.Exists(path) == false)
        {
            Console.WriteLine("Error : the file you are trying to copy does not exist.");
        }
        else if (Directory.Exists(target) == false)
        {
            if (File.Exists(target))
            {
                Console.WriteLine("Error : a file with the same name already exists in the target directory.");
            }
            else
            {
                File.Copy(path, target);
                Console.WriteLine("File copied.");   
            }
        }
        else
        {
            File.Copy(path, final_target);
            Console.WriteLine("File copied.");
        }
    }

    else if (user_input.StartsWith("move "))
    {
        parts = user_input.Split(" ", 3);
        if (parts.Length < 3 || parts[1] == "" || parts[2] == "")
        {
            Console.WriteLine("Error : missing arguments in the command.");
            continue;
        }
        string path = ResolvePath(parts[1]);
        string target = ResolvePath(parts[2]);
        string final_target = Path.Combine(target, Path.GetFileName(path));


        if (File.Exists(final_target))
        {
            Console.WriteLine("Error : a file with the same name already exists in the target directory.");
        }
        else if (File.Exists(path) == false)
        {
            if (Directory.Exists(path))
            {
                if (Directory.Exists(final_target))
                {
                    Console.WriteLine("Error : a folder with the same name already exists in the target directory.");
                }
                else if (Directory.Exists(target))
                {
                    Directory.Move(path, final_target);
                    Console.WriteLine("Folder moved.");
                }
                else
                {
                    Directory.Move(path, target);
                    Console.WriteLine("Folder moved.");
                }
            }
            else
            {
                Console.WriteLine("Error : the file you are trying to move does not exist.");   
            }
        }
        else if (Directory.Exists(target) == false)
        {
            if (File.Exists(target))
            {
                Console.WriteLine("Error : a file with the same name already exists in the target directory.");   
            }
            else if (File.Exists(path))
            {
                File.Move(path, target);
                Console.WriteLine("File moved.");
            }
        }
        else
        {
            File.Move(path, final_target);
            Console.WriteLine("File moved.");
        }
    }

    else if (user_input.StartsWith("rename "))
    {
        parts = user_input.Split(" ", 3);
        if (parts.Length < 3 || parts[1] == "" || parts[2] == "")
        {
            Console.WriteLine("Error : missing arguments in the command.");
            continue;
        }
        string path = ResolvePath(parts[1]);
        string target = ResolvePath(parts[2]);

        if (File.Exists(path))
        {
            if (File.Exists(target) || Directory.Exists(target))
            {
                Console.WriteLine("Error : an item with the same name already exists in the current directory.");
            }
            else
            {
                File.Move(path, target);
                Console.WriteLine("File renamed.");
            }
        }
        else if (Directory.Exists(path))
        {
            if (File.Exists(target) || Directory.Exists(target))
            {
                Console.WriteLine("Error : an item with the same name already exists in the current directory.");
            }
            else
            {
                Directory.Move(path, target);
                Console.WriteLine("Folder renamed.");   
            }
        }
    }

    else if (user_input == "help")
    {
        Console.WriteLine("""
        - list : Show the list of files and folders in the current directory
        - go : Change directory, use .. to go to the parent directory
        - delete : Delete a file or directory (delete item_name) (only works on empty directories)
        - makefile : Make a new empty file  (makefile new_file)
        - makedir : Create a new empty directory (makedir new_directory)
        - copy : copy a file (copy file_name)
        - move : move an item (move item_name target_directory)
        - rename : rename an item (rename old_name new_name)
        - exit : Exit the app
        - help : Show every command
        """);
    }

    else if (user_input == "exit")
    {
        break;
    }
    
    else
    {
        Console.WriteLine("Error : invalid command.");
    }
}
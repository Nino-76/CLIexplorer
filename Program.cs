string current_directory = Directory.GetCurrentDirectory();
string[] parts = [];
while (true)
{
    Console.Write($"{current_directory} > ");
    string user_input = Console.ReadLine();

    if (user_input == "list")
    {
        string[] files = Directory.GetFiles(current_directory);
        
        foreach (string item in files)
        {
            Console.WriteLine(item);
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
        if (parts[1].StartsWith(".."))
        {
            var directory_parent = Directory.GetParent(current_directory);
            current_directory = directory_parent.FullName;
        }
        else if (Directory.Exists(Path.Combine(current_directory, parts[1])))
        {
             current_directory = Path.Combine(current_directory, parts[1]);
        }
    }

    else if (user_input.StartsWith("delete "))
    {
        parts = user_input.Split(" ", 2);
        string path = Path.Combine(current_directory, parts[1]);
        if (File.Exists(path))
        {
            Console.Write("Are you sure ? (y/n) > ");
            user_input = Console.ReadLine();
            if (user_input == "y")
            {
                File.Delete(path);   
            }   
        }
        else if (Directory.Exists(path))
        {
            Console.Write("Are you sure ? (y/n) > ");
            user_input = Console.ReadLine();
            if (user_input == "y")
            {
                Directory.Delete(path);
            }
        }

    }

    else if (user_input.StartsWith("makefile "))
    {
        parts = user_input.Split(" ", 2);
        string path = Path.Combine(current_directory, parts[1]);
        File.Create(path).Dispose();
    }

    else if (user_input.StartsWith("makedir "))
    {
        parts = user_input.Split(" ", 2);
        string path = Path.Combine(current_directory, parts[1]);
        Directory.CreateDirectory(path);
    }

    else if (user_input == "help")
    {
        Console.WriteLine("""
        - list : Show the list of files and folders in the current directory
        - go : Change directory, use .. to go to the parent directory
        - delete : Delete a chosen file or directory (only works on empty directories)
        - makefile : Make a new empty file
        - makedir : Make a new empty directory
        - exit : Exit the app
        - help : Show this message
        """);
    }

    else if (user_input == "exit")
    {
        break;
    }
}
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
    }
    else if (user_input.StartsWith("go "))
    {
        parts = user_input.Split(" ", 2);
        if (parts[1] == "..")
        {
            var directory_parent = Directory.GetParent(current_directory);
            current_directory = directory_parent.FullName;
        }
        else if (Directory.Exists(Path.Combine(current_directory, parts[1])))
        {
             current_directory = Path.Combine(current_directory, parts[1]);
        }
    }

    else if (user_input == "exit")
    {
        break;
    }
}
string path = args.Length > 0
        ? args[0]
        : Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;

Console.WriteLine($"[WATCH]: {path}");
CreateFileWatcher(path);
Thread.Sleep(-1);


static void CreateFileWatcher(string path)
{
    FileSystemWatcher watcher = new FileSystemWatcher
    {
        Path = path,
        NotifyFilter = NotifyFilters.LastAccess
                    | NotifyFilters.LastWrite
                    | NotifyFilters.FileName
                    | NotifyFilters.DirectoryName
                    | NotifyFilters.Attributes
                    | NotifyFilters.Security,
        IncludeSubdirectories = true,
    };

    // Add event handlers.
    watcher.Changed += new FileSystemEventHandler(OnChanged);
    watcher.Created += new FileSystemEventHandler(OnChanged);
    watcher.Deleted += new FileSystemEventHandler(OnChanged);
    watcher.Renamed += new RenamedEventHandler(OnRenamed);

    // Begin watching.
    watcher.EnableRaisingEvents = true;
}

static void OnChanged(object source, FileSystemEventArgs e)
{
    switch (e.ChangeType)
    {
        case WatcherChangeTypes.Created:
            Console.WriteLine($"[CREATED]: {e.FullPath}");
            break;
        case WatcherChangeTypes.Deleted:
            Console.WriteLine($"[DELETED]: {e.FullPath}");
            break;
        case WatcherChangeTypes.Changed:
            Console.WriteLine($"[CHANGED]: {e.FullPath}");
            break;
    }
}

static void OnRenamed(object source, RenamedEventArgs e)
{
    Console.WriteLine($"[RENAMED]: {e.OldFullPath} -> {e.FullPath}");
}

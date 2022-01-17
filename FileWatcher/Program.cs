using System;
using System.IO;
using System.Threading;

namespace FileWatcher
{
    class Program {
        static void Main(string[] agrs) {
            string path = agrs.Length > 0
                  ? agrs[0]
                  : Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            Console.WriteLine($"[WATCH]: {path}");
            CreateFileWatcher(path);
            Thread.Sleep(-1);
        }

        public static void CreateFileWatcher(string path) {
            FileSystemWatcher watcher = new FileSystemWatcher {
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

        private static void OnChanged(object source, FileSystemEventArgs e) {
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

        private static void OnRenamed(object source, RenamedEventArgs e) {
            Console.WriteLine($"[RENAMED]: {e.OldFullPath} -> {e.FullPath}");
        }
    }
}

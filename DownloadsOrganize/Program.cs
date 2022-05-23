using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace DownloadsOrganize
{
    internal class Program
    {
        private static string path;
        public static void Main(string[] args)
        {
            if(args[0])
            {
              if(Directory.Exists(args[0]))
              { path = args[0]; OrganizeFolder();

            }
            Console.Write("Paste path to folder you want to organize: ");
            path = Console.ReadLine() + '\\' ;
            Console.WriteLine("\nOrganizing folder...");
            OrganizeFolder();
        }

        private static void OrganizeFolder()
        {
            List<string> extensions = new List<string>();
            List<string> preFiles = new List<string>();
            foreach (var file in Directory.GetFiles(path))
            {
                preFiles.Add(file);
                var ext = Path.GetExtension(file);
                if (extensions.Contains(ext) || !Path.HasExtension(file)) continue;
                extensions.Add(ext);
            }
            foreach (var ext in extensions)
            {
                if (Directory.Exists($@"{path}/{ext.Substring(1)}")) continue;
                
                Directory.CreateDirectory($@"{path}/{ext.Substring(1)}");
                Directory.CreateDirectory($@"{path}/misc");
            }
            foreach (var file in preFiles)
            {
                if(!Path.HasExtension(file)){ File.Move(file, $@"{path}/misc");
                    continue;
                }
                if ( File.Exists($@"{path}/{Path.GetExtension(file).Substring(1)}")) continue; 
                
                Console.WriteLine($@"{path}/{Path.GetExtension(file).Substring(1)}/{Path.GetFileName(file)}");
                
                File.Move(file, $@"{path}/{Path.GetExtension(file).Substring(1)}/{Path.GetFileName(file)}");
                File.Delete(file);
            }
            Console.WriteLine("Press anykey to continue, or enter redo to go again.");
            switch (Console.ReadLine()?.ToLower())
            {
                case "redo": Main(null);
                    break;
                default: System.Environment.Exit(0); break;
            }
        }
    }
}

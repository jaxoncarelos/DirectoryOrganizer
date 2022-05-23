using System;
using System.Collections.Generic;
using System.IO;

namespace DownloadsOrganize
{
    internal class Program
    {
        private static string path;
        public static void Main(string[] args)
        {
            Console.Write("Paste path to folder you want to organize: ");
            path = Console.ReadLine();
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
                if (extensions.Contains(ext)) continue;
                extensions.Add(ext);
            }
            foreach (var ext in extensions)
            {
                if (Directory.Exists($@"{path}/{ext.Substring(1)}")) continue;
                
                Directory.CreateDirectory($@"{path}/{ext.Substring(1)}");
            }
            foreach (var file in preFiles)
            {
                if (File.Exists($@"{path}/{Path.GetExtension(file).Substring(1)}")) continue; 
                
                Console.WriteLine($@"{path}/{Path.GetExtension(file).Substring(1)}/{Path.GetFileName(file)}");
                
                File.Move(file, $@"{path}/{Path.GetExtension(file).Substring(1)}/{Path.GetFileName(file)}");
                File.Delete(file);
            }
        }
    }
}
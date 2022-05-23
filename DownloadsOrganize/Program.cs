using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using MimeMapping;

namespace DownloadsOrganize
{
    internal class Program
    {
        private static string path;
        public static void Main(string[] args)
        {
            if(args.Length > 0 && args[0] != null)
            {
              if(Directory.Exists(args[0]))
              { path = args[0]; OrganizeFolder(); }
            }
            Console.Write("Paste path to folder you want to organize: ");
            path = Console.ReadLine() + '\\' ;
            Console.WriteLine("1: Advanced Organization (File extensions)\n2: MIME Organization (File type)");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    OrganizeFolder();
                    break;
                case "2":
                    OrganizeMime();
                    break;
            }
        }

        private static void OrganizeMime()
        {
            List<string> MIMETypes = new List<string>();
            List<string> preFiles = new List<string>();
            foreach (var file in Directory.GetFiles(path))
            {
                preFiles.Add(file);
                var mime = MimeMapping.MimeUtility.GetMimeMapping(file);
                if (MIMETypes.Contains(mime)) continue;
                MIMETypes.Add(mime);
            }

            foreach (var type in MIMETypes)
            {
                var path1 = $@"{path}\{type.Split('/')[0]}";
                if (Directory.Exists(path1)) continue;
                
                Directory.CreateDirectory(path1);
                Directory.CreateDirectory($@"{path}/misc");
            }

            foreach (var file in preFiles)
            {
                
                var pathL = $@"{path}{MimeMapping.MimeUtility.GetMimeMapping(file).Split('/')[0]}";
                if ( File.Exists(pathL)) continue;
                Console.WriteLine($@"{pathL}\{Path.GetFileName(file)}");
                if (MimeUtility.GetMimeMapping(file) == MimeUtility.UnknownMimeType){ File.Move(file, $@"{path}/misc\{Path.GetFileName(file)}"); continue;} 
                
                
                File.Move(file, $@"{pathL}\{Path.GetFileName(file)}");
                File.Delete(file);
            }
            
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
                if (Directory.Exists($@"{path}\{ext.Substring(1)}")) continue;
                
                Directory.CreateDirectory($@"{path}\{ext.Substring(1)}");
                Directory.CreateDirectory($@"{path}/misc");
            }
            foreach (var file in preFiles)
            {
                if(!Path.HasExtension(file)){ File.Move(file, $@"{path}/misc");
                    continue;
                }
                if ( File.Exists($@"{path}\{Path.GetExtension(file).Substring(1)}")) continue; 
                
                Console.WriteLine($@"{path}\{Path.GetExtension(file).Substring(1)}\{Path.GetFileName(file)}");
                
                File.Move(file, $@"{path}\{Path.GetExtension(file).Substring(1)}\{Path.GetFileName(file)}");
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

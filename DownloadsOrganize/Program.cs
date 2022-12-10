using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                if(Directory.Exists(args[0])) path = args[0]; OrganizeFolder("1");
            }
            Console.Write("Paste path to folder you want to organize: ");
            path = Console.ReadLine() + '\\' ;
            
            Console.WriteLine("1: Advanced Organization (File extensions)\n2: MIME Organization (File type)");
            Console.Write("Choice: ");
            
            var option = Console.ReadLine();
            if (new []{"1", "2"}.Contains(option)) OrganizeFolder(option);

        }
        

        private static void OrganizeFolder(string option)
        {
            if( !Directory.Exists($@"{path}/misc")) Directory.CreateDirectory($@"{path}/misc");
            
            foreach (var file in Directory.GetFiles(path))
            {
                if (Path.GetExtension(file) == ".url") continue;
                if (!Path.HasExtension(file))
                {
                    File.Move(file, $@"{path}/misc/{Path.GetFileName(file)}");
                    continue;
                }
                var ext = (option == "1" ? Path.GetExtension(file).Substring(1) : MimeUtility.GetMimeMapping(file));
                var goalPath = $@"{path}{ext}";
                
                PathUtil.MoveAndDeleteFile(file, goalPath);
            }

            redoOption();
        }

        public static void redoOption()
        {
            Console.WriteLine("Press any key to continue, or enter redo to go again.");
            switch (Console.ReadLine()?.ToLower())
            {
                case "redo": Main(null);
                    break;
                default: 
                    Environment.Exit(0);
                    break;
            }
        }
    }
}

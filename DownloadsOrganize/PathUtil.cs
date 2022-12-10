using System;
using System.IO;

namespace DownloadsOrganize
{
    public class PathUtil
    {
        public static void MoveAndDeleteFile(string file, string goalPath)
        {
            if (!Directory.Exists(goalPath)) Directory.CreateDirectory(goalPath);
            
            Console.WriteLine($@"Moving {Path.GetFileName(file)} to {goalPath}");
            
            File.Move(file, Path.Combine(goalPath, Path.GetFileName(file)));
            File.Delete(file);
        }
    }
}
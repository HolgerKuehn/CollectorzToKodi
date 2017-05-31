/// <summary>
/// Rename all Files in given folder, by removing pattern
/// </summary>
namespace Rename
{
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                new Exception("Es werden zwei Parameter erwartet.");
                return;
            }

            string folder = args[0];
            string pattern = args[1];

            DirectoryInfo directory = new DirectoryInfo(folder);
            FileInfo[] files = directory.GetFiles("*.*");

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(pattern))
                { 
                    file.MoveTo(file.Directory.FullName + "\\" + file.Name.Replace(pattern, String.Empty));
                }
            }
        }
    }
}

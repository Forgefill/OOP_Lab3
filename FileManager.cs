using System;
using System.Collections.Generic;
using System.IO;
namespace OOP_Lab3
{
    class FileManager
    {
        public List<string> GetDrives()
        {
            List<string> result = new List<string>();

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo i in  drives)
            {
                result.Add(i.Name);
            }
            return result;
        }
        public List<string> GetFiles(string path, string filetype)
        {
            List<string> result = new List<string>();

            filetype = System.IO.Path.GetExtension(filetype);
            DirectoryInfo dir = new DirectoryInfo(path);

            DirectoryInfo[] dirs = dir.GetDirectories();

            FileInfo[] files = dir.GetFiles();
            if (filetype == "")
            {
                foreach (var crrFile in files)
                {
                    string File = crrFile.ToString();
                    result.Add(File.Remove(0, File.LastIndexOf('\\') + 1));
                }
            }
            else
            {
                foreach (FileInfo crrFile in files)
                {

                    string File = crrFile.ToString();
                    string extension = System.IO.Path.GetExtension(crrFile.FullName);
                    if (extension == filetype) result.Add(File.Remove(0, File.LastIndexOf('\\') + 1));
                }
            }
            return result;
        }
        public List<string> GetFolders(string path)
        {
            List<string> result = new List<string>();

            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] dirs = dir.GetDirectories();

            foreach (var folder in dirs)
            {
                result.Add(folder.Name.Remove(0, folder.Name.LastIndexOf('\\') + 1));
            }

            return result;
        }

    }
}

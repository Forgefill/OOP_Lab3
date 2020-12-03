using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
namespace OOP_Lab3
{
    class HTMLFinder
    {
        List<string> words;
        public HTMLFinder(List<string> words)
        {
            this.words = words;
        }

        public List<string> GetHTML(string path)
        {
            List<string> result = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(path);

            DirectoryInfo[] dirs = dir.GetDirectories();

            FileInfo[] files = dir.GetFiles("*.html");

            foreach(string word in words)
            {
                files = (from html in files
                         where (File.ReadAllLines(html.FullName).Any(line => line.Contains(word)))
                         select html).ToArray();
            }
            foreach(FileInfo res in files)
            {
                result.Add(res.Name);
            }
            return result;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace OOP_Lab3
{
    static class HTMLMover
    {

        public static List<string> GetImgPath(string crrText)
        {
            StringBuilder Text = new StringBuilder(crrText);
            List<string> ways = new List<string>();
            while (Text.ToString().Contains("<img"))
            {
                for (int i = Text.ToString().IndexOf("src="); i < Text.Length; ++i)
                {
                    if (Text[i] == '\"')
                    {
                        string result = "";
                        for (int j = i + 1; Text[j] != '\"'; ++j)
                        {
                            result += Text[j];
                        }
                        ways.Add(result);
                        Text.Remove(0, i + result.Length);
                    }
                }
            }

            return ways;
        }
        public static void WriteNewHTML(string path, string Text)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.Write("<!DOCTYPE html>\n<html>\n<head>\n</head>\n<body>");
            sw.Write(Text);
            sw.Write("</body>\n</html>");
            sw.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace OOP_Lab3
{
    class RegisterChanger
    {
        StringBuilder result;
        public RegisterChanger(string text)
        {
            result = new StringBuilder(text);
        }
        public string GetChangedText()
        {
            
            bool Reg = true;
            for(int i = 0; i < result.Length; ++i)
            {
                if (char.IsLetter(result[i]) && Reg)
                {
                    result[i] = char.ToUpper(result[i]);
                    Reg = false;
                }
                if(result[i] == '.' || result[i] == '?' || result[i] == '!')
                {
                    Reg = true;
                    for(int s = i; s > 0; --s)
                    {
                        if (char.IsLetter(result[s]))
                        {
                            result[s] = char.ToUpper(result[s]);
                            break;
                        }
                    }
                }
            }
            return result.ToString();
        }
    }
}

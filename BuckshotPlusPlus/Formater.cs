using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    public static class Formater
    {
        public static string FormatFileData(string FileData)
        {
            int i = 0;
            int spaceCount = 0;
            bool isQuote = false;

            while (i < FileData.Length )
            {
                // Gérer les chaines de character
                if (FileData[i] == '"')
                {
                    isQuote = !isQuote;
                }
                if (FileData[i] == ' ' || FileData[i] == '\t' && isQuote == false)
                {
                    while (FileData[spaceCount + i] == ' ' || FileData[spaceCount + i] == '\t')
                    {
                        spaceCount++;
                    }

                    if (i == 0)
                    {
                        FileData = FileData.Remove(i, spaceCount);
                    }
                    else if(FileData[i - 1] == '\n') {
                        FileData = FileData.Remove(i, spaceCount);
                    } else
                    {
                        if (spaceCount > 1)
                        {
                            FileData = FileData.Remove(i, spaceCount - 1);
                        }
                        
                    }
                    spaceCount = 0;
                }
                i++;
            }
            return FileData;
        }
        
        public static bool SafeContains(string Value, char c)
        {
            bool isQuote = false;

            for (int i = 0; i < Value.Length; i++)
            {
                if (Value[i] == '"')
                {
                    isQuote = !isQuote;
                }
                if (isQuote == false && Value[i] == c)
                {
                    return true;
                } 
            }
            return false;
        }

        public struct UnsafeCharStruct
        {
            public bool IsUnsafeChar { get; set; }
            public bool IsFirstChar { get; set; }
            public int UnsafeCharId { get; set; }
        }
        public static UnsafeCharStruct IsUnsafeChar(string[] UnsafeCharsList, char c)
        {
            UnsafeCharStruct UnsafeCharValue = new UnsafeCharStruct();
            for (int i = 0; i < UnsafeCharsList.Length; i++)
            {
                UnsafeCharValue.UnsafeCharId = i;
                if(c == UnsafeCharsList[i][0])
                {
                    UnsafeCharValue.IsFirstChar = true;
                    UnsafeCharValue.IsUnsafeChar = true;
                    return UnsafeCharValue;
                }
                else if(c == UnsafeCharsList[i][1])
                {
                    UnsafeCharValue.IsFirstChar = false;
                    UnsafeCharValue.IsUnsafeChar = true;
                    return UnsafeCharValue;
                }
            }
            UnsafeCharValue.IsUnsafeChar = false;
            return UnsafeCharValue;
        }
        public static List<string> SafeSplit(string Value, char c)
        {
            List<string> SplitedString = new List<string>();

            string[] UnsafeChars = { "\"\"", "()" };
            UnsafeCharStruct LastUnsafeChar = new UnsafeCharStruct();
            LastUnsafeChar.IsUnsafeChar = false;

            int Count = 0;
            int LastPos = 0;

            for (int i = 0; i < Value.Length; i++)
            {
                Count++;
                if (LastUnsafeChar.IsUnsafeChar)
                {
                    UnsafeCharStruct CurrentUnsafeChar = Formater.IsUnsafeChar(UnsafeChars, Value[i]);
                    if (CurrentUnsafeChar.IsUnsafeChar)
                    {
                        if((CurrentUnsafeChar.IsFirstChar == false || CurrentUnsafeChar.UnsafeCharId == 0) && CurrentUnsafeChar.UnsafeCharId == LastUnsafeChar.UnsafeCharId)
                        {
                            LastUnsafeChar.IsUnsafeChar = false;
                        }
                    }
                }
                else
                {
                    
                    UnsafeCharStruct CurrentUnsafeChar = Formater.IsUnsafeChar(UnsafeChars, Value[i]);
                    if (CurrentUnsafeChar.IsUnsafeChar)
                    {
                        LastUnsafeChar = CurrentUnsafeChar;
                    }
                    else
                    {
                        if (Value[i] == c)
                        {
                            SplitedString.Add(Value.Substring(i + 1 - Count, Count - 1));
                            LastPos = i + 1;
                            Count = 0;
                        }
                    }
                    
                }

                
            }
            SplitedString.Add(Value.Substring(LastPos, Value.Length - LastPos));

            return SplitedString;
        }

        public static void CriticalError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error : " + error);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("----------||  BUCKSHOT++  ||----------");
            Environment.Exit(-1);
        }

        public static void Warn(string error)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning : " + error);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void TokenCriticalError(string Error, Token MyToken)
        {
            Formater.CriticalError(Error + " in file : " + MyToken.FileName + " at line : " + MyToken.LineNumber + Environment.NewLine + "=> " + MyToken.LineData);
        }

        public static void DebugMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Debug : " + msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

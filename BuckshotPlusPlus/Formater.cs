using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace BuckshotPlusPlus
{
    public static class Formater
    {
        public struct SpecialCharacterToClean
        {
            public char Character;
            public bool CleanLeft;
            public bool CleanRight;
        }

        public static string FormatFileData(string fileData)
        {
            int i = 0;
            int spaceCount = 0;
            bool isQuote = false;

            List<SpecialCharacterToClean> charactersToClean = new List<SpecialCharacterToClean>
            {
                new() { Character = '+', CleanLeft = true, CleanRight = true },
                new() { Character = ',', CleanLeft = true, CleanRight = true }
            };

            while (i < fileData.Length)
            {
                if (fileData[i] == '"')
                {
                    isQuote = !isQuote;
                }
                if ((fileData[i] == ' ' || fileData[i] == '\t') && isQuote == false)
                {
                    while (fileData[spaceCount + i] == ' ' || fileData[spaceCount + i] == '\t')
                    {
                        spaceCount++;
                    }

                    if (i == 0)
                    {
                        fileData = fileData.Remove(i, spaceCount);
                    }
                    else if (fileData[i - 1] == '\n')
                    {
                        fileData = fileData.Remove(i, spaceCount);
                    }
                    else
                    {
                        foreach(SpecialCharacterToClean charToCLean in charactersToClean)
                        {
                            if (fileData[spaceCount + i] == charToCLean.Character && charToCLean.CleanLeft)
                            {
                                fileData = fileData.Remove(i, spaceCount);
                            }
                            else if (fileData[i - 1] == charToCLean.Character && charToCLean.CleanRight)
                            {
                                fileData = fileData.Remove(i, spaceCount);
                                i--;
                            }
                        }
                    }
                    spaceCount = 0;
                }
                i++;
            }
            return fileData;
        }

        public static string SafeRemoveSpacesFromString(string content)
        {
            int i = 0;
            int spaceCount = 0;
            bool isQuote = false;

            while (i < content.Length)
            {
                // Gérer les chaines de character
                if (content[i] == '"')
                {
                    isQuote = !isQuote;
                }
                if (content[i] == ' ' || content[i] == '\t' && isQuote == false)
                {
                    while (content[spaceCount + i] == ' ' || content[spaceCount + i] == '\t')
                    {
                        spaceCount++;
                    }

                    if (i == 0)
                    {
                        content = content.Remove(i, spaceCount);
                    }
                    else
                    {
                        if (spaceCount > 0)
                        {
                            content = content.Remove(i, spaceCount);
                        }
                    }
                    spaceCount = 0;
                }
                i++;
            }
            return content;
        }

        public static bool SafeContains(string value, char c)
        {
            return StringHandler.SafeContains(value, c);
        }

        public struct UnsafeCharStruct
        {
            public bool IsUnsafeChar { get; set; }
            public bool IsFirstChar { get; set; }
            public int UnsafeCharId { get; set; }
        }

        public static UnsafeCharStruct IsUnsafeChar(string[] unsafeCharsList, char c)
        {
            UnsafeCharStruct unsafeCharValue = new UnsafeCharStruct();
            for (int i = 0; i < unsafeCharsList.Length; i++)
            {
                unsafeCharValue.UnsafeCharId = i;
                if (c == unsafeCharsList[i][0])
                {
                    unsafeCharValue.IsFirstChar = true;
                    unsafeCharValue.IsUnsafeChar = true;
                    return unsafeCharValue;
                }
                else if (c == unsafeCharsList[i][1])
                {
                    unsafeCharValue.IsFirstChar = false;
                    unsafeCharValue.IsUnsafeChar = true;
                    return unsafeCharValue;
                }
            }
            unsafeCharValue.IsUnsafeChar = false;
            return unsafeCharValue;
        }

        public static List<string> SafeSplit(string value, char c, bool onlyStrings = false)
        {
            return StringHandler.SafeSplit(value, c);
        }

        public static void CriticalError(string error)
        {
            AnsiConsole.Markup($"[maroon on default]Error : {error}[/]");

            Environment.Exit(-1);
        }

        public static void RuntimeError(string error, Token myToken)
        {
            if(myToken == null)
            {
                AnsiConsole.Markup($"[maroon on default]Runtime error : {error}[/]");
            }
            else
            {
                AnsiConsole.Markup("[maroon on default]Runtime error : " +
                error +
                " in file : "
                    + myToken.FileName
                    + " at line : "
                    + myToken.LineNumber
                    + Environment.NewLine
                    + "=> "
                    + myToken.LineData + "[/]\n");
            }
        }

        public static void Warn(string error)
        {
            AnsiConsole.Markup($"[orange3 on default]Warning : {error}[/]");
            AnsiConsole.Write("\n");
        }

        public static void TokenCriticalError(string error, Token myToken)
        {
            Console.WriteLine(error);
            Console.WriteLine(myToken.FileName);
            Console.WriteLine(myToken.LineNumber);
            Console.WriteLine(myToken.LineData);
            CriticalError($"{error.ToString()} in file {myToken.FileName} at line : {myToken.LineNumber.ToString()}\n=> {myToken.LineData}");
        }

        public static void DebugMessage(string msg)
        {
            AnsiConsole.Markup($"[dodgerblue3 on default]Debug : {msg}[/]");
            AnsiConsole.Write("\n");
        }

        public static void SuccessMessage(string msg)
        {
            AnsiConsole.Markup($"[green4 on default]Success : {msg}[/]");
            AnsiConsole.Write("\n");
        }
    }
}

using System;
using System.Collections.Generic;
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

        public static string FormatFileData(string FileData)
        {
            int i = 0;
            int spaceCount = 0;
            bool isQuote = false;

            List<SpecialCharacterToClean> CharactersToClean = new List<SpecialCharacterToClean>();

            CharactersToClean.Add(new SpecialCharacterToClean{ Character = '+', CleanLeft = true, CleanRight = true });
            CharactersToClean.Add(new SpecialCharacterToClean{ Character = ',', CleanLeft = true, CleanRight = true });



            while (i < FileData.Length)
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
                    else if (FileData[i - 1] == '\n')
                    {
                        FileData = FileData.Remove(i, spaceCount);
                    }
                    else
                    {
                        bool CleanedSpecialChar = false;

                        foreach(SpecialCharacterToClean CharToCLean in CharactersToClean)
                        {
                            if (FileData[spaceCount + i] == CharToCLean.Character && CharToCLean.CleanLeft)
                            {
                                FileData = FileData.Remove(i, spaceCount);
                                CleanedSpecialChar = true;
                            }
                            else if (FileData[i - 1] == CharToCLean.Character && CharToCLean.CleanRight)
                            {
                                FileData = FileData.Remove(i, spaceCount);
                                CleanedSpecialChar = true;
                            }
                        }

                        if (spaceCount > 1 && CleanedSpecialChar)
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

        public static string SafeRemoveSpacesFromString(string Content)
        {
            int i = 0;
            int spaceCount = 0;
            bool isQuote = false;

            while (i < Content.Length)
            {
                // Gérer les chaines de character
                if (Content[i] == '"')
                {
                    isQuote = !isQuote;
                }
                if (Content[i] == ' ' || Content[i] == '\t' && isQuote == false)
                {
                    while (Content[spaceCount + i] == ' ' || Content[spaceCount + i] == '\t')
                    {
                        spaceCount++;
                    }

                    if (i == 0)
                    {
                        Content = Content.Remove(i, spaceCount);
                    }
                    else
                    {
                        if (spaceCount > 0)
                        {
                            Content = Content.Remove(i, spaceCount);
                        }
                    }
                    spaceCount = 0;
                }
                i++;
            }
            return Content;
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
                if (c == UnsafeCharsList[i][0])
                {
                    UnsafeCharValue.IsFirstChar = true;
                    UnsafeCharValue.IsUnsafeChar = true;
                    return UnsafeCharValue;
                }
                else if (c == UnsafeCharsList[i][1])
                {
                    UnsafeCharValue.IsFirstChar = false;
                    UnsafeCharValue.IsUnsafeChar = true;
                    return UnsafeCharValue;
                }
            }
            UnsafeCharValue.IsUnsafeChar = false;
            return UnsafeCharValue;
        }

        public static List<string> SafeSplit(string Value, char c, bool only_strings = false)
        {
            List<string> SplitedString = new List<string>();

            string[] UnsafeChars = { "\"\"", "()" };

            if (only_strings)
            {
                UnsafeChars[1] = "\"\"";
            }

            UnsafeCharStruct LastUnsafeChar = new UnsafeCharStruct();
            LastUnsafeChar.IsUnsafeChar = false;

            int Count = 0;
            int LastPos = 0;

            for (int i = 0; i < Value.Length; i++)
            {
                Count++;
                if (LastUnsafeChar.IsUnsafeChar)
                {
                    UnsafeCharStruct CurrentUnsafeChar = Formater.IsUnsafeChar(
                        UnsafeChars,
                        Value[i]
                    );
                    if (CurrentUnsafeChar.IsUnsafeChar)
                    {
                        if (
                            (
                                CurrentUnsafeChar.IsFirstChar == false
                                || CurrentUnsafeChar.UnsafeCharId == 0
                            )
                            && CurrentUnsafeChar.UnsafeCharId == LastUnsafeChar.UnsafeCharId
                        )
                        {
                            LastUnsafeChar.IsUnsafeChar = false;
                        }
                    }
                }
                else
                {
                    UnsafeCharStruct CurrentUnsafeChar = Formater.IsUnsafeChar(
                        UnsafeChars,
                        Value[i]
                    );
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
            AnsiConsole.Markup($"[maroon on default]Error : {error}[/]");

            Environment.Exit(-1);
        }

        public static void RuntimeError(string error, Token MyToken)
        {
            if(MyToken == null)
            {
                AnsiConsole.Markup($"[maroon on default]Runtime error : {error}[/]");
            }
            else
            {
                AnsiConsole.Markup($"[maroon on default]Runtime error : " +
                error +
                " in file : "
                    + MyToken.FileName
                    + " at line : "
                    + MyToken.LineNumber
                    + Environment.NewLine
                    + "=> "
                    + MyToken.LineData + "[/]\n");
            }
        }

        public static void Warn(string error)
        {
            AnsiConsole.Markup($"[orange3 on default]Warning : {error}[/]");
            AnsiConsole.Write("\n");
        }

        public static void TokenCriticalError(string Error, Token MyToken)
        {
            Formater.CriticalError(
                Error
                    + " in file : "
                    + MyToken.FileName
                    + " at line : "
                    + MyToken.LineNumber
                    + Environment.NewLine
                    + "=> "
                    + MyToken.LineData
            );
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

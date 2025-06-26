using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace BuckshotPlusPlus
{
    public static class Formater
    {
        private static bool _debugEnabled = false;

        private static string EscapeMarkup(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Replace("[", "[[").Replace("]", "]]");
        }

        private static void SafeMarkup(string style, string message, bool newLine = true)
        {
            try
            {
                string safeMessage = EscapeMarkup(message);
                if (newLine)
                {
                    AnsiConsole.MarkupLine($"[{style}]{safeMessage}[/]");
                }
                else
                {
                    AnsiConsole.Markup($"[{style}]{safeMessage}[/]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in markup rendering: {ex.Message}");
                Console.WriteLine($"Attempted to render: {message}");
                Console.WriteLine($"Style was: {style}");
            }
        }

        public static void EnableDebug()
        {
            _debugEnabled = true;
            DebugMessage("Debug mode enabled");
        }

        public struct SpecialCharacterToClean
        {
            public char Character;
            public bool CleanLeft;
            public bool CleanRight;
        }

        public static string FormatFileData(string fileData)
        {
            if (_debugEnabled) DebugMessage($"Formatting file data of length: {fileData.Length}");

            // First handle // comments
            StringBuilder result = new StringBuilder();
            string[] lines = fileData.Split('\n');
            
            foreach (string line in lines)
            {
                string processedLine = line.Trim();
                
                // Handle // comments
                int commentIndex = processedLine.IndexOf("//");
                if (commentIndex >= 0)
                {
                    if (commentIndex == 0)
                    {
                        // Skip pure comment lines
                        continue;
                    }
                    else
                    {
                        // Strip comment from end of line
                        processedLine = processedLine.Substring(0, commentIndex).TrimEnd();
                    }
                }
                
                if (!string.IsNullOrWhiteSpace(processedLine))
                {
                    result.AppendLine(processedLine);
                }
            }
            
            // Then process the rest of the formatting
            string cleanedData = result.ToString();
            int i = 0;
            int spaceCount = 0;
            bool isQuote = false;

            List<SpecialCharacterToClean> charactersToClean = new()
            {
                new() { Character = '+', CleanLeft = true, CleanRight = true },
                new() { Character = ',', CleanLeft = true, CleanRight = true },
                new() { Character = ':', CleanLeft = true, CleanRight = true }
            };

            result = new StringBuilder(cleanedData);

            while (i < result.Length)
            {
                if (result[i] == '"')
                    isQuote = !isQuote;

                if ((result[i] == ' ' || result[i] == '\t') && !isQuote)
                {
                    spaceCount = 0;
                    while ((i + spaceCount) < result.Length &&
                           (result[i + spaceCount] == ' ' || result[i + spaceCount] == '\t'))
                    {
                        spaceCount++;
                    }

                    if (i == 0 || result[i - 1] == '\n')
                    {
                        result.Remove(i, spaceCount);
                    }
                    else
                    {
                        foreach (var charToClean in charactersToClean)
                        {
                            if (i + spaceCount < result.Length &&
                                result[i + spaceCount] == charToClean.Character &&
                                charToClean.CleanLeft)
                            {
                                result.Remove(i, spaceCount);
                            }
                            else if (i > 0 &&
                                     result[i - 1] == charToClean.Character &&
                                     charToClean.CleanRight)
                            {
                                result.Remove(i, spaceCount);
                                i--;
                            }
                        }
                    }
                    spaceCount = 0;
                }
                i++;
            }

            return result.ToString().TrimEnd();
        }

        public static string StripComments(string line)
        {
            if (string.IsNullOrEmpty(line)) return line;
            
            int commentIndex = line.IndexOf("//");
            if (commentIndex >= 0)
            {
                return line.Substring(0, commentIndex).TrimEnd();
            }
            return line;
        }

        public static string SafeRemoveSpacesFromString(string content)
        {
            if (_debugEnabled) DebugMessage($"Removing spaces from: {content}");

            int i = 0;
            int spaceCount = 0;
            bool isQuote = false;
            StringBuilder result = new(content);

            while (i < result.Length)
            {
                if (result[i] == '"')
                {
                    isQuote = !isQuote;
                }
                if ((result[i] == ' ' || result[i] == '\t') && !isQuote)
                {
                    spaceCount = 0;
                    while ((i + spaceCount) < result.Length &&
                           (result[i + spaceCount] == ' ' || result[i + spaceCount] == '\t'))
                    {
                        spaceCount++;
                    }

                    if (i == 0 || spaceCount > 0)
                    {
                        result.Remove(i, spaceCount);
                    }
                    spaceCount = 0;
                }
                i++;
            }

            var finalResult = result.ToString();
            if (_debugEnabled) DebugMessage($"Space removal result: {finalResult}");
            return finalResult;
        }

        public static bool SafeContains(string value, char c)
        {
            if (_debugEnabled) DebugMessage($"Checking if '{value}' contains '{c}'");
            return StringHandler.SafeContains(StripComments(value), c);
        }

        public struct UnsafeCharStruct
        {
            public bool IsUnsafeChar { get; set; }
            public bool IsFirstChar { get; set; }
            public int UnsafeCharId { get; set; }
        }

        public static UnsafeCharStruct IsUnsafeChar(string[] unsafeCharsList, char c)
        {
            if (_debugEnabled) DebugMessage($"Checking unsafe char: {c}");

            UnsafeCharStruct unsafeCharValue = new UnsafeCharStruct();
            for (int i = 0; i < unsafeCharsList.Length; i++)
            {
                unsafeCharValue.UnsafeCharId = i;
                if (c == unsafeCharsList[i][0])
                {
                    unsafeCharValue.IsFirstChar = true;
                    unsafeCharValue.IsUnsafeChar = true;
                    if (_debugEnabled) DebugMessage($"Found unsafe first char at index {i}");
                    return unsafeCharValue;
                }
                else if (c == unsafeCharsList[i][1])
                {
                    unsafeCharValue.IsFirstChar = false;
                    unsafeCharValue.IsUnsafeChar = true;
                    if (_debugEnabled) DebugMessage($"Found unsafe second char at index {i}");
                    return unsafeCharValue;
                }
            }
            unsafeCharValue.IsUnsafeChar = false;
            return unsafeCharValue;
        }

        public static List<string> SafeSplit(string value, char c, bool onlyStrings = false)
        {
            if (_debugEnabled) DebugMessage($"Splitting: '{value}' on character: '{c}'");
            var result = StringHandler.SafeSplit(StripComments(value), c);
            if (_debugEnabled) DebugMessage($"Split result: {string.Join(" | ", result)}");
            return result;
        }

        public static void CriticalError(string error)
        {
            SafeMarkup("red bold", $"CRITICAL ERROR: {error}");
            Environment.Exit(-1);
        }

        public static void RuntimeError(string error, Token myToken)
        {
            if (myToken == null)
            {
                SafeMarkup("maroon", $"Runtime error: {error}");
            }
            else
            {
                var message = new StringBuilder()
                    .AppendLine($"Runtime error: {error}")
                    .AppendLine($"File: {myToken.FileName}")
                    .AppendLine($"Line: {myToken.LineNumber}")
                    .AppendLine($"Content: {myToken.LineData}");

                SafeMarkup("maroon", message.ToString());
            }
        }

        public static void Warn(string error)
        {
            SafeMarkup("orange3", $"Warning: {error}");
        }

        public static void TokenCriticalError(string error, Token myToken)
        {
            var message = new StringBuilder()
                .AppendLine(error)
                .AppendLine($"File: {myToken.FileName}")
                .AppendLine($"Line: {myToken.LineNumber}")
                .AppendLine($"Content: {myToken.LineData}");

            CriticalError(message.ToString());
        }

        public static void DebugMessage(string msg)
        {
            SafeMarkup("dodgerblue3", $"Debug: {msg}");
        }

        public static void TraceMessage(string category, string msg)
        {
            if (_debugEnabled)
            {
                SafeMarkup("grey", $"[{category}] {msg}");
            }
        }

        public static void SuccessMessage(string msg)
        {
            SafeMarkup("green4", $"Success: {msg}");
        }

        public static void DumpToken(Token token, string context = "")
        {
            if (!_debugEnabled) return;

            var dump = new StringBuilder()
                .AppendLine($"Token Dump {(context != "" ? $"({context})" : "")}")
                .AppendLine($"  File: {token.FileName}")
                .AppendLine($"  Line: {token.LineNumber}")
                .AppendLine($"  Type: {token.Type}")
                .AppendLine($"  Data Type: {token.Data?.GetType().Name}")
                .AppendLine($"  Content: {token.LineData}");

            TraceMessage("TOKEN", dump.ToString());
        }
    }
}
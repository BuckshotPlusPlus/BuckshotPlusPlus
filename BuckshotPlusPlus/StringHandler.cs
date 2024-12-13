using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public static class StringHandler
    {
        private enum StringParseState
        {
            Normal,
            InSingleQuote,
            InDoubleQuote,
            Escaped
        }

        public static bool SafeContains(string input, char searchChar)
        {
            // Add null check
            if (string.IsNullOrEmpty(input)) return false;

            var state = StringParseState.Normal;

            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];

                switch (state)
                {
                    case StringParseState.Normal:
                        if (currentChar == '"')
                            state = StringParseState.InDoubleQuote;
                        else if (currentChar == '\'')
                            state = StringParseState.InSingleQuote;
                        else if (currentChar == '\\')
                            state = StringParseState.Escaped;
                        else if (currentChar == searchChar)
                            return true;
                        break;

                    case StringParseState.InDoubleQuote:
                        if (currentChar == '\\')
                            state = StringParseState.Escaped;
                        else if (currentChar == '"')
                            state = StringParseState.Normal;
                        break;

                    case StringParseState.InSingleQuote:
                        if (currentChar == '\\')
                            state = StringParseState.Escaped;
                        else if (currentChar == '\'')
                            state = StringParseState.Normal;
                        break;

                    case StringParseState.Escaped:
                        state = (state == StringParseState.Normal)
                            ? StringParseState.Normal
                            : (state == StringParseState.InDoubleQuote)
                                ? StringParseState.InDoubleQuote
                                : StringParseState.InSingleQuote;
                        break;
                }
            }

            return false;
        }

        public static List<string> SafeSplit(string input, char delimiter)
        {
            var result = new List<string>();
            var currentSegment = new System.Text.StringBuilder();
            var state = StringParseState.Normal;

            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];

                switch (state)
                {
                    case StringParseState.Normal:
                        if (currentChar == delimiter)
                        {
                            result.Add(currentSegment.ToString());
                            currentSegment.Clear();
                        }
                        else
                        {
                            if (currentChar == '"')
                                state = StringParseState.InDoubleQuote;
                            else if (currentChar == '\'')
                                state = StringParseState.InSingleQuote;
                            else if (currentChar == '\\')
                                state = StringParseState.Escaped;

                            currentSegment.Append(currentChar);
                        }
                        break;

                    case StringParseState.InDoubleQuote:
                    case StringParseState.InSingleQuote:
                        if (currentChar == '\\')
                            state = StringParseState.Escaped;
                        else if ((state == StringParseState.InDoubleQuote && currentChar == '"') ||
                                (state == StringParseState.InSingleQuote && currentChar == '\''))
                            state = StringParseState.Normal;

                        currentSegment.Append(currentChar);
                        break;

                    case StringParseState.Escaped:
                        currentSegment.Append(currentChar);
                        state = (state == StringParseState.Normal)
                            ? StringParseState.Normal
                            : (state == StringParseState.InDoubleQuote)
                                ? StringParseState.InDoubleQuote
                                : StringParseState.InSingleQuote;
                        break;
                }
            }

            if (currentSegment.Length > 0)
                result.Add(currentSegment.ToString());

            return result;
        }

        public static bool IsInsideQuotes(string input, int position)
        {
            if (position >= input.Length)
                return false;

            var state = StringParseState.Normal;

            for (int i = 0; i < position; i++)
            {
                char currentChar = input[i];

                switch (state)
                {
                    case StringParseState.Normal:
                        if (currentChar == '"')
                            state = StringParseState.InDoubleQuote;
                        else if (currentChar == '\'')
                            state = StringParseState.InSingleQuote;
                        else if (currentChar == '\\')
                            state = StringParseState.Escaped;
                        break;

                    case StringParseState.InDoubleQuote:
                        if (currentChar == '\\')
                            state = StringParseState.Escaped;
                        else if (currentChar == '"')
                            state = StringParseState.Normal;
                        break;

                    case StringParseState.InSingleQuote:
                        if (currentChar == '\\')
                            state = StringParseState.Escaped;
                        else if (currentChar == '\'')
                            state = StringParseState.Normal;
                        break;

                    case StringParseState.Escaped:
                        state = (state == StringParseState.Normal)
                            ? StringParseState.Normal
                            : (state == StringParseState.InDoubleQuote)
                                ? StringParseState.InDoubleQuote
                                : StringParseState.InSingleQuote;
                        break;
                }
            }

            return state == StringParseState.InDoubleQuote || state == StringParseState.InSingleQuote;
        }
    }
}
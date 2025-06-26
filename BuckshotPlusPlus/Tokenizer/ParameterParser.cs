using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BuckshotPlusPlus
{
    public static class ParameterParser
    {
        public static bool IsParameterizedView(string lineData)
        {
            // Matches: view ViewName(param1, param2, ...) {
            // Or: view ViewName:Parent(param1, param2, ...) {
            string pattern = @"^view\s+\w+(\s*:\s*\w+)?\s*\([^)]*\)\s*(\{\s*)?$";
            return Regex.IsMatch(lineData.Trim(), pattern);
        }

        public static bool IsViewCall(string variableData)
        {
            // Matches: ViewName(arg1, arg2, ...)
            return Regex.IsMatch(variableData.Trim(), @"^\w+\s*\([^)]*\)\s*$");
        }

        public static (string viewName, string parentViewName, List<string> parameters) ParseParameterizedViewDefinition(string lineData)
        {
            // Extract view name, parent view name, and parameters from: 
            // view ViewName:ParentView(param1, param2, ...) {
            // or view ViewName(param1, param2, ...) {
            var match = Regex.Match(lineData.Trim(), 
                @"^view\s+(\w+)(?:\s*:\s*(\w+))?\s*\(([^)]*)\)\s*(\{\s*)?$");
            
            if (!match.Success)
            {
                Formater.DebugMessage($"Failed to parse parameterized view definition: {lineData}");
                return (null, null, new List<string>());
            }

            string viewName = match.Groups[1].Value.Trim();
            string parentViewName = match.Groups[2].Success ? match.Groups[2].Value.Trim() : null;
            string parametersString = match.Groups[3].Value.Trim();

            var parameters = new List<string>();
            if (!string.IsNullOrEmpty(parametersString))
            {
                foreach (var param in parametersString.Split(','))
                {
                    string trimmedParam = param.Trim();
                    if (!string.IsNullOrEmpty(trimmedParam))
                    {
                        parameters.Add(trimmedParam);
                    }
                }
            }


            return (viewName, parentViewName, parameters);
        }


        public static (string viewName, List<string> arguments) ParseViewCall(string variableData)
        {
            // Extract view name and arguments from: ViewName(arg1, arg2, ...)
            var match = Regex.Match(variableData.Trim(), @"^(\w+)\s*\(([^)]*)\)\s*$");
            
            if (!match.Success)
            {
                return (null, new List<string>());
            }

            string viewName = match.Groups[1].Value;
            string argumentsString = match.Groups[2].Value.Trim();

            var arguments = new List<string>();
            if (!string.IsNullOrEmpty(argumentsString))
            {
                foreach (var arg in argumentsString.Split(','))
                {
                    string trimmedArg = arg.Trim();
                    // Remove quotes if it's a string literal
                    if (trimmedArg.StartsWith("\"") && trimmedArg.EndsWith("\""))
                    {
                        trimmedArg = trimmedArg.Substring(1, trimmedArg.Length - 2);
                    }
                    arguments.Add(trimmedArg);
                }
            }

            return (viewName, arguments);
        }
    }
}

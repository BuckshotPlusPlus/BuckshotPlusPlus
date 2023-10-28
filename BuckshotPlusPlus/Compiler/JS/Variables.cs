using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.JS
{
    /// <summary>
    /// Class for handling variables in BuckshotPlusPlus compiler.
    /// </summary>
    public static class Variables
    {
        /// <summary>
        /// Gets a variable string for the given server-side and function tokens at a particular token index.
        /// </summary>
        /// <param name="serverSideTokens">The list of server-side tokens.</param>
        /// <param name="functionTokens">The list of function tokens.</param>
        /// <param name="currentTokenIndex">The index of the current token.</param>
        /// <returns>A string representing the variable, empty if conditions are not met.</returns>
        public static string GetVarString(List<Token> serverSideTokens, List<Token> functionTokens, int currentTokenIndex)
        {
            // Check if the index is out of range
            if (currentTokenIndex >= functionTokens.Count || currentTokenIndex < 0)
            {
                return "";
            }

            // Initialize current token and its name
            Token currentToken = functionTokens[currentTokenIndex];
            string currentTokenName = TokenUtils.GetTokenName(currentToken);

            // Check if the current token's data is of type TokenDataVariable
            if (currentToken.Data is not TokenDataVariable myVarData) return "";
            // Initialize the variable string and counters
            string varString = "let ";
            int tokenCounter = 0;
            int tokensWithNameFound = 0;

            // Loop through the function tokens to find occurrences of the current token name
            foreach (Token containerChildToken in functionTokens)
            {
                if (currentTokenName == TokenUtils.GetTokenName(containerChildToken))
                {
                    tokensWithNameFound++;
                }

                if (currentTokenName == TokenUtils.GetTokenName(containerChildToken) && tokenCounter < currentTokenIndex)
                {
                    varString = "";  // Found an earlier declaration, no need for 'let'
                    break;
                }

                tokenCounter++;
            }

            // If only one occurrence of this variable name is found, it can be a 'const'
            if (tokensWithNameFound == 1)
            {
                varString = "const";
            }

            return $"{varString} {myVarData.VariableName} = {myVarData.GetCompiledVariableData(serverSideTokens)}";

            // Return empty string if conditions are not met
        }
    }
}

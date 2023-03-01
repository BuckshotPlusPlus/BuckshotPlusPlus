namespace BuckshotPlusPlus
{
    public class TokenDataLogic : TokenData
    {
        public string LogicToken { get; set; }

        public static string[] LogicTokens = { "if", "then", "else", "or", "and", "on" };

        public TokenDataLogic(Token MyToken)
        {
            MyToken.Type = "logic";
        }

        public static bool IsTokenDataLogic(Token MyToken)
        {
            foreach (string TokenType in LogicTokens)
            {
                if (MyToken.LineData == TokenType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Databases
{
    public class MySqlDatabase : BaseDatabase
    {
        private MySqlConnection connection;

        public MySqlDatabase(Dictionary<string, string> Parameters , Tokenizer MyTokenizer) : base(Parameters, MyTokenizer)
        {
            string connectionString = $"Server={DatabaseParameters["Server"]};Database={DatabaseParameters["Database"]};User ID={DatabaseParameters["UserId"]};Password={DatabaseParameters["Password"]};";
            connection = new MySqlConnection(connectionString);
        }

        public override Token Query(string query)
        {
            string TokenLineData = "data{\n";

            // Open the connection
            connection.Open();

            // Create a command object
            MySqlCommand cmd = new MySqlCommand(query, connection);

            // Execute the query and get the result set
            MySqlDataReader reader = cmd.ExecuteReader();

            //int rowIndex = 0;

            // Iterate through the result set
            while (reader.Read())
            {
                /*string row_data = ""
                // Iterate through the columns in the current row
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    // Get the column name and value
                    string columnName = reader.GetName(i);
                    string columnValue = reader[i].ToString();

                    // Add the column name and value to the lineData property of the Token object
                    token.lineData.Add(columnName, columnValue);
                }

                // Add the token to the result list
                result.Add(token);

                rowIndex++;*/
            }

            // Close the reader and the connection
            reader.Close();
            connection.Close();

            return new Token("mysql_auto_generated", TokenLineData, 0, this.MyTokenizer); ;
        }
    }
}

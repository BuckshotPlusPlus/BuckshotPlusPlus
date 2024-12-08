# BuckshotPlusPlus Project Documentation


## Project Files

### File: BuckshotPlusPlus.csproj

```csharp
﻿<Project Sdk="Microsoft.NET.Sdk;Microsoft.NET.Sdk.Publish">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
	  <PackageReference Include="Appwrite" Version="0.4.2" />
	  
    <PackageReference Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.14.0" />
    <PackageReference Include="MySql.Data" Version="8.1.0" />
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
    <PackageReference Include="Spectre.Console" Version="0.46.0" />
  </ItemGroup>
</Project>
```


## Source Files

### File: Analytics\AnalyticTimedEvent.cs

```csharp
﻿using System;

namespace BuckshotPlusPlus.Analytics;

public class AnalyticTimedEvent
{
    public string EventName { get; set; }
    public string EventTimestamp { get; set; }

    public AnalyticTimedEvent(string @event)
    {
        var now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        EventTimestamp = now.ToString();
        EventName = @event;

    }
}

```

### File: Analyzer\Array.cs

```csharp
﻿using System.Collections.Generic;

namespace BuckshotPlusPlus.Analyzer
{
    public class Array
    {
        public static List<Token> GetArrayValues(Token myToken)
        {
            TokenDataVariable myArray = (TokenDataVariable)myToken.Data;
            List<Token> values = new List<Token>();
            if (myArray.VariableType != "array")
            {
                Formater.TokenCriticalError("Can't read array variable, because the following token is not an array", myToken);
            }
            else
            {
                List<string> arrayValues = Formater.SafeSplit(
                    myArray.VariableData.Substring(1, myArray.VariableData.Length - 2),
                    ','
                );
                foreach (string arrayValue in arrayValues)
                {
                    values.Add(


                        new Token(
                            myToken.FileName,
                            arrayValue,
                            myToken.LineNumber,
                            myToken.MyTokenizer
                        )
                    );
                }
            }
            return values;
        }
    }
}

```

### File: Compiler\CSS\Properties.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.CSS
{
    public class Properties
    {
        static List<String> _props = new()
        {
            "align-content",
            "align-items",
            "align-self",
            "all",
            "animation",
            "animation-delay",
            "animation-direction",
            "animation-duration",
            "animation-fill-mode",
            "animation-iteration-count",
            "animation-name",
            "animation-play-state",
            "animation-timing-function",
            "backface-visibility",
            "background",
            "background-attachment",
            "background-blend-mode",
            "background-clip",
            "background-color",
            "background-image",
            "background-origin",
            "background-position",
            "background-repeat",
            "background-size",
            "border",
            "border-bottom",
            "border-bottom-color",
            "border-bottom-left-radius",
            "border-bottom-right-radius",
            "border-bottom-style",
            "border-bottom-width",
            "border-collapse",
            "border-color",
            "border-image",
            "border-image-outset",
            "border-image-repeat",
            "border-image-slice",
            "border-image-source",
            "border-image-width",
            "border-left",
            "border-left-color",
            "border-left-style",
            "border-left-width",
            "border-radius",
            "border-right",
            "border-right-color",
            "border-right-style",
            "border-right-width",
            "border-spacing",
            "border-style",
            "border-top",
            "border-top-color",
            "border-top-left-radius",
            "border-top-right-radius",
            "border-top-style",
            "border-top-width",
            "border-width",
            "bottom",
            "box-shadow",
            "box-sizing",
            "caption-side",
            "caret-color",
            "clear",
            "clip",
            "clip-path",
            "color",
            "column-count",
            "column-fill",
            "column-gap",
            "column-rule",
            "column-rule-color",
            "column-rule-style",
            "column-rule-width",
            "column-span",
            "column-width",
            "columns",
            //"content", We need to remove this otherwise we can't edit content in events
            "counter-increment",
            "counter-reset",
            "cursor",
            "direction",
            "display",
            "empty-cells",
            "filter",
            "flex",
            "flex-basis",
            "flex-direction",
            "flex-flow",
            "flex-grow",
            "flex-shrink",
            "flex-wrap",
            "float",
            "font",
            "font-family",
            "font-kerning",
            "font-size",
            "font-size-adjust",
            "font-stretch",
            "font-style",
            "font-variant",
            "font-weight",
            "grid",
            "grid-area",
            "grid-auto-columns",
            "grid-auto-flow",
            "grid-auto-rows",
            "grid-column",
            "grid-column-end",
            "grid-column-gap",
            "grid-column-start",
            "grid-gap",
            "grid-row",
            "grid-row-end",
            "grid-row-gap",
            "grid-row-start",
            "grid-template",
            "grid-template-areas",
            "grid-template-columns",
            "grid-template-rows",
            "height",
            "hyphens",
            "justify-content",
            "left",
            "letter-spacing",
            "line-height",
            "list-style",
            "list-style-image",
            "list-style-position",
            "list-style-type",
            "margin",
            "margin-bottom",
            "margin-left",
            "margin-right",
            "margin-top",
            "max-height",
            "max-width",
            "min-height",
            "min-width",
            "object-fit",
            "object-position",
            "opacity",
            "order",
            "outline",
            "outline-color",
            "outline-offset",
            "outline-style",
            "outline-width",
            "overflow",
            "overflow-x",
            "overflow-y",
            "padding",
            "padding-bottom",
            "padding-left",
            "padding-right",
            "padding-top",
            "page-break-after",
            "page-break-before",
            "page-break-inside",
            "perspective",
            "perspective-origin",
            "pointer-events",
            "position",
            "quotes",
            "right",
            "scroll-behavior",
            "table-layout",
            "text-align",
            "text-align-last",
            "text-decoration",
            "text-decoration-color",
            "text-decoration-line",
            "text-decoration-style",
            "text-indent",
            "text-justify",
            "text-overflow",
            "text-shadow",
            "text-transform",
            "top",
            "transform",
            "transform-origin",
            "transform-style",
            "transition",
            "transition-delay",
            "transition-duration",
            "transition-property",
            "transition-timing-function",
            "user-select",
            "vertical-align",
            "visibility",
            "white-space",
            "width",
            "word-break",
            "word-spacing",
            "word-wrap",
            "writing-mode",
            "z-index"
        };

        public static string GetCssString(List<Token> serverSideTokens, Token myToken)
        {
            string compiledCss = "";
            TokenDataContainer viewContainer = (TokenDataContainer)myToken.Data;
            
            foreach (String name in _props)
            {
                TokenDataVariable myCssProp = TokenUtils.FindTokenDataVariableByName(
                    viewContainer.ContainerData,
                    name
                );
                if (myCssProp != null)
                {
                    if (myCssProp.VariableType == "ref" && myCssProp.RefData != null)
                    {
                        if (myCssProp.RefData.Data.GetType() == typeof(TokenDataVariable))
                        {
                            TokenDataVariable myRefData = (TokenDataVariable)myCssProp.RefData.Data;
                            compiledCss +=
                                name + ':' + myRefData.GetCompiledVariableData(serverSideTokens) + ";";
                        }
                    }
                    else
                    {
                        compiledCss +=
                            name + ':' + myCssProp.GetCompiledVariableData(serverSideTokens) + ";";
                    }
                }
            }
            TokenDataVariable myFloatProp = TokenUtils.FindTokenDataVariableByName(
                viewContainer.ContainerData,
                "float"
            );
            if (myFloatProp != null)
            {
                compiledCss += "float:" + myFloatProp.GetCompiledVariableData(serverSideTokens) + ";";
            }
            return compiledCss;
        }

        public static bool IsCssProp(Token myToken)
        {
            TokenDataVariable myVar = (TokenDataVariable)myToken.Data;
            foreach (String prop in _props)
            {
                if (myVar.VariableName == prop)
                {
                    return true;
                }
            }
            return false;
        }

        public static string ToDomProp(string name)
        {
            string[] result = name.Split('-');
            for (int i = 1; i < result.Length; i++)
            {
                result[i] = char.ToUpper(result[i][0]) + result[i].Substring(1);
            }
            
            return String.Join("", result);
        }
    }
}

```

### File: Compiler\HTML\Atributes.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class Attributes
    {
        static List<(String, Boolean)> _props = new()
        {
            ("href", false),
            ("id", false),
            ("class", false),
            ("target", false),
            ("disabled", true)
        };

        public static string GetHtmlAttributes(List<Token> serverSideTokens,Token myToken)
        {
            string compiledAtributes = "";
            TokenDataContainer viewContainer = (TokenDataContainer)myToken.Data;
            
            foreach ((String name, bool withoutValue) in _props)
            {
                TokenDataVariable myHtmlAttribute = TokenUtils.TryFindTokenDataVariableValueByName(
                    serverSideTokens,
                    viewContainer.ContainerData,
                    name
                );
                
                if (myHtmlAttribute != null)
                {
                    if ( withoutValue )
                    {
                        compiledAtributes += name;
                    }
                    else
                    {
                        compiledAtributes +=
                            name
                            + "=\""
                            + myHtmlAttribute.GetCompiledVariableData(serverSideTokens)
                            + "\"";
                    }
                }
            }
            
            return compiledAtributes;
        }
    }
}

```

### File: Compiler\HTML\Events.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    class Events
    {
        static List<String> _props = new()
        {
            // Window Events
            "onafterprint",
            "onbeforeprint",
            "onbeforeunload",
            "onerror",
            "onhashchange",
            "onload",
            "onmessage",
            "onoffline",
            "ononline",
            "onpagehide",
            "onpageshow",
            "onpopstate",
            "onresize",
            "onstorage",
            "onunload",

            // Form Events
            "onblur",
            "onchange",
            "oncontextmenu",
            "onfocus",
            "oninput",
            "oninvalid",
            "onreset",
            "onsearch",
            "onselect",
            "onsubmit",

            // Keyboard Events
            "onkeydown",
            "onkeypress",
            "onkeyup",

            // Mouse Events
            "onclick",
            "ondblclick",
            "onmousedown",
            "onmousemove",
            "onmouseout",
            "onmouseover",
            "onmouseup",
            "onmousewheel",
            "onwheel",

            // Drag Events
            "ondrag",
            "ondragend",
            "ondragenter",
            "ondragleave",
            "ondragover",
            "ondragstart",
            "ondrop",
            "onscroll",

            // Clipboard Events
            "oncopy",
            "oncut",
            "onpaste",

            // Media Events
            "onabort",
            "oncanplay",
            "oncanplaythrough",
            "oncuechange",
            "ondurationchange",
            "onemptied",
            "onended",
            "onerror",
            "onloadeddata",
            "onloadedmetadata",
            "onloadstart",
            "onpause",
            "onplay",
            "onplaying",
            "onprogress",
            "onratechange",
            "onseeked",
            "onseeking",
            "onstalled",
            "onsuspend",
            "ontimeupdate",
            "onvolumechange",
            "onwaiting",

            // Misc Events
            "ontoggle"
        };

        public static string GetHtmlEvents(List<Token> serverSideTokens, Token myToken)
        {
            string compiledEvents = " ";

            TokenDataContainer viewContainer = (TokenDataContainer)myToken.Data;
            foreach (String name in _props)
            {
                TokenDataVariable myJsEventVar = TokenUtils.FindTokenDataVariableByName(
                    viewContainer.ContainerData,
                    name
                );

                if (myJsEventVar != null)
                {
                    Token myJsEvent = TokenUtils.FindTokenByName(
                        myToken.MyTokenizer.FileTokens,
                        myJsEventVar.GetCompiledVariableData(serverSideTokens)
                    );

                    if (myJsEvent != null)
                    {
                        compiledEvents +=
                            name + "=\"" + JS.Event.GetEventString(serverSideTokens,myJsEvent) + "\" ";
                    }
                }
            }

            return compiledEvents;
        }
    }
}

```

### File: Compiler\HTML\View.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    /// <summary>
    /// Class for handling views in BuckshotPlusPlus HTML compiler.
    /// </summary>
    public class View
    {
        /// <summary>
        /// Compiles an HTML view based on server-side tokens and a view token.
        /// </summary>
        /// <param name="serverSideTokens">List of server-side tokens.</param>
        /// <param name="myViewToken">The view token to compile.</param>
        /// <returns>The compiled HTML string.</returns>
        public static string CompileView(List<Token> serverSideTokens, Token myViewToken)
        {
            TokenUtils.EditAllTokensOfContainer(serverSideTokens, myViewToken);

            if (myViewToken.Data is not TokenDataContainer myContainer)
            {
                Formater.TokenCriticalError("Invalid view token!", myViewToken);
                return "";
            }

            TokenDataVariable viewTypeToken = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "type");

            string viewType = viewTypeToken?.GetCompiledVariableData(serverSideTokens) ?? throw new InvalidOperationException("Missing view type!");

            TokenDataVariable viewContent = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "content");

            string html = $"<{viewType}";
            string htmlAttributes = Attributes.GetHtmlAttributes(serverSideTokens, myViewToken);

            if (!string.IsNullOrEmpty(htmlAttributes))
            {
                html += $" {htmlAttributes}";
            }

            string htmlEvents = Events.GetHtmlEvents(serverSideTokens, myViewToken);
            if (!string.IsNullOrEmpty(htmlEvents))
            {
                html += $" {htmlEvents}";
            }

            string style = CSS.Properties.GetCssString(serverSideTokens, myViewToken);
            html += !string.IsNullOrEmpty(style) ? $" style=\"{style}\">" : ">";

            html += CompileContent(serverSideTokens, viewContent, myContainer);

            return html + $"</{viewType}>";
        }

        /// <summary>
        /// Compiles the content of an HTML view.
        /// </summary>
        /// <param name="serverSideTokens">List of server-side tokens.</param>
        /// <param name="viewContent">The content token data.</param>
        /// <param name="myContainer">The containing token data.</param>
        /// <returns>The compiled content string.</returns>
        public static string CompileContent(List<Token> serverSideTokens, TokenDataVariable viewContent, TokenDataContainer myContainer)
        {
            if (viewContent == null)
            {
                return "";
            }

            switch (viewContent.VariableType)
            {
                case "string":
                case "multiple":
                    return viewContent.GetCompiledVariableData(serverSideTokens);

                case "ref":
                    Token foundToken = TokenUtils.FindTokenByName(serverSideTokens, viewContent.GetCompiledVariableData(serverSideTokens));

                    if (foundToken.Data is TokenDataContainer)
                    {
                        return CompileView(serverSideTokens, foundToken);
                    }

                    return viewContent.GetCompiledVariableData(serverSideTokens, true);

                case "array":
                    string result = "";
                    foreach (Token childViewToken in Analyzer.Array.GetArrayValues(viewContent.VariableToken))
                    {
                        TokenDataVariable childView = (TokenDataVariable)childViewToken.Data;
                        result += CompileView(serverSideTokens, TokenUtils.FindTokenByName(serverSideTokens, childView.GetCompiledVariableData(serverSideTokens)));
                    }
                    return result;

                default:
                    return "";
            }
        }
    }
}

```

### File: Compiler\JS\Event.cs

```csharp
﻿using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.JS
{
    class Event
    {
        public static string GetEventString(List<Token> serverSideTokens, Token myJsEventToken)
        {
            TokenDataContainer myJsEvent = (TokenDataContainer)myJsEventToken.Data;

            string eventString = "";

            int tokenId = 0;
            foreach (Token childToken in myJsEvent.ContainerData)
            {
                if (childToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable childVar = (TokenDataVariable)childToken.Data;
                    if (CSS.Properties.IsCssProp(childToken))
                    {
                        eventString +=
                            "this.style."
                            + CSS.Properties.ToDomProp(childVar.VariableName)
                            + " = '"
                            + childVar.GetCompiledVariableData(serverSideTokens)
                            + "';";
                    }
                    else if (childVar.VariableName == "content")
                    {
                        eventString += "this.textContent = '" + childVar.GetCompiledVariableData(serverSideTokens) + "';";
                    }
                    else
                    {
                        eventString += Variables.GetVarString(serverSideTokens,myJsEvent.ContainerData, tokenId) + ";";
                    }
                }
                else
                {
                    eventString += childToken.LineData.Replace("\"", "'") + ";";
                }
                
                tokenId++;
            }

            return eventString;
        }
    }
}

```

### File: Compiler\JS\Variables.cs

```csharp
﻿using System.Collections.Generic;

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

```

### File: Databases\BaseDatabase.cs

```csharp
﻿using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public abstract class BaseDatabase
    {

        public Dictionary<string, string> DatabaseParameters { get; set; }
        public Tokenizer MyTokenizer { get; set; }

        public BaseDatabase(Dictionary<string, string> parameters, Tokenizer myTokenizer) { 
            DatabaseParameters = parameters;
            this.MyTokenizer = myTokenizer;
        }

        public abstract Token Query(string query);
    }
}

```

### File: Databases\MySqlDatabase.cs

```csharp
﻿using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Databases
{
    public class MySqlDatabase : BaseDatabase
    {
        private MySqlConnection _connection;

        public MySqlDatabase(Dictionary<string, string> parameters , Tokenizer myTokenizer) : base(parameters, myTokenizer)
        {
            string connectionString = $"Server={DatabaseParameters["Server"]};Database={DatabaseParameters["Database"]};User ID={DatabaseParameters["UserId"]};Password={DatabaseParameters["Password"]};";
            _connection = new MySqlConnection(connectionString);
        }

        public override Token Query(string query)
        {
            string tokenLineData = "data{\n";

            // Open the connection
            _connection.Open();

            // Create a command object
            MySqlCommand cmd = new MySqlCommand(query, _connection);

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
            _connection.Close();

            return new Token("mysql_auto_generated", tokenLineData, 0, this.MyTokenizer); ;
        }
    }
}

```

### File: Env.cs

```csharp
namespace BuckshotPlusPlus
{
    using System;
    using System.IO;

    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(
                    '=',
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}

```

### File: Formater.cs

```csharp
﻿using System;
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
            bool isQuote = false;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '"')
                {
                    isQuote = !isQuote;
                }
                if (isQuote == false && value[i] == c)
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
            List<string> splitedString = new List<string>();

            string[] unsafeChars = { "\"\"", "()" };

            if (onlyStrings)
            {
                unsafeChars[1] = "\"\"";
            }

            UnsafeCharStruct lastUnsafeChar = new UnsafeCharStruct();
            lastUnsafeChar.IsUnsafeChar = false;

            int count = 0;
            int lastPos = 0;

            for (int i = 0; i < value.Length; i++)
            {
                count++;
                if (lastUnsafeChar.IsUnsafeChar)
                {
                    UnsafeCharStruct currentUnsafeChar = IsUnsafeChar(
                        unsafeChars,
                        value[i]
                    );
                    if (currentUnsafeChar.IsUnsafeChar)
                    {
                        if (
                            (
                                currentUnsafeChar.IsFirstChar == false
                                || currentUnsafeChar.UnsafeCharId == 0
                            )
                            && currentUnsafeChar.UnsafeCharId == lastUnsafeChar.UnsafeCharId
                        )
                        {
                            lastUnsafeChar.IsUnsafeChar = false;
                        }
                    }
                }
                else
                {
                    UnsafeCharStruct currentUnsafeChar = IsUnsafeChar(
                        unsafeChars,
                        value[i]
                    );
                    if (currentUnsafeChar.IsUnsafeChar)
                    {
                        lastUnsafeChar = currentUnsafeChar;
                    }
                    else
                    {
                        if (value[i] == c)
                        {
                            splitedString.Add(value.Substring(i + 1 - count, count - 1));
                            lastPos = i + 1;
                            count = 0;
                        }
                    }
                }
            }
            splitedString.Add(value.Substring(lastPos, value.Length - lastPos));

            return splitedString;
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

```

### File: Logic\LogicTest.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public class LogicTest
    {
        public static string[] LogicTestsTypes = { "==", "!=" };
        public string LogicTestType {  get; set; }
        public string LeftValue { get; set; }
        public string LeftValueType { get; set; }
        public string RightValue { get; set; }
        public string RightValueType { get; set; }

        public LogicTest(string logicTestString, Token myToken) {
            LogicTestType = FindLogicTestType(logicTestString);
            if(LogicTestType == null)
            {
                Formater.TokenCriticalError("Not valid test found for logic test : " + logicTestString, myToken);
            }
            string[] values = logicTestString.Split(LogicTestType);
            LeftValue = values[0];
            LeftValueType = TokenDataVariable.FindVariableType(LeftValue, myToken);
            
            RightValue = values[1];
            RightValueType = TokenDataVariable.FindVariableType(RightValue, myToken);

        }

        public bool RunLogicTest(List<Token> tokenList, Token myToken)
        {
            if(LeftValueType == "ref")
            {
                Token foundToken = TokenUtils.FindTokenByName(tokenList, LeftValue);
                if(foundToken != null)
                {
                    TokenDataVariable foundVar = (TokenDataVariable)foundToken.Data;
                    LeftValue = foundVar.VariableData;
                    LeftValueType = foundVar.VariableType;
                    if(LeftValueType == "string")
                    {
                        LeftValue = '"' + LeftValue + "\"";
                    }
                }
            }
            if(RightValueType == "ref")
            {
                Token foundToken = TokenUtils.FindTokenByName(tokenList, RightValue);
                if(foundToken != null)
                {
                    TokenDataVariable foundVar = (TokenDataVariable)foundToken.Data;
                    RightValue = foundVar.VariableData;
                    RightValueType = foundVar.VariableType;
                }
            }

            if(LeftValueType == RightValueType)
            {
                switch(LogicTestType)
                {
                    case "==":
                        if(LeftValue == RightValue)
                        {
                            return true;
                        }
                        return false;
                    case "!=":
                        if (LeftValue != RightValue)
                        {
                            return true;
                        }
                        return false;
                }
                Formater.TokenCriticalError("Test type '" + LogicTestType + "' not recognized.", myToken);
                return false;
            }
            else
            {
                Formater.TokenCriticalError("Data type mismatch for logical test", myToken);
                return false;
            }
        }

        public static string FindLogicTestType(string logicTestString) {

            string result = null;

            foreach (string localLogicTestType in LogicTestsTypes)
            {
                List<string> testSides = Formater.SafeSplit(logicTestString, localLogicTestType[0]);

                if(testSides.Count > 1)
                {
                    if(testSides.Count > 2 && localLogicTestType == "==")
                    {
                        return "==";
                    }
                    else
                    {
                        if (localLogicTestType.Length > 1)
                        {

                            if (testSides[1][0] == localLogicTestType[1])
                            {
                                return localLogicTestType;
                            }
                        }
                        else
                        {
                            return localLogicTestType;
                        }
                    }
                    
                }
            }

            return result;
        }
    }
}

```

### File: Program.cs

```csharp
﻿using BuckshotPlusPlus.WebServer;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BuckshotPlusPlus
{
    internal class Program
    {
        public static Tokenizer CompileMainFile(string filePath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Tokenizer myTokenizer = new Tokenizer(filePath);

            stopwatch.Stop();
            Formater.SuccessMessage($"Successfully compiled in {stopwatch.ElapsedMilliseconds} ms");
            return myTokenizer;
        }

        public static void DeleteDirectory(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }

        public static void ExportWebsite(string filePath, string exportDirectory)
        {
            // For now export directory is absolute only
            Tokenizer myTokenizer = CompileMainFile(filePath);

            if (Path.Exists(exportDirectory))
            {
                DeleteDirectory(exportDirectory);
            }

            Directory.CreateDirectory(exportDirectory);

            foreach (Token pageToken in myTokenizer.PagesTokens)
            {
                TokenDataContainer myPageData = (TokenDataContainer)pageToken.Data;

                var icon = TokenUtils.FindTokenByName(myPageData.ContainerData, "icon");
                if (icon != null)
                {
                    var data = icon.Data;
                    var fileName = ((data as TokenDataVariable)!).VariableData;
                    string icoPath = Path.Combine(filePath, @"..\" + fileName);
                    File.WriteAllBytes(exportDirectory + "/" + fileName, File.ReadAllBytes(icoPath));
                }

                Formater.DebugMessage("Starting to export page " + myPageData.ContainerName + "...");
                File.WriteAllText(exportDirectory + "/" + myPageData.ContainerName + ".html", Page.RenderWebPage(myTokenizer.FileTokens, pageToken));
                Formater.SuccessMessage("Successfully exported page " + myPageData.ContainerName + ".html");
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome on BuckShotPlusPlus!");
            
            if (args.Length == 0)
            {
                Formater.CriticalError("To display all commands: -h");
            }

            string filePath = args[0];

            if(filePath == "export")
            {
                if(args.Length == 3)
                {
                    ExportWebsite(args[1], args[2]);
                }
                else
                {
                    Formater.CriticalError("You need the following arguments to export your bpp website:\n" +
                        "\t- export\n" +
                        "\t- path/to/your/main.bpp\n" +
                        "\t- path/to/your/export/directory");
                }
            }
            else
            {
                Tokenizer myTokenizer = Program.CompileMainFile(filePath);
            
                var dotenv = Path.Combine(myTokenizer.RelativePath, ".env");
                DotEnv.Load(dotenv);

                WebServer.WebServer myWebServer = new WebServer.WebServer {};
                myWebServer.Start(myTokenizer);
            }
        }
    }
}

```

### File: Security\Keys.cs

```csharp
﻿using System;
using System.Linq;

namespace BuckshotPlusPlus.Security
{
    public class Keys
    {
        public static string CreateRandomKey()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public static string CreateRandomUniqueKey()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            return Convert.ToBase64String(time.Concat(key).ToArray());
        }
    }
}

```

### File: Security\UserSession.cs

```csharp
﻿using System;
using System.Collections.Generic;
using BuckshotPlusPlus.Analytics;

namespace BuckshotPlusPlus.Security
{
    public class UserSession
    {
        public string SessionId { get; set; }
        public string SessionLang { get; set; }
        public string SessionPlatform { get; set; }
        public string SessionIp { get; set; }
        public long SessionStarted { get; set; }
        public List<AnalyticTimedEvent> UrlHistory { get; set; }
        public DateTime LastUserInteraction { get; set; }

        public UserSession(Dictionary<string, string> requestHeaders)
        {
            SessionId = Keys.CreateRandomUniqueKey();
            UrlHistory = new List<AnalyticTimedEvent>();
            if (requestHeaders.ContainsKey("platform"))
            {
                SessionPlatform = requestHeaders["platform"];
            }
            else
            {
                SessionPlatform = "unknown";
            }

            if (requestHeaders.ContainsKey("lang"))
            {
                SessionLang = requestHeaders["lang"];
            }
            else
            {
                SessionLang = "unknown";
            }

            if (requestHeaders.ContainsKey("ip"))
            {
                SessionIp = requestHeaders["ip"];
            }
            else
            {
                SessionIp = "unknown";
            }

            SessionStarted = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            LastUserInteraction = DateTime.Now;
        }

        public string GetUserSessionLineData()
        {
            return "data session{\n" +
                   "ip = \"" + SessionIp + "\"\n" +
                   "id = \"" + SessionId + "\"\n" +
                   "lang = \"" + SessionLang + "\"\n" +
                   "platform = \"" + SessionPlatform + "\"\n" +
                   "start = \"" + SessionStarted.ToString() + "\"\n" +
                   "url_visited_num = \"" + UrlHistory.Count + "\"\n" +
                   "}\n";
        }

        public Token GetToken(Tokenizer myTokenizer)
        {
            return new Token("", GetUserSessionLineData(), 0, myTokenizer);
        }

        public void AddUrl(string url)
        {
            UrlHistory.Add(new AnalyticTimedEvent(url));
            LastUserInteraction = DateTime.Now;
        }
    }
}

```

### File: Security\UserSessionManager.cs

```csharp
﻿using System;
using System.Collections.Generic;
using System.Net;

namespace BuckshotPlusPlus.Security
{
    public class UserSessionManager
    {
        public Dictionary<string, UserSession> ActiveUsers { get; set; }

        public UserSessionManager()
        {
            ActiveUsers = new Dictionary<string, UserSession>();
        }

        public UserSession AddOrUpdateUserSession(HttpListenerRequest req, HttpListenerResponse response)
        {
            bool sessionCookieFound = false;
            string userSessionId = null;

            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            requestHeaders.Add("ip", req.RemoteEndPoint.ToString());

            System.Collections.Specialized.NameValueCollection headers = req.Headers;
            // Get each header and display each value.
            foreach (string key in headers.AllKeys)
            {
                string[] values = headers.GetValues(key);
                if (values.Length > 0)
                {
                    if(key == "sec-ch-ua-platform")
                    {
                        requestHeaders.Add("platform", values[0]);
                    }else if(key == "Accept-Language")
                    {
                        requestHeaders.Add("lang", values[0]);
                    }
                }
            }

            foreach (Cookie cook in req.Cookies)
            {
                if (cook.Name == "bpp_session_id")
                {
                    sessionCookieFound = true;
                    userSessionId = cook.Value;
                }

                /*Console.WriteLine("Cookie:");
                Console.WriteLine("{0} = {1}", cook.Name, cook.Value);
                Console.WriteLine("Domain: {0}", cook.Domain);
                Console.WriteLine("Path: {0}", cook.Path);
                Console.WriteLine("Port: {0}", cook.Port);
                Console.WriteLine("Secure: {0}", cook.Secure);

                Console.WriteLine("When issued: {0}", cook.TimeStamp);
                Console.WriteLine("Expires: {0} (expired? {1})",
                    cook.Expires, cook.Expired);
                Console.WriteLine("Don't save: {0}", cook.Discard);
                Console.WriteLine("Comment: {0}", cook.Comment);
                Console.WriteLine("Uri for comments: {0}", cook.CommentUri);
                Console.WriteLine("Version: RFC {0}", cook.Version == 1 ? "2109" : "2965");

                // Show the string representation of the cookie.
                Console.WriteLine("String: {0}", cook.ToString());*/
            }

            if (sessionCookieFound)
            {
                UserSession session;
                if (ActiveUsers.TryGetValue(userSessionId, out session))
                {
                    return session;
                }

                return CreateNewUserSession(requestHeaders, response);
            }

            return CreateNewUserSession(requestHeaders, response);
        }

        public UserSession CreateNewUserSession(Dictionary<string, string> requestHeaders, HttpListenerResponse response)
        {
            UserSession newUserSession = new UserSession(requestHeaders);
            ActiveUsers.Add(newUserSession.SessionId, newUserSession);

            Cookie sessionIdCookie = new Cookie("bpp_session_id", newUserSession.SessionId);
            response.SetCookie(sessionIdCookie);

            return newUserSession;
        }

        public void RemoveInactiveUserSessions()
        {
            DateTime now = DateTime.Now;
            foreach (KeyValuePair<string, UserSession> user in ActiveUsers)
            {
                if ((now - user.Value.LastUserInteraction).TotalSeconds > 10)
                {
                    ActiveUsers.Remove(user.Key);
                }
            }
        }
    }
}

```

### File: Tokenizer\Token.cs

```csharp
﻿using System;

namespace BuckshotPlusPlus
{
    public class Token
    {
        public string Type { get; set; }
        public TokenData Data { get; set; }

        public TokenDataContainer Parent { get; set; }

        public string FileName { get; set; }
        public string LineData { get; set; }
        public int LineNumber { get; set; }
        public Tokenizer MyTokenizer { get; set; }

        public Token NextToken { get; set; }
        public Token PreviousToken { get; set; }

        public Token(
            string fileName,
            string lineData,
            int lineNumber,
            Tokenizer myTokenizer,
            TokenDataContainer parent = null,
            Token previousToken = null
        )
        {
            this.FileName = fileName;
            this.LineData = lineData;
            this.LineNumber = lineNumber;
            this.MyTokenizer = myTokenizer;
            this.Parent = parent;
            this.PreviousToken = previousToken;

            // If Line Contains "=" load data of a variable
            if (TokenDataContainer.IsTokenDataContainer(this))
            {
                Data = new TokenDataContainer(this);
            }
            else if (TokenDataFunctionCall.IsTokenDataFunctionCall(this))
            {
                Data = new TokenDataFunctionCall(this);
            }
            else if (TokenDataVariable.IsTokenDataVariable(this))
            {
                Data = new TokenDataVariable(this);
            }
            else
            {
                Formater.TokenCriticalError("Unkown instruction", this);
            }
        }
    }
}

```

### File: Tokenizer\TokenDataContainer.cs

```csharp
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BuckshotPlusPlus
{
    public class TokenDataContainer : TokenData
    {
        public string ContainerName { get; set; }
        public List<Token> ContainerData { get; set; }
        public List<string> ContainerLines { get; set; }
        public string ContainerType { get; set; }
        public TokenData ContainerMetaData { get; set; }
        public Token ContainerToken { get; set; }

        public static string[] SupportedContainerTypes =
        {
            "data",
            "request",
            "database",
            "object",
            "meta",
            "function",
            "view",
            "server",
            "route",
            "page",
            "event",
            "table",
            "if",
            "else",
            "elseif"
        };

        public TokenDataContainer(Token myToken)
        {
            List<string> linesData = Formater.SafeSplit(myToken.LineData, '\n');
            this.ContainerData = new List<Token>();
            this.ContainerType = "";
            this.ContainerName = "";
            this.ContainerToken = myToken;
            this.ContainerLines = new List<string> { };

            int openCount = 0;
            List<string> childContainerLines = new List<string>();

            foreach (string lineData in linesData)
            {
                if (Formater.SafeContains(lineData, '{'))
                {
                    openCount++;

                    if (openCount == 1)
                    {
                        // SPLIT FIRST LINE INTO AN ARRAY
                        List<string> myArgs = Formater.SafeSplit(lineData, ' ');

                        // STORE CONTAINER NAME
                        if (myArgs[1][^1] == '{')
                        {
                            this.ContainerName = myArgs[1].Substring(0, myArgs[1].Length - 1);
                        }
                        else
                        {
                            this.ContainerName = myArgs[1];
                        }

                        // CHECK AND STORE CONTAINER TYPE (OBJECT, FUNCTION)
                        
                        foreach (string containerType in SupportedContainerTypes)
                        {
                            if (myArgs[0] == containerType)
                            {
                                if (containerType == "if" || containerType == "else")
                                {
                                    this.ContainerType = "logic";
                                    myToken.Type = this.ContainerType;
                                    ContainerMetaData = new TokenDataLogic(myToken);
                                    
                                }
                                else
                                {
                                    this.ContainerType = containerType;
                                    myToken.Type = this.ContainerType;
                                }
                                
                            }
                        }
                        if (this.ContainerType == "")
                        {
                            Formater.TokenCriticalError("Invalid container type", myToken);
                        }

                        if (this.ContainerName.Contains(':'))
                        {
                            string[] splitedName = this.ContainerName.Split(':');
                            this.ContainerName = splitedName[0];

                            string parentName = splitedName[1];
                            bool parentFound = false;

                            foreach (Token localToken in myToken.MyTokenizer.FileTokens)
                            {
                                if (localToken.Data is TokenDataContainer localTokenDataContainer)
                                {
                                    if (localTokenDataContainer.ContainerName == parentName)
                                    {
                                        if (
                                            localTokenDataContainer.ContainerType
                                            != this.ContainerType
                                        )
                                        {
                                            Formater.TokenCriticalError(
                                                "Invalid parent container type",
                                                myToken
                                            );
                                        }
                                        foreach (
                                            Token localTokenData in localTokenDataContainer.ContainerData
                                        )
                                        {
                                            ContainerData.Add(localTokenData);
                                        }
                                        parentFound = true;
                                    }
                                }
                            }

                            if (!parentFound)
                            {
                                Formater.CriticalError("View " + parentName + " not found!");
                            }
                        }
                    }
                }
                else if (openCount == 1 && !Formater.SafeContains(lineData, '}'))
                {
                    ContainerLines.Add(lineData);
                }

                if (openCount == 2)
                {
                    ContainerLines.Add(lineData);
                }

                if (Formater.SafeContains(lineData, '}') && openCount == 2)
                {
                    openCount--;
                    ContainerLines.Add(lineData);

                }
                else if (Formater.SafeContains(lineData, '}'))
                {
                    openCount--;
                }
            }

            int currentLineNumber = 0;
            while (currentLineNumber < ContainerLines.Count)
            {
                ProcessedLine currentLine = Tokenizer.ProcessLineData(new UnprocessedLine(ContainerLines, currentLineNumber));
                currentLineNumber = currentLine.CurrentLine;

                switch (currentLine.LineType)
                {
                    case LineType.Include:
                        {
                            // Manage includes inside of containers
                            break;
                        }
                    case LineType.Container:
                        {
                            Token previousToken = null;
                            if (ContainerData.Count > 0)
                            {
                                previousToken = ContainerData.Last();
                            }
                            Token newContainerToken = new Token(
                                    myToken.FileName,
                                    String.Join('\n', currentLine.ContainerData),
                                    currentLineNumber,
                                    myToken.MyTokenizer,
                                    null,
                                    previousToken
                                );

                            
                            AddChildToContainerData(ContainerData, newContainerToken);
                            break;
                        }
                    case LineType.Variable:
                        {
                            Token myNewToken = new Token(
                                myToken.FileName,
                                currentLine.LineData,
                                myToken.LineNumber + linesData.IndexOf(currentLine.LineData) - 1,
                                myToken.MyTokenizer,
                                this
                            );
                            AddChildToContainerData(ContainerData, myNewToken);
                            break;
                        }
                    case LineType.Empty:
                        break;
                    case LineType.Comment:
                        {
                            break;
                        }
                }
            }
        }

        public static void AddChildToContainerData(List<Token> containerData, Token newChild)
        {
            if(!Formater.SafeContains(TokenUtils.GetTokenName(newChild), '.'))
            {
                Token foundToken = TokenUtils.FindTokenByName(
                    containerData,
                    TokenUtils.GetTokenName(newChild)
                );
                
                if (foundToken != null)
                {
                    containerData.Remove(foundToken);
                }
            }

            containerData.Add(newChild);
        }

        public static bool IsTokenDataContainer(Token myToken)
        {
            string localType = Formater.SafeSplit(myToken.LineData, ' ')[0];
            
            foreach (string type in SupportedContainerTypes)
            {
                if (localType == type)
                {
                    bool containsContainerSymbol = Formater.SafeContains(myToken.LineData, '{');
                    if (containsContainerSymbol)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
            }

            return false;
        }
    }
}

```

### File: Tokenizer\TokenDataFunctionCall.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    class TokenDataFunctionCall : TokenData
    {
        public string FuncName { get; set; }
        public List<Token> FuncArgs { get; set; }

        public TokenDataFunctionCall(Token myToken)
        {
            myToken.Type = "function_call";

            this.FuncName = GetFunctionCallName(myToken.LineData, myToken);
            this.FuncArgs = GetFunctionArgs(myToken.LineData, myToken);

            Console.WriteLine(
                "I found a function call of name : "
                    + this.FuncName
                    + " and "
                    + this.FuncArgs.Count
                    + " args"
            );
        }
        

        public static bool IsTokenDataFunctionCall(Token myToken)
        {
            return Formater.SafeContains(myToken.LineData, '(');
        }

        public static string GetFunctionCallName(string value, Token myToken)
        {
            string funName = "";
            foreach (char c in value)
            {
                if (c != '(')
                {
                    funName += c;
                }
                else
                {
                    return funName;
                }
            }
            Formater.TokenCriticalError("Invalid function name", myToken);
            return "";
        }

        public static List<Token> GetFunctionArgs(string value, Token myToken)
        {
            List<Token> functionArgs = new List<Token>();
            string currentVar = "";
            bool isArgs = false;
            int subPar = 0;

            foreach (char c in value)
            {
                if (c == '(')
                {
                    isArgs = true;
                    subPar++;
                }
                else if (c == ')')
                {
                    subPar--;
                    if (subPar == 0)
                    {
                        Token myNewToken = new Token(
                            myToken.FileName,
                            currentVar,
                            myToken.LineNumber,
                            myToken.MyTokenizer
                        );
                        new TokenDataVariable(myNewToken);
                        functionArgs.Add(myNewToken);
                        return functionArgs;
                    }
                }
                else
                {
                    if (isArgs && c == ',')
                    {
                        Token myNewToken = new Token(
                            myToken.FileName,
                            currentVar,
                            myToken.LineNumber,
                            myToken.MyTokenizer
                        );
                        new TokenDataVariable(myNewToken);
                        functionArgs.Add(myNewToken);
                        currentVar = "";
                    }
                    else if (isArgs)
                    {
                        currentVar += c;
                    }
                }
            }
            Formater.TokenCriticalError("Invalid function args", myToken);
            return functionArgs;
        }
    }
}

```

### File: Tokenizer\TokenDataLogic.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public class TokenDataLogic : TokenData
    {
        public static string[] LogicTokens = { "if", "else" };

        public string LogicType { get; set; }
        public LogicTest TokenLogicTest { get; set; }
        public TokenDataLogic NextLogicToken { get; set; }
        private Token ParentToken { get; set; }

        public bool LastLogicTestResult { get; set; }

        public TokenDataLogic(Token myToken)
        {
            ParentToken = myToken;
            myToken.Type = "logic";
            LogicType = FindLogicTokenType(myToken);
            if(LogicType == "if")
            {
                string testString = Formater.SafeRemoveSpacesFromString(GetLogicTestString(myToken));
                TokenLogicTest = new LogicTest(testString, myToken);
            }else if(LogicType == "else")
            {
                if(myToken.PreviousToken != null)
                {
                    myToken.PreviousToken.NextToken = myToken;
                }
                
            }
        }

        public static bool IsTokenDataLogic(Token myToken)
        {
            string logicTokenType = FindLogicTokenType(myToken);
            if(logicTokenType == "invalid") {
                return false;
            }
            return true;
        }

        public static string FindLogicTokenType(Token myToken)
        {
            foreach (string tokenType in LogicTokens)
            {
                if (myToken.LineData.Length > tokenType.Length)
                {
                    if (myToken.LineData.StartsWith(tokenType))
                    {
                        return tokenType;
                    }
                }

            }
            return "invalid";
        }

        public static string GetLogicTestString(Token myToken)
        {
            return Formater.SafeSplit(Formater.SafeSplit(myToken.LineData, '(', true)[1], ')', true)[0];
        }

        private void OnLogicTestSuccess(List<Token> tokenList)
        {
            TokenDataContainer parentTokenDataContainer = (TokenDataContainer)ParentToken.Data;
            foreach (Token localToken in parentTokenDataContainer.ContainerData)
            {
                if (localToken.Type == "edit")
                {
                    TokenUtils.EditTokenData(tokenList, localToken);
                }

            }
            LastLogicTestResult = true;
        }

        public bool RunLogicTest(List<Token> tokenList)
        {
            if(LogicType == "if")
            {
                if (TokenLogicTest.RunLogicTest(tokenList, ParentToken))
                {
                    OnLogicTestSuccess(tokenList);
                }
                else
                {
                    LastLogicTestResult = false;
                }
                
            }else if(LogicType == "else")
            {
                Token previousToken = ParentToken.PreviousToken;
                if(previousToken.Type == "logic")
                {
                    TokenDataContainer previousTokenDataContainer = (TokenDataContainer)previousToken.Data;
                    TokenDataLogic previousLogic = (TokenDataLogic)previousTokenDataContainer.ContainerMetaData;
                    if(previousLogic.LastLogicTestResult == false)
                    {
                        OnLogicTestSuccess(tokenList);
                    }
                    else
                    {
                        LastLogicTestResult = false;
                    }
                }else { LastLogicTestResult = false; }
            }
            
            return LastLogicTestResult;
        }
    }
}

```

### File: Tokenizer\TokenDataVariable.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public class TokenDataVariable : TokenData
    {
        public string VariableType { get; set; }
        public string VariableData { get; set; }
        public string VariableName { get; set; }
        public Token RefData { get; set; }
        public Token VariableToken { get; set; }

        public TokenDataVariable(Token myToken)
        {
            VariableToken = myToken;
            myToken.Type = "variable";
            string[] myVariableParams = Formater.SafeSplit(myToken.LineData, ' ').ToArray();
            //Console.WriteLine(MyVariableParams.Length);
            // check if all parameters of a vriables are present

            if (Formater.SafeContains(myToken.LineData, '+'))
            {
                this.VariableName = myVariableParams[0];
                this.VariableData = myVariableParams[2];
                this.VariableType = "multiple";
            }
            else if (myVariableParams.Length == 3)
            {
                this.VariableType = FindVariableType(myVariableParams[2], myToken);
                this.VariableName = myVariableParams[0];
                this.VariableData = myVariableParams[2];

                string[] variablePath = VariableName.Split('.');
                if(variablePath.Length > 1)
                {
                    myToken.Type = "edit";
                }
            }
            else if (myVariableParams.Length == 1)
            {
                this.VariableName = "";
                this.VariableData = myVariableParams[0];
                this.VariableType = FindVariableType(myVariableParams[0], myToken);
            }
            else if (myVariableParams.Length == 4)
            {
                this.VariableType = FindVariableType(myVariableParams[2], myToken);
                this.VariableName = myVariableParams[0];
                this.VariableData = myVariableParams[2];
            }
            else
            {
                Formater.TokenCriticalError("Invalid variable init ", myToken);
            }

            if (this.VariableType == "")
            {
                Formater.TokenCriticalError("Unknown variable type ", myToken);
            }

            if (this.VariableType == "string")
            {
                this.VariableData = GetValueFromString(this.VariableData, myToken);
            }

            //Console.WriteLine("I found a variable of type " + this.VariableType + " and name : " + this.VariableName + " Value : " + this.VariableData);

            if (this.VariableType == "ref")
            {
                this.RefData = TokenUtils.FindTokenByName(
                    myToken.MyTokenizer.FileTokens,
                    this.VariableData
                );
                if (RefData == null)
                {
                    if (myToken.Parent != null)
                    {
                        this.RefData = TokenUtils.FindTokenByName(
                            myToken.Parent.ContainerData,
                            this.VariableData
                        );
                    }
                }
            }
        }

        public static string GetValueFromString(string initialValue, Token myToken)
        {
            if (
                    initialValue[0] != '"'
                )
            {
                Formater.TokenCriticalError("Invalid string value", myToken);
            }
            return initialValue.Substring(1, initialValue.Length - 2);
        }

        public static string FindVariableType(string value, Token myToken)
        {
            int variableIntData = 0;
            float variableFloatData = 0;
            bool variableBoolData = false;

            if (value[0] == '[' && value[^1] == ']')
            {
                return "array";
            }
            else if (value.Contains('"'))
            {
                return "string";
            }
            else if (int.TryParse(value, out variableIntData))
            {
                return "int";
            }
            else if (float.TryParse(value, out variableFloatData))
            {
                return "float";
            }
            else if (bool.TryParse(value, out variableBoolData))
            {
                return "bool";
            } //else if(TokenUtils.FindTokenByName(MyToken.MyTokenizer.FileTokens,Value) != null)
            else
            {
                return "ref";
            }
            //Formater.TokenCriticalError("Unknown variable type ", MyToken);
        }

        public static bool IsTokenDataVariable(Token myToken)
        {
            if (Formater.SafeContains(myToken.LineData, '='))
            {
                return true;
            }
            else if (FindVariableType(myToken.LineData, myToken) != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetCompiledVariableData(List<Token> fileTokens, bool compileRef = false)
        {
            if(this.VariableType == "multiple") {
                List<string> variables = Formater.SafeSplit(this.VariableData, '+');

                string result = "";

                foreach (string variable in variables)
                {
                    string safeVariableType = FindVariableType(variable, null);

                    if(safeVariableType == "string") {
                        result += GetValueFromString(variable, VariableToken);
                    }else if(safeVariableType == "ref") {
                        TokenDataVariable foundToken = TokenUtils.FindTokenDataVariableByName(fileTokens, variable);
                        if(foundToken != null)
                        {
                            result += foundToken.VariableData;
                        }
                        else
                        {
                            Formater.RuntimeError("Token not found!", this.VariableToken);
                        }
                        
                    }
                }
                return result;
            }else if(this.VariableType == "ref" && compileRef)
            {
                Console.WriteLine("Editing ref value for var " + this.VariableName);
                TokenDataVariable foundToken = TokenUtils.FindTokenDataVariableByName(fileTokens, this.VariableData);
                if (foundToken != null)
                {
                    return foundToken.VariableData;
                }
                else
                {
                    Formater.RuntimeError("Token not found!", this.VariableToken);
                }
            }

            return this.VariableData;
        } 
    }
}

```

### File: Tokenizer\Tokenizer.cs

```csharp
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using File = System.IO.File;

namespace BuckshotPlusPlus
{
    // TokenData is the main class for all tokens
    public abstract class TokenData { 
    }

    public enum LineType
    {
        Container,
        Comment,
        Variable,
        Include,
        Empty
    }

    public struct UnprocessedLine
    {
        public List<string> Lines;
        public int CurrentLine;

        public UnprocessedLine(List<string> lines, int lineNumber)
        {
            this.Lines = lines;
            this.CurrentLine = lineNumber;
        }
    }

    public struct ProcessedLine
    {
        public int CurrentLine;
        public LineType LineType;
        public string LineData;
        public List<string> ContainerData;

        public ProcessedLine(int lineNumber, LineType type, string data, List<string> containerData = null)
        {
            this.LineData = data;
            this.LineType = type;
            this.CurrentLine = lineNumber;
            this.ContainerData = containerData;
        }
    }

    public class Tokenizer
    {
        Dictionary<string, string> UnprocessedFileDataDictionary { get; set; }
        Dictionary<string, string> FileDataDictionary { get; set; }

        public List<Token> FileTokens { get; }
        public List <Token> PagesTokens { get; }

        public string RelativePath { get; }

        public Tokenizer(string filePath)
        {
            PagesTokens = new List<Token>();
            FileTokens = new List<Token>();
            UnprocessedFileDataDictionary = new Dictionary<string, string>();
            FileDataDictionary = new Dictionary<string, string>();
            RelativePath = Path.GetDirectoryName(filePath);

            IncludeFile(filePath);
        }

        public bool IsHttp(string filePath)
        {
            return filePath.Contains("http");
        }

        public string GetIncludeValue(string filePath)
        {
            if (Formater.SafeContains(filePath, '+'))
            {
                List<string> variables = Formater.SafeSplit(filePath, '+');

                string result = "";

                foreach (string variable in variables)
                {
                    string safeVariableType = TokenDataVariable.FindVariableType(variable, null);

                    if (safeVariableType == "string")
                    {
                        result += TokenDataVariable.GetValueFromString(variable, null);
                    }
                    else if (safeVariableType == "ref")
                    {
                        TokenDataVariable foundToken = TokenUtils.FindTokenDataVariableByName(FileTokens, variable);
                        if (foundToken != null)
                        {
                            result += foundToken.VariableData;
                        }
                        else
                        {
                            Formater.CriticalError("Token not found for include: " + filePath);
                        }

                    }
                }

                return '"' + result + '"';
            }

            return filePath;
        }

        public void AnalyzeFileData(string fileName, string fileData)
        {
            if (UnprocessedFileDataDictionary.ContainsKey(fileName))
            {
                Formater.CriticalError("Circular dependency detected of " + fileName);
            }
            else
            {
                UnprocessedFileDataDictionary.Add(fileName, fileData);
                FileDataDictionary.Add(fileName, fileData);

                int currentLineNumber = 0;

                List<string> myFileLines = fileData.Split('\n').OfType<string>().ToList();

                while (currentLineNumber < myFileLines.Count)
                {
                    ProcessedLine currentLine = ProcessLineData(new UnprocessedLine(myFileLines, currentLineNumber));
                    currentLineNumber = currentLine.CurrentLine;

                    switch (currentLine.LineType)
                    {
                            case LineType.Include:
                            {
                                string includePath = Formater.SafeSplit(currentLine.LineData, ' ')[1];
                                includePath = GetIncludeValue(includePath);

                                if (IsHttp(includePath))
                                {
                                    IncludeFile(
                                        includePath.Substring(
                                            1,
                                            includePath.Length - 2
                                        )
                                    );
                                }
                                else
                                {
                                    IncludeFile(
                                        Path.Combine(
                                            RelativePath,
                                            includePath.Substring(
                                                1,
                                                includePath.Length - 2
                                            )
                                        )
                                    );
                                }
                                break;
                            }
                            case LineType.Container:
                            {
                                AddContainerToken(fileName, currentLine.ContainerData, currentLineNumber);
                                break;
                            }
                            case LineType.Variable:
                            {
                                Token myNewToken = new Token(fileName, currentLine.LineData, currentLineNumber, this);

                                if (!TokenUtils.SafeEditTokenData(currentLine.LineData, FileTokens, myNewToken))
                                {
                                    FileTokens.Add(myNewToken);
                                }
                                break;
                            }
                        case LineType.Empty:
                            break;
                            case LineType.Comment: {
                                break;
                            }
                    }

                }
            }
        }

        public void AddContainerToken(string fileName, List<string> containerData, int currentLineNumber)
        {
            Token previousToken = null;
            if (FileTokens.Count > 0)
            {
                previousToken = FileTokens.Last();
            }
            Token newContainerToken = new Token(
                    fileName,
                    String.Join('\n', containerData),
                    currentLineNumber,
                    this,
                    null,
                    previousToken
                );

            TokenDataContainer newContainerTokenData = (TokenDataContainer)newContainerToken.Data;
            if (newContainerTokenData.ContainerType == "logic")
            {
                // RUN LOGIC TEST
                TokenDataLogic myLogic = (TokenDataLogic)newContainerTokenData.ContainerMetaData;
                myLogic.RunLogicTest(FileTokens);

            }
            if (((TokenDataContainer)newContainerToken.Data).ContainerType == "page")
            {
                PagesTokens.Add(newContainerToken);
            }
            FileTokens.Add(
                newContainerToken
            );
        }

        public static ProcessedLine ProcessLineData(UnprocessedLine uLine)
        {

            string lineData = uLine.Lines[uLine.CurrentLine];
            int currentLineNumber = uLine.CurrentLine;
            if (lineData.Length >= 2)
            {

                if (lineData.Length > 3)
                {
                    if (lineData[0] + "" + lineData[1] + lineData[2] == "###")
                    {
                        while (currentLineNumber < uLine.Lines.Count)
                        {
                            currentLineNumber++;
                            string nextLine = uLine.Lines[currentLineNumber];
                            if (nextLine.Length > 2)
                            {
                                if (nextLine[0] + "" + nextLine[1] + nextLine[2] == "###" || nextLine[^1] + "" + nextLine[^2] + nextLine[3] == "###")
                                {
                                    currentLineNumber++;
                                    break;
                                }
                            }

                        }
                        return new ProcessedLine(currentLineNumber + 1, LineType.Comment, lineData);
                    }
                }

                if (lineData[0] + "" + lineData[1] == "##")
                {
                    currentLineNumber++;
                    return new ProcessedLine(currentLineNumber + 1, LineType.Comment, lineData);
                }
            }
            if (lineData.Length > 1)
            {

                if (lineData[^1] == 13)
                {
                    lineData = lineData.Substring(0, lineData.Length - 1);
                }

                if (Formater.SafeSplit(lineData, ' ')[0] == "include")
                {
                    return new ProcessedLine(currentLineNumber + 1, LineType.Include, lineData);
                }
                else
                {
                    if (Formater.SafeContains(lineData, '{'))
                    {
                        List<string> myString = Formater.SafeSplit(lineData, ' ');

                        foreach (
                            string containerType in TokenDataContainer.SupportedContainerTypes
                        )
                        {
                            if (myString[0] == containerType)
                            {
                                int containerCount = 1;
                                List<string> containerData = new List<string>();
                                containerData.Add(lineData);

                                while (containerCount > 0)
                                {
                                    currentLineNumber++;
                                    lineData = uLine.Lines[currentLineNumber];

                                    if (lineData == "")
                                    {
                                        continue;
                                    }
                                    
                                    if (lineData[^1] == 13)
                                    {
                                        lineData = lineData.Substring(0, lineData.Length - 1);
                                    }

                                    containerData.Add(lineData);
                                    if (Formater.SafeContains(lineData, '{'))
                                    {
                                        containerCount++;
                                    }
                                    else if (Formater.SafeContains(lineData, '}'))
                                    {
                                        containerCount--;
                                        if (containerCount == 0)
                                        {
                                            // Add container token
                                            return new ProcessedLine(currentLineNumber + 1, LineType.Container, lineData, containerData);
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        return new ProcessedLine(currentLineNumber + 1, LineType.Variable, lineData);
                    }
                }
            }

            return new ProcessedLine(currentLineNumber + 1, LineType.Empty, lineData);
        }

        public void IncludeFile(string filePath)
        {
            string content = "";
            if (IsHttp(filePath))
            {
                using var webClient = new HttpClient();
                content = webClient.GetStringAsync(filePath).Result;
            }
            else
            {
                if (File.Exists(filePath))
                {
                    content = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
                } else {
                    Formater.CriticalError($"File {filePath} not found");
                }
            }

            if (content.Length == 0)
            {
                Formater.DebugMessage($"File {filePath} has no contents");
                return;
            }

            Formater.DebugMessage($"File {filePath} Found!");

            AnalyzeFileData(filePath, Formater.FormatFileData(content));

            Formater.DebugMessage($"Compilation of {filePath} done");
        }
    }
}

```

### File: Tokenizer\TokenUtils.cs

```csharp
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BuckshotPlusPlus
{
    public class TokenUtils
    {
        public static string GetTokenName(Token myToken)
        {
            if (myToken.Data.GetType() == typeof(TokenDataVariable))
            {
                TokenDataVariable myVar = (TokenDataVariable)myToken.Data;
                return myVar.VariableName;
            }
            else if (myToken.Data.GetType() == typeof(TokenDataContainer))
            {
                TokenDataContainer myContainer = (TokenDataContainer)myToken.Data;
                return myContainer.ContainerName;
            }

            return null;
        }

        public static Token FindTokenByName(List<Token> myTokenList, string tokenName, bool returnParent = false)
        {
            string[] subTokenNames = tokenName.Split('.');
            int remain = subTokenNames.Length;
            foreach (string localTokenName in subTokenNames)
            {
                remain--;
                foreach (Token myToken in myTokenList)
                {
                    if (myToken.Data.GetType() == typeof(TokenDataVariable))
                    {
                        TokenDataVariable myVar = (TokenDataVariable)myToken.Data;
                        if (myVar.VariableName == localTokenName)
                        {
                            if (remain > 0)
                            {
                                Formater.TokenCriticalError("Not a container!", myToken);
                            }
                            else
                            {
                                return myToken;
                            }
                        }
                    }
                    else if (myToken.Data.GetType() == typeof(TokenDataContainer))
                    {
                        TokenDataContainer myContainer = (TokenDataContainer)myToken.Data;
                        if (myContainer.ContainerName == localTokenName)
                        {
                            if (remain > 0 && !returnParent)
                            {
                                myTokenList = myContainer.ContainerData;
                                break;
                            }

                            return myToken;
                        }
                    }
                }
            }

            return null;
        }

        public static bool EditTokenData(List<Token> myTokenList, Token myToken)
        {
            TokenDataVariable var = (TokenDataVariable)myToken.Data;
            Token tokenToEdit = FindTokenByName(myTokenList, var.VariableName);
            if(tokenToEdit == null)
            {
                Token parentToken = FindTokenByName(myTokenList, var.VariableName, true);
                if(parentToken == null)
                {
                    Formater.TokenCriticalError("Can't find token with name: " + var.VariableName, myToken);
                    return false;
                }

                TokenDataContainer container = (TokenDataContainer)parentToken.Data;
                var.VariableName = var.VariableName.Split('.').Last();
                container.ContainerData.Add(myToken);
                return true;
            }

            TokenDataVariable myVar = (TokenDataVariable)tokenToEdit.Data;
            myVar.VariableData = var.GetCompiledVariableData(myTokenList);
            myVar.VariableType = var.VariableType == "multiple" ? "string" : var.VariableType;

            return true;
        }

        public static bool SafeEditTokenData(string lineData,List<Token> myTokenList, Token myToken)
        {
            if (Formater.SafeSplit(lineData, '.').Count > 1)
            {
                return EditTokenData(myTokenList, myToken);
            }

            return false;
        }

        public static void EditAllTokensOfContainer(List<Token> fileTokens,Token myContainer)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine(myContainer.LineData);
            TokenDataContainer pageTokenDataContainer = (TokenDataContainer)myContainer.Data;
            if (pageTokenDataContainer == null)
            {
                stopwatch.Stop();
                Formater.TokenCriticalError("The provided token is not a container!", myContainer);
            }
            else
            {
                foreach(Token childToken in pageTokenDataContainer.ContainerData)
                {
                    if(childToken.Data.GetType() == typeof(TokenDataVariable))
                    {
                        TokenDataVariable varToken = (TokenDataVariable)childToken.Data;
                        if (varToken != null)
                        {
                            SafeEditTokenData(varToken.VariableName, fileTokens, childToken);

                            if (varToken.VariableType == "ref")
                            {
                                Token referencedToken = FindTokenByName(fileTokens, varToken.VariableData);
                                if (referencedToken == null)
                                {
                                    Formater.TokenCriticalError("Token not super found " + varToken.VariableData, childToken);
                                }
                            }
                        }
                    }
                    else if(childToken.Data.GetType() == typeof(TokenDataContainer))
                    {
                        TokenDataContainer newContainerTokenData = (TokenDataContainer)childToken.Data;
                        if (newContainerTokenData.ContainerType == "logic")
                        {
                            // RUN LOGIC TEST
                            TokenDataLogic myLogic = (TokenDataLogic)newContainerTokenData.ContainerMetaData;
                            myLogic.RunLogicTest(fileTokens);

                        }
                    }
                    
                }
            }

            stopwatch.Stop();
            //Formater.SuccessMessage($"It took {stopwatch.ElapsedMilliseconds} ms to run EditAllTokensOfContainer of container {PageTokenDataContainer.ContainerName}");
        }

        public static TokenDataVariable FindTokenDataVariableByName(
            List<Token> myTokenList,
            string tokenName
        )
        {
            Token foundToken = FindTokenByName(myTokenList, tokenName);
            if (foundToken != null)
            {
                if (foundToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable myVar = (TokenDataVariable)foundToken.Data;
                    return myVar;
                }
            }

            return null;
        }

        public static TokenDataVariable TryFindTokenDataVariableValueByName(
            List<Token> fileTokens,
            List<Token> localTokenList,
            string tokenName,
            bool replaceRef = true
            )
        {
            Token foundToken = TryFindTokenValueByName(fileTokens, localTokenList, tokenName, replaceRef);
            if (foundToken != null)
            {
                if (foundToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable myVar = (TokenDataVariable)foundToken.Data;
                    return myVar;
                }
            }
            return null;
        }


        public static TokenDataContainer TryFindTokenDataContainerValueByName(
            List<Token> fileTokens,
            List<Token> localTokenList,
            string tokenName,
            bool replaceRef = true
            )
        {
            Token foundToken = TryFindTokenValueByName(fileTokens, localTokenList, tokenName, replaceRef);
            if (foundToken != null)
            {
                if (foundToken.Data.GetType() == typeof(TokenDataContainer))
                {
                    TokenDataContainer myContainer = (TokenDataContainer)foundToken.Data;
                    return myContainer;
                }
            }
            return null;
        }

        public static Token TryFindTokenValueByName(
            List<Token> fileTokens,
            List<Token> localTokenList,
            string tokenName,
            bool replaceRef = true
            )
        {
            Token foundToken = FindTokenByName(localTokenList, tokenName);
            if (foundToken != null)
            {
                if (foundToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable myVar = (TokenDataVariable)foundToken.Data;
                    if(myVar.VariableType == "ref" && replaceRef)
                    {
                        return TryFindTokenValueByName(fileTokens, fileTokens, myVar.VariableData);
                    }

                    return foundToken;
                }

                return foundToken;
            }

            return null;
        }

        public static TokenDataContainer FindTokenDataContainerByName(
            List<Token> myTokenList,
            string tokenName
        )
        {
            Token foundToken = FindTokenByName(myTokenList, tokenName);
            if (foundToken == null) return null;
            if (foundToken.Data.GetType() == typeof(TokenDataContainer))
            {
                TokenDataContainer myVar = (TokenDataContainer)foundToken.Data;
                return myVar;
            }

            return null;
        }
    }
}

```

### File: WebServer\MetaData.cs

```csharp
﻿using System.Collections.Generic;

namespace BuckshotPlusPlus.WebServer
{
    public enum WebMetaDataType
    {
        Query,
        Header
    }

    public class MetaData
    {
        public Dictionary<string, string> Data;
        public WebMetaDataType Type;
    }
}

```

### File: WebServer\Page.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.WebServer
{
    internal class Page
    {
        static string _basicPage = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF - 8\"> <meta http-equiv=\"X - UA - Compatible\" content =\"IE = edge\" > <meta name=\"viewport\" content =\"width=device-width, height=device-height, initial-scale=1.0, user-scalable=yes\" ><title>";

        public static string RenderWebPage(List<Token> serverSideTokens, Token myPage)
        {
            TokenUtils.EditAllTokensOfContainer(serverSideTokens, myPage);

            TokenDataContainer myPageContainer = (TokenDataContainer)myPage.Data;
            TokenDataVariable myPageTitle = TokenUtils.TryFindTokenDataVariableValueByName(
                serverSideTokens,
                myPageContainer.ContainerData,
                "title"
            );

            TokenDataVariable customHead = TokenUtils.TryFindTokenDataVariableValueByName(
                serverSideTokens,
                myPageContainer.ContainerData,
                "head"
            );

            TokenDataVariable myPageBody = TokenUtils.TryFindTokenDataVariableValueByName(
                serverSideTokens,
                myPageContainer.ContainerData,
                "body",
                false
            );

            string page = (String)_basicPage.Clone();
            if (myPageTitle != null)
            {
                page += myPageTitle.GetCompiledVariableData(serverSideTokens);
            }
            else
            {
                page += myPageContainer.ContainerName;
            }

            page += "</title>";

            Token myPageMeta = TokenUtils.FindTokenByName(myPageContainer.ContainerData, "meta");
            if (myPageMeta != null)
            {
                foreach (Token arrayValue in Analyzer.Array.GetArrayValues(myPageMeta))
                {
                    TokenDataVariable arrayVar = (TokenDataVariable)arrayValue.Data;
                    if(arrayVar.VariableType == "ref")
                    {
                        TokenDataContainer meta = TokenUtils.TryFindTokenDataContainerValueByName(
                            serverSideTokens,
                            serverSideTokens,
                            arrayVar.VariableData
                        );

                        string metaArgs = "";

                        foreach(Token metaVarToken in meta.ContainerData)
                        {
                            TokenDataVariable localMetaVar = (TokenDataVariable)metaVarToken.Data;
                            metaArgs += " " + localMetaVar.VariableName + "=" + '"';
                            metaArgs += localMetaVar.GetCompiledVariableData(serverSideTokens, true);
                            metaArgs += '"';
                        }

                        page += "<meta " + metaArgs + ">";
                    }
                    //Page += $"<script src=\"{ArrayVar.VariableData}\">";
                }
            }

            Token myPageIcon = TokenUtils.FindTokenByName(myPageContainer.ContainerData, "icon");
            if (myPageIcon != null)
            {
                TokenDataVariable var = (TokenDataVariable)myPageIcon.Data;
                page += "<link rel=\"icon\" type=\"image/x-icon\" href=\"" + var.VariableData + "\">";
            }

            Token myPageFonts = TokenUtils.FindTokenByName(myPageContainer.ContainerData, "fonts");
            if (myPageFonts != null)
            {
                page += "<style>";
                foreach (Token arrayValue in Analyzer.Array.GetArrayValues(myPageFonts))
                {
                    TokenDataVariable arrayVar = (TokenDataVariable)arrayValue.Data;
                    page += "@import url('" + arrayVar.VariableData + "');";
                }
                page += "</style>";
            }

            Token myPageCss = TokenUtils.FindTokenByName(myPageContainer.ContainerData, "css");
            if (myPageCss != null)
            {
                foreach (Token arrayValue in Analyzer.Array.GetArrayValues(myPageCss))
                {
                    TokenDataVariable arrayVar = (TokenDataVariable)arrayValue.Data;
                    page += $"<link rel=\"stylesheet\" href=\"{arrayVar.VariableData}\">";
                }
            }

            Token myPageScript = TokenUtils.FindTokenByName(
                myPageContainer.ContainerData,
                "scripts"
            );

            if (myPageScript != null)
            {
                foreach (Token arrayValue in Analyzer.Array.GetArrayValues(myPageScript))
                {
                    TokenDataVariable arrayVar = (TokenDataVariable)arrayValue.Data;
                    page += $"<script src=\"{arrayVar.VariableData}\"></script>";
                }
            }

            if (customHead is { VariableType: "string" })
            {
                page += customHead.VariableData;
            }

            

            page += "</head>";

            if (myPageBody != null)
            {
                page += Compiler.HTML.View.CompileContent(serverSideTokens, myPageBody, myPageContainer);
            }
            else
            {
                page += "<body><h1>" + myPageContainer.ContainerName + "</h1></body>";
            }

            return page + "</html>";
        }
    }
}

```

### File: WebServer\SitemapGenerator.cs

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace BuckshotPlusPlus.WebServer
{
    public static class SitemapGenerator
    {
        /// <summary>
        /// Generates an XML sitemap for the given list of URLs.
        /// </summary>
        /// <param name="urls">List of URLs to include in the sitemap.</param>
        public static void GenerateSitemap(Tokenizer myTokenizer,List<string> urls)
        {
            // Create a new XML document
            XmlDocument doc = new XmlDocument();

            // Add necessary XML declarations and namespaces
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.CreateElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            doc.AppendChild(xmlDeclaration);
            doc.AppendChild(root);

            // Iterate over the list of URLs and add them to the XML document
            foreach (var url in urls)
            {
                // Create a new url element
                XmlElement urlElement = doc.CreateElement("url");

                // Create loc element and set its value
                XmlElement locElement = doc.CreateElement("loc");
                locElement.InnerText = url;
                urlElement.AppendChild(locElement);

                // Add url element to root
                root.AppendChild(urlElement);
            }

            // Save the XML document to a file
            doc.Save(myTokenizer.RelativePath + "/sitemap.xml");
        }
        
        /// <summary>
        /// Generates an XML sitemap from a list of Tokens.
        /// </summary>
        /// <param name="tokens">List of Tokens to generate the sitemap from.</param>
        public static void GenerateSitemapFromTokens(Tokenizer myTokenizer)
        {
            List<string> urls = new List<string>();
            
            foreach (Token token in myTokenizer.FileTokens)
            {
                if (token.Data.GetType() == typeof(TokenDataContainer))
                {
                    TokenDataContainer tokenData = (TokenDataContainer)token.Data;

                    if (tokenData.ContainerType == "page")
                    {
                        string pageName = tokenData.ContainerName;
                        string envBaseUrl = Environment.GetEnvironmentVariable("base_url");

                        if (envBaseUrl == null)
                        {
                            envBaseUrl = "http://localhost:8080";
                        }
                        
                        if (pageName == "index")
                        {
                            urls.Add( envBaseUrl + "/");
                        }
                        else
                        {
                            urls.Add(envBaseUrl+"/" + pageName);
                        }
                        
                    }
                }
            }
            
            GenerateSitemap(myTokenizer, urls);
        }
    }
}

```

### File: WebServer\WebServer.cs

```csharp
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BuckshotPlusPlus.Security;

namespace BuckshotPlusPlus.WebServer
{
    internal class WebServer
    {
        public HttpListener Listener;
        public int RequestCount = 0;
        public bool RunServer = true;
        

        public async Task HandleIncomingConnections(Tokenizer myTokenizer)
        {
            UserSessionManager userSessions = new UserSessionManager();
            
            SitemapGenerator.GenerateSitemapFromTokens(myTokenizer);

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (RunServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await Listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                string absolutePath = req.Url!.AbsolutePath;
                if (absolutePath.Contains(".ico"))
                {
                    string path = "." + absolutePath;
                    if (File.Exists(path))
                    {
                        var data = File.ReadAllBytes("." + absolutePath);
                        resp.ContentType = "image/x-icon";
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = data.LongLength;

                        await resp.OutputStream.WriteAsync(data, 0, data.Length);

                        resp.Close();
                    }
                }
                else
                {
                    bool pageFound = false;

                    foreach (Token myToken in myTokenizer.FileTokens)
                    {
                        if (myToken.Data.GetType() == typeof(TokenDataContainer))
                        {
                            TokenDataContainer myTokenDataContainer = (TokenDataContainer)myToken.Data;
                            if (myTokenDataContainer.ContainerType == "page")
                            {
                                string pageName = myTokenDataContainer.ContainerName;

                                if (
                                    req.Url.AbsolutePath == "/" + pageName
                                    || (req.Url.AbsolutePath == "/" && pageName == "index")
                                )
                                {
                                    pageFound = true;

                                    var stopwatch = new Stopwatch();
                                    stopwatch.Start();

                                    userSessions.RemoveInactiveUserSessions();

                                    string clientIp = ctx.Request.RemoteEndPoint.ToString();

                                    List<Token> serverSideTokenList = new List<Token>();

                                    serverSideTokenList.AddRange(myTokenizer.FileTokens);

                                    UserSession foundUserSession = userSessions.AddOrUpdateUserSession(req, resp);

                                    foundUserSession.AddUrl(req.Url.AbsolutePath);

                                    serverSideTokenList.Add(foundUserSession.GetToken(myTokenizer));

                                    // Write the response info
                                    string disableSubmit = !RunServer ? "disabled" : "";
                                    string pageData = Page.RenderWebPage(serverSideTokenList, myToken);

                                    byte[] data = Encoding.UTF8.GetBytes(
                                        pageData
                                    );

                                    resp.ContentType = "text/html";
                                    resp.ContentEncoding = Encoding.UTF8;
                                    resp.ContentLength64 = data.LongLength;

                                    // Write out to the response stream (asynchronously), then close it
                                    await resp.OutputStream.WriteAsync(data, 0, data.Length);



                                    resp.Close();

                                    stopwatch.Stop();
                                    Formater.SuccessMessage($"Successfully sent page {pageName} in {stopwatch.ElapsedMilliseconds} ms");
                                }
                            }
                        }
                    }

                    if (!pageFound)
                    {
                        string disableSubmit = !RunServer ? "disabled" : "";
                        string pageData = "404 not found";

                        byte[] data = Encoding.UTF8.GetBytes(
                            pageData
                        );
                        resp.ContentType = "text";
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = data.LongLength;

                        // Write out to the response stream (asynchronously), then close it
                        await resp.OutputStream.WriteAsync(data, 0, data.Length);
                        resp.Close();
                    }
                }
            }
        }

        public void Start(Tokenizer myTokenizer, string localIp = "*")
        {
            // Create a Http server and start listening for incoming connections

            string url = "http://" + (Environment.GetEnvironmentVariable("BPP_HOST") is { Length: > 0 } v ? v : localIp + ":8080") + "/";
            try
            {
                Listener = new HttpListener();
                Listener.Prefixes.Add(url);
                Listener.Start();
                Formater.SuccessMessage($"Listening for connections on {url}");
                
                
                // Handle requests
                Task listenTask = HandleIncomingConnections(myTokenizer);
                listenTask.GetAwaiter().GetResult();

                // Close the listener
                Listener.Close();
            }
            catch (HttpListenerException e)
            {
                Formater.Warn($"Error: {e.Message} for local ip " + localIp);
                Start(myTokenizer, "localhost");
            }
        }
    }
}

```


## Project Summary

### File Count by Type

- Project Files: 1
- Source Files: 28

Total files processed: 29
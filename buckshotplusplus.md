# BuckshotPlusPlus Project Documentation


## Project Files

### File: BuckshotPlusPlus.csproj

```csharp
﻿<Project Sdk="Microsoft.NET.Sdk;Microsoft.NET.Sdk.Publish">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
	  <PackageReference Include="Appwrite" Version="0.4.2" />
	  <PackageReference Include="LibGit2Sharp" Version="0.27.2" />
	  
    <PackageReference Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.14.0" />
	  
    <PackageReference Include="MongoDB.Driver" Version="2.23.1" />
    <PackageReference Include="MySql.Data" Version="8.1.0" />
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
    <PackageReference Include="Serilog" Version="3.1.1" />
    <PackageReference Include="Serilog.Sinks.Console" Version="5.0.1" />
    <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
    <PackageReference Include="Spectre.Console" Version="0.46.0" />
    <PackageReference Include="Stripe.net" Version="43.9.0" />
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
﻿using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.CSS
{
    public class Properties
    {
        public static List<string> props = [
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
        ];

        public static string GetCssString(List<Token> serverSideTokens, Token myToken)
        {
            string compiledCss = "";
            TokenDataContainer viewContainer = (TokenDataContainer)myToken.Data;
            
            foreach (string name in props)
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
            foreach (string prop in props)
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
                result[i] = char.ToUpper(result[i][0]) + result[i][1..];
            }
            
            return string.Join("", result);
        }
    }
}

```

### File: Compiler\HTML\Atributes.cs

```csharp
﻿using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class Attributes
    {
        static List<(string, bool)> props = [
            // Core attributes
            ("id", false),
            ("class", false),
            ("style", false),
            ("title", false),
            ("lang", false),
            ("dir", false),
            
            // Link and navigation
            ("href", false),
            ("target", false),
            ("rel", false),
            ("download", false),
            ("hreflang", false),
            
            // Form attributes
            ("action", false),
            ("method", false),
            ("name", false),
            ("value", false),
            ("type", false),
            ("placeholder", false),
            ("pattern", false),
            ("min", false),
            ("max", false),
            ("step", false),
            ("maxlength", false),
            ("minlength", false),
            ("accept", false),
            ("autocomplete", false),
            ("enctype", false),
            
            // Media attributes
            ("src", false),
            ("alt", false),
            ("width", false),
            ("height", false),
            ("autoplay", true),
            ("controls", true),
            ("loop", true),
            ("muted", true),
            ("preload", false),
            ("poster", false),
            
            // Table attributes
            ("colspan", false),
            ("rowspan", false),
            ("headers", false),
            ("scope", false),
            
            // Form state attributes
            ("disabled", true),
            ("checked", true),
            ("selected", true),
            ("readonly", true),
            ("required", true),
            ("multiple", true),
            
            // ARIA accessibility
            ("role", false),
            ("aria-label", false),
            ("aria-describedby", false),
            ("aria-hidden", false),
            ("aria-live", false),
            ("aria-atomic", false),
            ("aria-expanded", false),
            ("aria-controls", false),
            ("aria-current", false),
            ("aria-disabled", false),
            ("aria-selected", false),
            
            // Data attributes
            ("data-*", false),
            
            // Meta information
            ("content", false),
            ("http-equiv", false),
            ("charset", false),
            
            // Draggable functionality
            ("draggable", false),
            ("dropzone", false),
            
            // Interactive attributes
            ("contenteditable", false),
            ("spellcheck", false),
            ("tabindex", false),
            
            // Frame/iframe attributes
            ("sandbox", false),
            ("srcdoc", false),
            ("frameborder", false),
            ("allowfullscreen", true),
            ("loading", false),
            
            // List attributes
            ("start", false),
            ("reversed", true),
            
            // Script attributes
            ("async", true),
            ("defer", true),
            ("integrity", false),
            ("crossorigin", false),
            
            // Form validation
            ("novalidate", true),
            ("formnovalidate", true),
            ("autocapitalize", false),
            ("inputmode", false),
            
            // Security
            ("referrerpolicy", false),
            
            // Misc attributes
            ("translate", false),
            ("hidden", true),
            ("cite", false),
            ("datetime", false)
        ];

        public static string GetHtmlAttributes(List<Token> serverSideTokens,Token myToken)
        {
            string compiledAtributes = "";
            TokenDataContainer viewContainer = (TokenDataContainer)myToken.Data;
            
            foreach ((string name, bool withoutValue) in props)
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
﻿using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    class Events
    {
        static List<string> props = [
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
        ];

        public static string GetHtmlEvents(List<Token> serverSideTokens, Token myToken)
        {
            string compiledEvents = " ";

            // Check if we even have event handlers before trying to process them
            bool hasEvents = false;
            TokenDataContainer viewContainer = (TokenDataContainer)myToken.Data;

            // Quick pre-check for any event handlers
            foreach (Token childToken in viewContainer.ContainerData)
            {
                if (childToken.Data is TokenDataVariable var && var.VariableName.StartsWith("on"))
                {
                    hasEvents = true;
                    break;
                }
            }

            // Only process events if we found any
            if (hasEvents)
            {
                foreach (Token childToken in viewContainer.ContainerData)
                {
                    if (childToken.Data is TokenDataVariable var && var.VariableName.StartsWith("on"))
                    {
                        if (props.Contains(var.VariableName))
                        {
                            Token myJsEvent = TokenUtils.FindTokenByName(
                                myToken.MyTokenizer.FileTokens,
                                var.GetCompiledVariableData(serverSideTokens)
                            );

                            if (myJsEvent != null)
                            {
                                compiledEvents +=
                                    var.VariableName + "=\"" +
                                    JS.Event.GetEventString(serverSideTokens, myJsEvent) +
                                    "\" ";
                            }
                        }
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
using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class View
    {
        public static string CompileView(List<Token> serverSideTokens, Token myViewToken)
        {
            try
            {
                TokenUtils.EditAllTokensOfContainer(serverSideTokens, myViewToken);

                if (myViewToken?.Data is not TokenDataContainer myContainer)
                {
                    Formater.TokenCriticalError("Invalid view token!", myViewToken);
                    return "";
                }

                var viewTypeToken = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "type");
                string viewType = viewTypeToken?.GetCompiledVariableData(serverSideTokens) ?? "div";  // Default to div

                string html = $"<{viewType} data-view=\"{myContainer.ContainerName}\"";

                // Only check input-type for input elements
                if (viewType == "input")
                {
                    var inputTypeToken = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "input-type");
                    if (inputTypeToken != null)
                    {
                        html += $" type=\"{inputTypeToken.GetCompiledVariableData(serverSideTokens)}\"";
                    }
                }

                // Add HTML attributes
                string htmlAttributes = Attributes.GetHtmlAttributes(serverSideTokens, myViewToken);
                if (!string.IsNullOrEmpty(htmlAttributes))
                {
                    html += $" {htmlAttributes}";
                }

                // Add events
                string htmlEvents = Events.GetHtmlEvents(serverSideTokens, myViewToken);
                if (!string.IsNullOrEmpty(htmlEvents))
                {
                    html += $" {htmlEvents}";
                }

                // Add CSS
                string style = CSS.Properties.GetCssString(serverSideTokens, myViewToken);
                html += !string.IsNullOrEmpty(style) ? $" style=\"{style}\">" : ">";

                // Add content
                var viewContent = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "content");
                html += CompileContent(serverSideTokens, viewContent, myContainer);

                return html + $"</{viewType}>";
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error compiling view: {ex.Message}", myViewToken);
                Formater.DebugMessage($"Stack trace: {ex.StackTrace}");
                return "";
            }
        }

        public static string CompileContent(List<Token> serverSideTokens, TokenDataVariable viewContent, TokenDataContainer myContainer)
        {
            if (viewContent == null)
            {
                return "";
            }

            try
            {
                switch (viewContent.VariableType)
                {
                    case "string":
                    case "multiple":
                        return viewContent.GetCompiledVariableData(serverSideTokens);

                    case "viewcall":
                        // Handle view calls directly
                        return viewContent.GetCompiledVariableData(serverSideTokens);

                    case "ref":
                        Token foundToken = TokenUtils.FindTokenByName(serverSideTokens, viewContent.GetCompiledVariableData(serverSideTokens));
                        if (foundToken?.Data is TokenDataContainer)
                        {
                            return CompileView(serverSideTokens, foundToken);
                        }
                        // If it's a parameterized view, handle it
                        else if (foundToken?.Data is TokenDataParameterizedView parameterizedView)
                        {
                            // This handles the case where a view is referenced directly without parameters
                            var viewCall = new TokenDataViewCall(parameterizedView.ViewName, new List<string>(), viewContent.VariableToken);
                            return viewCall.CompileViewCall(serverSideTokens);
                        }
                        return viewContent.GetCompiledVariableData(serverSideTokens, true);

                    case "array":
                        var result = new System.Text.StringBuilder();
                        foreach (Token childViewToken in Analyzer.Array.GetArrayValues(viewContent.VariableToken))
                        {
                            if (childViewToken?.Data is TokenDataVariable childView)
                            {
                                if (childView.VariableType == "viewcall")
                                {
                                    // Handle direct view calls in arrays
                                    result.Append(childView.GetCompiledVariableData(serverSideTokens));
                                }
                                else
                                {
                                    var childToken = TokenUtils.FindTokenByName(serverSideTokens, childView.GetCompiledVariableData(serverSideTokens));
                                    if (childToken != null)
                                    {
                                        result.Append(CompileView(serverSideTokens, childToken));
                                    }
                                }
                            }
                        }
                        return result.ToString();

                    default:
                        return "";
                }
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error compiling content: {ex.Message}", viewContent?.VariableToken);
                Formater.DebugMessage($"Stack trace: {ex.StackTrace}");
                return "";
            }
        }
    }
}
```

### File: Compiler\JS\Event.cs

```csharp
﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuckshotPlusPlus.Compiler.JS
{
    class Event
    {
        private static string EscapeJsString(string str)
        {
            if (str == null) return "null";

            return str.Replace("\\", "\\\\")
                     .Replace("'", "\\'")
                     .Replace("\"", "\\\"")
                     .Replace("\r", "\\r")
                     .Replace("\n", "\\n")
                     .Replace("\t", "\\t")
                     .Replace("\f", "\\f")
                     .Replace("\b", "\\b");
        }

        private static string GenerateSourceFetchCode(string sourceName, string propertyPath, bool useForEach = false)
        {
            // Use escaped values in the template
            sourceName = EscapeJsString(sourceName);
            propertyPath = EscapeJsString(propertyPath);

            StringBuilder code = new StringBuilder();
            code.Append($"fetch('/source/{sourceName}')");
            code.Append(".then(response => response.json())");
            code.Append(".then(data => {");
            code.Append("if (data.success) {");
            code.Append($"const result = data.data.{propertyPath};");
            code.Append("if (result !== undefined) {");
            code.Append(useForEach ? "el.textContent = result;" : "this.textContent = result;");
            code.Append("}");
            code.Append("}");
            code.Append("})");
            code.Append(".catch(error => console.error('Error:', error))");

            if (useForEach)
            {
                code.Append("});");
            }

            return code.ToString();
        }

        public static string GetEventString(List<Token> serverSideTokens, Token myJsEventToken)
        {
            TokenDataContainer myJsEvent = (TokenDataContainer)myJsEventToken.Data;
            StringBuilder eventString = new StringBuilder();

            int tokenId = 0;
            foreach (Token childToken in myJsEvent.ContainerData)
            {
                if (childToken.Data is TokenDataVariable childVar)
                {
                    // Handle view references (e.g., otherview.content or otherview.background-color)
                    if (childVar.VariableName.Contains('.'))
                    {
                        string[] parts = childVar.VariableName.Split('.');
                        string viewName = parts[0];
                        string property = parts[1];

                        // Create a new token for property checking
                        var propertyToken = new Token(
                            childToken.FileName,
                            property + " = " + childVar.GetCompiledVariableData(serverSideTokens),
                            childToken.LineNumber,
                            childToken.MyTokenizer
                        );
                        propertyToken.Data = new TokenDataVariable(propertyToken);

                        // Generate JS to update all instances of the referenced view
                        eventString.Append($"document.querySelectorAll('[data-view=\\'{viewName}\\']').forEach(el => {{");

                        if (property == "content")
                        {
                            if (childVar.VariableType == "ref")
                            {
                                // Check if it's a source reference
                                string[] refParts = childVar.VariableData.Split('.');
                                if (refParts.Length >= 2)
                                {
                                    var sourceToken = TokenUtils.FindTokenByName(serverSideTokens, refParts[0]);
                                    if (sourceToken?.Data is TokenDataContainer container &&
                                        container.ContainerType == "source")
                                    {
                                        eventString.Append(GenerateSourceFetchCode(refParts[0], string.Join(".", refParts.Skip(1)), true));
                                        continue;
                                    }
                                }
                                // Handle normal ref
                                eventString.Append($"el.textContent = '{EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens))}';");
                            }
                            else
                            {
                                eventString.Append($"el.textContent = '{EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens))}';");
                            }
                        }
                        else if (CSS.Properties.IsCssProp(propertyToken))
                        {
                            // Handle CSS property updates using the proper DOM style property name
                            eventString.Append($"el.style.{CSS.Properties.ToDomProp(property)} = '{EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens))}';");
                        }
                        else
                        {
                            // Handle any custom properties or attributes
                            eventString.Append($"el.setAttribute('{EscapeJsString(property)}', '{EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens))}');");
                        }

                        eventString.Append("});");
                    }
                    // Handle existing self-referential properties
                    else if (CSS.Properties.IsCssProp(childToken))
                    {
                        eventString.Append(
                            "this.style." +
                            CSS.Properties.ToDomProp(childVar.VariableName) +
                            " = '" +
                            EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens)) +
                            "';"
                        );
                    }
                    else if (childVar.VariableName == "content")
                    {
                        if (childVar.VariableType == "ref")
                        {
                            // Check if it's a source reference
                            string[] parts = childVar.VariableData.Split('.');
                            if (parts.Length >= 2)
                            {
                                var sourceToken = TokenUtils.FindTokenByName(serverSideTokens, parts[0]);
                                if (sourceToken?.Data is TokenDataContainer container &&
                                    container.ContainerType == "source")
                                {
                                    eventString.Append(GenerateSourceFetchCode(parts[0], string.Join(".", parts.Skip(1)), false));
                                    continue;
                                }
                            }
                        }
                        eventString.Append($"this.textContent = '{EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens))}';");
                    }
                    else
                    {
                        eventString.Append(Variables.GetVarString(
                            serverSideTokens,
                            myJsEvent.ContainerData,
                            tokenId
                        ) + ";");
                    }
                }
                else
                {
                    eventString.Append(childToken.LineData.Replace("\"", "'") + ";");
                }

                tokenId++;
            }

            return eventString.ToString();
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
                string part_0 = "";
                string part_1 = "";
                bool bIsFirstPart = true;
                foreach (char c in line)
                {
                    if (c == '=' & bIsFirstPart) {
                        bIsFirstPart = false;
                        continue;
                    }

                    if (bIsFirstPart)
                    {
                        part_0 += c;
                        continue;
                    }

                    part_1 += c;
                }

                Console.WriteLine(part_0 + "=" + part_1);
                Environment.SetEnvironmentVariable(part_0, part_1);
            }
        }
    }
}

```

### File: Formater.cs

```csharp
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
                Token foundTokenGlobal = TokenUtils.FindTokenByName(tokenList, LeftValue);
                if(foundTokenGlobal != null)
                {
                    TokenDataVariable foundVar = (TokenDataVariable)foundTokenGlobal.Data;
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
        private static void ShowHelp()
        {
            Console.WriteLine(@"
BuckshotPlusPlus - A simple and efficient web development language

Usage:
  bpp <file>              Run a BuckshotPlusPlus file (e.g., bpp main.bpp)
  bpp export <file> <dir> Export your website to static files
  bpp merge <file>        Merge all includes into a single file
  bpp -h                  Show this help message
  bpp --version          Show version information

Examples:
  bpp main.bpp           Start server with main.bpp
  bpp export main.bpp ./dist   Export website to ./dist directory
  bpp merge main.bpp     Create a merged version of your project

Options:
  -h, --help            Show this help message
  -v, --version         Show version information
");
        }

        private static void ShowVersion()
        {
            Console.WriteLine("BuckshotPlusPlus v0.5.0");
        }

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to BuckshotPlusPlus!");

            if (args.Length == 0)
            {
                Formater.CriticalError("No input file provided. Use -h for help.");
                return;
            }

            // Handle command line arguments
            string path = args[0];
            string arg = path.ToLower();
            if (arg == "-h" || arg == "--help")
            {
                ShowHelp();
                return;
            }

            if (arg == "-v" || arg == "--version")
            {
                ShowVersion();
                return;
            }

            if (arg == "export")
            {
                if (args.Length == 3)
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
                return;
            }
            else if (arg == "merge")
            {
                if (args.Length == 2)
                {
                    ProgramExtensions.GenerateCompleteProject(args[1]);
                }
                else
                {
                    Formater.CriticalError("You need to provide the path to your main.bpp file:\n" +
                        "\t- merge\n" +
                        "\t- path/to/your/main.bpp");
                }
                return;
            }

            // Regular BPP server startup
            await StartRegularServer(path);
        }

        private static async Task StartRegularServer(string filePath)
        {
            try
            {
                // If file doesn't exist, try looking in the current working directory
                if (!File.Exists(filePath))
                {
                    // Try using the absolute path from the current directory
                    string currentDirectory = Directory.GetCurrentDirectory();
                    filePath = Path.GetFullPath(Path.Combine(currentDirectory, filePath));

                    if (!File.Exists(filePath))
                    {
                        Formater.CriticalError($"File not found: {filePath}");
                        return;
                    }
                }

                // Get the directory containing the file to properly handle includes and relative paths
                string workingDirectory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(workingDirectory))
                {
                    // Change the current directory to where the file is located
                    Directory.SetCurrentDirectory(workingDirectory);
                }

                Tokenizer myTokenizer = CompileMainFile(filePath);

                var dotenv = Path.Combine(myTokenizer.RelativePath, ".env");
                if (File.Exists(dotenv))
                {
                    DotEnv.Load(dotenv);
                }

                WebServer.WebServer myWebServer = new() { };
                myWebServer.Start(myTokenizer);

                // Wait for Ctrl+C
                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;

                    cts.Cancel();
                };

                await Task.Delay(-1, cts.Token);
            }
            catch (Exception ex)
            {
                Formater.CriticalError($"Error: {ex.Message}");
            }
        }

        public static Tokenizer CompileMainFile(string filePath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Tokenizer myTokenizer = new(filePath);

            stopwatch.Stop();
            Formater.SuccessMessage($"Successfully compiled in {stopwatch.ElapsedMilliseconds} ms");
            return myTokenizer;
        }

        public static void ExportWebsite(string filePath, string exportDirectory)
        {
            // For now export directory is absolute only
            Tokenizer myTokenizer = CompileMainFile(filePath);
            if (Path.Exists(exportDirectory))
            {
                Directory.Delete(exportDirectory, true);
            }

            Directory.CreateDirectory(exportDirectory);
            foreach (Token pageToken in myTokenizer.PagesTokens)
            {
                TokenDataContainer myPageData = (TokenDataContainer)pageToken.Data;

                var icon = TokenUtils.FindTokenByName(myPageData.ContainerData, "icon");
                if (icon != null)
                {
                    var data = icon.Data as TokenDataVariable;
                    var fileName = data.VariableData;

                    string icoPath = Path.Combine(filePath, @"..\" + fileName);
                    File.WriteAllBytes(exportDirectory + "/" + fileName, File.ReadAllBytes(icoPath));
                }

                Formater.DebugMessage("Starting to export page " + myPageData.ContainerName + "...");
                File.WriteAllText(exportDirectory + "/" + myPageData.ContainerName + ".html", Page.RenderWebPage(myTokenizer.FileTokens, pageToken));
                Formater.SuccessMessage("Successfully exported page " + myPageData.ContainerName + ".html");
            }
        }
    }
}
```

### File: ProjectMerger.cs

```csharp
﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace BuckshotPlusPlus
{
    public class ProjectMerger
    {
        private readonly string _projectPath;
        private readonly HashSet<string> _processedFiles;
        private readonly StringBuilder _mergedContent;

        public ProjectMerger(string projectPath)
        {
            _projectPath = projectPath;
            _processedFiles = new HashSet<string>();
            _mergedContent = new StringBuilder();
        }

        public void MergeProject()
        {
            try
            {
                // Add header comment
                _mergedContent.AppendLine("## CompleteProject.BPP");
                _mergedContent.AppendLine($"## Generated on: {DateTime.Now}");
                _mergedContent.AppendLine("## This is an auto-generated file containing all BPP code from the project");
                _mergedContent.AppendLine();

                // Start with the main file
                ProcessFile(_projectPath);

                // Write the merged content to CompleteProject.BPP
                string outputPath = Path.Combine(Path.GetDirectoryName(_projectPath), "CompleteProject.BPP");
                File.WriteAllText(outputPath, _mergedContent.ToString());

                Formater.SuccessMessage($"Successfully created CompleteProject.BPP at {outputPath}");
            }
            catch (Exception ex)
            {
                Formater.CriticalError($"Failed to merge project: {ex.Message}");
            }
        }

        private void ProcessFile(string filePath)
        {
            // Avoid processing the same file twice
            if (_processedFiles.Contains(filePath))
                return;

            _processedFiles.Add(filePath);

            try
            {
                string content = File.ReadAllText(filePath);
                string formattedContent = Formater.FormatFileData(content);

                // Add file header
                _mergedContent.AppendLine($"## File: {filePath}");
                _mergedContent.AppendLine();

                // Process includes
                var lines = formattedContent.Split('\n');
                foreach (var line in lines)
                {
                    if (line.TrimStart().StartsWith("include"))
                    {
                        string includePath = ParseIncludePath(line);
                        if (!string.IsNullOrEmpty(includePath))
                        {
                            string fullPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePath), includePath));
                            ProcessFile(fullPath);
                        }
                    }
                    else
                    {
                        _mergedContent.AppendLine(line);
                    }
                }

                _mergedContent.AppendLine();
            }
            catch (Exception ex)
            {
                Formater.Warn($"Error processing file {filePath}: {ex.Message}");
            }
        }

        private string ParseIncludePath(string includeLine)
        {
            try
            {
                var parts = includeLine.Split('"');
                if (parts.Length >= 2)
                {
                    return parts[1];
                }
            }
            catch (Exception)
            {
                // Ignore parsing errors
            }
            return null;
        }
    }

    // Extension for Program.cs
    public static class ProgramExtensions
    {
        public static void GenerateCompleteProject(string filePath)
        {
            var merger = new ProjectMerger(filePath);
            merger.MergeProject();
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

### File: Source\BaseSource.cs

```csharp
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuckshotPlusPlus.Source
{
    public abstract class BaseSource
    {
        protected Dictionary<string, string> SourceParameters { get; set; }
        protected TokenDataContainer SourceContainer { get; set; }
        protected Tokenizer SourceTokenizer { get; set; }

        protected BaseSource(TokenDataContainer container, Tokenizer tokenizer)
        {
            SourceParameters = [];
            SourceContainer = container;
            SourceTokenizer = tokenizer;

            foreach (Token token in container.ContainerData)
            {
                if (token.Data is TokenDataVariable variable)
                {
                    SourceParameters[variable.VariableName] = variable.VariableData;
                }
            }
        }

        public abstract Task<Token> FetchDataAsync();

        protected abstract Token TransformResponse(object rawResponse);

        public abstract bool ValidateConfiguration();

        protected Token CreateDataToken(string data)
        {
            string tokenData = $"data {SourceContainer.ContainerName}_data {{\n{data}\n}}";
            return new Token(
                SourceContainer.ContainerToken.FileName,
                tokenData,
                SourceContainer.ContainerToken.LineNumber,
                SourceTokenizer
            );
        }

        public static BaseSource CreateSource(TokenDataContainer container, Tokenizer tokenizer)
        {
            var typeVar = TokenUtils.FindTokenDataVariableByName(container.ContainerData, "type");
            string sourceType = typeVar?.VariableData ?? "http";

            return sourceType.ToLower() switch
            {
                "http" => new HttpSource(container, tokenizer),
                _ => throw new NotSupportedException($"Source type {sourceType} is not supported")
            };
        }
    }
}
```

### File: Source\HttpSource.cs

```csharp
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuckshotPlusPlus.Source
{
    public class HttpSource : BaseSource
    {
        private readonly HttpClient _httpClient;
        private const int DEFAULT_TIMEOUT = 30; // TODO: Configure timeout

        public HttpSource(TokenDataContainer container, Tokenizer tokenizer) : base(container, tokenizer)
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT)
            };
        }

        public override async Task<Token> FetchDataAsync()
        {
            try
            {
                if (!ValidateConfiguration())
                {
                    Formater.TokenCriticalError("Invalid HTTP source configuration", SourceContainer.ContainerToken);
                    return null;
                }

                Formater.DebugMessage("Creating HTTP request...");
                var request = new HttpRequestMessage(
                    GetHttpMethod(),
                    SourceParameters["url"]
                );

                Formater.DebugMessage($"URL: {request.RequestUri}");
                Formater.DebugMessage($"Method: {request.Method}");

                try
                {
                    AddHeaders(request);
                }
                catch (Exception ex)
                {
                    Formater.DebugMessage($"Error adding headers: {ex}");
                    throw;
                }

                Formater.DebugMessage("Sending request...");
                var response = await _httpClient.SendAsync(request);

                Formater.DebugMessage($"Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Formater.DebugMessage($"Response content length: {content?.Length ?? 0}");

                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("Empty response from server");
                }

                return TransformResponse(content);
            }
            catch (HttpRequestException ex)
            {
                Formater.RuntimeError($"HTTP request failed: {ex.Message}", SourceContainer.ContainerToken);
                return null;
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error during request: {ex.Message}", SourceContainer.ContainerToken);
                Formater.DebugMessage($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        private HttpMethod GetHttpMethod()
        {
            var method = SourceParameters.GetValueOrDefault("method", "GET").ToUpper();
            return method switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                _ => HttpMethod.Get
            };
        }

        private void AddHeaders(HttpRequestMessage request)
        {
            var headersVar = TokenUtils.FindTokenDataVariableByName(
                SourceContainer.ContainerData,
                "headers"
            );

            Formater.DebugMessage($"Headers variable found: {headersVar != null}");
            if (headersVar?.VariableType == "array")
            {
                Formater.DebugMessage($"Headers array data: {headersVar.VariableData}");
                var headerTokens = Analyzer.Array.GetArrayValues(headersVar.VariableToken);
                Formater.DebugMessage($"Number of header tokens: {headerTokens.Count}");

                foreach (Token headerToken in headerTokens)
                {
                    Formater.DebugMessage($"Processing header token: {headerToken.LineData}");
                    if (headerToken.Data is TokenDataVariable headerVar)
                    {
                        Formater.DebugMessage($"Header variable type: {headerVar.VariableType}");
                        if (headerVar.VariableType == "ref")
                        {
                            var refName = headerVar.VariableData;
                            Formater.DebugMessage($"Looking for referenced token: {refName}");

                            var refToken = TokenUtils.FindTokenByName(
                                SourceContainer.ContainerToken.MyTokenizer.FileTokens,
                                refName
                            );

                            if (refToken == null)
                            {
                                Formater.RuntimeError($"Referenced header token not found: {refName}", headerToken);
                                continue;
                            }

                            if (refToken.Data is TokenDataContainer kvContainer)
                            {
                                Formater.DebugMessage($"Found KV container: {kvContainer.ContainerName}");
                                var key = TokenUtils.FindTokenDataVariableByName(kvContainer.ContainerData, "key");
                                var value = TokenUtils.FindTokenDataVariableByName(kvContainer.ContainerData, "value");

                                if (key == null || value == null)
                                {
                                    Formater.RuntimeError("Invalid KV container - missing key or value", refToken);
                                    continue;
                                }

                                var headerName = key.GetCompiledVariableData(new List<Token>());
                                var headerValue = value.GetCompiledVariableData(new List<Token>());

                                // Remove quotes if present
                                headerName = headerName.Trim('"');
                                headerValue = headerValue.Trim('"');

                                Formater.DebugMessage($"Adding header: {headerName}: {headerValue}");
                                if (!request.Headers.TryAddWithoutValidation(headerName, headerValue))
                                {
                                    Formater.RuntimeError($"Failed to add header: {headerName}", headerToken);
                                }
                            }
                            else
                            {
                                Formater.RuntimeError($"Referenced token is not a KV container: {refName}", headerToken);
                            }
                        }
                    }
                }
            }
            else
            {
                Formater.DebugMessage("No headers found or headers is not an array");
            }
        }

        protected override Token TransformResponse(object rawResponse)
        {
            if (rawResponse is not string stringResponse)
            {
                Formater.DebugMessage("Invalid response type - not a string");
                return null;
            }

            try
            {
                Formater.DebugMessage("Parsing JSON response...");
                var jsonData = JObject.Parse(stringResponse);
                var dataLines = new List<string>();

                // Create individual variable tokens
                var containerData = new List<Token>();
                FlattenJson("", jsonData, dataLines);

                foreach (var line in dataLines)
                {
                    var parts = line.Split('=', 2);
                    if (parts.Length == 2)
                    {
                        var varToken = new Token(
                            SourceContainer.ContainerToken.FileName,
                            $"{parts[0].Trim()} = {parts[1].Trim()}",
                            SourceContainer.ContainerToken.LineNumber,
                            SourceTokenizer
                        );
                        containerData.Add(varToken);
                    }
                }

                // Create the data container
                string containerName = $"{SourceContainer.ContainerName}_data";
                string tokenData = $"data {containerName} {{\n{string.Join("\n", dataLines)}\n}}";

                var containerToken = new Token(
                    SourceContainer.ContainerToken.FileName,
                    tokenData,
                    SourceContainer.ContainerToken.LineNumber,
                    SourceTokenizer
                );

                if (containerToken.Data is TokenDataContainer container)
                {
                    // Add all variable tokens to the container
                    container.ContainerData.AddRange(containerData);
                    SourceContainer.ContainerData.Add(containerToken);

                    // Add to global tokens
                    if (SourceTokenizer.FileTokens != null)
                    {
                        SourceTokenizer.FileTokens.Add(containerToken);
                        foreach (var varToken in containerData)
                        {
                            SourceTokenizer.FileTokens.Add(varToken);
                        }
                    }
                }

                return containerToken;
            }
            catch (JsonException ex)
            {
                Formater.RuntimeError($"Failed to parse JSON response: {ex.Message}", SourceContainer.ContainerToken);
                Formater.DebugMessage($"Raw response: {stringResponse}");
                return null;
            }
        }

        private void FlattenJson(string prefix, JToken token, List<string> dataLines)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (var prop in (token as JObject).Properties())
                    {
                        string name = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}_{prop.Name}";
                        if (prop.Value.Type == JTokenType.Object)
                        {
                            FlattenJson(name, prop.Value, dataLines);
                        }
                        else
                        {
                            dataLines.Add($"{name} = {FormatJsonValue(prop.Value)}");
                        }
                    }
                    break;

                case JTokenType.Array:
                    dataLines.Add($"{prefix} = [{string.Join(",", token.Select(t => FormatJsonValue(t)))}]");
                    break;

                default:
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        dataLines.Add($"{prefix} = {FormatJsonValue(token)}");
                    }
                    break;
            }
        }

        private string FormatJsonValue(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.String:
                    return $"\"{token.ToString().Replace("\"", "\\\"")}\"";
                case JTokenType.Date:
                    return $"\"{token.ToObject<DateTime>():dd/MM/yyyy HH:mm:ss}\"";
                case JTokenType.Boolean:
                    return token.ToObject<bool>().ToString().ToLower();
                case JTokenType.Integer:
                case JTokenType.Float:
                    return token.ToString();
                case JTokenType.Null:
                    return "\"\"";
                default:
                    return $"\"{token}\"";
            }
        }

        public override bool ValidateConfiguration()
        {
            Formater.DebugMessage("Validating HTTP source configuration...");

            if (!SourceParameters.ContainsKey("url"))
            {
                Formater.RuntimeError("HTTP source must specify a url", SourceContainer.ContainerToken);
                return false;
            }

            if (!Uri.TryCreate(SourceParameters["url"], UriKind.Absolute, out var uri))
            {
                Formater.RuntimeError("Invalid URL format", SourceContainer.ContainerToken);
                return false;
            }

            Formater.DebugMessage($"Configuration valid. URL: {uri}");
            return true;
        }
    }
}
```

### File: StringHandler.cs

```csharp
﻿using System;
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
```

### File: Tokenizer\ParameterParser.cs

```csharp
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
            else if (TokenDataVariable.IsTokenDataVariable(this))
            {
                Data = new TokenDataVariable(this);
            }
            else if (TokenDataFunctionCall.IsTokenDataFunctionCall(this))
            {
                Data = new TokenDataFunctionCall(this);
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
﻿using BuckshotPlusPlus.Source;
using System;
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
            "elseif",
            "source",
            "kv"
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

            if (this.ContainerType == "source")
            {
                try
                {
                    var source = Source.BaseSource.CreateSource(this, myToken.MyTokenizer);
                    if (source != null)
                    {
                        var sourceData = source.FetchDataAsync().Result;
                        if (sourceData?.Data is TokenDataContainer dataContainer)
                        {
                            // Transfer the data from the container to our ContainerData
                            foreach (var dataToken in dataContainer.ContainerData)
                            {
                                ContainerData.Add(dataToken);
                            }
                            Formater.DebugMessage($"Source data initialized for {ContainerName}");
                        }
                        else
                        {
                            Formater.RuntimeError($"Invalid source data format", myToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Formater.RuntimeError($"Failed to initialize source: {ex.Message}", myToken);
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
        }
        

        public static bool IsTokenDataFunctionCall(Token myToken)
        {
            int parenPos = myToken.LineData.IndexOf('(');
            Formater.DebugMessage(parenPos.ToString() + " -> " + StringHandler.IsInsideQuotes(myToken.LineData, parenPos).ToString());
            return parenPos != -1 && !StringHandler.IsInsideQuotes(myToken.LineData, parenPos);
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

### File: Tokenizer\TokenDataParameterizedView.cs

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuckshotPlusPlus
{
    public class TokenDataParameterizedView : TokenData
    {
        public string ViewName { get; set; }
        public string ParentViewName { get; set; }
        public List<string> Parameters { get; set; }
        public List<Token> ViewContent { get; set; }
        public Token ViewToken { get; set; }
        public TokenDataParameterizedView ParentView { get; private set; }

        public TokenDataParameterizedView(string viewName, string parentViewName, List<string> parameters, List<Token> content, Token token, List<Token> serverSideTokens = null)
        {
            if (string.IsNullOrWhiteSpace(viewName))
            {
                throw new ArgumentNullException(nameof(viewName), "View name cannot be null or empty");
            }
            
            ViewName = viewName.Trim();
            ParentViewName = parentViewName?.Trim();
            Parameters = parameters ?? new List<string>();
            ViewContent = content ?? new List<Token>();
            ViewToken = token ?? throw new ArgumentNullException(nameof(token));
            
            // Try to find the parent view in the server tokens
            if (!string.IsNullOrEmpty(parentViewName))
            {
                ParentView = TokenUtils.FindParameterizedView(serverSideTokens, parentViewName);
                if (ParentView == null)
                {
                    Formater.TokenCriticalError($"Parent view '{parentViewName}' not found for view '{ViewName}'", ViewToken);
                }
                else
                {
                    Formater.DebugMessage($"View '{ViewName}' inherits from '{ParentViewName}' with {Parameters.Count} parameters");
                    
                    // Merge parent parameters with child parameters
                    if (ParentView.Parameters.Count > 0)
                    {
                        var allParameters = new List<string>(ParentView.Parameters);
                        allParameters.AddRange(Parameters);
                        Parameters = allParameters;
                    }
                }
            }
            else
            {
                Formater.DebugMessage($"Created parameterized view '{ViewName}' with {Parameters.Count} parameters");
            }
        }

        public TokenDataContainer InstantiateView(List<string> arguments, List<Token> serverSideTokens)
        {
            try
            {
                if (arguments == null)
                {
                    Formater.RuntimeError("Arguments list is null", ViewToken);
                    return null;
                }

                if (arguments.Count != Parameters.Count)
                {
                    Formater.RuntimeError($"Parameter count mismatch for view '{ViewName}'. Expected {Parameters.Count}, got {arguments.Count}", ViewToken);
                    return null;
                }


                // Create a unique instance identifier
                string uniqueInstanceId = Guid.NewGuid().ToString("N")[..8]; // Short GUID
                string uniqueInstanceName = $"{ViewName}_instance_{uniqueInstanceId}";

                // Process each token in the view content
                var instantiatedContent = new List<Token>();

                // Create a dictionary to store the variables from parent and child views
                var viewVariables = new Dictionary<string, Token>();

                // First, process parent view content if this view inherits from another
                if (ParentView != null)
                {
                    Formater.DebugMessage($"Including content from parent view '{ParentView.ViewName}' in view '{ViewName}'");
                    
                    // Only pass arguments to the parent if it expects parameters
                    var parentArguments = ParentView.Parameters.Count > 0 ? arguments : new List<string>();
                    var parentContent = ParentView.InstantiateView(parentArguments, serverSideTokens)?.ContainerData;
                    
                    if (parentContent != null)
                    {
                        // Add parent's content first, but don't add to instantiatedContent yet
                        foreach (var token in parentContent)
                        {
                            if (token?.Data is TokenDataVariable varData)
                            {
                                viewVariables[varData.VariableName] = token;
                            }
                        }
                    }
                }


                // Process current view's content, which will override parent's content
                foreach (var token in ViewContent)
                {
                    try
                    {
                        var clonedToken = CloneTokenWithParameterSubstitution(token, arguments, serverSideTokens);
                        if (clonedToken != null && clonedToken.Data is TokenDataVariable varData)
                        {
                            // Store or update the variable in our dictionary
                            viewVariables[varData.VariableName] = clonedToken;
                        }
                    }
                    catch (Exception ex)
                    {
                        Formater.RuntimeError($"Error cloning token in view '{ViewName}': {ex.Message}", token);
                        Formater.DebugMessage($"Stack trace in token at line {token?.LineNumber}: {ex.StackTrace}");
                    }
                }


                // Instead of creating a container, we'll create a special token that represents the view instance
                var viewInstanceToken = new Token(
                    fileName: ViewToken?.FileName ?? "unknown",
                    lineData: $"view {uniqueInstanceName}",
                    lineNumber: ViewToken?.LineNumber ?? 0,
                    myTokenizer: ViewToken?.MyTokenizer,
                    parent: null,
                    previousToken: null
                );
                
                // Add all variables to the final content
                var finalContent = new List<Token>(viewVariables.Values);
                
                // Create a custom container that will handle the view content
                var viewContainer = new TokenDataContainer(viewInstanceToken)
                {
                    // Add all variables to the container
                    ContainerData = finalContent
                };
                
                // Set the container name to indicate it's a view instance
                viewContainer.ContainerName = uniqueInstanceName;
                viewContainer.ContainerType = "view";
                
                return viewContainer;
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error instantiating view '{ViewName}': {ex.Message}", ViewToken);
                Formater.DebugMessage($"Stack trace in view '{ViewName}': {ex.StackTrace}");
                return null;
            }
        }

        private Token CloneTokenWithParameterSubstitution(Token originalToken, List<string> arguments, List<Token> serverSideTokens)
        {
            var clonedToken = new Token(
                fileName: originalToken.FileName,
                lineData: originalToken.LineData,
                lineNumber: originalToken.LineNumber,
                myTokenizer: originalToken.MyTokenizer,
                parent: null,
                previousToken: null
            );

            if (originalToken.Data is TokenDataVariable variable)
            {
                // Handle parameter references in variable data
                string newVariableData = variable.VariableData;
                string trimmedValue = variable.VariableData.Trim();
                
                // If the variable data exactly matches a parameter name, replace it with the argument
                if (Parameters.Contains(trimmedValue))
                {
                    int paramIndex = Parameters.IndexOf(trimmedValue);
                    if (paramIndex >= 0 && paramIndex < arguments.Count)
                    {
                        newVariableData = arguments[paramIndex];
                        // If the original was a reference, keep it as a reference
                        if (variable.VariableType == "ref")
                        {
                            clonedToken.Data = new TokenDataVariable(clonedToken)
                            {
                                VariableName = variable.VariableName,
                                VariableData = newVariableData,
                                VariableType = "string"  // Change to string since we're substituting the value directly
                            };
                            return clonedToken;
                        }
                    }
                }
                
                clonedToken.Data = new TokenDataVariable(clonedToken)
                {
                    VariableName = variable.VariableName,
                    VariableData = newVariableData,
                    VariableType = variable.VariableType
                };
            }
            else if (originalToken.Data is TokenDataContainer container)
            {
                // For containers, we need to create a new container with the same type and content
                // but with parameters substituted in the content
                clonedToken.Data = new TokenDataContainer(clonedToken);
                var newContainer = (TokenDataContainer)clonedToken.Data;
                
                // Copy container properties
                newContainer.ContainerName = container.ContainerName;
                newContainer.ContainerType = container.ContainerType;
                
                // Clone and substitute parameters in container data
                foreach (var childToken in container.ContainerData)
                {
                    var clonedChild = CloneTokenWithParameterSubstitution(childToken, arguments, serverSideTokens);
                    if (clonedChild != null)
                    {
                        newContainer.ContainerData.Add(clonedChild);
                    }
                }
            }
            else
            {
                clonedToken.Data = originalToken.Data;
            }

            return clonedToken;
        }

        private string SubstituteParameters(string content, List<string> arguments)
        {
            if (content == null)
                return string.Empty;

            try
            {
                // If the content exactly matches a parameter, replace it
                for (int i = 0; i < Math.Min(Parameters.Count, arguments.Count); i++)
                {
                    if (Parameters[i] != null && arguments[i] != null && content.Trim() == Parameters[i])
                    {
                        return arguments[i];
                    }
                }
                
                // If we get here, no exact match was found
                return content;
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error substituting parameters in view '{ViewName}': {ex.Message}", ViewToken);
                Formater.DebugMessage($"Parameter substitution error in view '{ViewName}':\nContent: {content}\nParameters: {string.Join(", ", Parameters)}\nArguments: {string.Join(", ", arguments)}");
                return content; // Return original content on error
            }
        }
    }
}

```

### File: Tokenizer\TokenDataVariable.cs

```csharp
using System;
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

            if (this.VariableType == "")
            {
                return;
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
            // Trim any whitespace first
            initialValue = initialValue.Trim();

            // Check if the string starts and ends with quotes
            if (initialValue.Length < 2 ||
                (initialValue[0] != '"' && initialValue[0] != '\'') ||
                (initialValue[^1] != '"' && initialValue[^1] != '\''))
            {

                //Formater.TokenCriticalError("Invalid string value", myToken);
                return initialValue;
            }

            // Return the string without the quotes
            return initialValue.Substring(1, initialValue.Length - 2);
        }

        public static string FindVariableType(string value, Token myToken)
        {
            // Trim the value first
            value = value.Trim();

            // Check for view call first
            if (ParameterParser.IsViewCall(value))
                return "viewcall";

            // Check for array first
            if (value.StartsWith("[") && value.EndsWith("]"))
                return "array";

            // Check for string (both single and double quotes)
            if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                (value.StartsWith("'") && value.EndsWith("'")))
                return "string";

            // Try parsing as other types
            if (int.TryParse(value, out _))
                return "int";
            if (float.TryParse(value, out _))
                return "float";
            if (bool.TryParse(value, out _))
                return "bool";

            // If none of the above, treat as reference
            return "ref";
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
            // Handle view calls
            if (this.VariableType == "viewcall")
            {
                var (viewName, arguments) = ParameterParser.ParseViewCall(this.VariableData);
                var viewCall = new TokenDataViewCall(viewName, arguments, this.VariableToken);
                return viewCall.CompileViewCall(fileTokens);
            }
            else if (this.VariableType == "multiple")
            {
                List<string> variables = Formater.SafeSplit(this.VariableData, '+');
                string result = "";

                foreach (string variable in variables)
                {
                    string trimmedVar = variable.Trim();
                    string safeVariableType = FindVariableType(trimmedVar, null);

                    if (safeVariableType == "string")
                    {
                        // Already a string literal, just remove the quotes and add to result
                        result += GetValueFromString(trimmedVar, VariableToken);
                    }
                    else if (safeVariableType == "ref")
                    {
                        TokenDataVariable foundToken = TokenUtils.FindTokenDataVariableByName(fileTokens, trimmedVar);
                        if (foundToken != null)
                        {
                            string value = foundToken.VariableData;

                            // If the value isn't already a quoted string and we're in a string context,
                            // we should use the raw value without quotes
                            if (foundToken.VariableType == "string")
                            {
                                value = GetValueFromString(value, foundToken.VariableToken);
                            }

                            result += value;
                        }
                        else
                        {
                            Formater.RuntimeError($"Token not found: {trimmedVar}", this.VariableToken);
                        }
                    }
                }
                return result;
            }
            else if (this.VariableType == "ref")
            {
                var sourceVar = TokenUtils.ResolveSourceReference(fileTokens, this.VariableData);
                if (sourceVar != null)
                {
                    return sourceVar.VariableData;
                }

                if (compileRef)
                {
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
            }

            return this.VariableData;
        }
    }
}

```

### File: Tokenizer\TokenDataViewCall.cs

```csharp
using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public class TokenDataViewCall : TokenData
    {
        public string ViewName { get; set; }
        public List<string> Arguments { get; set; }
        public Token ViewCallToken { get; set; }

        public TokenDataViewCall(string viewName, List<string> arguments, Token token)
        {
            ViewName = viewName?.Trim() ?? throw new ArgumentNullException(nameof(viewName));
            Arguments = arguments ?? new List<string>();
            ViewCallToken = token ?? throw new ArgumentNullException(nameof(token));
        }

        public string CompileViewCall(List<Token> fileTokens)
        {
            try
            {
                if (fileTokens == null)
                {
                    Formater.RuntimeError("File tokens list is null", ViewCallToken);
                    return "";
                }
                
                Formater.DebugMessage($"Looking for parameterized view '{ViewName}' with {Arguments.Count} arguments");

                // Find the parameterized view definition
                var paramView = FindParameterizedView(fileTokens, ViewName);
                if (paramView == null)
                {
                    // The error is already logged in FindParameterizedView
                    return string.Empty;
                }

                // Instantiate the view with arguments
                var viewInstance = paramView.InstantiateView(Arguments, fileTokens);
                if (viewInstance == null)
                {
                    Formater.RuntimeError($"Failed to instantiate view '{ViewName}'", ViewCallToken);
                    return "";
                }

                // Compile the view content directly
                var result = new System.Text.StringBuilder();
                
                // Process the view instance to ensure all properties (like color) are properly handled
                if (viewInstance.ContainerData != null)
                {
                    // Create a token for the view instance and compile it
                    var viewToken = new Token(
                        fileName: ViewCallToken.FileName,
                        lineData: $"view {viewInstance.ContainerName} {{ ... }}",
                        lineNumber: ViewCallToken.LineNumber,
                        myTokenizer: ViewCallToken.MyTokenizer,
                        parent: null,
                        previousToken: null
                    );
                    viewToken.Data = viewInstance;
                    
                    // Compile the view which will handle all its children
                    string compiledView = Compiler.HTML.View.CompileView(fileTokens, viewToken);
                    
                    // Only append if we have content
                    if (!string.IsNullOrEmpty(compiledView))
                    {
                        result.Append(compiledView);
                    }
                }
                
                if (result.Length == 0)
                {
                    Formater.DebugMessage($"Empty result when compiling view '{ViewName}' at line {ViewCallToken?.LineNumber}");
                }
                
                return result.ToString().Trim();
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error compiling view call '{ViewName}': {ex.Message}", ViewCallToken);
                Formater.DebugMessage($"Stack trace: {ex.StackTrace}");
                return $"<!-- Error compiling view '{ViewName}': {ex.Message} -->";
            }
        }

        private TokenDataParameterizedView FindParameterizedView(List<Token> fileTokens, string viewName)
        {
            if (fileTokens == null || string.IsNullOrEmpty(viewName))
            {
                Formater.DebugMessage($"Invalid parameters - fileTokens: {fileTokens?.Count ?? 0} items, viewName: '{viewName}'");
                return null;
            }

            // Use the TokenUtils helper method to find the parameterized view
            var paramView = TokenUtils.FindParameterizedView(fileTokens, viewName);
            
            if (paramView != null)
            {
                Formater.DebugMessage($"Found parameterized view '{paramView.ViewName}'");
                return paramView;
            }
            
            string errorMessage = $"View '{viewName}' not found in current scope";
            Formater.DebugMessage(errorMessage);
            Formater.TokenCriticalError(errorMessage, ViewCallToken);
            return null;
        }
    }
}

```

### File: Tokenizer\Tokenizer.cs

```csharp
using System;
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
        Empty,
        ParameterizedView
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

        private void ProcessParameterizedViewLine(ProcessedLine processedLine, List<string> fileLines, ref int currentLineNumber, string fileName)
        {
            var (viewName, parentViewName, parameters) = ParameterParser.ParseParameterizedViewDefinition(processedLine.LineData);
            
            if (string.IsNullOrEmpty(viewName))
            {
                Formater.RuntimeError($"Invalid parameterized view syntax at line {processedLine.CurrentLine}", null);
                return;
            }
            
            Formater.DebugMessage($"Processing parameterized view '{viewName}'{(parentViewName != null ? $" inheriting from '{parentViewName}'" : "")} with {parameters.Count} parameters at line {processedLine.CurrentLine}");

            // Read the view content (similar to container processing)
            var viewContent = new List<Token>();
            int startLine = currentLineNumber;
            int braceCount = 1; // We already have the opening brace
            currentLineNumber++;
            
            // Process the view's content tokens
            while (currentLineNumber < fileLines.Count && braceCount > 0)
            {
                string currentLine = fileLines[currentLineNumber].Trim();
                
                // Update brace count
                braceCount += currentLine.Count(c => c == '{');
                braceCount -= currentLine.Count(c => c == '}');
                
                if (braceCount == 0)
                {
                    currentLineNumber++;
                    break;
                }
                
                // Process the current line
                var nextProcessedLine = ProcessLineData(new UnprocessedLine(fileLines, currentLineNumber));
                
                // Skip empty lines and comments
                if (nextProcessedLine.LineType != LineType.Empty && 
                    nextProcessedLine.LineType != LineType.Comment)
                {
                    // For container lines, we need to process them as containers
                    if (nextProcessedLine.LineType == LineType.Container && nextProcessedLine.ContainerData != null)
                    {
                        // Create a container token for the content
                        var containerToken = new Token(
                            fileName,
                            nextProcessedLine.LineData,
                            currentLineNumber,
                            this
                        );
                        
                        // Set the container data
                        containerToken.Data = new TokenDataContainer(containerToken);
                        viewContent.Add(containerToken);
                    }
                    else if (nextProcessedLine.LineType == LineType.Variable)
                    {
                        // Process as a regular variable
                        var contentToken = new Token(
                            fileName, 
                            nextProcessedLine.LineData, 
                            currentLineNumber, 
                            this,
                            null,
                            null
                        );
                        
                        if (contentToken != null && contentToken.Data != null)
                        {
                            viewContent.Add(contentToken);
                        }
                    }
                }
                
                currentLineNumber++;
            }

            // Create the parameterized view token
            var token = new Token(
                fileName: fileName,
                lineData: $"view {viewName}({string.Join(", ", parameters)})",
                lineNumber: startLine,
                myTokenizer: this,
                parent: null,
                previousToken: null
            );
            
            try
            {
                // Set the token data with parent view support
                token.Data = new TokenDataParameterizedView(viewName, parentViewName, parameters, viewContent, token, FileTokens);
                FileTokens.Add(token);
                Formater.DebugMessage($"Successfully created parameterized view '{viewName}'{(parentViewName != null ? $" inheriting from '{parentViewName}'" : "")} with {viewContent.Count} content tokens");
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Failed to create parameterized view '{viewName}': {ex.Message}", token);
            }
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
                        case LineType.ParameterizedView:
                            Formater.DebugMessage($"Found parameterized view definition at line {currentLineNumber}");
                            ProcessParameterizedViewLine(currentLine, myFileLines, ref currentLineNumber, fileName);
                            // Skip the lines that were processed by ProcessParameterizedViewLine
                            currentLineNumber--; // Decrement because the loop will increment it
                            break;
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
                        case LineType.Comment:
                            break;
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
            
            // Handle // comments first
            int commentIndex = lineData.IndexOf("//");
            if (commentIndex >= 0)
            {
                if (commentIndex == 0)
                {
                    // Pure comment line
                    return new ProcessedLine(currentLineNumber + 1, LineType.Comment, lineData);
                }
                // Strip comment from end of line
                lineData = lineData.Substring(0, commentIndex).TrimEnd();
                
                // If line is empty after stripping comment, treat as empty line
                if (string.IsNullOrWhiteSpace(lineData))
                {
                    return new ProcessedLine(currentLineNumber + 1, LineType.Empty, lineData);
                }
            }
            
            // Check for parameterized view definition
            string trimmedLine = lineData.Trim();
            if (ParameterParser.IsParameterizedView(trimmedLine))
            {
                Formater.DebugMessage($"Found potential parameterized view: {trimmedLine}");
                return new ProcessedLine(currentLineNumber, LineType.ParameterizedView, trimmedLine);
            }
            
            if (lineData.Length >= 2)
            {

                if (lineData.Length > 3)
                {
                    // Handle block comments (###)
                if (lineData.Length > 2 && lineData[0] + "" + lineData[1] + lineData[2] == "###")
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

                // Handle single-line comments (##)
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
using System;
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

        public static TokenDataParameterizedView FindParameterizedView(List<Token> tokens, string viewName)
        {
            if (tokens == null || string.IsNullOrEmpty(viewName))
            {
                return null;
            }

            foreach (var token in tokens)
            {
                // Check for parameterized view
                if (token?.Data is TokenDataParameterizedView paramView && 
                    string.Equals(paramView.ViewName, viewName, StringComparison.OrdinalIgnoreCase))
                {
                    return paramView;
                }
                
                // Check for regular view (TokenDataContainer with ContainerType "view")
                if (token?.Data is TokenDataContainer container)
                {
                    if (container.ContainerType == "view" && 
                        string.Equals(container.ContainerName, viewName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Convert the regular view to a parameterized view with no parameters
                        return new TokenDataParameterizedView(
                            container.ContainerName,
                            null, // No parent view
                            new List<string>(), // No parameters
                            container.ContainerData,
                            token
                        );
                    }
                    
                    // Recursively search in container data
                    if (container.ContainerData != null)
                    {
                        var foundInContainer = FindParameterizedView(container.ContainerData, viewName);
                        if (foundInContainer != null)
                        {
                            return foundInContainer;
                        }
                    }
                }
            }
            
            return null;
        }

        public static Token FindTokenByName(List<Token> myTokenList, string tokenName, bool returnParent = false)
        {
            // Skip debug for common property types
            bool shouldLog = ShouldLogTokenSearch(tokenName);

            string[] subTokenNames = tokenName.Split('.');
            int remain = subTokenNames.Length;
            foreach (string localTokenName in subTokenNames)
            {
                remain--;
                foreach (Token myToken in myTokenList)
                {
                    if (myToken?.Data == null) continue;

                    if (myToken.Data is TokenDataVariable)
                    {
                        TokenDataVariable myVar = (TokenDataVariable)myToken.Data;
                        if (myVar.VariableName == localTokenName)
                        {
                            if (remain > 0)
                            {
                                if (shouldLog) Formater.TokenCriticalError("Not a container!", myToken);
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

            // Only log if it's a token we care about debugging
            if (shouldLog)
            {
                Formater.DebugMessage($"Token not found: {tokenName}");
            }
            return null;
        }

        private static bool ShouldLogTokenSearch(string tokenName)
        {
            // Don't log CSS properties
            if (Compiler.CSS.Properties.props.Contains(tokenName))
                return false;

            // Don't log common HTML attributes
            var commonAttrs = new[] {
        "type", "content", "style", "class", "id", "href", "src",
        "title", "alt", "width", "height", "name", "value", "target",
        "method", "action"
    };
            if (commonAttrs.Contains(tokenName))
                return false;

            // Don't log event handlers
            if (tokenName.StartsWith("on"))
                return false;

            // Only log specific types of tokens
            bool isSourceData = tokenName.Contains("_data");
            bool isKvPair = tokenName.Equals("key") || tokenName.Equals("value");
            bool isHeader = tokenName.Equals("headers");

            return isSourceData || isKvPair || isHeader;
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
            TokenDataContainer pageTokenDataContainer = (TokenDataContainer)myContainer.Data;
            if (pageTokenDataContainer == null)
            {
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
                                    Formater.TokenCriticalError("Token super not found " + varToken.VariableData, childToken);
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
        }

        public static TokenDataVariable FindTokenDataVariableByName(List<Token> myTokenList, string tokenName)
        {
            // Only debug log for source data or non-style/attribute lookups
            bool shouldLog = tokenName.Contains("_data") ||
                            (!IsStyleProperty(tokenName) && !IsHtmlAttribute(tokenName));

            Token foundToken = FindTokenByName(myTokenList, tokenName);
            if (foundToken != null)
            {
                if (foundToken.Data is TokenDataVariable myVar)
                {
                    return myVar;
                }
            }
            else if (shouldLog)
            {
                Formater.DebugMessage($"Token not found: {tokenName}");
            }

            return null;
        }

        // Helper methods to identify token types
        private static bool IsStyleProperty(string name)
        {
            // List of common CSS properties to avoid logging
            return Compiler.CSS.Properties.props.Contains(name);
        }

        private static bool IsHtmlAttribute(string name)
        {
            // List of common HTML attributes to avoid logging
            var htmlAttrs = new[] { "type", "content", "style", "class", "id", "href", "src" };
            return htmlAttrs.Contains(name);
        }

        public static TokenDataVariable TryResolveSourceReference(
            List<Token> fileTokens,
            string tokenName
        )
        {
            string[] parts = tokenName.Split('.');
            if (parts.Length < 2) return null;

            string sourceName = parts[0];
            Token sourceData = WebServer.SourceEndpoint.GetSourceData(fileTokens, sourceName);

            if (sourceData?.Data is TokenDataContainer container)
            {
                string dataPath = string.Join(".", parts.Skip(1));
                return FindTokenDataVariableByName(container.ContainerData, dataPath);
            }

            return null;
        }

        public static TokenDataVariable ResolveSourceReference(List<Token> tokens, string reference)
        {
            string[] parts = reference.Split('.');
            if (parts.Length < 2) return null;

            string sourceName = parts[0];
            string propertyPath = parts[1];

            var sourceToken = FindTokenByName(tokens, sourceName);
            if (sourceToken?.Data is TokenDataContainer container && container.ContainerType == "source")
            {
                // Look for the data in the source's container data
                foreach (Token dataToken in container.ContainerData)
                {
                    if (dataToken.Data is TokenDataContainer dataContainer && dataContainer.ContainerType == "data")
                    {
                        return FindTokenDataVariableByName(dataContainer.ContainerData, propertyPath);
                    }
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

### File: WebServer\Extensions\DictionaryExtensions.cs

```csharp
﻿using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BuckshotPlusPlus.WebServer.Extensions
{
    public static class DictionaryExtensions
    {
        public static async Task<TValue> GetOrAddAsync<TKey, TValue>(
    this ConcurrentDictionary<TKey, TValue> dictionary,
    TKey key,
    Func<TKey, Task<TValue>> valueFactory) where TValue : class
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (dictionary.TryGetValue(key, out TValue value) && value != null)
            {
                return value;
            }

            value = await valueFactory(key);
            if (value != null)
            {
                dictionary.TryAdd(key, value);
            }
            return value;
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
﻿using System.Collections.Generic;
using System.Linq;

namespace BuckshotPlusPlus.WebServer
{
    internal class Page
    {
        // TODO: hardcoded things
        static string _basicPage = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>";

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

            string page = _basicPage;
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

                        string metaArgs = string.Join(" ", meta.ContainerData.Select(n => {
                            TokenDataVariable localMetaVar = (TokenDataVariable)n.Data;
                            return localMetaVar.VariableName + "=" + '"' +  localMetaVar.GetCompiledVariableData(serverSideTokens, true) + '"';
                        }));

                        page += "<meta " + metaArgs + ">";
                    }
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

### File: WebServer\SourceEndpoint.cs

```csharp
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuckshotPlusPlus.Source;

namespace BuckshotPlusPlus.WebServer
{
    public static class SourceEndpoint
    {
        private static readonly Dictionary<string, BaseSource> SourceCache = new();

        public static async Task<Token> HandleSourceRequest(TokenDataContainer sourceContainer, Tokenizer tokenizer)
        {
            string sourceId = $"{sourceContainer.ContainerType}_{sourceContainer.ContainerName}";

            if (!SourceCache.ContainsKey(sourceId))
            {
                var source = BaseSource.CreateSource(sourceContainer, tokenizer);
                if (source != null)
                {
                    SourceCache[sourceId] = source;
                }
                else
                {
                    return null;
                }
            }

            return await SourceCache[sourceId].FetchDataAsync();
        }

        public static Token GetSourceData(List<Token> serverSideTokens, string sourceName)
        {
            var sourceToken = TokenUtils.FindTokenByName(serverSideTokens, sourceName);
            if (sourceToken?.Data is TokenDataContainer container)
            {
                return HandleSourceRequest(container, sourceToken.MyTokenizer).Result;
            }
            return null;
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
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BuckshotPlusPlus.Security;

namespace BuckshotPlusPlus.WebServer
{
    internal class WebServer
    {
        public HttpListener Listener;

        public async Task HandleIncomingConnections(Tokenizer myTokenizer)
        {
            UserSessionManager userSessions = new();
            
            SitemapGenerator.GenerateSitemapFromTokens(myTokenizer);

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (true)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await Listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                string absolutePath = req.Url!.AbsolutePath;
                if (absolutePath.StartsWith("/source/"))
                {
                    await HandleSourceRequest(ctx, myTokenizer);
                    continue;
                }

                if (absolutePath.Contains(".ico"))
                {
                    // TODO: put file in cache

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

                                    List<Token> serverSideTokenList = [.. myTokenizer.FileTokens];

                                    UserSession foundUserSession = userSessions.AddOrUpdateUserSession(req, resp);
                                    foundUserSession.AddUrl(req.Url.AbsolutePath);

                                    serverSideTokenList.Add(foundUserSession.GetToken(myTokenizer));

                                    // Write the response info
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
                        byte[] data = Encoding.UTF8.GetBytes(
                            "404 not found"
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

        private async Task HandleSourceRequest(HttpListenerContext ctx, Tokenizer myTokenizer)
        {
            var req = ctx.Request;
            var resp = ctx.Response;

            string sourceName = req.Url.AbsolutePath.Split('/').Last();

            var sourceToken = TokenUtils.FindTokenByName(myTokenizer.FileTokens, sourceName);
            if (sourceToken?.Data is TokenDataContainer container && container.ContainerType == "source")
            {
                var source = Source.BaseSource.CreateSource(container, myTokenizer);
                if (source != null)
                {
                    var sourceData = await source.FetchDataAsync();
                    if (sourceData?.Data is TokenDataContainer dataContainer)
                    {
                        // Convert container data to a dictionary
                        var responseData = new Dictionary<string, object>();
                        foreach (var token in dataContainer.ContainerData)
                        {
                            if (token.Data is TokenDataVariable variable)
                            {
                                responseData[variable.VariableName] = variable.VariableData;
                            }
                        }

                        string jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            success = true,
                            data = responseData
                        });

                        byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);
                        resp.ContentType = "application/json";
                        resp.ContentLength64 = buffer.Length;
                        await resp.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                        resp.Close();
                        return;
                    }
                }
            }

            // Handle error case
            string errorResponse = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                success = false,
                error = "Source not found or failed to fetch data"
            });
            byte[] errorBuffer = Encoding.UTF8.GetBytes(errorResponse);
            resp.ContentType = "application/json";
            resp.ContentLength64 = errorBuffer.Length;
            await resp.OutputStream.WriteAsync(errorBuffer, 0, errorBuffer.Length);
            resp.Close();
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
- Source Files: 37

Total files processed: 38
using System.Collections.Generic;

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

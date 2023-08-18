using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    class Events
    {
        static List<String> Props = new List<string>
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

        public static string GetHTMLEvents(List<Token> ServerSideTokens, Token MyToken)
        {
            string CompiledEvents = " ";

            TokenDataContainer ViewContainer = (TokenDataContainer)MyToken.Data;
            foreach (String Name in Props)
            {
                TokenDataVariable MyJSEventVar = TokenUtils.FindTokenDataVariableByName(
                    ViewContainer.ContainerData,
                    Name
                );

                if (MyJSEventVar != null)
                {
                    Token MyJSEvent = TokenUtils.FindTokenByName(
                        MyToken.MyTokenizer.FileTokens,
                        MyJSEventVar.GetCompiledVariableData(ServerSideTokens)
                    );

                    if (MyJSEvent != null)
                    {
                        CompiledEvents +=
                            Name + "=\"" + JS.Event.GetEventString(ServerSideTokens,MyJSEvent) + "\" ";
                    }
                }
            }

            return CompiledEvents;
        }
    }
}

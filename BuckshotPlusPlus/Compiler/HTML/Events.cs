using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BuckshotPlusPlus.Compiler.HTML
{
    class Events
    {
        // Form Events
        public string onblur = "";
        public string onchange = "";
        public string oncontextmenu = "";
        public string onfocus = "";
        public string oninput = "";
        public string oninvalid = "";
        public string onreset = "";
        public string onsearch = "";
        public string onselect = "";
        public string onsubmit = "";

        // Keyboard Events
        public string onkeydown = "";
        public string onkeypress = "";
        public string onkeyup = "";

        // Mouse Events
        public string onclick = "";
        public string ondblclick = "";
        public string onmousedown = "";
        public string onmousemove = "";
        public string onmouseout = "";
        public string onmouseover = "";
        public string onmouseup = "";
        public string onmousewheel = "";
        public string onwheel = "";

        // Drag Events
        public string ondrag = "";
        public string ondragend = "";
        public string ondragenter = "";
        public string ondragleave = "";
        public string ondragover = "";
        public string ondragstart = "";
        public string ondrop = "";
        public string onscroll = "";

        // Clipboard Events
        public string oncopy = "";
        public string oncut = "";
        public string onpaste = "";

        // Media Events
        public string onabort = "";
        public string oncanplay = "";
        public string oncanplaythrough = "";
        public string oncuechange = "";
        public string ondurationchange = "";
        public string onemptied = "";
        public string onended = "";
        public string onerror = "";
        public string onloadeddata = "";
        public string onloadedmetadata = "";
        public string onloadstart = "";
        public string onpause = "";
        public string onplay = "";
        public string onplaying = "";
        public string onprogress = "";
        public string onratechange = "";
        public string onseeked = "";
        public string onseeking = "";
        public string onstalled = "";
        public string onsuspend = "";
        public string ontimeupdate = "";
        public string onvolumechange = "";
        public string onwaiting = "";

        public static string GetHTMLEvents(Token MyToken)
        {
            string CompiledEvents = " ";
            //Formater.TokenCriticalError("LLLOOOOL", MyToken);
            FieldInfo[] HTMLEvents = typeof(HTML.Events).GetFields();
            TokenDataContainer ViewContainer = (TokenDataContainer)MyToken.Data;
            foreach (FieldInfo HTMLEvent in HTMLEvents)
            {
                TokenDataVariable MyJSEventVar = TokenUtils.FindTokenDataVariableByName(
                    ViewContainer.ContainerData,
                    HTMLEvent.Name
                );
                if (MyJSEventVar != null)
                {
                    Token MyJSEvent = TokenUtils.FindTokenByName(
                        MyToken.MyTokenizer.FileTokens,
                        MyJSEventVar.VariableData
                    );
                    if (MyJSEvent != null)
                    {
                        CompiledEvents +=
                            HTMLEvent.Name + "=\"" + JS.Event.GetEventString(MyJSEvent) + "\" ";
                        //Formater.TokenCriticalError("LLLOOOOL", MyJSEvent);
                    }
                }
            }
            return CompiledEvents;
        }
    }
}

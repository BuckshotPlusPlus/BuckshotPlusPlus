using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class Attributes
    {
        static List<(String, Boolean)> _props = new()
        {
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

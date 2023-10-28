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

using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.CSS
{
    public class Properties
    {
        static List<String> Props = new List<string>
        {
            "align_content",
            "align_items",
            "align_self",
            "animation_delay",
            "animation_direction",
            "animation_duration",
            "animation_fill_mode",
            "animation_iteration_count",
            "animation_name",
            "animation_play_state",
            "animation_timing_function",
            "animation",
            "background_attachment",
            "background_clip",
            "background_color",
            "background_image",
            "background_origin",
            "background_position",
            "background_repeat",
            "background_size",
            "background",
            "border_bottom_color",
            "border_bottom_left_radius",
            "border_bottom_right_radius",
            "border_bottom_style",
            "border_bottom_width",
            "border_bottom",
            "border_collapse",
            "border_color",
            "border_left_color",
            "border_left_style",
            "border_left_width",
            "border_left",
            "border_radius",
            "border_right_color",
            "border_right_style",
            "border_right_width",
            "border_right",
            "border_style",
            "border_top_color",
            "border_top_left_radius",
            "border_top_right_radius",
            "border_top_style",
            "border_top_width",
            "border_top",
            "border_width",
            "border",
            "bottom",
            "box_shadow",
            "box_sizing",
            "clear",
            "color",
            "column_count",
            "column_gap",
            "column_width",
            "css_content",
            "cursor",
            "display",
            "flex",
            "flex_basis",
            "flex_direction",
            "flex_flow",
            "flex_grow",
            "flex_shrink",
            "flex_wrap",
            "_float",
            "font_family",
            "font_size",
            "font_style",
            "font_variant",
            "font_weight",
            "font",
            "height",
            "justify_content",
            "left",
            "letter_spacing",
            "line_height",
            "list_style_image",
            "list_style_position",
            "list_style_type",
            "list_style",
            "margin_bottom",
            "margin_left",
            "margin_right",
            "margin_top",
            "margin",
            "max_height",
            "max_width",
            "min_height",
            "min_width",
            "mix_blend_mode",
            "opacity",
            "order",
            "outline_color",
            "outline_style",
            "outline_width",
            "outline",
            "overflow_wrap",
            "overflow_x",
            "overflow_y",
            "overflow",
            "padding_bottom",
            "padding_left",
            "padding_right",
            "padding_top",
            "padding",
            "pointer_events",
            "position",
            "resize",
            "right",
            "text_align",
            "text_decoration",
            "text_indent",
            "text_overflow",
            "text_shadow",
            "text_transform",
            "top",
            "transform_origin",
            "transform",
            "transition_delay",
            "transition_duration",
            "transition_property",
            "transition_timing_function",
            "transition",
            "white_space",
            "width",
            "will_change",
            "word_break",
            "word_spacing",
            "z_index",
            "filter",
            "user_select",
            "outline_offset"
        };

        public static string GetCSSString(Token MyToken)
        {
            string CompiledCSS = "";
            TokenDataContainer ViewContainer = (TokenDataContainer)MyToken.Data;
            
            foreach (String Name in Props)
            {
                TokenDataVariable MyCSSProp = TokenUtils.FindTokenDataVariableByName(
                    ViewContainer.ContainerData,
                    Name
                );
                if (MyCSSProp != null)
                {
                    if (MyCSSProp.VariableType == "ref" && MyCSSProp.RefData != null)
                    {
                        if (MyCSSProp.RefData.Data.GetType() == typeof(TokenDataVariable))
                        {
                            TokenDataVariable MyRefData = (TokenDataVariable)MyCSSProp.RefData.Data;
                            CompiledCSS +=
                                Name.Replace('_', '-') + ':' + MyRefData.VariableData + ";";
                        }
                    }
                    else
                    {
                        CompiledCSS +=
                            Name.Replace('_', '-') + ':' + MyCSSProp.VariableData + ";";
                    }
                }
            }
            TokenDataVariable MyFloatProp = TokenUtils.FindTokenDataVariableByName(
                ViewContainer.ContainerData,
                "float"
            );
            if (MyFloatProp != null)
            {
                CompiledCSS += "float:" + MyFloatProp.VariableData + ";";
            }
            return CompiledCSS;
        }

        public static bool isCSSProp(Token MyToken)
        {
            TokenDataVariable MyVar = (TokenDataVariable)MyToken.Data;
            foreach (String Prop in Props)
            {
                if (MyVar.VariableName == Prop)
                {
                    return true;
                }
            }
            return false;
        }

        public static string toDOMProp(string css_prop_name)
        {
            string[] result = css_prop_name.Split('_');
            for (int i = 1; i < result.Length; i++)
            {
                result[i] = char.ToUpper(result[i][0]) + result[i].Substring(1);
            }
            return String.Join("", result);
        }
    }
}

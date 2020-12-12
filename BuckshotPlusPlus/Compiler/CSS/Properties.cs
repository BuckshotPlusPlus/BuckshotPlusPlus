using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BuckshotPlusPlus.Compiler.CSS
{
    public class Properties
    {
		public string align_content = "";
		public string align_items = "";
		public string align_self = "";
		public string animation_delay = "";
		public string animation_direction = "";
		public string animation_duration = "";
		public string animation_fill_mode = "";
		public string animation_iteration_count = "";
		public string animation_name = "";
		public string animation_play_state = "";
		public string animation_timing_function = "";
		public string animation = "";
		public string background_attachment = "";
		public string background_clip = "";
		public string background_color = "";
		public string background_image = "";
		public string background_origin = "";
		public string background_position = "";
		public string background_repeat = "";
		public string background_size = "";
		public string background = "";
		public string border_bottom_color = "";
		public string border_bottom_left_radius = "";
		public string border_bottom_right_radius = "";
		public string border_bottom_style = "";
		public string border_bottom_width = "";
		public string border_bottom = "";
		public string border_collapse = "";
		public string border_color = "";
		public string border_left_color = "";
		public string border_left_style = "";
		public string border_left_width = "";
		public string border_left = "";
		public string border_radius = "";
		public string border_right_color = "";
		public string border_right_style = "";
		public string border_right_width = "";
		public string border_right = "";
		public string border_style = "";
		public string border_top_color = "";
		public string border_top_left_radius = "";
		public string border_top_right_radius = "";
		public string border_top_style = "";
		public string border_top_width = "";
		public string border_top = "";
		public string border_width = "";
		public string border = "";
		public string bottom = "";
		public string box_shadow = "";
		public string box_sizing = "";
		public string clear = "";
		public string color = "";
		public string column_count = "";
		public string column_gap = "";
		public string column_width = "";
		public string css_content = "";
		public string cursor = "";
		public string display = "";
		public string flex_basis = "";
		public string flex_direction = "";
		public string flex_flow = "";
		public string flex_grow = "";
		public string flex_shrink = "";
		public string flex_wrap = "";
		public string _float = "";
		public string font_family = "";
		public string font_size = "";
		public string font_style = "";
		public string font_variant = "";
		public string font_weight = "";
		public string font = "";
		public string height = "";
		public string justify_content = "";
		public string left = "";
		public string letter_spacing = "";
		public string line_height = "";
		public string list_style_image = "";
		public string list_style_position = "";
		public string list_style_type = "";
		public string list_style = "";
		public string margin_bottom = "";
		public string margin_left = "";
		public string margin_right = "";
		public string margin_top = "";
		public string margin = "";
		public string max_height = "";
		public string max_width = "";
		public string min_height = "";
		public string min_width = "";
		public string mix_blend_mode = "";
		public string opacity = "";
		public string order = "";
		public string outline_color = "";
		public string outline_style = "";
		public string outline_width = "";
		public string outline = "";
		public string overflow_wrap = "";
		public string overflow_x = "";
		public string overflow_y = "";
		public string overflow = "";
		public string padding_bottom = "";
		public string padding_left = "";
		public string padding_right = "";
		public string padding_top = "";
		public string padding = "";
		public string pointer_events = "";
		public string position = "";
		public string resize = "";
		public string right = "";
		public string text_align = "";
		public string text_decoration = "";
		public string text_indent = "";
		public string text_overflow = "";
		public string text_shadow = "";
		public string text_transform = "";
		public string top = "";
		public string transform_origin = "";
		public string transform = "";
		public string transition_delay = "";
		public string transition_duration = "";
		public string transition_property = "";
		public string transition_timing_function = "";
		public string transition = "";
		public string white_space = "";
		public string width = "";
		public string will_change = "";
		public string word_break = "";
		public string word_spacing = "";
		public string z_index = "";
		public string filter = "";

		public static string GetCSSString(Token MyToken)
		{
			string CompiledCSS = "";
			FieldInfo[] CSSProps = typeof(CSS.Properties).GetFields();
			TokenDataContainer ViewContainer = (TokenDataContainer)MyToken.Data;
			foreach (FieldInfo CSSProp in CSSProps)
			{
				TokenDataVariable MyCSSProp = TokenUtils.FindTokenDataVariableByName(ViewContainer.ContainerData, CSSProp.Name);
				if(MyCSSProp != null)
                {
					if(MyCSSProp.VariableType == "ref" && MyCSSProp.RefData != null)
                    {
						if(MyCSSProp.RefData.Data.GetType() == typeof(TokenDataVariable))
                        {
							TokenDataVariable MyRefData = (TokenDataVariable)MyCSSProp.RefData.Data;
							CompiledCSS += CSSProp.Name.Replace('_', '-') + ':' + MyRefData.VariableData + ";";
						}
						
                    }
                    else
                    {
						CompiledCSS += CSSProp.Name.Replace('_', '-') + ':' + MyCSSProp.VariableData + ";";
					}
					
				}
			}
			TokenDataVariable MyFloatProp = TokenUtils.FindTokenDataVariableByName(ViewContainer.ContainerData, "float");
			if (MyFloatProp != null)
			{
				CompiledCSS += "float:" + MyFloatProp.VariableData + ";";
			}
			return CompiledCSS;
		}

		public static bool isCSSProp(Token MyToken)
        {
			FieldInfo[] CSSProps = typeof(CSS.Properties).GetFields();
			TokenDataVariable MyVar = (TokenDataVariable)MyToken.Data;
			foreach (FieldInfo CSSProp in CSSProps)
			{
				if(MyVar.VariableName == CSSProp.Name)
                {
					return true;
                }
			}
			return false;
		}

		public static string toDOMProp(string css_prop_name)
        {
			string[] result = css_prop_name.Split('_');
			for(int i = 1; i < result.Length; i++)
            {
				result[i] = char.ToUpper(result[i][0]) + result[i].Substring(1);
            }
			return String.Join("", result);
        }
	}
}

﻿// Copyright © 2015-2017 Alex Kukhtin. All rights reserved.

using System;
using System.Windows.Markup;

namespace A2v10.Xaml
{
	public enum SpaceMode
	{
		None,
		Before,
		After
	}

	[ContentProperty("Content")]
	public class Span : Inline
	{
		public Object Content { get; set; }
		public Boolean Small { get; set; }
		public Boolean Big { get; set; }
		public SpaceMode Space { get; set; }

		internal override void RenderElement(RenderContext context, Action<TagBuilder> onRender = null)
		{
			if (SkipRender(context))
				return;
			if (Space == SpaceMode.Before)
				context.Writer.Write("&#160;");
			var span = new TagBuilder("span", null, IsInGrid);
			onRender?.Invoke(span);
			MergeAttributes(span, context);

			var cbind = GetBinding(nameof(Content));
			if (cbind != null)
			{
				span.MergeAttribute("v-text", cbind.GetPathFormat(context));
				if (cbind.NegativeRed)
				{
					if (GetBinding(nameof(Color)) != null)
						throw new XamlException("NegativeRed property is not compatible with Color binding");
					span.MergeAttribute(":class", $"$getNegativeRedClass({cbind.GetPath(context)})");
				}
			}
			span.AddCssClassBool(Small, "small");
			span.AddCssClassBool(Big, "text-big");

			span.RenderStart(context);
			if (Content is String)
				context.Writer.Write(context.Localize(Content.ToString()));
			span.RenderEnd(context);
			if (Space == SpaceMode.After)
				context.Writer.Write("&#160;");
		}
	}
}

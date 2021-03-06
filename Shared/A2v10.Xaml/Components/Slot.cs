﻿// Copyright © 2015-2017 Alex Kukhtin. All rights reserved.

using System;
using System.Windows.Markup;

namespace A2v10.Xaml
{
	[ContentProperty("Children")]
	public class Slot : UIElementBase
	{
		const String SLOT_ITEM = "__si__";

		public Object Scope { get; set; }
		public UIElement Fallback { get; set; }
		public UIElementCollection Children { get; set; } = new UIElementCollection();

		internal override void RenderElement(RenderContext context, Action<TagBuilder> onRender = null)
		{
			if (SkipRender(context))
				return;
			var scopeBind = GetBinding(nameof(Scope));
			if (scopeBind == null)
				throw new XamlException("Scope must be specified for Slot component");
			var tag = new TagBuilder("template", null, IsInGrid);
			tag.MergeAttribute("v-if", $"!!{scopeBind.GetPathFormat(context)}");
			tag.MergeAttribute("v-for", $"{SLOT_ITEM} in [{scopeBind.GetPath(context)}]");
			tag.RenderStart(context);
			using (var ctx = new ScopeContext(context, SLOT_ITEM))
			{
				foreach (var c in Children)
					c.RenderElement(context);
			}
			tag.RenderEnd(context);
			if (Fallback != null)
			{
				var fb = new TagBuilder("template");
				fb.MergeAttribute("v-if", $"!{scopeBind.GetPathFormat(context)}");
				fb.RenderStart(context);
				Fallback.RenderElement(context);
				fb.RenderEnd(context);
			}
		}
	}
}

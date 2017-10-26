﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace A2v10.Xaml
{
    [ContentProperty("Inlines")]

    public class Text : UIElementBase
    {
        public InlineCollection Inlines { get; } = new InlineCollection();

        public TextSize Size { get; set; }

        internal override void RenderElement(RenderContext context, Action<TagBuilder> onRender = null)
        {
            var tag = new TagBuilder("span", null, IsInGrid);
            MergeAttributes(tag, context);
            if (Size != TextSize.Normal)
                tag.AddCssClass("text-" + Size.ToString().ToLowerInvariant());
            tag.RenderStart(context);
            Inlines.Render(context);
            tag.RenderEnd(context);
            //throw new NotImplementedException();
        }
    }
}

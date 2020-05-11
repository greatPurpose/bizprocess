using System;
using Campus.Report.Base;

namespace Campus.Report.Elements
{
    public class TextElement : IElement
    {
        public String Value { get; set; }
        public Style Style { get; set; }

        public TextElement()
        {
            Style = new Style();
            Value = String.Empty;
        }
    }
}

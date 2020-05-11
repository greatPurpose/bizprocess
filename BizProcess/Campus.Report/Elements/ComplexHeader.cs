using System.Collections.Generic;
using Campus.Report.Base;

namespace Campus.Report.Elements
{
    public class ComplexHeader : List<ComplexHeaderCell>, IElement
    {
        public Style Style { get; set; }
    }
}

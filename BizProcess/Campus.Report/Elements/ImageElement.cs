using System;
using System.Drawing;
using Campus.Report.Base;

namespace Campus.Report.Elements
{
    public class ImageElement : IElement
    {
        public Image Image { get; set; }
        public Uri Url { get; set; }

        public ImageElement()
        {
        }

        public ImageElement(string url)
        {
            Url = new Uri(url);
        }

        public ImageElement(System.Drawing.Image image)
        {
            Image = image;
        }

        public Style Style { get; private set; }
    }
}

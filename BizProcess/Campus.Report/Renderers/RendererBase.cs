using Campus.Report.Base;

namespace Campus.Report.Renderers
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RendererBase : IRenderer
    {
        public abstract byte[] Render(global::Campus.Report.Base.Report report);

        public static string ColorToRgb(System.Drawing.Color color)
        {
            return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

    }
}

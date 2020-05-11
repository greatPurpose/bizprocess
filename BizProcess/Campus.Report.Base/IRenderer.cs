namespace Campus.Report.Base
{
    public interface IRenderer
    {
        byte[] Render(Campus.Report.Base.Report report);
    }
}

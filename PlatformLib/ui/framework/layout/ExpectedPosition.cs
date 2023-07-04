namespace PlatformLib.ui.framework.layout
{
    public enum ExpectedAlign
    {
        Default,
        Left,
        Right,
        Center,
        Start,
        End,
    }
    public interface IExpectedPosition
    {
        ExpectedAlign ExpectedAlign { get; set; }
    }
}

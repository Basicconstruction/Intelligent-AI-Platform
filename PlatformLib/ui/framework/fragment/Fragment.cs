namespace PlatformLib.ui.framework.fragment
{
    public interface IFragment:IPanel
    {
        void Init();
        void Destroy();
        void UpdateView();
    }
}

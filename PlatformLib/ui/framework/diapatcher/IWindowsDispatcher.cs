using System.Windows;

namespace PlatformLib.ui.framework.diapatcher
{
    public interface IWindowsDispatcher
    {
        FrameworkElement Dispatcher(IPanel panel,double width,double height);
        FrameworkElement Dispatcher(FrameworkElement frameworkElement);
    }
}
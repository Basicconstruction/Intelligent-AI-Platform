using System.Windows;
using PlatformLib.ui.framework;

namespace Intelligent_AI_Platform.dispatcher
{
    public interface IWindowsDispatcher
    {
        FrameworkElement Dispatcher(IPanel panel);
    }
}
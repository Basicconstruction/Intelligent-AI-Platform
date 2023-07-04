using System.Windows;
using Intelligent_AI_Platform.linker;
using PlatformLib.ui.framework;
using PlatformLib.ui.framework.activitymanager;
using PlatformLib.ui.framework.enumList;

namespace Intelligent_AI_Platform.dispatcher
{
    public class WindowsDispatcher: IWindowsDispatcher
    {
        public static readonly WindowsDispatcher OnlyDispatcher = new WindowsDispatcher();
        public FrameworkElement Dispatcher(IPanel panel)
        {
            var mainWindow = new MainWindow
            {
                Width = ((ISizeBox)panel).SWidth,
                Height = ((ISizeBox)panel).SHeight+MainWindow.FixHeight,
                // UiRootFrame = Linker.RootNavigator,
            };
            var oldWindow = (MainWindow)Application.Current.MainWindow;
            
            if (oldWindow != null)
            {
                mainWindow.UiRootFrame.GetPanels().AddRange(oldWindow.UiRootFrame.GetPanels());
                oldWindow.UiRootFrame.Navigate(null);
                oldWindow.Close();
            }

            Application.Current.MainWindow = mainWindow;
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((IActivityManager)mainWindow).Push(panel,PutStyle.FitParent);
            mainWindow.Show();
            return mainWindow;
        }
    }
}
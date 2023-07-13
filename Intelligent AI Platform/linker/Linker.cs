using System.Windows.Media;
using Intelligent_AI_Platform.dispatcher;
using Intelligent_AI_Platform.linker.manager;

namespace Intelligent_AI_Platform.linker
{
    public class Linker
    {
        public static readonly NavigatorManager NavigatorManager = NavigatorManager.AppNavigatorManager;
        public static readonly WindowsDispatcher WindowsDispatcher = WindowsDispatcher.OnlyDispatcher;
        //public static SessionGroup SessionGroup;
        public static readonly string Location = "preview";
        //public static Configuration Configuration;// = new Configuration();
        public static readonly SolidColorBrush ContextBrush = new SolidColorBrush(Colors.Chartreuse);
        public static readonly SolidColorBrush NotContextBrush = new SolidColorBrush(Colors.Coral);
    }
}
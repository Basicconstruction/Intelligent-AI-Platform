using Intelligent_AI_Platform.config;
using Intelligent_AI_Platform.dispatcher;
using Intelligent_AI_Platform.linker.manager;
using Intelligent_AI_Platform.Model.platform.app.GenericChat.chat;

namespace Intelligent_AI_Platform.linker
{
    public class Linker
    {
        public static readonly NavigatorManager NavigatorManager = NavigatorManager.AppNavigatorManager;
        public static WindowsDispatcher WindowsDispatcher = WindowsDispatcher.OnlyDispatcher;
        public static SessionGroup SessionGroup;
        private static bool Debug = true;
        public static readonly string Location = "preview";
        public static Configuration Configuration;// = new Configuration();
        public static void Init()
        {
            if (Debug)
            {
            }
        }
    }
}
using System;
using System.Collections.Generic;
using PlatformLib.ui.framework.activitymanager;

namespace Intelligent_AI_Platform.linker.manager
{
    public class NavigatorManager
    {
        public static readonly NavigatorManager AppNavigatorManager = new NavigatorManager();
        private readonly Dictionary<NavigatorLabel, IActivityManager> _dict = new Dictionary<NavigatorLabel, IActivityManager>();
        public void Register(NavigatorLabel key, IActivityManager manager)
        {
            _dict[key] = manager;
        }
        public IActivityManager GetActivityManager(NavigatorLabel key)
        {
            if (_dict.TryGetValue(key, out var manager))
            {
                return manager;
            }
            else
            {
                throw new Exception("错误来源，代码编写者，位于Navigator Manager抛出--某个使用的Activity未注册");
            }
        }
        public void Unregister(NavigatorLabel key)
        {
            _dict.Remove(key);
        }
    }

    public enum NavigatorLabel
    {
        Root,Chat
    }
}
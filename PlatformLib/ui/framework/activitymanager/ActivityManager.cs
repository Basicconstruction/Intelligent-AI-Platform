using PlatformLib.ui.framework.enumList;

namespace PlatformLib.ui.framework.activitymanager
{
    public interface IActivityManager
    {
        /*
        * 初始化
        * *
        */
        void InitActivity();

        IPanel PutIn(IPanel panel, PutInStrategy putInStrategy = PutInStrategy.Push,
            PutStyle putStyle = PutStyle.FitChild);
        IPanel PutIn(IPanel panel, double width,double height, PutInStrategy putInStrategy = PutInStrategy.Push,
            PutStyle putStyle = PutStyle.FitChild);
        
        /*
         * 移除并返回当前 页面Activity，页面显示之前的页面
         * *
         */
        IPanel PoP();
        /*
        * 替换当前的 页面
        * *
        */
        IPanel Replace(IPanel panel,PutStyle putStyle);
        /*
        * 压入一个页面，显示新的页面
        * *
        */
        IPanel Push(IPanel panel,PutStyle putStyle);
        IPanel BubbleUp(double width,double height);
        IPanel BubbleDown(double width, double height);

    }
}

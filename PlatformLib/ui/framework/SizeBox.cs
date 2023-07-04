namespace PlatformLib.ui.framework
{
    // Activity 抽象层盒子
    public interface ISizeBox
    {
        double SWidth { get; set; } 
        double SHeight { get; set; }// 可以认为是抽象层的高度和宽度
        double PreferredWidth { get; set; } // 意愿宽度
        double PreferredHeight { get; set; }// 意愿高度

        double SMinWidth// 最小宽度
        {
            get;set;
        }

        double SMaxWidth { get; set; }// 最大宽度
        double SMinHeight { get; set; }//最小高度
        double SMaxHeight { get; set; }// 最大高度
        double Flexibility// 伸缩意愿，当父组件发生大小变化时，父组件根据这个值来调整子组件的宽高
        {
            get;set;
        }
        
        
        
    }
}

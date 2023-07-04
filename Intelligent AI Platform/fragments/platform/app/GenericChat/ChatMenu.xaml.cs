using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using PlatformLib.ui.framework;
using PlatformLib.ui.framework.fragment;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat
{
    /// <summary>
    /// ChatMenu.xaml 的交互逻辑
    /// </summary>
    public partial class ChatMenu: IFragment,ISizeBox
    {
        public ChatMenu()
        {
            InitializeComponent();
            Init();
            
        }

        public void Init()
        {
            var fileContextMenu = new ContextMenu();
            var setting = new MenuItem
            {
                Header = "首选项",
            };
            setting.Click += (sender, args) =>
            {
                
            };
            var open = new MenuItem
            {
                Header = "打开",
            };
            open.Click += (sender, args) =>
            {

            };
            fileContextMenu.Items.Add(setting);
            fileContextMenu.Items.Add(open);
            fileContextMenu.Placement = PlacementMode.MousePoint;
            File.ContextMenu = fileContextMenu;
            File.Click += (sender, args) =>
            {
                var mousePosition = Mouse.GetPosition(this); // 获取鼠标位置
                const double offsetY = 10; // 向下偏移距离

                fileContextMenu.Placement = PlacementMode.MousePoint;
                fileContextMenu.VerticalOffset =  offsetY;
                fileContextMenu.IsOpen = true;
            };
        }

        public void Destroy()
        {
            
        }

        public void UpdateView()
        {
            
        }
        public double PreferredWidth
        {
            get => 800;
            set => throw new NotImplementedException();
        }
        public double PreferredHeight
        {
            get => 30;
            set => throw new NotImplementedException();
        }
        public double SWidth { get => Width; set => Width = value; }
        public double SHeight { get => Height; set => Height = value; }
        public double SMinWidth { get => MinWidth; set => MinWidth = value; }
        public double SMaxWidth { get => MaxWidth; set => MaxHeight = value; }
        public double SMinHeight { get => MinHeight; set => MinHeight = value; }
        public double SMaxHeight { get => MaxHeight; set => MaxHeight = value; }
        public double Flexibility { get => 1.0; set => throw new NotImplementedException(); }
    }
}

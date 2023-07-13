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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

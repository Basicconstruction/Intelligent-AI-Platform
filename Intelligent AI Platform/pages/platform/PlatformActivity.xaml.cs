using System;
using Intelligent_AI_Platform.linker;
using Intelligent_AI_Platform.linker.manager;
using Intelligent_AI_Platform.pages.platform.app.GenericChat;
using PlatformLib.ui.framework;
using PlatformLib.ui.framework.enumList;

namespace Intelligent_AI_Platform.pages
{
    public partial class PlatformActivity: IActivity,ISizeBox
    {
        public PlatformActivity()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            
        }

        public void Destroy()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateView()
        {
            throw new System.NotImplementedException();
        }

        public double PreferredWidth
        {
            get => 800;
            set => throw new NotImplementedException();
        }
        public double PreferredHeight
        {
            get => 450;
            set => throw new NotImplementedException();
        }
        public double SWidth { get => Width; set => Width = value; }
        public double SHeight { get => Height; set => Height = value; }
        public double SMinWidth { get => MinWidth; set => MinWidth = value; }
        public double SMaxWidth { get => MaxWidth; set => MaxHeight = value; }
        public double SMinHeight { get => MinHeight; set => MinHeight = value; }
        public double SMaxHeight { get => MaxHeight; set => MaxHeight = value; }
        public double Flexibility { get => 1.0; set => throw new NotImplementedException(); }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Linker.NavigatorManager.GetActivityManager(NavigatorLabel.Root).Replace(new ChatActivity(),PutStyle.FitParent);
        }
    }
}
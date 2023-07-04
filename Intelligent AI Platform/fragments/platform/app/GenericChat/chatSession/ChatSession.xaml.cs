using System;
using System.Threading;
using Intelligent_AI_Platform.linker;
using Intelligent_AI_Platform.Model.platform.app.GenericChat.chat;
using OpenAI;
using PlatformLib.ui.framework;
using PlatformLib.ui.framework.fragment;
using PlatformLib.ui.framework.layout;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession
{
    public partial class ChatSession : IFragment, ISizeBox
    {
        public ChatSession()
        {
            InitializeComponent();
            Init();
        }

        public SessionContext SessionContext
        {
            get;
            set;
        }

        private Session _session;
        public Session Session
        {
            set
            {
                _session = value;
                InitSession();
            }
            get => _session;
        }

        private void InitSession()
        {
            foreach (var talk in Session.Talks)
            {
                var bubble = new Bubble(talk.Participant.ToString(),
                    Vm.Width * 0.8, talk.Participant == Participant.User ? ExpectedAlign.Right : ExpectedAlign.Left,
                    talk.Content);
                Vm.UserSource.Add(new VerticalArrangedComponentManager.InnerObject()
                {
                    Element = bubble,
                    Time = 0
                });

            }
            Vm.Render();

        }
        public void Init()
        {
            Vm.Parent = this;
            InputBox.Send += async msg =>
            {
                //var config = Linker.Configuration;
                var talk = new Talk(Participant.User, msg) { Time= DateTimeOffset.Now.ToUnixTimeMilliseconds() };
                Session.Talks.Add(talk);
                SessionContext.AddContext(talk);
                var time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var source = new CancellationTokenSource();
                Vm.InsertBack(new Bubble("user",Vm.Width*0.8,
                    ExpectedAlign.Right,talk.Content),time,true);
                time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                InputBox.AddCancelToken(source);
                await Vm.PutNewTask(SessionContext, time,source);
                SessionGroup.Serialize(Linker.SessionGroup,Linker.Location);
                InputBox.RemoveStop();
                //await Vm.PutTask(msg, time);
            };
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
            get => 600;
            set => throw new NotImplementedException();
        }

        public double PreferredHeight
        {
            get => 720;
            set => throw new NotImplementedException();
        }

        public double SWidth
        {
            get => Width;
            set => Width = value;
        }

        public double SHeight
        {
            get => Height;
            set => Height = value;
        }

        public double SMinWidth
        {
            get => MinWidth;
            set => MinWidth = value;
        }

        public double SMaxWidth
        {
            get => MaxWidth;
            set => MaxHeight = value;
        }

        public double SMinHeight
        {
            get => MinHeight;
            set => MinHeight = value;
        }

        public double SMaxHeight
        {
            get => MaxHeight;
            set => MaxHeight = value;
        }

        public double Flexibility
        {
            get => 1.0;
            set => throw new NotImplementedException();
        }

        private void ChatSessionFragmentSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            Vm.Height = ActualHeight - 200;
            Vm.Width = ActualWidth;
            InputBox.Width = ActualWidth;
        }
    }
}
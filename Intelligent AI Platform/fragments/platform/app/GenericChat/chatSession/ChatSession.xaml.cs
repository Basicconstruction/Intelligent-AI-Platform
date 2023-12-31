﻿using System;
using System.Threading;
using Intelligent_AI_Platform.dataCenter;
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
        private bool _useContext = true;
        public bool UseContext
        {
            get => _useContext;
            set => _useContext = value; 
        }
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
                    Vm.Width * 0.85, talk.Participant == Participant.User ? ExpectedAlign.Right : ExpectedAlign.Left,
                    talk)
                { Vm = Vm
                };
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
            InputBox.Parent = this;
            InputBox.Send += async msg =>
            {
                //var config = Linker.Configuration;
                var talk = new Talk(Participant.User, msg) { Time= DateTimeOffset.Now.ToUnixTimeMilliseconds() };
                if (!UseContext)
                {
                    SessionContext.Clear();
                    Vm.ContextPaint();
                }
                Session.Talks.Add(talk);
                SessionContext.AddContext(talk);
                var time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var source = new CancellationTokenSource();
                Vm.InsertBack(new Bubble("user",Vm.Width*0.8,
                    ExpectedAlign.Right,talk){Vm = Vm},time,true);
                time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                InputBox.AddCancelToken(source);
                await Vm.PutNewTask(SessionContext, time,source);
                SessionGroup.Serialize(DataCenter.SessionGroup,Linker.Location);
                InputBox.RemoveStop();
                if (Session.Theme == null || Session.Theme.Trim() == Session.DefaultTheme)
                {
                    Session.Theme = Session.GetTheme(Session);
                    DataCenter.ChatList.UpdateView();
                }
                Vm.ContextPaint();
                GC.Collect();
                GC.WaitForFullGCComplete();
                
                //await Vm.PutTask(msg, time);
            };
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void UpdateView()
        {
            throw new NotImplementedException();
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
﻿#pragma checksum "..\..\..\..\..\..\fragments\platform\app\GenericChat\ChatList.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "02A0913D927CFD259775AF2940CFFFDE90C851234ED6EFF03822C86CBCAC5C6E"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat {
    
    
    /// <summary>
    /// ChatList
    /// </summary>
    public partial class ChatList : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\..\..\fragments\platform\app\GenericChat\ChatList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NewChat;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\..\..\..\fragments\platform\app\GenericChat\ChatList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ChatTopicList;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\..\..\fragments\platform\app\GenericChat\ChatList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Settings;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Intelligent_AI_Platform;component/fragments/platform/app/genericchat/chatlist.xa" +
                    "ml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\fragments\platform\app\GenericChat\ChatList.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\..\..\..\fragments\platform\app\GenericChat\ChatList.xaml"
            ((System.Windows.Controls.Canvas)(target)).SizeChanged += new System.Windows.SizeChangedEventHandler(this.ChatListSizeChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.NewChat = ((System.Windows.Controls.Button)(target));
            
            #line 9 "..\..\..\..\..\..\fragments\platform\app\GenericChat\ChatList.xaml"
            this.NewChat.Click += new System.Windows.RoutedEventHandler(this.NewChat_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ChatTopicList = ((System.Windows.Controls.ListView)(target));
            
            #line 10 "..\..\..\..\..\..\fragments\platform\app\GenericChat\ChatList.xaml"
            this.ChatTopicList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SelectNewSession);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Settings = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\..\..\..\fragments\platform\app\GenericChat\ChatList.xaml"
            this.Settings.Click += new System.Windows.RoutedEventHandler(this.Settings_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


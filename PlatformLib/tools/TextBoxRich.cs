using System;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PlatformLib.tools
{
    public class TextBoxRich: TextBox
    {
        public TextBoxRich()
        {
            ReadyToEdit = false;
            AcceptsReturn = true;
            TextWrapping = TextWrapping.Wrap;
            Foreground = Brushes.Gray;
            MouseEnter += ((sender, args) =>
            {
                Focus();
                if (!ReadyToEdit)
                {
                    if (Clipboard.GetText() != "")
                    {
                        Text = "按下 Tab 自动填入剪切板内容 \n" + Clipboard.GetText();
                    }
                    // Foreground = Brushes.Gray;
                }
                else
                {
                    // Foreground = Brushes.Black;
                }
            });
            MouseLeave += (sender, args) =>
            {
                if (!ReadyToEdit)
                {
                    PlaceHolder = _defaultPlaceHolder;
                }
            };
            PreviewKeyDown += (sender, args) =>
            {
                // Console.WriteLine("base down");
                if (ReadyToEdit) return;
                Text = args.Key == Key.Tab ? Clipboard.GetText() : "";
                SelectionStart = Text.Length;
                SelectionLength = 0;
                ReadyToEdit = true;
                if (args.Key == Key.Tab)
                {
                    args.Handled = true;
                }
            };
            TextChanged += ((sender, args) =>
            {
                // Console.WriteLine("base changed");
                if (Text != ""&&ReadyToEdit)
                {
                    // ReadyToEdit =true;
                    Foreground = Brushes.Black;
                }
                else
                {
                    ReadyToEdit = false;
                    Foreground = Brushes.Gray;
                }
                
            });

        }

        public new void Clear()
        {
            Text = "";
            ReadyToEdit = false;
        }

        private bool _readyEdit;
        public bool ReadyToEdit
        {
            get => _readyEdit;
            set
            {
                _readyEdit = value;
                if (ReadyToEdit)
                {
                    Foreground = Brushes.Black;
                }
            }
        }

        private string _placeholder;
        private string _defaultPlaceHolder;

        public string PlaceHolder
        {
            set
            {
                _placeholder = value;
                Text = _placeholder;
                _defaultPlaceHolder = value;
            }
            get => _placeholder;
        }
        
        
    }
}
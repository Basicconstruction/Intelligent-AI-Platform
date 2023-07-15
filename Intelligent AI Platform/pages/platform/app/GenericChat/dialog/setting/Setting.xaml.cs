using Intelligent_AI_Platform.config;
using System.Windows;
using System.Windows.Controls;
using Intelligent_AI_Platform.dataCenter;
using Intelligent_AI_Platform.linker;
using OpenAI.instance;
using System;

namespace Intelligent_AI_Platform.pages.platform.app.GenericChat.dialog.setting
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting
    {
        private readonly Configuration _configuration = DataCenter.Configuration.Copy();
        public Setting()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Temperature.Text = _configuration.Temperature+"";
            Address.Text = _configuration.ProviderUrl;
            MaxTokens.Text = _configuration.MaxTokens + "";
            RequestRate.Text = _configuration.RequestRate + "";
            switch (_configuration.Model)
            {
                case "gpt-3.5-turbo-0613":
                    Model.SelectedIndex = 0;
                    break;
                case "gpt-3.5-turbo-16k-0613":
                    Model.SelectedIndex = 1;
                    break;
                case "gpt-4-0613":
                    Model.SelectedIndex = 2;
                    break;
                case "gpt-4-32k-0613":
                    Model.SelectedIndex = 3;
                    break;
                default: 
                    Model.SelectedIndex = 4;
                    break;
            }
            RealModel.Text = _configuration.Model;
            Key.Password = _configuration.Key;
            FirstPrompt.Text = _configuration.FirstPrompt;
        }

        private void Official_Selected(object sender, RoutedEventArgs e)
        {
            Address.Text = "https://api.openai.com";
        }

        private void ShuaiBi_OnSelected(object sender, RoutedEventArgs e)
        {
            Address.Text = "https://api.openai-sb.com";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        void ApplySettings()
        {

            _configuration.Model = RealModel.Text.ToString();
            _configuration.ProviderUrl = Address.Text;
            var parse1 = double.TryParse(Temperature.Text,out var v);
            if (parse1)
            {
                _configuration.Temperature = v;
            }

            var parse2 = double.TryParse(MaxTokens.Text, out var v2);
            if (parse2)
            {
                _configuration.MaxTokens = (int)v2;
            }
            var parse3 = double.TryParse(RequestRate.Text, out var v3);
            if (parse3)
            {
                _configuration.RequestRate = v3;
            }

            _configuration.FirstPrompt = FirstPrompt.Text;
            _configuration.Key = Key.Password;
            OpenAi.ApiKey = _configuration.Key;
            OpenAi.BaseUrl = _configuration.ProviderUrl;
            var config = DataCenter.Configuration;
            config.Model = _configuration.Model;
            Console.WriteLine($"Setting {_configuration.Model}");
            config.Temperature = _configuration.Temperature;
            config.ProviderUrl = _configuration.ProviderUrl;
            config.MaxTokens = _configuration.MaxTokens;
            config.RequestRate = _configuration.RequestRate;
            config.Key = _configuration.Key;
            config.FirstPrompt = _configuration.FirstPrompt;
            Configuration.Serialize(DataCenter.Configuration,Linker.Location);
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            ApplySettings();    
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            ApplySettings();
            Close();
        }

        private void Model_Selected(object sender, RoutedEventArgs e)
        {
            if(RealModel != null)
            {
                if (Model.SelectedIndex <= 3)
                {
                    RealModel.Text = ((ComboBoxItem)Model.SelectedItem).Tag.ToString();
                }
                
            }
            
        }

        private void RealModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(RealModel != null)
            {
                switch (RealModel.Text)
                {
                    case "gpt-3.5-turbo-0613":
                        Model.SelectedIndex = 0;
                        break;
                    case "gpt-3.5-turbo-16k-0613":
                        Model.SelectedIndex = 1;
                        break;
                    case "gpt-4-0613":
                        Model.SelectedIndex = 2;
                        break;
                    case "gpt-4-32k-0613":
                        Model.SelectedIndex = 3;
                        break;
                    default:
                        Model.SelectedIndex = 4;
                        break;
                }
                e.Handled = true;
            }
            
        }
    }
}

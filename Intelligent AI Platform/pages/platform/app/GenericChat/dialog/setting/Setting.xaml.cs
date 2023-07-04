using System.Globalization;
using Intelligent_AI_Platform.config;
using System.Windows;
using System.Windows.Controls;
using Intelligent_AI_Platform.linker;
using OpenAI.instance;

namespace Intelligent_AI_Platform.pages.platform.app.GenericChat.dialog.setting
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        private Configuration _configuration = Linker.Configuration.Copy();
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
                case "gpt-3.5-turbo-0613-16":
                    Model.SelectedIndex = 1;
                    break;
                case "gpt-4-0613":
                    Model.SelectedIndex = 2;
                    break;
                case "gpt-4-0613-16":
                    Model.SelectedIndex = 3;
                    break;
            }
            Key.Text = _configuration.Key;
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
            _configuration.Model = ((ComboBoxItem)Model.SelectedItem).Tag.ToString();
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
                _configuration.RequestRate = (int)v3;
            }
            _configuration.Key = Key.Text;
            OpenAi.ApiKey = _configuration.Key;
            OpenAi.BaseUrl = _configuration.ProviderUrl;
            var config = Linker.Configuration;
            config.Model = _configuration.Model;
            config.Temperature = _configuration.Temperature;
            config.ProviderUrl = _configuration.ProviderUrl;
            config.MaxTokens = _configuration.MaxTokens;
            config.RequestRate = _configuration.RequestRate;
            config.Key = _configuration.Key;
            Configuration.Serialize(Linker.Configuration,Linker.Location);
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
    }
}

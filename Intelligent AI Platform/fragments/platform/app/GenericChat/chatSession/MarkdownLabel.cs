﻿using System.Windows.Controls;
using System.Windows.Documents;
using Markdig;
using Markdig.Wpf;
using Markdown = Markdig.Wpf.Markdown;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession
{
    // 这个类用来实现聊天框中的基本聊天组件中的文本组件
    //TextBlock
    public class MarkdownLabel: RichTextBox
    {
        public double DesignedWidth {
            set;get;
        }

        public string OriginalText
        {
            set;
            get;
        }

        private const int DefaultFontSize = 18;
        public bool UseMarkdownFlag = true;
        public MarkdownLabel(string content, double designedWidth)
        {
            DesignedWidth = designedWidth;
            OriginalText = content;
            content = content.Replace("\\n", "\n");
            IsReadOnly = true;
            var pipeline = new MarkdownPipelineBuilder().UseSupportedExtensions().Build();
            var flowDocument = Markdown.ToFlowDocument(content, pipeline);
            BorderThickness = new System.Windows.Thickness(0.0);
            // 在 RichTextBox 中显示解析后的内容
            //FontSize = 128;
            flowDocument.FontSize = DefaultFontSize;
            Document = flowDocument;
            MaxWidth = designedWidth;
        }

        public void UseMarkdown(bool useMarkdown)
        {
            UseMarkdownFlag = useMarkdown;
            if (useMarkdown)
            {
                var content = OriginalText.Replace("\\n", "\n");
                var pipeline = new MarkdownPipelineBuilder().UseSupportedExtensions().Build();
                var flowDocument = Markdown.ToFlowDocument(content, pipeline);
                Document = flowDocument;
            }
            else
            {
                var document = new FlowDocument();
                var paragraph = new Paragraph(new Run(OriginalText));
                document.Blocks.Add(paragraph);
                Document = document;
            }

            Document.FontSize = DefaultFontSize;
        }

        public void RePaint(string content, double designedWidth=-1)
        {
            OriginalText = content;
            if (designedWidth > 0)
            {
                DesignedWidth = designedWidth;
                MaxWidth = designedWidth;
            }
            UseMarkdown(UseMarkdownFlag);
        }
    }
}

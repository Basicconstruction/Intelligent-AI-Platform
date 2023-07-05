# Intelligent-AI-Platform  
使用C# wpf编写的chatgpt请求客户端。 目前支持基本的chat/completion 模型。   
优点是即使是debug下进行编译，压缩后软件包很小，目前只有1点多m。  
OpenAI 包是 https://github.com/anasfik/openai 的C# 迁移。 不过只迁移了部分，并且还有一些已经在chat/completion 解决的其他分块的bug。  
对于当前的主要使用 chat/completion 的流的部分没有什么副作用呢。  
![image](https://github.com/Basicconstruction/Intelligent-AI-Platform/assets/66370519/e4a1567e-2815-4cd7-a800-d53e73c1fa8b)
效果如上图。  
注意这不是付费产品，你需要提供自己的Api key ，以及自己的服务商api地址。  
目前，输出效果较差，C# 的富文本的显示效果对于格式复杂的文本显示效果并不好。  

预计支持以下特性：  
选中消息可支持上下文操作： 从此开始作为上下文消息，移出上下文，添加到上下文，  
显示原文，更改配置项和消息的文字显示大小，  
配置自定义的颜色，尤其是对于消息组件的前景色后景色等。  
储存多个秘钥或连接方案。  
精确的查找消息并跳转  
导出消息  
多任务并行请求（给定多个任务，然后多线程执行请求操作）。  
更优雅的客户端UI，高效的聊天消息虚拟列表。  


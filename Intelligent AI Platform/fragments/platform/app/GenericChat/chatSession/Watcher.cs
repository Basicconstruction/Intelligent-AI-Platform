namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession
{
    public interface IWatcher
    {
        bool NeedIgnore(string text);
    }

    public class LineWater: IWatcher
    {
        public bool NeedIgnore(string text)
        {
            if (text.Trim() == "")
            {
                return true;
            }

            return false;
        }
    }
}
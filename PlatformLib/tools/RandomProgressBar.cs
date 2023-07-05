using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PlatformLib.tools
{
    public class RandomProgressBar:ProgressBar
    {
        public volatile bool IsCancelled = false;
        public readonly Task InnerTask;
        public RandomProgressBar(double width = 200, double height = 40)
        {
            Width = width;
            Height = height;    
            InnerTask = Task.Run(async () =>
            {
                var r = new Random((int)DateTimeOffset.Now.Ticks);
                while (!IsCancelled)
                {
                    await Task.Delay(200);
                    await Dispatcher.InvokeAsync(() =>
                    {
                        if (Value < Maximum)
                        {
                            Value += r.Next(50);
                        }
                        else
                        {
                            Value = 0;
                        }
                    });
                }
            });
            //InnerTask.
        }
        
    }
}
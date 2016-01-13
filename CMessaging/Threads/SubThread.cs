
using System;
using System.Text;
using System.Threading;
using ILvYou.Zero.Config;
using ILvYou.Zero.Logs;
using ZeroMQ;

namespace ILvYou.Zero.Threads
{
    public class SubThread : ZeroThread
    {
        #region Field
        public ZeroRoute Config { get; set; }

        public string ExchangeName { get; set; }
        #endregion

        #region APIs
        public override void Run()
        {
            ZeroLog.LogInfo("thread run....");

            using (var context = ZmqContext.Create())
            {
                using (ZmqSocket socket = context.CreateSocket(SocketType.SUB))
                {
                    if (Config.SubscriptionPrefixes() == null ||
                        Config.SubscriptionPrefixes().Count == 0)
                    {
                        socket.SubscribeAll();
                    }
                    else
                    {
                        foreach (var subscriptionPrefix in Config.SubscriptionPrefixes())
                        {
                            socket.Subscribe(Encoding.UTF8.GetBytes(subscriptionPrefix));
                        }
                    }

                    foreach (var endPoint in Config.ConnectEndPoints())
                    {
                        socket.Connect(endPoint);
                    }

                    while (!this.Halted)
                    {
                        if (this.Halted)
                        {
                            break;
                        }

                        //3
                        ZeroLog.LogInfo("thread receive...." + DateTime.Now.ToLongTimeString());

                        var msg = socket.Receive(Encoding.UTF8, new TimeSpan(0, 0, Config.SendTimeout));

                        if (!string.IsNullOrEmpty(msg))
                        {
                            ZeroLog.LogInfo("thread receive...." + msg);
                        }
                    }
                }
            }
        }
        #endregion
    }
}

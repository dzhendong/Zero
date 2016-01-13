using ILvYou.Zero.Base;
using ILvYou.Zero.Config;
using ILvYou.Zero.Logs;
using ZeroMQ;

namespace ILvYou.Zero.Service
{
    public class PushService : AbstractService
    {
        #region Ctor
        private static PushService pushService = new PushService();

        public static PushService GetInstance()
        {
            return PushService.pushService;
        }

        private PushService()
        {
        }
        #endregion

        #region APIs
        public override void Bind(ZmqSocket socket, ZeroRoute config)
        {
            ZeroLog.LogInfo("push bind....");

            foreach (var endPoint in config.ConnectEndPoints())
            {
                socket.Connect(endPoint);

                ZeroLog.LogInfo("push bind....");
            }
        }

        public override void Connect(ZmqSocket socket, ZeroRoute config)
        {

        }
        #endregion
    }
}

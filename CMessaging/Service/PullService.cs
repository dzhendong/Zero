using ILvYou.Zero.Base;
using ILvYou.Zero.Config;
using ILvYou.Zero.Logs;
using ZeroMQ;

namespace ILvYou.Zero.Service
{
    public class PullService : AbstractService
    {
        #region Ctor
        private static PullService pullService = new PullService();

        public static PullService GetInstance()
        {
            return PullService.pullService;
        }

        private PullService()
        {
        }
        #endregion

        #region APIs
        public override void Bind(ZmqSocket socket, ZeroRoute config)
        {
            ZeroLog.LogInfo("pull bind....");

            foreach (var endPoint in config.ConnectEndPoints())
            {
                socket.Bind(endPoint);

                ZeroLog.LogInfo("pull bind....");
            }
        }

        public override void Connect(ZmqSocket socket, ZeroRoute config)
        {

        }
        #endregion
    }
}

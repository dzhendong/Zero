using System;
using System.Text;
using ZeroMQ;

namespace ILvYou.Zero.API
{
    /// <summary>
    /// 消息发送接口
    /// </summary>
    public interface TProcessor
    {
        #region interface
        SendStatus Send(string exchangeName, byte[] buffer);

        SendStatus Send(string exchangeName, string message);

        SendStatus SendMore(string exchangeName, byte[] buffer);

        SendStatus SendMore(string exchangeName, string message);

        string Receive(string exchangeName);

        Frame ReceiveFrame(string exchangeName);

        Frame ReceiveFrame(string exchangeName, Frame frame);

        SendStatus SendFrame(string exchangeName, Frame frame);

        ZmqMessage ReceiveMessage(string exchangeName);

        ZmqMessage ReceiveMessage(string exchangeName, ZmqMessage message);

        SendStatus SendMessage(string exchangeName, ZmqMessage message);

        int Receive(string exchangeName, byte[] buffer);


        int Receive(string exchangeName, byte[] buffer, SocketFlags flags);

        byte[] Receive(string exchangeName, byte[] buffer, out int size);


        byte[] Receive(string exchangeName, byte[] buffer, SocketFlags flags, out int size);

        int Send(string exchangeName, byte[] buffer, int size, SocketFlags flags);

        void Forward(string exchangeName, ZmqSocket destination);

        void AddTcpAcceptFilter(string exchangeName, string filter);

        void ClearTcpAcceptFilter(string exchangeName);
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goABClient.Scripts
{
    public class SocketCilent
    {
        private Socket client = null;
        private int HEAD_SIZE = 4;



        public SocketCilent()
        {
            InitClient();
        }

        public void InitClient()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ConnectServer();
        }

        private void ConnectServer()
        {

            if (client == null) InitClient();

            Config.ConnectState = ConnEnumStateCode.Connecting;
            client.BeginConnect(new IPEndPoint(IPAddress.Parse(Config.IPAddress), Config.Port), (connectCallBack) =>
             {
                 try
                 {
                    Socket socket = connectCallBack.AsyncState as Socket;
                     socket.EndConnect(connectCallBack);
                 }
                 catch (SocketException scokErr)
                 {
                     Console.WriteLine(scokErr);
                     Close("连接异常", string.Format("Error Code:{0},Error Message:{1}", scokErr.ErrorCode, scokErr.Message)
       , MessageBoxButtons.OK, MessageBoxIcon.Error);

                     return;
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine("connect server failed :" + ex);
                     Close("连接中异常", string.Format("Error Message:{0}", ex.Message),
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return;
                 }

                 Config.ConnectState = ConnEnumStateCode.Connected;
                 Console.WriteLine("连接成功!!!");
                 Task.Run(delegate { ReciveMsg(); });

             }, client);

        }



        /// <summary>
        /// 接收消息
        /// </summary>
        private void ReciveMsg()
        {
            byte[] buffer = new byte[HEAD_SIZE];
            int recvLen, recvLeft = HEAD_SIZE, pos = 0;
            SocketError socketError;

            try
            {
                //接收消息头数据
                while (HEAD_SIZE > 0)
                {
                    recvLen = client.Receive(buffer, pos, recvLeft, SocketFlags.None, out socketError);
                    if (recvLen == 0)
                    {
                        Close(string.Format("接收的消息头长度为:{0}",recvLen));
                        return;
                    }

                    if (recvLen < 0 || socketError != SocketError.Success)
                        throw new SocketException((int)socketError);

                    recvLeft -= recvLen;
                    pos += recvLen;
                }

                //获取消息体长度
                long msgLen = GetMessageLength(buffer);

                if (msgLen < 0 || msgLen > int.MaxValue - HEAD_SIZE)
                    throw new Exception("head size is invalid(无效的)");

                byte[] data = new byte[msgLen];

                recvLeft = (int)msgLen;
                pos = 0;

                //接收数据
                while (recvLeft>0)
                {
                    recvLen = client.Receive(data,pos,recvLeft,SocketFlags.None,out socketError);

                    recvLen = client.Receive(buffer, pos, recvLeft, SocketFlags.None, out socketError);
                    if (recvLen == 0)
                    {
                        Close();

                        return;
                    }

                    if (recvLen < 0 || socketError != SocketError.Success)
                        throw new SocketException((int)socketError);

                    recvLeft -= recvLen;
                    pos += recvLen;
                }

                //将数据放入消息队列中
                if (data==null)
                {
                    Close();
                    return;
                }

                MsgEvent msgEvent = ObjectPool<MsgEvent>.Instance.Get();
                msgEvent.Code = MsgEnumStateCode.ReadSuccess;
                msgEvent.Data = data;
                MsgQueue.Instance.Push(msgEvent);

            }
            catch (SocketException socketErr)
            {
                Console.WriteLine("SocketException recive message is failed:", socketErr);
                Close("接收消息异常",string.Format("Error Code:{0},Error Message:{1}",socketErr.ErrorCode, socketErr.Message), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception recive message is failed:", ex);
                Close("接收消息异常", string.Format("Error Message:{0}", ex.Message),MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        private void Close(string title=null, object content = null, MessageBoxButtons boxButtons = MessageBoxButtons.OK,MessageBoxIcon icon = MessageBoxIcon.None)
        {
            Console.WriteLine("关闭客户端");
            if (!string.IsNullOrEmpty(title))
            {
                MessageBox.Show(content.ToString(), title, boxButtons, icon);
            }

            if (Config.ConnectState == ConnEnumStateCode.Connecting || client ==null ) return;

                client.Disconnect(true);
                client.Dispose();
                client.Close();
                Config.ConnectState = ConnEnumStateCode.DisConnect;
        }




        /// <summary>
        /// 对消息头进行解码得到消息体长度
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private long GetMessageLength(byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}

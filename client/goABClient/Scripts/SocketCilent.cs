using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace goABClient.Scripts
{
    public class SocketCilent
    {
        private Socket client = null;
        private int HEAD_SIZE = 4;

        public SocketCilent(string address,int port)
        {
             
            Task.Run(delegate { InitClient(address, port);});

        }

        private void InitClient(string address, int port)
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(new IPEndPoint(IPAddress.Parse(address), port));
                Console.WriteLine("连接成功!!!");
                Console.WriteLine(address, port);

            }
            catch (Exception ex)
            {
                Console.WriteLine("connect server failed :"+ex);
                throw;
            }

            Task.Run(delegate { ReciveMsg(); });
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        private void ReciveMsg()
        {
            byte[] buffer = new byte[HEAD_SIZE];
            int recvLen, recvLeft = HEAD_SIZE, pos = 0;
            SocketError socketError;
            MsgStruct msgStruct;
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
                        Close(string.Format("接收的消息头长度为:{0}", recvLen));
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
                    Close("读取的消息为空");
                    return;
                }
                msgStruct = MsgStruct.Pool.Get();
                msgStruct.Code = MsgEnumCode.ConnectedSuccess;
                msgStruct.Data = data;
                MsgQueue.Instance.Push(msgStruct);
                MsgStruct.Pool.Put(msgStruct);

            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException recive message is failed:", ex);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception recive message is failed:", ex);
                throw;
            }

        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        private void Close(object recvLen = null)
        {
            Console.WriteLine(recvLen);
            client.Close();
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

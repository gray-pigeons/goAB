﻿using System;
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

        private static SocketCilent _instance;

        public static SocketCilent Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SocketCilent();
                }
                return _instance;
            }
        }


        private SocketCilent()
        {
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
            client.BeginConnect(new IPEndPoint(IPAddress.Parse(Config.IPAddress), Config.TcpPort), (connectCallBack) =>
             {
                 try
                 {
                    Socket socket = connectCallBack.AsyncState as Socket;
                     socket.EndConnect(connectCallBack);
                 }
                 catch (SocketException scokErr)
                 {
                     Console.WriteLine(scokErr);

                     MessageBox.Show("连接异常", string.Format("Error Code:{0},Error Message:{1}", scokErr.ErrorCode, scokErr.Message),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                     Close();
                     return;
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine("connect server failed :" + ex);
                     MessageBox.Show( string.Format("Error Message:{0}", ex.Message), "连接中异常",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                     Close();
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
                        Console.WriteLine(string.Format("接收的消息头长度为:{0}",recvLen));
                        Close();
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


                if (data==null)
                {
                    Close();
                    Console.WriteLine("接收的消息为null");
                    return;
                }

                //将数据放入消息队列中
                MsgEvent msgEvent = ObjectPool<MsgEvent>.Instance.Get();
                msgEvent.Code = MsgEnumStateCode.ReadSuccess;
                msgEvent.Data = data;
                MsgQueue.Instance.Push(msgEvent);

            }
            catch (SocketException socketErr)
            {
                Console.WriteLine("SocketException recive message is failed:", socketErr);
                MessageBox.Show(string.Format("Error Code:{0},Error Message:{1}",socketErr.ErrorCode, socketErr.Message), "接收消息异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception recive message is failed:", ex);
                MessageBox.Show( string.Format("Error Message:{0}", ex.Message), "接收消息异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;

            }

        }


        /// <summary>
        /// 给服务端发送消息
        /// </summary>
        /// <param name="msgEvent"></param>
        /// <returns></returns>
        public void SendMsg(byte[] data)
        {
            if (data.Length > int.MaxValue - HEAD_SIZE)
                throw new Exception("data is too long");

            byte[] headBuf = SetMessgeLength(data.Length);
            long totleLength = data.Length + HEAD_SIZE;
            byte[] totleBuffer = new byte[totleLength];
            headBuf.CopyTo(totleBuffer,0);
            data.CopyTo(totleBuffer, headBuf.Length);

            int needSendLen = totleBuffer.Length, alReadySendLen, pos = 0;
            while (needSendLen > 0)
            {
                alReadySendLen = client.Send(totleBuffer, 0, needSendLen, SocketFlags.None, out SocketError socketError);
                if (alReadySendLen < 0 || socketError != SocketError.Success)
                    throw new SocketException((int)socketError);

                needSendLen -= alReadySendLen;
                pos += alReadySendLen;
            }

        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        private void Close()
        {
            if (client!=null)
            {
                client.Close();
                client.Dispose();
                if (Config.ConnectState == ConnEnumStateCode.Connected)
                {
                    client.Disconnect(false);
                }
                return;
            }
            Config.ConnectState = ConnEnumStateCode.DisConnect;
        }


        /// <summary>
        /// 对消息头长度进行编码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] SetMessgeLength(long length)
        {
            return BitConverter.GetBytes(length);
        }

        /// <summary>
        /// 解码消息头
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private uint GetMessageLength(byte[] buffer)
        {
            return BitConverter.ToUInt32(buffer,0);
        }
    }
}

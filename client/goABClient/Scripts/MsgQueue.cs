using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goABClient.Scripts
{


    /// <summary>
    /// 消息代码 
    /// </summary>
    public enum MsgEnumCode
    {
        /// <summary>
        /// 开始连接服务端
        /// </summary>
        BeginConnect = 0,

        /// <summary>
        /// 与服务端连接建立成功
        /// </summary>
        ConnectedSuccess = 1,

        /// <summary>
        /// 与服务端连接建立失败
        /// </summary>
        ConnectedFailed = 2,

        /// <summary>
        /// 从服务端读取数据失败, 一般情况下为连接断开
        /// </summary>
        ReadFailed = 3,

        /// <summary>
        /// 从服务端读取数据成功
        /// </summary>
        ReadSuccess = 4,
    }


    /// <summary>
    /// 消息事件
    /// </summary>
    public struct MsgStruct
    {
        public  MsgEnumCode Code { get; set; }

        public  object Data { get; set; }

        public static StructPool<MsgStruct> Pool = new StructPool<MsgStruct>();
    }

    /// <summary>
    /// 消息队列
    /// </summary>
    public class MsgQueue
    {
        private static MsgQueue _instance;

        private MsgQueue() { }

        private object lockObj = new object();

        private Queue<MsgStruct> que_ = new Queue<MsgStruct>();

        public static MsgQueue Instance
        {
            get
            {
                if (_instance==null)
                {
                    _instance = new MsgQueue();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="msgEvent"></param>
        public void Push(MsgStruct msgEvent)
        {
            lock (lockObj)
            {
                que_.Enqueue(msgEvent);
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="msgEvent"></param>
        public MsgStruct Pop(MsgStruct msgEvent)
        {
            lock (lockObj)
            {
                if (que_.Count == 0)
                    return new MsgStruct{ };
                return que_.Dequeue();
            }
        }

    }


}

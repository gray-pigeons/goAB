using System.Collections.Generic;


namespace goABClient.Scripts
{

    /// <summary>
    /// 消息事件
    /// </summary>
    public class MsgEvent
    {
        public MsgEnumStateCode Code;

        public object Data;

    }

    /// <summary>
    /// 消息队列
    /// </summary>
    public class MsgQueue
    {
        private static MsgQueue _instance;

        private MsgQueue() { }

        private object lockObj = new object();

        private Queue<MsgEvent> que_ = new Queue<MsgEvent>();

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
        public void Push(MsgEvent msgEvent)
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
        public MsgEvent Pop(MsgEvent msgEvent)
        {
            lock (lockObj)
            {
                if (que_.Count == 0)
                    return new MsgEvent{ };
                return que_.Dequeue();
            }
        }

    }


}

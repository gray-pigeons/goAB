using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goABClient.Scripts
{


    /// <summary>
    /// 结构体数据对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StructPool<T> where T : struct
    {
        static StructPool<T> _instance;
        private object lockObj = new object();
        Stack<T> stack = new Stack<T>();

        public static StructPool<T> Instance {
            get 
            {
                if (_instance==null)
                {
                    _instance = new StructPool<T>();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 从池中获取
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            lock (lockObj)
            {
                if (stack.Count==0)
                {
                    return new T();
                }
                return stack.Pop();
            }
        }

        /// <summary>
        /// 把对象放入池中
        /// </summary>
        /// <param name="t"></param>
        public void Put(T t)
        {
            Console.WriteLine("这里应该判断是否为空");
            lock (lockObj)
            {
                stack.Push(t);
            }
        }


    }
}

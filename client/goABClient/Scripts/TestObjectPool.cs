using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goABClient.Scripts
{
    internal class TestObjectPool<T> where T : new()
    {
        private readonly Stack<T> stack = new Stack<T>();
        private readonly Action<T> actionOnGet;
        private readonly Action<T> actionOnRelease;

        public int countAll { get; private set; }
        public int countInactive { get { return stack.Count; } }
        public int countActive { get { return countAll - countInactive; } }

        public TestObjectPool(Action<T> actionOnGet,Action<T> actionOnRelease)
        {
            this.actionOnGet = actionOnGet;
            this.actionOnRelease = actionOnRelease;
        }

        /// <summary>
        /// 冲池子里获取
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            T element;
            if (stack.Count == 0)
            {
                element = new T();
                countAll++;
            }
            else
            {
                element = stack.Pop();
            }

            if (actionOnGet != null)
                actionOnGet(element);
            return element;
        }

        /// <summary>
        /// 放回池子里
        /// </summary>
        /// <param name="element"></param>
        public void Release(T element)
        {
            if (stack.Count > 0 && ReferenceEquals(stack.Peek(),element))
            {
                Console.WriteLine("Internal error, 尝试销毁已经释放到池中的对象");
            }

            if (actionOnRelease != null)
            {
                actionOnRelease(element);
            }
            stack.Push(element);
        }


    }
    internal static class ListPool<T>
    {

    }
}

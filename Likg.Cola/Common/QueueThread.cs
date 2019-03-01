using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Likg.Cola.Common
{
    public class QueueThread<T>
    {
        private Queue<T> queue;
        private int threadCount;
        private int threadUsed;//已使用的线程数
        private Action<T> action;
        private AutoResetEvent waitEvent;
        private ManualResetEvent exitEvent;
        private bool isExit = false;

        public QueueThread(Action<T> action)
        {
            this.queue = new Queue<T>();
            this.waitEvent = new AutoResetEvent(false);
            this.exitEvent = new ManualResetEvent(false);
            this.action = action;
        }

        public void Enqueue(T item)
        {
            if (isExit)
                throw new Exception("队列正在等待结束，不能添加新的处理项,请在调用WaitEnd前添加处理项");

            this.queue.Enqueue(item);
            this.waitEvent.Set();
        }

        public void Start(int threadCount)
        {
            if (threadCount < 1)
                throw new ArgumentException("参数错误，线程数必须大于1");

            this.threadCount = threadCount;
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                while (true)
                {
                    if (isExit && queue.Count < 1 && threadUsed == 0)
                        break;

                    if (queue.Count > 0 && (threadCount - threadUsed) > 0)
                    {
                        var item = queue.Dequeue();
                        Interlocked.Increment(ref threadUsed);
                        ThreadPool.QueueUserWorkItem((o) =>
                        {
                            action((T)o);
                            Interlocked.Decrement(ref threadUsed);
                            this.waitEvent.Set();
                        }, item);
                    }
                    else
                    {
                        this.waitEvent.WaitOne();
                    }
                }

                this.exitEvent.Set();
            });
        }

        public void WaitEnd()
        {
            if (this.threadCount < 1)
                throw new Exception("请先调用Start");

            this.isExit = true;
            this.waitEvent.Set();
            this.exitEvent.WaitOne();//等待通知退结束
        }
    }
}

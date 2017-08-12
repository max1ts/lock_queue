using System;
using System.Threading;

namespace LockQueue
{
    public class Program
    {
        private static readonly LockQueue<string> m_queue = new LockQueue<string>();

        public static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ReadThread);
            ThreadPool.QueueUserWorkItem(ReadThread);

            ThreadPool.QueueUserWorkItem(WriteThread);
            ThreadPool.QueueUserWorkItem(WriteThread);
            ThreadPool.QueueUserWorkItem(WriteThread);

            Console.ReadLine();
            m_queue.Stop();
            Console.ReadLine();
        }

        private static void WriteThread(object arg)
        {
            for (int n = 1; n <= 10; n++)
                m_queue.Push(Guid.NewGuid().ToString());
            Console.WriteLine("WriteThread {0} thread finished.", Thread.CurrentThread.ManagedThreadId);
        }

        private static void ReadThread(object arg)
        {
            string item;
            while ((item = m_queue.Pop()) != null)
            {
                Thread.Sleep(200);
                Console.WriteLine("From ReadThread {0}, value {1}", Thread.CurrentThread.ManagedThreadId, item);
            }
            Console.WriteLine("ReadThread {0} thread finished.", Thread.CurrentThread.ManagedThreadId);
        }
    }
}





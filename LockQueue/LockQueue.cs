using System.Collections.Generic;
using System.Threading;

namespace LockQueue
{
    public class LockQueue<T>
    {
        private readonly Queue<T> m_queue = new Queue<T>();
        private bool m_stop;

        public bool Push(T item)
        {
            if (m_stop)
                return false;

            lock (m_queue)
            {
                if (m_stop)
                    return false;

                m_queue.Enqueue(item);
                Monitor.Pulse(m_queue);
            }

            return true;
        }

        public T Pop()
        {
            if (m_stop)
                return default(T);

            lock (m_queue)
            {
                if (m_stop)
                    return default(T);

                while (m_queue.Count == 0)
                {
                    Monitor.Wait(m_queue);
                    if (m_stop)
                        return default(T);
                }

                return m_queue.Dequeue();
            }
        }

        public void Stop()
        {
            if (m_stop)
                return;

            lock (m_queue)
            {
                if (m_stop)
                    return;

                m_stop = true;
                Monitor.PulseAll(m_queue);
            }
        }
    }
}



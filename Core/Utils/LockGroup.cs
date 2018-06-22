using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class LockGroup<T>
    {
        private object[] locks;
        private Func<T, int> getLockFunc;
        public LockGroup(int lockCount, Func<T, int> getLockFunc)
        {
            this.locks = new object[lockCount];
            for (int i = 0; i < lockCount; i++)
            {
                this.locks[i] = new object();
            }
            this.getLockFunc = getLockFunc;
        }
        public object GetLock(T t)
        {
            return locks[(getLockFunc(t) % locks.Length + locks.Length) % locks.Length];
        }
    }
}

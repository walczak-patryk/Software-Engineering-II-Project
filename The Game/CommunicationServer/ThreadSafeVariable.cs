
namespace CommunicationServer
{
    internal class ThreadSafeVariable<T>
    {
        private readonly object locker = new object();

        private T variable = default;

        // Thread-safe access to Property using locking 
        public T Value
        {
            get
            {
                lock (locker)
                {
                    return variable;
                }
            }
            set
            {
                lock (locker)
                {
                    variable = value;
                }
            }
        }
    }
}

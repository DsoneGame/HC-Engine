using System.Collections.Generic;
using System;

namespace Engine.Events
{
    public class InterfaceEvent<T> : IEvent<T> where T : class
    {
        internal List<T> m_Subscribes = new List<T>();

        public virtual void Subscribe(T eventObject)
        {
            if (eventObject == null) throw new ArgumentNullException();

            m_Subscribes.Add(eventObject);
        }

        public virtual void Unsubscribe(T eventObject)
        {
            if (eventObject == null) throw new ArgumentNullException();

            m_Subscribes.Remove(eventObject);
        }

        public void UnsubscribeAll()
        {
            m_Subscribes.Clear();
        }

        public void CleanNulls()
        {
            m_Subscribes.RemoveAll(item => item == null || item.Equals(null));
        }

        public virtual void Invoke(Action<T> invoke)
        {
            List<T> subs = new List<T>(m_Subscribes);
            foreach (var item in subs)
            {
                if (item != null && !item.Equals(null))
                    invoke.Invoke(item);
            }
        }
    }
}
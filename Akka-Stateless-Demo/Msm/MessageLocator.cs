namespace Akka_Stateless_Demo.Msm
{
    public class MessageLocator
    {
        //private Dictionary<Type, object> _typeTofiredEventMap;
        private object _lastMessage;

        public MessageLocator()
        {
            //_typeTofiredEventMap = new Dictionary<Type, object>();
            _lastMessage = null;
        }

        public object LastMessage
        {
            get { return _lastMessage;  }
        }

        public void Set(object firedEvent)
        {
            //_typeTofiredEventMap[typeof(T)] = firedEvent;
            _lastMessage = firedEvent;
        }

        public bool TryGet<T>(out T firedEvent)
        {
            //var done = false;
            //object firedEventObj;
            //done = _typeTofiredEventMap.TryGetValue(typeof(T), out firedEventObj);
            //firedEvent = done ? (T) firedEventObj : default(T);
            //return done;
            var got = _lastMessage != null && _lastMessage.GetType() == typeof(T);
            firedEvent = got ? (T)_lastMessage : default(T);
            return got;
        }

        public void Unset()
        {
            _lastMessage = null;
        }
    }
}
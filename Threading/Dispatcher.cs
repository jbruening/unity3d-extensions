using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using Object = System.Object;

namespace System.Threading
{
    //needs to be monobehaviour so we can correctly get the thread dispatches should run on
    public class Dispatcher : MonoBehaviour
    {
        private static Dispatcher _instance;
        [ThreadStatic]
        private static bool _isUnityThread;

        public static Dispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    var gobj = new GameObject("Dispatcher Instance");
                    DontDestroyOnLoad(gobj);
                    _instance = gobj.AddComponent<Dispatcher>();
                }
                return _instance;
            }
        }

        public int MaxDispatchesPerUpdate = 50;
        public int MinDispatchesPerUpdate = 2;

        private class DispatchAction
        {
            public Action Action;
            public Action<object> ActionState;
            public object State;
        }

        Object _eLock = new Object();
        Queue<DispatchAction> _queuedActions = new Queue<DispatchAction>();
        Queue<DispatchAction> _enqueueQueue = new Queue<DispatchAction>();

        void Awake()
        {
            if (_instance == null)
                _instance = this;
            _isUnityThread = true;
        }

        void Update()
        {
            //using two queues to provide least amount of blocking to other threads.
            //using this as enqueuing takes significantly less time than dequeuing.
            lock (_eLock)
            {
                while (_enqueueQueue.Count > 0)
                {
                    _queuedActions.Enqueue(_enqueueQueue.Dequeue());
                }
            }

            //execute half of the actions
            var qSize = _queuedActions.Count / 2;
            qSize = Mathf.Clamp(qSize, MinDispatchesPerUpdate, MaxDispatchesPerUpdate);

            //make sure that we haven't min-clamped more than the number of actions that actually existed
            qSize = Mathf.Min(_queuedActions.Count, qSize);
            DispatchAction dispatch;
            while (_queuedActions.Count >= qSize && _queuedActions.Count != 0)
            {
                dispatch = _queuedActions.Dequeue();
                if (dispatch.Action != null)
                    dispatch.Action();
                else
                    dispatch.ActionState(dispatch.State);
            }
        }

        /// <summary>
        /// Run the action on the main unity thread
        /// even if this is called from the main unity thread, it will still be added to the end of the queue.
        /// </summary>
        /// <param name="action"></param>
        public void Dispatch(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            DispatchAction dispatch = new DispatchAction() { Action = action };
            
        }
        /// <summary>
        /// Run the action on the main unity thread, with the specified state
        /// even if this is called from the main unity thread, it will still be added to the end of the queue.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="state"></param>
        public void Dispatch(Action<object> action, object state)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            DispatchAction dispatch = new DispatchAction() { ActionState = action, State = state };
        }

        void QueueDispatch(DispatchAction dispatch)
        {
            if (_isUnityThread)
                _queuedActions.Enqueue(dispatch);
            else
            {
                lock (_eLock)
                {
                    _enqueueQueue.Enqueue(dispatch);
                }
            }
        }
    }
}

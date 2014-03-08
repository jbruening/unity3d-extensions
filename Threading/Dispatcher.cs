using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using Object = System.Object;

namespace UEx.Threading
{
    //needs to be monobehaviour so we can correctly get the thread dispatches should run on
    public class Dispatcher : MonoBehaviour
    {
        private static Dispatcher _instance;
        private static Thread _unityThread;

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

        Object _eLock = new Object();
        Queue<Action> _queuedActions = new Queue<Action>();
        Queue<Action> _enqueueQueue = new Queue<Action>();

        void Awake()
        {
            if (_instance == null)
                _instance = this;
            if (_unityThread == null)
                _unityThread = Thread.CurrentThread;
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
            while (_queuedActions.Count > qSize)
            {
                _queuedActions.Dequeue()();
            }
        }

        /// <summary>
        /// Run the action on the main unity thread
        /// </summary>
        /// <param name="action"></param>
        public void Dispatch(Action action)
        {
            lock (_eLock)
            {
                _enqueueQueue.Enqueue(action);
            }
        }
    }
}

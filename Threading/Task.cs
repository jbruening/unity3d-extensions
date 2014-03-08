using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using Object = System.Object;

namespace UEx.Threading.Tasks
{
    /// <summary>
    /// System.Threading.Tasks.Task implementation
    /// </summary>
    public class Task
    {
        private Action _action;
        private Action<object> _stateAction;
        private Object _asyncState;
        
        public Object AsyncState { get { return _asyncState; } }
        public int Id { get; internal set; }

        protected Task()
        {

        }

        public Task(Action action)
        {
            // TODO: Complete member initialization
            _action = action;
            //Id = TaskMaster.GetId();
        }

        public Task(Action<Object> action, Object asyncState)
        {
            _stateAction = action;
            _asyncState = asyncState;
            //Id = TaskMaster.GetId();
        }

        public void Start()
        {
            TaskMaster.Run(this);
        }

        /// <summary>
        /// Original Task.Wait implementation (blocks the thread)
        /// </summary>
        public void WaitThread()
        {
            while (!IsCompleted)
            {
                //use some kind of coroutine thing via taskmaster instead of sleeping?
                Thread.Sleep(0);
            }
        }

        /// <summary>
        /// Yields upon completion of the task. ONLY WORKS FROM A COROUTINE, INSIDE MAIN UNITY THREAD
        /// <remarks>
        /// void Wait() was not created due to the fact that it would hang the application until completion.
        /// for the original wait implementation, use WaitThread
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public Coroutine Wait()
        {
            return Dispatcher.Instance.StartCoroutine(WaitRoutine());
        }

        IEnumerator WaitRoutine()
        {
            while (!IsCompleted)
            {
                yield return null;
            }
        }

        internal void DoTask()
        {

        }

        public static Task Run(Action action)
        {
            var task = new Task(action);

            task.Start();
            return task;
        }

        public static Task<TResult> Run<TResult>(Func<TResult> function)
        {
            var task = new Task<TResult>();

            task.Start();
            return task;
        }

        public bool IsCompleted { get; set; }
    }

    public class Task<TResult> : Task
    {
        public TResult Result { get; private set; }

    }

    internal static class TaskMaster
    {
        //todo: use some kind of coroutine thing so that more tasks can execute than threads

        static Object _tLock = new Object();
        static Dictionary<int, Task> _tasks = new Dictionary<int, Task>();
        static int _idCounter;

        public static void Run(Task task)
        {
            ThreadPool.QueueUserWorkItem(OnThreadBecameAvailable, task);
        }

        public static int GetId()
        {
            //lock collisions if we make tasks inside of other tasks
            int ret;
            lock (_tLock)
            {
                ret = ++_idCounter;
            }
            return ret;
        }

        private static void OnThreadBecameAvailable(Object state)
        {
            //var id = (int)state;
            //_tasks[id].DoTask();
            var task = state as Task;
            //lock (_tLock)
            //{
            //    _tasks.Remove(id);
            //}
        }
    }
}

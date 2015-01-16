using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using Object = System.Object;

namespace System.Threading.Tasks
{
    /// <summary>
    /// System.Threading.Tasks.Task implementation
    /// </summary>
    public class Task
    {
        private Action _action;
        private Action<object> _stateAction;
        protected Object _asyncState;
        
        public Object AsyncState { get { return _asyncState; } }
        public int Id { get; internal set; }

        private Object _statLock = new Object();
        public TaskStatus Status { get; internal set; }

        public AggregateException Exception { get; private set; }
        
        TaskScheduler _scheduler;

        protected Task()
        {
            Status = TaskStatus.Created;
            _scheduler = TaskScheduler.Default;
            //Id = TaskMaster.GetId();
        }

        public Task(Action action) : this()
        {
            if (action == null)
                throw new ArgumentNullException("action");
            
            _action = action;
        }

        public Task(Action<Object> action, Object asyncState) : this()
        {
            if (action == null)
                throw new ArgumentNullException("action");

            _stateAction = action;
            _asyncState = asyncState;
        }

        public void Start()
        {
            _scheduler.InternalQueueTask(this);
        }

        /// <summary>
        /// Original Task.Wait implementation (blocks the thread)
        /// If calling from unity thread, use the Coroutine Wait() method
        /// </summary>
        public void WaitTask()
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
        /// for the original wait implementation, use WaitTask
        /// It is faster to do your own wait spin, via doing a while(!task.IsCompleted) yield return null
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

        internal void InternalExecuteTask()
        {
            Status = TaskStatus.Running;
            try
            {
                ExecuteTask();
                Status = TaskStatus.RanToCompletion;
            }
            catch (Exception e)
            {
                Status = TaskStatus.Faulted;
                Exception = new AggregateException(e);
            }
            IsCompleted = true;
            //cleanup
            _scheduler = null;
        }

        protected virtual void ExecuteTask()
        {
            if (_action != null)
            {
                _action();
            }
            else
            {
                _stateAction(_asyncState);
            }
        }

        public static Task Run(Action action)
        {
            var task = new Task(action);
            task.Start();
            return task;
        }

        public static Task<TResult> Run<TResult>(Func<TResult> function)
        {
            var task = new Task<TResult>(function);

            task.Start();
            return task;
        }

        public bool IsCompleted { get; protected set; }
    }

    public class Task<TResult> : Task
    {
        /// <summary>
        /// get the result of the task. Blocking. It is recommended you yield on the wait before accessing this value
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public TResult Result
        {
            get
            {
                while (!IsCompleted)
                {
                    Thread.Sleep(0);
                }
                return _result;
            }
        }

        private TResult _result;

        private Func<TResult> _function;
        private Func<Object, TResult> _stateFunction;

        public Task(Func<TResult> function) : base()
        {
            if (function == null)
                throw new ArgumentNullException("function");

            _function = function;
        }

        public Task(Func<Object, TResult> function, Object asyncState) : base()
        {
            if (function == null)
                throw new ArgumentNullException("function");

            _stateFunction = function;
            _asyncState = asyncState;
        }

        protected override void ExecuteTask()
        {
            if (_function != null)
            {
                _result = _function();
            }
            else
            {
                _result = _stateFunction(_asyncState);
            }
        }
    }
}

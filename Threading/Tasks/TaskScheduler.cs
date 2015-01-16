using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Threading.Tasks
{
    public abstract class TaskScheduler
    {
        static TaskScheduler _default;
        public static TaskScheduler Default
        {
            get
            {
                return _default ?? (_default = new LimitedConcurrencyLevelTaskScheduler(Environment.ProcessorCount * 4));
            }
        }

        public abstract int MaximumConcurrencyLevel { get; }

        protected abstract void QueueTask(Task task);
        protected abstract bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued);
        protected abstract bool TryDequeue(Task task);
        protected abstract IEnumerable<Task> GetScheduledTasks();

        /// <summary>
        /// Attempts to execute the provided System.Threading.Tasks.Task on this scheduler.
        /// </summary>
        /// <param name="task">A System.Threading.Tasks.Task object to be executed.</param>
        /// <returns>A Boolean that is true if task was successfully executed, false if it was
        ///     not. A common reason for execution failure is that the task had previously
        ///     been executed or is in the process of being executed by another thread.</returns>
        protected bool TryExecuteTask(Task task)
        {
            bool doTask;
            lock (task)
            {
                doTask = task.Status != TaskStatus.Created;
                if (doTask)
                    task.Status = TaskStatus.WaitingToRun;
            }

            if (doTask)
            {
                task.InternalExecuteTask();
                return true;
            }
            else
                return false;
        }

        internal void InternalQueueTask(Task task)
        {
            QueueTask(task);
        }
    }
}

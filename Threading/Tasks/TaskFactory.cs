using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Threading.Tasks
{
    public class TaskFactory
    {
        private TaskScheduler _scheduler;

        public TaskFactory(TaskScheduler scheduler)
        {
            _scheduler = scheduler;
        }
    }
}

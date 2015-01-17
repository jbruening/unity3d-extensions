using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace UEx
{
    public class Dispatcher : MonoBehaviour
    {
        readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
        private static Dispatcher _current;
        public static Dispatcher Current { get { return _current; } }

        public static async Task InvokeAsync(Action action)
        {
            if (_current == null)
                throw new Exception("please run Dispatcher.Initialize() or attach the component to a gameobject in the starting scene");

            var tks = new TaskCompletionSource<bool>();
            _current._actions.Enqueue(() =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                tks.SetResult(true);
            });
            await tks.Task;
        }

        public static void Initialize()
        {
            if (!Application.isPlaying)
                return;
            if (_current != null) return;
            var g = new GameObject("Dispatcher");
            var d = g.AddComponent<Dispatcher>();
            d.InitializeAsThis();
        }

        void InitializeAsThis()
        {
            _current = this;
            DontDestroyOnLoad(this);
        }

        void Awake()
        {
            if (_current == null)
                InitializeAsThis();
            else
                Destroy(this);
        }

        void Update()
        {
            Action action;
            while (_actions.TryDequeue(out action))
            {
                if (action == null)
                    break;
                action();
            }
        }

        void OnDestroy()
        {
            if (_current == this)
            {
                _current = null;
            }
        }
    }

    public static class TaskExtensions
    {
        public static Coroutine Yield(this Task task)
        {
            return Dispatcher.Current.StartCoroutine(CompletionEnumerator(task));
        }

        private static IEnumerator CompletionEnumerator(Task task)
        {
            while (!task.IsCompleted)
                yield return null;
        }
    }
}

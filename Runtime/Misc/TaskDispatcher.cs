using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandcraftedGames.Utils
{
    public class TaskDispatcher : MonoBehaviour
    {
        private static TaskDispatcher _shared;
        public static TaskDispatcher Shared
        {
            get
            {
                if(_shared == null)
                    _shared = Create("General Purpose");
                return _shared;
            }
        }

        public static TaskDispatcher Create(string name)
        {
            var go = new GameObject("TaskDispatcher - " + name);
            DontDestroyOnLoad(go);
            return go.AddComponent<TaskDispatcher>();
        }

        public void Stop(object task)
        {
            var coroutine = task as Coroutine;
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        /// <summary>
        /// Schedules a single action with delay
        /// </summary>
        /// <param name="action">Action to schedule</param>
        /// <param name="delay">Delay in realtime in seconds</param>
        /// <returns>task object for cancelling</returns>
        public object Schedule(System.Action action, float delay)
        {
            return StartCoroutine(ScheduleAction(action, delay));
        }

        private IEnumerator ScheduleAction(System.Action action, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            action();
        }

        /// <summary>
        /// Schedules an repeating action with delay until the action returns false
        /// </summary>
        /// <param name="action">Action to repeat until it returns false</param>
        /// <param name="delay">Delay in realtime in seconds</param>
        /// <param name="interval">Interval between task. Defaults to 0.0 which means it runs every frame.</param>
        /// <returns>task object for cancelling</returns>
        public object Schedule(System.Func<bool> action, float delay, float interval = 0.0f)
        {
            return StartCoroutine(ScheduleAction(action, delay, interval));
        }

        private IEnumerator ScheduleAction(System.Func<bool> action, float delay, float interval)
        {
            yield return new WaitForSecondsRealtime(delay);
            while(action())
            {
                if(interval > 0)
                    yield return new WaitForSecondsRealtime(interval);
                else
                    yield return null;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandcraftedGames.Utils
{
    public abstract class TweenBase<T>: ITween<T>
    {
        private T value;
        public T Value => value;

        private object runningTask = null;
        public bool IsRunning => runningTask != null;
        private float timer = 0.0f;

        public TweenBase(T initialValue)
        {
            value = initialValue;
        }

        public void Start(T startValue, T endValue, float duration)
        {
            if(IsRunning)
                return;

            timer = 0.0f;

            runningTask = TaskDispatcher.Shared.Schedule(() => {
                timer += Time.unscaledDeltaTime;
                value = Increment(startValue, endValue, timer/duration);
                if(timer >= duration)
                {
                    runningTask = null;
                    this.value = endValue;
                    return false;
                }
                return true;
            }, 0.0f);
        }

        protected abstract T Increment(T startValue, T endValue, float t);

        public void Stop()
        {
            TaskDispatcher.Shared.Stop(runningTask);
            runningTask = null;
        }
    }
}
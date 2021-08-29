using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandcraftedGames.Utils
{
    public interface ITween<T>
    {
        T Value { get; }

        bool IsRunning { get; }

        void Start(T startValue, T endValue, float duration);
        void Stop();
    }

    public static class ITweenExtension
    {
        public static void TweenTo<T>(this ITween<T> tween, T endValue, float duration)
        {
            tween.Start(tween.Value, endValue, duration);
        }
    }
}
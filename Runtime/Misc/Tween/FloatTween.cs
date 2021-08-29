using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandcraftedGames.Utils
{
    public class FloatTween : TweenBase<float>
    {
        protected override float Increment(float startValue, float endValue, float t)
        {
            return Mathf.Lerp(startValue, endValue, t);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HandcraftedGames.Utils.Tests
{
    public class TaskDispatcherTests
    {
        [UnityTest]
        public IEnumerator TestStop()
        {
            var dispatcher = TaskDispatcher.Shared;
            int counter = 0;
            var boxedCounter = (object)counter;
            var cancelToken = dispatcher.Schedule(() => {
                var unboxedCounter = (int)boxedCounter;
                unboxedCounter += 1;
                boxedCounter = (object)unboxedCounter;
                return true;
            }, 0.0f);
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            dispatcher.Stop(cancelToken);
            yield return null;
            Assert.AreEqual(4, (int)boxedCounter);
        }

        [UnityTest]
        public IEnumerator TestStopAndImmediateStart()
        {
            var dispatcher = TaskDispatcher.Shared;
            int counter = 0;
            var boxedCounter = (object)counter;
            var cancelToken = dispatcher.Schedule(() => {
                var unboxedCounter = (int)boxedCounter;
                unboxedCounter += 1;
                boxedCounter = (object)unboxedCounter;
                return true;
            }, 0.0f);
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            boxedCounter = (object)((int)0);
            dispatcher.Stop(cancelToken);
            dispatcher.Schedule(() => {
                var unboxedCounter = (int)boxedCounter;
                unboxedCounter += 1;
                boxedCounter = (object)unboxedCounter;
                return true;
            }, 0.0f);
            yield return null;
            Assert.AreEqual(1, (int)boxedCounter);
        }
    }
}
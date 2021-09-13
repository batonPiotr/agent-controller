using NUnit.Framework;
using HandcraftedGames.Utils;
using UnityEngine;

namespace HandcraftedGames.AgentController.Tests
{
    public class GenericDrawerTests
    {
        [Test]
        public void TestRegisteringAndDrawing()
        {
            GenericDrawer.Shared = new GenericDrawer();
            var drawer = new DrawerCallCounter<int>();

            GenericDrawer.Shared.Register(drawer);
            GenericDrawer.Shared.Draw(0);
            GenericDrawer.Shared.Draw(0, Rect.zero);
            Assert.AreEqual(1.0f, GenericDrawer.Shared.GetPropertyHeight(0, new GUIContent()));
            Assert.AreEqual(1, drawer.Draw0Counts);
            Assert.AreEqual(1, drawer.Draw1Counts);
            Assert.AreEqual(1, drawer.GetHeightCounts);
        }

        [Test]
        public void TestGenericsRegisteringAndDrawing()
        {
            GenericDrawer.Shared = new GenericDrawer();
            var drawer = new DrawerCallCounter<A>();

            GenericDrawer.Shared.Register(drawer);
            GenericDrawer.Shared.Draw(new A());
            GenericDrawer.Shared.Draw(new A(), Rect.zero);
            Assert.AreEqual(1.0f, GenericDrawer.Shared.GetPropertyHeight(new A(), new GUIContent()));
            Assert.AreEqual(1, drawer.Draw0Counts);
            Assert.AreEqual(1, drawer.Draw1Counts);
            Assert.AreEqual(1, drawer.GetHeightCounts);
        }

        [Test]
        public void TestGenericsRegisteringAndDrawingOfFalseType()
        {
            GenericDrawer.Shared = new GenericDrawer();
            var drawer = new DrawerCallCounter<A>();

            GenericDrawer.Shared.Register(drawer);
            Assert.That(() => GenericDrawer.Shared.Draw(new B()), Throws.Exception);
            Assert.That(() => GenericDrawer.Shared.Draw(new B(), Rect.zero), Throws.Exception);
            Assert.That(() => GenericDrawer.Shared.GetPropertyHeight(new B(), new GUIContent()), Throws.Exception);
            Assert.AreEqual(0, drawer.Draw0Counts);
            Assert.AreEqual(0, drawer.Draw1Counts);
            Assert.AreEqual(0, drawer.GetHeightCounts);

        }
        private class A {}
        private class B: A {}
        private class DrawerCallCounter<T> : GenericElementDrawer<T>
        {
            public int Draw0Counts = 0;
            public int Draw1Counts = 0;
            public int GetHeightCounts = 0;
            public void Draw(T item, Rect rect)
            {
                Draw0Counts += 1;
            }

            public void Draw(T item)
            {
                Draw1Counts += 1;
            }

            public float GetPropertyHeight(T item, GUIContent label)
            {
                GetHeightCounts += 1;
                return 1.0f;
            }
        }
    }
}
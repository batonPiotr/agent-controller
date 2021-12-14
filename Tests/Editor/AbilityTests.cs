using NUnit.Framework;

namespace HandcraftedGames.AgentController.Tests
{
    public class AbilityTests
    {
        [Test]
        public void TestEnablingWithoutAgent()
        {
            var ability = new SomeAbility();
            Assert.IsTrue(ability.IsEnabled);
            ability.Enable();
            Assert.IsTrue(ability.IsEnabled);
        }

        [Test]
        public void TestEnabling()
        {
            var validAgent = new ValidAgent();
            var ability = new SomeAbility();
            validAgent.AddAbility(ability);
            ability.Enable();
            Assert.IsTrue(ability.IsEnabled);
        }

        [Test]
        public void TestDisabling()
        {
            var validAgent = new ValidAgent();
            var ability = new SomeAbility();
            validAgent.AddAbility(ability);
            ability.Enable();
            Assert.IsTrue(ability.IsEnabled);
            ability.Disable();
            Assert.IsFalse(ability.IsEnabled);
        }

        [Test]
        public void TestEnableEvent()
        {
            var ability = new SomeAbility();
            var agent = new ValidAgent();
            agent.AddAbility(ability);
            var enable = (object)0;

            // Disable because it is enabled by default
            ability.Disable();

            ability.OnDidEnable += (ability) => {
                enable = (object)((int)enable + 1);
            };
            ability.Enable();
            // try multiple times
            ability.Enable();
            ability.Enable();
            Assert.AreEqual(1, (int)enable);
        }

        [Test]
        public void TestDisableEvent()
        {
            var ability = new SomeAbility();
            var agent = new ValidAgent();
            agent.AddAbility(ability);
            var disable = (object)0;

            ability.OnDidDisable += (ability) => {
                disable = (object)((int)disable + 1);
            };
            ability.Disable();
            // try multiple times
            ability.Disable();
            ability.Disable();
            Assert.AreEqual(1, (int)disable);
        }

        [Test]
        public void TestMultipleEnableAndDisableEvents()
        {
            var ability = new SomeAbility();
            var agent = new ValidAgent();
            agent.AddAbility(ability);
            var disable = (object)0;
            var enable = (object)0;

            ability.OnDidDisable += (ability) => {
                disable = (object)((int)disable + 1);
            };
            ability.OnDidEnable += (ability) => {
                enable = (object)((int)enable + 1);
            };
            ability.Disable();
            ability.Enable();
            ability.Disable();
            ability.Enable();
            ability.Disable();
            Assert.AreEqual(3, (int)disable);
            Assert.AreEqual(2, (int)enable);
        }

        [Test]
        public void TestActivateEvent()
        {
            var ability = new SomeAbility();
            var agent = new ValidAgent();
            agent.AddAbility(ability);
            var activate = (object)0;
            ability.OnDidActivate += (ability) => {
                activate = (object)((int)activate + 1);
            };

            agent.ActivateAbility(ability);
            agent.ActivateAbility(ability);
            Assert.AreEqual(1, (int)activate);
        }

        [Test]
        public void TestActivateAndStopEvent()
        {
            var ability = new SomeAbility();
            var agent = new ValidAgent();
            agent.AddAbility(ability);
            var activate = (object)0;
            var stop = (object)0;
            ability.OnDidActivate += (ability) => {
                activate = (object)((int)activate + 1);
            };
            ability.OnDidStop += (ability, reason) => {
                stop = (object)((int)stop + 1);
            };

            agent.ActivateAbility(ability);
            ability.Stop();
            agent.ActivateAbility(ability);
            Assert.AreEqual(2, (int)activate);
            Assert.AreEqual(1, (int)stop);
        }
    }
}
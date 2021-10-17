using NUnit.Framework;

namespace HandcraftedGames.AgentController.Tests
{
    public class AbilityTests
    {
        /// <summary>
        /// Tests if ability is not being enabled when agent is null.
        /// </summary>
        [Test]
        public void TestEnablingWithoutAgent()
        {
            var ability = new SomeAbility();
            Assert.IsTrue(ability.Enabled);
            ability.Enable();
            Assert.IsTrue(ability.Enabled);
        }
        
        /// <summary>
        /// Tests if ability is being added to a valid agent.
        /// </summary>
        [Test]
        public void TestEnabling()
        {
            var validAgent = new ValidAgent();
            var ability = new SomeAbility();
            validAgent.AddAbility(ability);
            ability.Enable();
            Assert.IsTrue(ability.Enabled);
        }
        
        /// <summary>
        /// Tests if ability is being added to a valid agent.
        /// </summary>
        [Test]
        public void TestDisabling()
        {
            var validAgent = new ValidAgent();
            var ability = new SomeAbility();
            validAgent.AddAbility(ability);
            ability.Enable();
            Assert.IsTrue(ability.Enabled);
            ability.Disable();
            Assert.IsFalse(ability.Enabled);
        }
    }
}
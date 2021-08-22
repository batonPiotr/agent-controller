using NUnit.Framework;
using Unity.PerformanceTesting;

namespace HandcraftedGames.AgentController.Tests
{
    public class AgentTests
    {
        /// <summary>
        /// Tests if ability is being added to a valid agent.
        /// </summary>
        [Test]
        public void TestAddingAbilityToValidAgent()
        {
            var validAgent = new ValidAgent();
            var ability = new SomeAbility();
            Assert.IsTrue(validAgent.AddAbility(ability), "Couldn't add ability");
            Assert.AreEqual(ability.Agent, validAgent);
        }

        /// <summary>
        /// Tests if ability fails to add if agent contiains already ability of this type.
        /// </summary>
        [Test]
        public void TestAddingDuplicatedAbilityToValidAgent()
        {
            var validAgent = new ValidAgent();
            var ability = new SomeAbility();
            Assert.IsTrue(validAgent.AddAbility(ability), "Couldn't add ability");
            Assert.IsFalse(validAgent.AddAbility(ability));
            Assert.AreEqual(ability.Agent, validAgent);
        }

        /// <summary>
        /// Tests if ability is being added to a valid agent.
        /// </summary>
        [Test]
        public void TestRetrievingAddedAbility()
        {
            var validAgent = new ValidAgent();
            var ability = new SomeAbility();
            Assert.IsTrue(validAgent.AddAbility(ability), "Couldn't add ability");
            Assert.AreEqual(ability.Agent, validAgent);
            Assert.AreEqual(validAgent.GetAbility<SomeAbility>(), ability);
        }

        /// <summary>
        /// Tests if ability is null if retriving non-existent ability.
        /// </summary>
        [Test]
        public void TestRetrievingNonExisingAbility()
        {
            var validAgent = new ValidAgent();
            Assert.AreEqual(validAgent.GetAbility<SomeAbility>(), null);
        }

        /// <summary>
        /// Tests if ability is being added to a valid agent.
        /// </summary>
        [Test]
        public void TestRetrievingFalseAbilityType()
        {
            var validAgent = new ValidAgent();
            var ability = new SomeAbility();
            Assert.IsTrue(validAgent.AddAbility(ability), "Couldn't add ability");
            Assert.AreEqual(ability.Agent, validAgent);
            Assert.AreNotEqual(validAgent.GetAbility<IAbility>(), ability);
            Assert.AreEqual(validAgent.GetAbility<IAbility>(), null);
        }

        [Test, Performance]
        public void TestPerformanceOfAbilityRetrieval()
        {
            var validAgent = new ValidAgent();
            validAgent.AddAbility(new AbilityA());
            validAgent.AddAbility(new AbilityB());
            validAgent.AddAbility(new AbilityC());
            Measure.Method(() => { 
                // UnityEngine.Debug.Log("Some string");
                validAgent.GetAbility<AbilityC>();
             })
                .WarmupCount(10)
                .MeasurementCount(50)
                .IterationsPerMeasurement(5000)
                .GC()
                .Run();
        }

        [Test, Performance]
        public void TestPerformanceOfAbilityActivation()
        {
            var validAgent = new ValidAgent();
            validAgent.AddAbility(new AbilityA());
            validAgent.AddAbility(new AbilityB());
            validAgent.AddAbility(new AbilityC());
            var abilityA = validAgent.GetAbility<AbilityA>();
            var abilityB = validAgent.GetAbility<AbilityB>();
            var abilityC = validAgent.GetAbility<AbilityC>();
            Measure.Method(() => { 
                // UnityEngine.Debug.Log("Some string");
                validAgent.ActivateAbility(abilityA);
                validAgent.ActivateAbility(abilityB);
                validAgent.ActivateAbility(abilityC);

                abilityA.Stop();
                abilityB.Stop();
                abilityC.Stop();
             })
                .WarmupCount(10)
                .MeasurementCount(50)
                .IterationsPerMeasurement(5000)
                .GC()
                .Run();
        }

    }

    class InvalidAgent : Agent
    {
    }

    class ValidAgent : Agent
    {
    }

    class SomeAbility : Ability
    {
        protected override bool ValidateAgent(IAgent agent)
        {
            return !(agent is InvalidAgent);
        }
    }

    class AbilityA: SomeAbility {}
    class AbilityB: SomeAbility {}
    class AbilityC: SomeAbility {}
}
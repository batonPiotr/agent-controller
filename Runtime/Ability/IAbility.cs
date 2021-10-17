namespace HandcraftedGames.AgentController.Abilities
{
    using System;
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class AbilityAttribute: Attribute
    {
        public string EditorItemName;
        public AbilityAttribute(string editorItemName)
        {
            EditorItemName = editorItemName;
        }
    }

    public interface IAbility: IDisposable
    {
        string Name { get; }
        /// <summary>
        /// Agent this ability is added to.
        /// </summary>
        IAgent Agent { get; }

        /// <summary>
        /// Tells if ability is currently running
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Tells if this ability can be activated
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Turns on the ability. It does not activate it, it just allows to run.
        /// </summary>
        void Enable();

        /// <summary>
        /// Turns off the ability. If running, it will be deactivated.
        /// </summary>
        void Disable();

        /// <summary>
        /// Called when this ability is running and another one is being activated.
        /// </summary>
        /// <param name="abilityToActivate">Ability that is about to activate</param>
        /// <returns>true if the new ability should be denied; false if it is not colliding with this ability.</returns>
        bool ShouldBlockActivatingAbility(IAbility abilityToActivate);

        /// <summary>
        /// Called when this ability is about to activate and another is running.
        /// </summary>
        /// <param name="activeAbility">Other active ability to be stopped.</param>
        /// <returns>True if the activeAbility should be stopped; False if not.</returns>
        bool ShouldActiveAbilityBeStopped(IAbility activeAbility);

        /// <summary>
        /// Called when this ability is running and other is about to be activated.
        /// </summary>
        /// <param name="activeAbility">Other active ability that is about to be activated.</param>
        /// <returns>True if this ability should be stopped due to a new activating ability; False if not.</returns>
        bool ShouldStopMyselfDueToActivatingAbility(IAbility abilityThatBlocks);

        /// <summary>
        /// Called when this ability is being added to the agent. It analyzes if the agent contains required components.
        /// </summary>
        /// <param name="agent">Agent to validate</param>
        bool TryToAdd(IAgent agent);

        void DetachFromAgent();

        /// <summary>
        /// Called if someone wants to activate this ability through agent. This ability has to meet following minimal requirements in order to be activated:
        /// - It must be enabled. All abilities are disabled upon creation
        /// - It must not be activated.
        /// - It must be assigned to an agent.
        /// </summary>
        bool TryToActivate();

        void Stop();
    }
}
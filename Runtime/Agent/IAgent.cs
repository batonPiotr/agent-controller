namespace HandcraftedGames.AgentController
{
    using System;
    using HandcraftedGames.AgentController.Abilities;
    using UnityEngine;
    public interface IAgent: IDisposable, IFixedUpdate, IUpdate
    {
        event System.Action<IAgent> OnDestroy;

        GameObject GameObject { get; }

        bool AddAbility(IAbility ability, bool enableOnAdd = true);
        T GetAbility<T>() where T: class, IAbility;

        /// <summary>
        /// Tries to activate the ability. It will ask other active abilities if this shouldn't be blocked.
        /// 
        /// This ability has to meet following minimal requirements in order to be activated:
        /// - It must be enabled. All abilities are disabled upon creation
        /// - It must not be activated.
        /// - It must be assigned to an agent.
        /// </summary>
        /// <param name="ability">Ability to activate</param>
        /// <returns>False if couldn't be activated</returns>
        bool ActivateAbility(IAbility ability);
    }
}
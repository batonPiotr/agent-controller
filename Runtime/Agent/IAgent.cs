namespace HandcraftedGames.AgentController
{
    using System;
    using System.Collections.Generic;
    using HandcraftedGames.AgentController.Abilities;
    using HandcraftedGames.AgentController.Properties;
    using UnityEngine;
    public interface IAgent: IDisposable, IFixedUpdate, IUpdate
    {
        event System.Action<IAgent> OnDestroy;

        GameObject GameObject { get; }

        List<IAbility> Abilities { get; }

        bool AddAbility(IAbility ability, bool enableOnAdd = true);
        void RemoveAbility(IAbility ability);
        T GetAbility<T>() where T: class, IAbility;

        void AddProperties(IProperties properties);
        T GetProperties<T>() where T: class, IProperties;

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
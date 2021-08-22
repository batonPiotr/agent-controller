namespace HandcraftedGames.AgentController
{
    using UnityEngine;
    public interface IAgent
    {
        event System.Action<IAgent> OnDestroy;

        GameObject GameObject { get; }

        bool AddAbility<T>(T ability) where T: class, IAbility;
        T GetAbility<T>() where T: class, IAbility;

        /// <summary>
        /// Tries to activate the ability. It will ask other active abilities if this shouldn't be blocked.
        /// </summary>
        /// <param name="ability">Ability to activate</param>
        /// <returns>False if couldn't be activated</returns>
        bool ActivateAbility(IAbility ability);
    }
}
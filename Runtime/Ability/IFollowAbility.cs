namespace HandcraftedGames.AgentController.Abilities
{
    using UnityEngine;
    public interface IFollowAbility: IAbility
    {
        /// <summary>
        /// Should the agent stop once reached the target?
        /// </summary>
        bool StopWhenReached { get; set; }

        /// <summary>
        /// Interval in seconds in which the position of the target will be obtained.
        /// </summary>
        float UpdateTargetPositionInterval { get; set; }

        GameObject Target { get; }

        bool Follow(GameObject target);
    }
}
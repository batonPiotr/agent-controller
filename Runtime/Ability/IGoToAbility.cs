namespace HandcraftedGames.AgentController.Abilities
{
    using UnityEngine;
    public interface IGoToAbility: IAbility
    {
        bool GoTo(Vector3 target);
    }
}
namespace HandcraftedGames.AgentController.Abilities
{
    using UnityEngine;
    public interface IGoToAbility: IAbility
    {
        void GoTo(Vector3 target);
    }
}
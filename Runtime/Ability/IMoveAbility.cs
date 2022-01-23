namespace HandcraftedGames.AgentController.Abilities
{
    using UnityEngine;
    public interface IMoveAbility: IAbility
    {
        bool SetInputVector(Vector2 input);
    }
}
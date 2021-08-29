namespace HandcraftedGames.AgentController.Abilities
{
    using UnityEngine;
    public interface IMoveAbility: IAbility
    {
        float SpeedMultiplier { get; set; }
        void SetInputVector(Vector2 input);
    }
}
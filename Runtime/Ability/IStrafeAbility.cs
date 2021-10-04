namespace HandcraftedGames.AgentController.Abilities
{
    using UnityEngine;
    public interface IStrafeAbility: IAbility
    {
        float SpeedMultiplier { get; set; }
        void SetInput(float value);
    }
}
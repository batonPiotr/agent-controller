namespace HandcraftedGames.AgentController.Abilities
{
    using UnityEngine;
    public interface IStrafeAbility: IAbility
    {
        bool SetInput(float value);
    }
}
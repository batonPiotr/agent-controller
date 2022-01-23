namespace HandcraftedGames.AgentController.Abilities
{
    using UnityEngine;
    public interface IRotateAbility: IAbility
    {
        bool SetRotationInput(float rotationInput);
    }
}
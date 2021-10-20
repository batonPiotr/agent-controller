namespace HandcraftedGames.AgentController.Abilities
{
    using UnityEngine;
    public interface IRotateAbility: IAbility
    {
        void SetRotationInput(float rotationInput);
    }
}
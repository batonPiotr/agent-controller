namespace HandcraftedGames.AgentController
{
    using UnityEngine;
    public interface IRotateAbility: IAbility
    {
        void SetRotationInput(float rotationInput);
    }
}
namespace HandcraftedGames.AgentController
{
    using UnityEngine;
    public interface IMoveAbility: IAbility
    {
        void SetInputVector(Vector2 input);
    }
}
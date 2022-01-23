namespace HandcraftedGames.AgentController.StarterAssetsAddon
{
    using HandcraftedGames.AgentController.Abilities;
    using StarterAssets;
    using System;
    using UnityEngine;

    [Serializable]
    [Ability("Unity/Third Person Character Controller/Move")]
    public class UnityCharacterControllerMovementAbility : Ability, IMoveAbility
    {
        public override string Name => "Unity 3rd person movement";

        [SerializeField]
        private StarterAssetsInputs input;

        public bool SetInputVector(Vector2 input)
        {
            if(!IsActive && !Agent.ActivateAbility(this))
            {
                return false;
            }
            if(input == null)
                return false;
            this.input.MoveInput(input);
            return true;
        }
    }
}
namespace HandcraftedGames.AgentController.Abilities.Animator
{
    using System;
    using HandcraftedGames.AgentController.Properties;
    using UnityEngine;

    [Serializable]
    [Ability("Generic/Change Speed")]
    public class ChangeSpeed : Ability, IChangeSpeedAbility
    {
        public override string Name => "Change Speed Ability";
        private MovementProperties movementProperties;

        public void SetSpeedMultiplier(float multiplier)
        {
            if(!Enabled)
                return;
            if(!IsActive)
            {
                if(!Agent.ActivateAbility(this))
                    return;
            }
            movementProperties.MovementSpeed = multiplier;
            Complete();
        }

        protected override bool ValidateAgent(IAgent agent)
        {
            movementProperties = agent.GetProperties<MovementProperties>();
            if(movementProperties == null)
                Debug.LogError("Couldn't add ability ChangeSpeed because the MovementProperties wasn't found!");
            return movementProperties != null;
        }
    }
}
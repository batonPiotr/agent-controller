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

        public bool SetSpeedMultiplier(float multiplier)
        {
            if(!IsEnabled)
                return false;
            if(!IsActive)
            {
                if(!Agent.ActivateAbility(this))
                    return false;
            }
            movementProperties.MovementSpeed = multiplier;
            Complete();
            return true;
        }

        protected override bool ShouldBeAddedToAgent(IAgentController agent)
        {
            movementProperties = agent.GetProperties<MovementProperties>();
            if(movementProperties == null)
                Debug.LogError("Couldn't add ability ChangeSpeed because the MovementProperties wasn't found!");
            return movementProperties != null;
        }
    }
}
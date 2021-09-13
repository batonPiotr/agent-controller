namespace HandcraftedGames.AgentController.Abilities.Animator
{
    using System;
    using UnityEngine;

    [Serializable]
    public class ChangeSpeed : Ability, IChangeSpeedAbility
    {
        public override string Name => "Change Speed Ability";
        private IMoveAbility moveAbility;

        public void SetSpeedMultiplier(float multiplier)
        {
            if(!Enabled)
                return;
            if(!IsActive)
            {
                if(!Agent.ActivateAbility(this))
                    return;
            }
            moveAbility.SpeedMultiplier = multiplier;
            Stop();
        }

        protected override bool ValidateAgent(IAgent agent)
        {
            moveAbility = agent.GetAbility<IMoveAbility>();
            if(moveAbility == null)
                Debug.LogError("Couldn't add ability ChangeSpeed because the IMoveAbility wasn't found!");
            return moveAbility != null;
        }
    }
}
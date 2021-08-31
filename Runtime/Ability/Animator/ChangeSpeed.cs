namespace HandcraftedGames.AgentController.Abilities.Animator
{
    using UnityEngine;
    public class ChangeSpeed : Ability, IChangeSpeedAbility
    {
        private Animator animator;
        private IMoveAbility moveAbility;

        public void SetSpeedMultiplier(float multiplier)
        {
            moveAbility.SpeedMultiplier = multiplier;
        }

        protected override bool ValidateAgent(IAgent agent)
        {
            moveAbility = agent.GetAbility<IMoveAbility>();
            if(moveAbility == null)
                Debug.LogError("Couldn't add ability ChangeSpeed because the IMoveAbility wasn't found!");
            animator = agent.GameObject.GetComponent<Animator>();
            return animator != null && moveAbility != null;
        }
    }
}
namespace HandcraftedGames.AgentController.Abilities
{
    using System;
    using UnityEngine;

    [Serializable]
    // [Ability("Generic/Input Handling")]
    public class InputHandlingAbility : Ability
    {
        public override string Name => "Input Handling";
        // public override bool ShouldActiveAbilityBeStopped(IAbility activeAbility) => false;

        // public override bool ShouldBlockActivatingAbility(IAbility abilityToActivate) => false;

        public override bool ShouldStopMyselfDueToActivatingAbility(IAbility abilityToActivate) => abilityToActivate is IGoToAbility;

        protected override bool ShouldBeActivated()
        {
            foreach(var a in Agent.Abilities)
            {
                if(!a.IsActive)
                    continue;
                if(a is IGoToAbility || a is IFollowAbility)
                    return false;
            }
            return true;
        }
    }
}
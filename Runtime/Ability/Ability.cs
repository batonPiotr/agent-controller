using System;
using UnityEngine;

namespace HandcraftedGames.AgentController.Abilities
{
    [Serializable]
    public class Ability : IAbility
    {
        public virtual string Name => "Unnamed Ability";
        private IAgent _Agent;
        public IAgent Agent => _Agent;

        protected bool _IsActive = false;
        public bool IsActive => _IsActive;
        [SerializeField]
        private bool _Enabled = true;
        public bool Enabled => _Enabled;

        public void Disable()
        {
            if(!Enabled)
                return;
            _Enabled = false;
            if(Agent == null)
                return;

            Stop();
            OnDisable();
        }

        public void Enable()
        {
            if(Enabled)
                return;
            _Enabled = true;

            if(Agent == null)
                return;
                
            OnEnable();
        }

        protected virtual void OnEnable() {}
        protected virtual void OnDisable() {}

        public virtual bool ShouldActiveAbilityBeStopped(IAbility activeAbility) => false;

        public virtual bool ShouldBlockActivatingAbility(IAbility abilityToActivate) => false;

        public virtual bool ShouldStopMyselfDueToActivatingAbility(IAbility abilityThatBlocks) => false;

        public bool TryToAdd(IAgent agent)
        {
            if(Agent != null)   
                return false;
            if(agent == null)
                return false;
            if(!ValidateAgent(agent))
                return false;
            _Agent = agent;
            return true;
        }

        protected virtual bool ValidateAgent(IAgent agent) => true;
        protected virtual bool ShouldBeActivated() => true;

        public bool TryToActivate()
        {
            if(!IsActive && Enabled && Agent != null && ShouldBeActivated())
            {
                _IsActive = true;
                return true;
            }
            return false;
        }

        public virtual void Stop() {
            _IsActive = false;
        }

        public virtual void Dispose() {}

        public void DetachFromAgent()
        {
            Stop();
            _Agent = null;
        }
    }
}
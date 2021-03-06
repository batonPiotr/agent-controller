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
        public bool IsEnabled => _Enabled;


        public event System.Action<IAbility> OnDidActivate;
        public event System.Action<IAbility, StopReason> OnDidStop;
        public event System.Action<IAbility> OnDidEnable;
        public event System.Action<IAbility> OnDidDisable;

        public void Disable()
        {
            if(!IsEnabled)
                return;
            _Enabled = false;
            if(Agent == null)
                return;

            Stop();
            OnDisable();
            OnDidDisable?.Invoke(this);
        }

        public void Enable()
        {
            if(IsEnabled)
                return;
            _Enabled = true;

            if(Agent == null)
                return;

            OnEnable();
            OnDidEnable?.Invoke(this);
        }

        protected virtual void OnEnable() {}
        protected virtual void OnDisable() {}

        protected virtual void OnStop(StopReason reason) {}

        public virtual bool ShouldActiveAbilityBeStopped(IAbility activeAbility) => false;

        public virtual bool ShouldBlockActivatingAbility(IAbility abilityToActivate) => false;

        public virtual bool ShouldStopMyselfDueToActivatingAbility(IAbility abilityThatBlocks) => false;

        protected virtual bool ShouldBeAddedToAgent(IAgent agent) => true;
        protected virtual bool ShouldBeActivated() => true;

        bool IAbility.TryToAdd(IAgent agent)
        {
            if(Agent != null)
                return false;
            if(agent == null)
                return false;
            if(!ShouldBeAddedToAgent(agent))
                return false;
            _Agent = agent;
            return true;
        }

        bool IAbility.TryToActivate()
        {
            if(!IsActive && IsEnabled && Agent != null && ShouldBeActivated())
            {
                _IsActive = true;
                OnDidActivate?.Invoke(this);
                return true;
            }
            return false;
        }

        public void Stop()
        {
            StopWithReason(StopReason.Interruption);
        }

        public virtual void Dispose() {}

        void IAbility.DetachFromAgent()
        {
            Stop();
            _Agent = null;
        }

        protected void Fail()
        {
            StopWithReason(StopReason.Failure);
        }

        protected void Complete()
        {
            StopWithReason(StopReason.Completion);
        }

        private void StopWithReason(StopReason reason)
        {
            if(!IsActive)
                return;
            _IsActive = false;
            OnStop(reason);
            OnDidStop?.Invoke(this, reason);
        }
    }
}
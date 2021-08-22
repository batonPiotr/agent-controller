namespace HandcraftedGames.AgentController
{
    public abstract class Ability : IAbility
    {
        private IAgent _Agent;
        public IAgent Agent => _Agent;

        protected bool _IsActive = false;
        public bool IsActive => _IsActive;

        private bool _Enabled = false;
        public bool Enabled => _Enabled;

        public void Disable()
        {
            if(!Enabled)
                return;
            _Enabled = false;
            OnDisable();
        }

        public void Enable()
        {
            if(Enabled || _Agent == null) 
                return;
            _Enabled = true;
            OnEnable();
        }

        protected virtual void OnEnable() {}
        protected virtual void OnDisable() {}

        public virtual bool ShouldActiveAbilityBeStopped(IAbility activeAbility) => false;

        public virtual bool ShouldBlockActivatingAbility(IAbility abilityToActivate) => false;

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

        protected abstract bool ValidateAgent(IAgent agent);

        public virtual bool TryToActivate()
        {
            return !IsActive && Enabled && Agent != null;
        }

        public virtual void Stop()
        {
        }
    }
}
namespace HandcraftedGames.AgentController
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;
    
    public class Agent : IAgent
    {
        private GameObject gameObject;
        public GameObject GameObject => gameObject;

        private List<IAbility> Abilities = new List<IAbility>();

        private List<IFixedUpdate> FixedUpdateAbilities = new List<IFixedUpdate>();
        private List<IUpdate> UpdateAbilities = new List<IUpdate>();

        public event Action<IAgent> OnDestroy;

        public Agent(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public bool AddAbility(IAbility ability, bool enableOnAdd = true)
        {
            if(Abilities.Contains(ability))
            {
                return false;
            }
            if(!ability.TryToAdd(this))
            {
                return false;
            }
            Abilities.Add(ability);
            if(ability is IUpdate)
                UpdateAbilities.Add((IUpdate)ability);
            if(ability is IFixedUpdate)
                FixedUpdateAbilities.Add((IFixedUpdate)ability);
            ability.Enable();
            return true;
        }

        public T GetAbility<T>() where T : class, IAbility
        {
            var type = typeof(T);
            foreach(var ability in Abilities)
                if(ability is T)
                    return (T)ability;
            return null;
        }

        public bool ActivateAbility(IAbility ability)
        {
            if(!Abilities.Contains(ability))
                return false;

            // Check if some running ability wants to prevent activating this ability
            foreach(var a in Abilities)
            {
                if(!a.IsActive)
                    continue;
                if(a.ShouldBlockActivatingAbility(ability))
                    return false;
            }

            // Try to activate this ability
            if(!ability.TryToActivate())
                return false;

            // Check if this new activated ability would like to stop some already running ability
            foreach(var a in Abilities)
            {
                if(!a.IsActive)
                    continue;
                if(ability.ShouldActiveAbilityBeStopped(a))
                    a.Stop();
            }
            return true;
        }

        public void Dispose()
        {
            foreach(var ability in Abilities)
                ability.Dispose();
        }

        public void FixedUpdate()
        {
            foreach(var fixedUpdate in FixedUpdateAbilities)
            {
                if(((IAbility)fixedUpdate).Enabled)
                    fixedUpdate.FixedUpdate();
            }
        }

        public void Update()
        {
            foreach(var update in UpdateAbilities)
            {
                if(((IAbility)update).Enabled)
                    update.Update();
            }
        }
    }
}
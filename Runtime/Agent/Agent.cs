namespace HandcraftedGames.AgentController
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;
    using HandcraftedGames.AgentController.Abilities;
    using System.Linq;
    using HandcraftedGames.AgentController.Properties;

    [Serializable]
    public class Agent : IAgent
    {
        private GameObject gameObject;
        public GameObject GameObject => gameObject;

        public List<IAbility> Abilities => abilities;

        [SerializeReference]
        private List<IAbility> abilities = new List<IAbility>();

        [SerializeReference]
        private List<IProperties> properties = new List<IProperties>();

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

            if(enableOnAdd)
                ability.Enable();

            return true;
        }

        public void RemoveAbility(IAbility ability)
        {
            if(ability.Agent != this)
                return;
            if(ability.IsActive)
                ability.Stop();
            abilities.Remove(ability);
            ability.DetachFromAgent();
        }

        public T GetAbility<T>() where T : class, IAbility
        {
            var type = typeof(T);
            foreach(var ability in Abilities)
                if(ability is T)
                    return ability as T;
            return null;
        }

        public bool ActivateAbility(IAbility ability)
        {
            if(!Abilities.Contains(ability))
                return false;

            if(!ability.IsEnabled)
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

            // Check if some ability wants to stop itself due to the newly activated ability
            foreach(var a in Abilities)
            {
                if(!a.IsActive)
                    continue;
                if(a.ShouldStopMyselfDueToActivatingAbility(ability))
                    a.Stop();
            }

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
                if(((IAbility)fixedUpdate).IsEnabled)
                    fixedUpdate.FixedUpdate();
            }
        }

        public void Update()
        {
            foreach(var update in UpdateAbilities)
            {
                if(((IAbility)update).IsEnabled)
                    update.Update();
            }
        }

        public void AddProperties(IProperties properties)
        {
            this.properties.Add(properties);
        }

        public T GetProperties<T>() where T : class, IProperties
        {
            var type = typeof(T);
            foreach(var prop in this.properties)
                if(prop is T)
                    return prop as T;
            return null;
        }

    }
}
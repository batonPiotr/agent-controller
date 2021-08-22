namespace HandcraftedGames.AgentController
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;
    
    public class Agent : IAgent
    {
        public GameObject GameObject => throw new NotImplementedException();

        private Dictionary<Type, IAbility> Abilities = new Dictionary<Type, IAbility>();

        public event Action<IAgent> OnDestroy;

        public bool AddAbility<T>(T ability) where T: class, IAbility
        {
            if(Abilities.ContainsKey(ability.GetType()))
            {
                return false;
            }
            if(!ability.TryToAdd(this))
            {
                return false;
            }
            Abilities[ability.GetType()] = ability;
            return true;
        }

        public T GetAbility<T>() where T : class, IAbility
        {
            var type = typeof(T);
            if(Abilities.ContainsKey(type))
            {
                return Abilities[type] as T;
            }
            return null;
        }

        public bool ActivateAbility(IAbility ability)
        {
            if(!Abilities.ContainsValue(ability))
                return false;

            // Check if some running ability wants to prevent activating this ability
            foreach(var a in Abilities.Values)
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
            foreach(var a in Abilities.Values)
            {
                if(!a.IsActive)
                    continue;
                if(ability.ShouldActiveAbilityBeStopped(a))
                    a.Stop();
            }
            return true;
        }
    }
}
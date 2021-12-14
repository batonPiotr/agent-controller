namespace HandcraftedGames.AgentController
{
    using System;
    using System.Collections.Generic;
    using HandcraftedGames.AgentController.Abilities;
    using UnityEngine;
    using System.Linq;
    using HandcraftedGames.Utils;
    using HandcraftedGames.AgentController.Abilities.Animator;
    using HandcraftedGames.AgentController.Abilities.Rigidbody;
    using HandcraftedGames.AgentController.Properties;

    [Serializable]
    public class MonoAgent : MonoBehaviour
    {
        [SerializeReference]
        public IAgent agent;

        [SerializeReference]
        private List<IAbility> abilities = new List<IAbility>();

        [SerializeReference]
        private List<IProperties> properties = new List<IProperties>();

        public MovementProperties lol = new MovementProperties();

        private void Awake()
        {
            agent = new Agent(gameObject);
            foreach(var prop in properties)
                agent.AddProperties(prop);
            var abilitiesToEnable = abilities.Where(i => i.IsEnabled);
            foreach(var ability in abilities)
            {
                if(!agent.AddAbility(ability, abilitiesToEnable.Contains(ability)))
                {
                    Debug.LogError("Couldn't Add Ability [" + ability + "]." + ability.Name);
                }
            }
        }

        void Update()
        {
            if(agent != null) agent.Update();
        }

        void FixedUpdate()
        {
            if(agent != null) agent.FixedUpdate();
        }
    }
}
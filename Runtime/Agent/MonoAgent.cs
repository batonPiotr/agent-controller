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

    [Serializable]
    public class MonoAgent : MonoBehaviour
    {
        [SerializeReference]
        public IAgent agent;

        [SerializeReference]
        public List<IAbility> abilities = new List<IAbility>();

        private void Awake()
        {
            agent = new Agent(gameObject);
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
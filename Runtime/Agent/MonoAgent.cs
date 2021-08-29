
namespace HandcraftedGames.AgentController
{
    using System.Collections;
    using UnityEngine;

    public class MonoAgent : MonoBehaviour
    {
        public IAgent agent;

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
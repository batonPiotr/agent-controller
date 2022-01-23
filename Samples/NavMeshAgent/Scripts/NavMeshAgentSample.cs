using System.Collections;
using System.Collections.Generic;
using HandcraftedGames.AgentController.Abilities;
using UnityEngine;

namespace HandcraftedGames.AgentController.Samples
{
    public class NavMeshAgentSample : MonoBehaviour
    {
        public MonoAgentController agent;
        public Transform goTo;

        private IGoToAbility goToAbility;

        void Update()
        {
            if(goToAbility == null)
            {
                goToAbility = agent.GetAbility<IGoToAbility>();
                goToAbility.GoTo(goTo.transform.position);
            }
        }
    }
}
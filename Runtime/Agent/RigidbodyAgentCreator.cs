using System.Collections;
using System.Collections.Generic;
using HandcraftedGames.AgentController.Abilities.Rigidbody;
using UnityEngine;

namespace HandcraftedGames.AgentController.Samples.Shared
{
    [RequireComponent(typeof(MonoAgent))]
    public class RigidbodyAgentCreator : MonoBehaviour
    {
        MonoAgent monoAgent;
        
        void Start()
        {
            monoAgent = GetComponent<MonoAgent>();

            monoAgent.agent.AddAbility(new RigidbodyMovement());
            monoAgent.agent.AddAbility(new RigidbodyRotation());
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using HandcraftedGames.AgentController.Abilities.Animator;
using HandcraftedGames.AgentController.Abilities.Rigidbody;
using UnityEngine;

namespace HandcraftedGames.AgentController.Samples.Shared
{
    [RequireComponent(typeof(MonoAgent))]
    public class AnimatorAgentCreator : MonoBehaviour
    {
        MonoAgent monoAgent;
        
        void Start()
        {
            monoAgent = GetComponent<MonoAgent>();

            monoAgent.agent.AddAbility(new MoveAbility("moveForward", -2.0f, 2.0f, "moveSideward", -2.0f, 2.0f));
            // monoAgent.agent.AddAbility(new RigidbodyRotation());
        }
    }
}
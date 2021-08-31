using System.Collections;
using System.Collections.Generic;
using HandcraftedGames.AgentController.Abilities;
using HandcraftedGames.AgentController.Abilities.Animator;
using HandcraftedGames.AgentController.Abilities.Rigidbody;
using UnityEngine;
using UnityEngine.AI;

namespace HandcraftedGames.AgentController.Samples.Shared
{
    [RequireComponent(typeof(MonoAgent))]
    public class AnimatorAgentCreator : MonoBehaviour
    {
        MonoAgent monoAgent;
        NavMeshAgent agent;
        Animator animator;
        
        void Start()
        {
            monoAgent = GetComponent<MonoAgent>();

            monoAgent.agent.AddAbility(new MoveAbility("moveForward", -1.0f, 1.0f, "moveSideward", -1.0f, 1.0f, "isMoving"));
            monoAgent.agent.AddAbility(new ChangeSpeed());
            monoAgent.agent.AddAbility(new GoToAbility());

            monoAgent.agent.GetAbility<IGoToAbility>().GoTo(new Vector3(-2, 0.5f, 10));
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            animator.applyRootMotion = true;
            // monoAgent.agent.AddAbility(new RigidbodyRotation());
        }


        // void OnAnimatorMove ()
        // {
            // Update position to agent position
            // transform.position = agent.nextPosition;
            // agent.nextPosition = transform.position;
        // }
    }
}
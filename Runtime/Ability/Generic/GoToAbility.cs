namespace HandcraftedGames.AgentController.Abilities
{
    using System;
    using UnityEngine;
    using UnityEngine.AI;
    [Serializable]

    public class GoToAbility : Ability, IGoToAbility, IUpdate
    {
        public override string Name => "Go To Ability";
        private IMoveAbility moveAbility;
        private NavMeshAgent navMeshAgent;
        private UnityEngine.Animator animator;


        Vector2 smoothDeltaPosition = Vector2.zero;
        Vector2 velocity = Vector2.zero;

        override protected bool ValidateAgent(IAgent agent)
        {
            moveAbility = agent.GetAbility<IMoveAbility>();
            navMeshAgent = agent.GameObject.GetComponent<NavMeshAgent>();
            animator = agent.GameObject.GetComponent<UnityEngine.Animator>();

            if(navMeshAgent == null)
                Debug.LogError("Couldn't add GoToAbility: NavMeshAgent not found!");
            else
            {
                navMeshAgent.updatePosition = false;
                navMeshAgent.updateRotation = false;
            }
            if(moveAbility == null)
                Debug.LogError("Couldn't add GoToAbility: IMoveAbility not found!");

            return moveAbility != null && navMeshAgent != null;
        }
        public void GoTo(Vector3 target)
        {
            Debug.Log("Trying to activate the ability: "+ Agent.ActivateAbility(this));
            navMeshAgent.SetDestination(target);
        }

        public void Update()
        {
            if(!IsActive)
                return;

            bool shouldMove = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
            if(!shouldMove)
                Stop();
            
            var vel = navMeshAgent.desiredVelocity.normalized;
            var inputVector = new Vector2(vel.x, vel.z);
            moveAbility.SetInputVector(inputVector);
            navMeshAgent.nextPosition = Agent.GameObject.transform.position;
        }
    }
}
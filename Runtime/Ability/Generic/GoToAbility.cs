namespace HandcraftedGames.AgentController.Abilities
{
    using System;
    using UnityEngine;
    using UnityEngine.AI;

    [Serializable]
    [Ability("Generic/Go To")]
    public class GoToAbility : Ability, IGoToAbility, IUpdate
    {
        public override string Name => "Go To Ability";
        private IMoveAbility moveAbility;
        private NavMeshAgent navMeshAgent;

        override protected bool ValidateAgent(IAgent agent)
        {
            moveAbility = agent.GetAbility<IMoveAbility>();
            navMeshAgent = agent.GameObject.GetComponent<NavMeshAgent>();

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
            if(!IsActive && !Agent.ActivateAbility(this))
                return;
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target);
        }

        public void Update()
        {
            if(!IsActive)
                return;

            bool shouldMove = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
            if(!shouldMove)
                Complete();

            var speed = Vector3.Project(navMeshAgent.desiredVelocity, Agent.GameObject.transform.forward).magnitude;
            if(speed > 0.3f)
                speed = 1.0f;
            if(speed < -0.3f)
                speed = -1.0f;

            var angle = Vector3.Angle(navMeshAgent.desiredVelocity, Agent.GameObject.transform.forward);
            var negative = Vector3.Angle(navMeshAgent.desiredVelocity, Agent.GameObject.transform.right) < 90.0f ? 1.0f : -1.0f;

            if(angle > 45.0f)
                speed = 0.0f;

            // var deadZone = 5.0f;
            // if(Mathf.Abs(angle) < deadZone)
            //     angle = 0.0f;

            angle = Mathf.Clamp(angle, 0.0f, 45.0f) / 45.0f;

            Debug.Log("Angle: " + angle);
            Debug.Log("Speed: " + speed);


            var inputVector = new Vector2(angle * negative, speed);
            moveAbility.SetInputVector(inputVector);
            navMeshAgent.nextPosition = Agent.GameObject.transform.position;
        }

        protected override void OnStop(StopReason reason)
        {
            navMeshAgent.isStopped = true;
            moveAbility.SetInputVector(Vector2.zero);
        }
    }
}
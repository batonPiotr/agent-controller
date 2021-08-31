namespace HandcraftedGames.AgentController.Abilities
{
    using UnityEngine;
    using UnityEngine.AI;

    public class GoToAbility : Ability, IGoToAbility, IUpdate
    {
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
            Debug.Log("Vel: "+ navMeshAgent.velocity);
            Debug.Log("Desired Vel: "+ navMeshAgent.desiredVelocity);
            // if(!IsActive)
            //     return;
            // Vector3 worldDeltaPosition = navMeshAgent.nextPosition - Agent.GameObject.transform.position;

            // // Map 'worldDeltaPosition' to local space
            // float dx = Vector3.Dot (Agent.GameObject.transform.right, worldDeltaPosition);
            // float dy = Vector3.Dot (Agent.GameObject.transform.forward, worldDeltaPosition);
            // Vector2 deltaPosition = new Vector2 (dx, dy);

            // // Low-pass filter the deltaMove
            // float smooth = Mathf.Min(1.0f, Time.deltaTime/0.15f);
            // smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, smooth);

            // // Update velocity if time advances
            // if (Time.deltaTime > 1e-5f)
            //     velocity = smoothDeltaPosition / Time.deltaTime;

            bool shouldMove = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
            if(!shouldMove)
                Stop();

            // Update animation parameters
            // anim.SetBool("move", shouldMove);
            var vel = navMeshAgent.desiredVelocity.normalized;
            var inputVector = new Vector2(vel.x, vel.z);
            moveAbility.SetInputVector(inputVector);
            navMeshAgent.nextPosition = Agent.GameObject.transform.position;
            // anim.SetFloat ("velx", velocity.x);
            // anim.SetFloat ("vely", velocity.y);
        }
    }
}
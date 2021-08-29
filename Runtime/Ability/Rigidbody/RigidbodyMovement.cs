namespace HandcraftedGames.AgentController.Abilities.Rigidbody
{
    using UnityEngine;
    public class RigidbodyMovement : Ability, IMoveAbility, IFixedUpdate
    {
        private Vector2 inputVector;
        private Rigidbody rigidbody;

        private float speedMultiplier = 1.0f;
        public float SpeedMultiplier { get => speedMultiplier; set => speedMultiplier = value; }

        protected override bool ValidateAgent(IAgent agent)
        {
            rigidbody = agent.GameObject.GetComponent<Rigidbody>();
            if(rigidbody == null)
                return false;

            rigidbody.drag = 10.0f;
            return true;
        }
        public void SetInputVector(Vector2 input)
        {
            inputVector = input;
        }
        public void FixedUpdate()
        {
            if(!IsActive && !Agent.ActivateAbility(this))
            {
                return;
            }
            rigidbody.AddRelativeForce(new Vector3(inputVector.x, 0.0f, inputVector.y) * 100.0f * speedMultiplier);
            inputVector = Vector2.zero;
            
            if(rigidbody.velocity.sqrMagnitude < 0.001f)
                _IsActive = false;
        }
    }
}
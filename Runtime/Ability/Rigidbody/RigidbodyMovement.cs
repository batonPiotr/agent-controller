namespace HandcraftedGames.AgentController.Abilities.Rigidbody
{
    using System;
    using UnityEngine;
    [Serializable]
    [Ability("Rigidbody/Move")]
    public class RigidbodyMovement : Ability, IMoveAbility, IFixedUpdate
    {
        public override string Name => "Rigidbody Move";
        private Vector2 inputVector;
        private Rigidbody rigidbody;

        [SerializeField]
        private float speedMultiplier = 1.0f;
        public float SpeedMultiplier { get => speedMultiplier; set => speedMultiplier = value; }

        protected override bool ShouldBeAddedToAgent(IAgentController agent)
        {
            rigidbody = agent.GameObject.GetComponent<Rigidbody>();
            if(rigidbody == null)
                return false;

            rigidbody.drag = 10.0f;
            return true;
        }
        public bool SetInputVector(Vector2 input)
        {
            if(!IsActive && !Agent.ActivateAbility(this))
                return false;

            inputVector = input;
            return true;
        }
        public void FixedUpdate()
        {
            rigidbody.AddRelativeForce(new Vector3(inputVector.x, 0.0f, inputVector.y) * 100.0f * speedMultiplier);
            inputVector = Vector2.zero;

            if(rigidbody.velocity.sqrMagnitude < 0.001f)
                _IsActive = false;
        }
    }
}
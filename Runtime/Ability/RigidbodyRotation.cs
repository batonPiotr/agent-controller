namespace HandcraftedGames.AgentController
{
    using UnityEngine;
    public class RigidbodyRotation : Ability, IRotateAbility, IFixedUpdate
    {
        private float rotationInput;
        private Rigidbody rigidbody;

        protected override bool ValidateAgent(IAgent agent)
        {
            rigidbody = agent.GameObject.GetComponent<Rigidbody>();
            if(rigidbody == null)
                return false;

            rigidbody.angularDrag = 10.0f;
            return true;
        }
        public void FixedUpdate()
        {
            if(!IsActive && !Agent.ActivateAbility(this))
            {
                return;
            }
            rigidbody.AddRelativeTorque(new Vector3(0, rotationInput, 0) * 100.0f);
            rotationInput = 0.0f;


            if(rigidbody.angularVelocity.sqrMagnitude < 0.001f)
                _IsActive = false;
        }

        public void SetRotationInput(float rotationInput)
        {
            this.rotationInput = rotationInput;
        }
    }
}
namespace HandcraftedGames.AgentController.Abilities.Rigidbody
{
    using System;
    using UnityEngine;
    [Serializable]
    [Ability("Rigidbody/Rotate")]
    public class RigidbodyRotation : Ability, IRotateAbility, IFixedUpdate
    {
        public override string Name => "Rigidbody Rotation";
        private float rotationInput;
        private Rigidbody rigidbody;

        protected override bool ShouldBeAddedToAgent(IAgentController agent)
        {
            rigidbody = agent.GameObject.GetComponent<Rigidbody>();
            if(rigidbody == null)
                return false;

            rigidbody.angularDrag = 10.0f;
            return true;
        }
        public void FixedUpdate()
        {
            rigidbody.AddRelativeTorque(new Vector3(0, rotationInput, 0) * 100.0f);
            rotationInput = 0.0f;

            if(rigidbody.angularVelocity.sqrMagnitude < 0.001f)
                _IsActive = false;
        }

        public bool SetRotationInput(float rotationInput)
        {
            if(!IsActive && !Agent.ActivateAbility(this))
            {
                return false;
            }
            this.rotationInput = rotationInput;
            return true;
        }
    }
}
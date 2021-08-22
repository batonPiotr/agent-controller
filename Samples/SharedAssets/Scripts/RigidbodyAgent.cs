using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandcraftedGames.AgentController.Samples.Shared
{
    public class RigidbodyAgent : MonoBehaviour
    {
        IAgent agent;
        IMoveAbility moveAbility;
        IRotateAbility rotateAbility;
        // Start is called before the first frame update
        void Start()
        {
            agent = new Agent(gameObject);

            agent.AddAbility(new RigidbodyMovement());
            agent.AddAbility(new RigidbodyRotation());
            moveAbility = agent.GetAbility<IMoveAbility>();
            rotateAbility = agent.GetAbility<IRotateAbility>();
        }

        private void Update()
        {
            agent.Update();
            var inputVector = Vector2.zero;
            var rotationInput = 0.0f;
            if(Input.GetKey(KeyCode.UpArrow))
                inputVector += new Vector2(0, 1);
            if(Input.GetKey(KeyCode.DownArrow))
                inputVector += new Vector2(0, -1);

            if(Input.GetKey(KeyCode.RightArrow))
                inputVector += new Vector2(1, 0);
            if(Input.GetKey(KeyCode.LeftArrow))
                inputVector += new Vector2(-1, 0);

            if(Input.GetKey(KeyCode.Q))
                rotationInput += -1.0f;
            if(Input.GetKey(KeyCode.E))
                rotationInput += 1.0f;

            moveAbility.SetInputVector(inputVector);
            rotateAbility.SetRotationInput(rotationInput);
        }

        private void FixedUpdate()
        {
            agent.FixedUpdate();
        }
    }

    class SomeAbility : Ability
    {
        protected override bool ValidateAgent(IAgent agent)
        {
            return true;
        }
    }
}
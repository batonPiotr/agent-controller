using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandcraftedGames.AgentController.Samples.Shared
{
    public class RigidbodyAgent : MonoBehaviour
    {
        IAgent agent;
        IMoveAbility moveAbility;
        // Start is called before the first frame update
        void Start()
        {
            agent = new Agent(gameObject);

            agent.AddAbility(new RigidbodyMovement(), true);
            moveAbility = agent.GetAbility<IMoveAbility>();
        }

        private void Update()
        {
            agent.Update();
            var inputVector = Vector2.zero;
            if(Input.GetKey(KeyCode.UpArrow))
                inputVector += new Vector2(0, 1);
            if(Input.GetKey(KeyCode.DownArrow))
                inputVector += new Vector2(0, -1);

            if(Input.GetKey(KeyCode.RightArrow))
                inputVector += new Vector2(1, 0);
            if(Input.GetKey(KeyCode.LeftArrow))
                inputVector += new Vector2(-1, 0);

            moveAbility.SetInputVector(inputVector);
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
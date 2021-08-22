namespace HandcraftedGames.AgentController
{
    using UnityEngine;

    public class MonoAgentSimpleInputHandler: MonoBehaviour
    {
        public MonoAgent target;

        public IMoveAbility moveAbility;
        public IRotateAbility rotateAbility;
        private void Update()
        {
            if (moveAbility == null || rotateAbility == null)
            {
                moveAbility = target.agent.GetAbility<IMoveAbility>();
                rotateAbility = target.agent.GetAbility<IRotateAbility>();
            }
            else
            {
                var inputVector = Vector2.zero;
                var rotationInput = 0.0f;
                if (Input.GetKey(KeyCode.UpArrow))
                    inputVector += new Vector2(0, 1);
                if (Input.GetKey(KeyCode.DownArrow))
                    inputVector += new Vector2(0, -1);

                if (Input.GetKey(KeyCode.RightArrow))
                    inputVector += new Vector2(1, 0);
                if (Input.GetKey(KeyCode.LeftArrow))
                    inputVector += new Vector2(-1, 0);

                if (Input.GetKey(KeyCode.Q))
                    rotationInput += -1.0f;
                if (Input.GetKey(KeyCode.E))
                    rotationInput += 1.0f;

                moveAbility.SetInputVector(inputVector);
                rotateAbility.SetRotationInput(rotationInput);
            }
        }
    }
}
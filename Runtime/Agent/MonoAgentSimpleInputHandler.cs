namespace HandcraftedGames.AgentController
{
    using HandcraftedGames.AgentController.Abilities;
    using HandcraftedGames.Utils;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class MonoAgentSimpleInputHandler: MonoBehaviour
    {
        public MonoAgent target;

        public IMoveAbility moveAbility;
        public IRotateAbility rotateAbility;
        public IChangeSpeed changeSpeedAbility;

        private FloatTween forwardInputTween = new FloatTween(0.0f);
        private FloatTween backwardInputTween = new FloatTween(0.0f);

        private FloatTween leftInputTween = new FloatTween(0.0f);
        private FloatTween rightInputTween = new FloatTween(0.0f);

        private FloatTween speedInputTween = new FloatTween(1.0f);

        float maxValue = 1.0f;
        float delay = 0.1f;
        
        #if ENABLE_LEGACY_INPUT_MANAGER
        private void Update()
        {
            if (moveAbility == null /*|| rotateAbility == null*/)
            {
                moveAbility = target.agent.GetAbility<IMoveAbility>();
                changeSpeedAbility = target.agent.GetAbility<IChangeSpeed>();
                // rotateAbility = target.agent.GetAbility<IRotateAbility>();
            }
            else
            {
                var rotationInput = 0.0f;
                
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    forwardInputTween.Stop();
                    forwardInputTween.TweenTo(maxValue, (maxValue - Mathf.Abs((forwardInputTween.Value / maxValue))) * delay);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    backwardInputTween.Stop();
                    backwardInputTween.TweenTo(maxValue, (maxValue - Mathf.Abs((backwardInputTween.Value / maxValue))) * delay);
                }
                if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    forwardInputTween.Stop();
                    forwardInputTween.TweenTo(0.0f, Mathf.Abs((forwardInputTween.Value / maxValue)) * delay);
                }
                if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    backwardInputTween.Stop();
                    backwardInputTween.TweenTo(0.0f, Mathf.Abs((backwardInputTween.Value / maxValue)) * delay);
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    leftInputTween.Stop();
                    leftInputTween.TweenTo(maxValue, (maxValue - Mathf.Abs((leftInputTween.Value / maxValue))) * delay);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    rightInputTween.Stop();
                    rightInputTween.TweenTo(maxValue, (maxValue - Mathf.Abs((rightInputTween.Value / maxValue))) * delay);
                }
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    leftInputTween.Stop();
                    leftInputTween.TweenTo(0.0f, Mathf.Abs((leftInputTween.Value / maxValue)) * delay);
                }
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    rightInputTween.Stop();
                    rightInputTween.TweenTo(0.0f, Mathf.Abs((rightInputTween.Value / maxValue)) * delay);
                }

                // if (Input.GetKey(KeyCode.Q))
                //     rotationInput += -1.0f;
                // if (Input.GetKey(KeyCode.E))
                //     rotationInput += 1.0f;

                moveAbility.SetInputVector(new Vector2(rightInputTween.Value - leftInputTween.Value, forwardInputTween.Value - backwardInputTween.Value));
                // rotateAbility.SetRotationInput(rotationInput);
            }

            if(moveAbility != null)
            {
                float minSpeed = 1.0f;
                float maxSpeed = 2.0f;
                #if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    speedInputTween.Stop();
                    speedInputTween.TweenTo(maxSpeed, (maxSpeed - Mathf.Abs((speedInputTween.Value / maxSpeed))) * delay);
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    speedInputTween.Stop();
                    speedInputTween.TweenTo(minSpeed, Mathf.Abs((speedInputTween.Value / maxSpeed)) * delay);
                }
                #endif

                changeSpeedAbility.SetSpeedMultiplier(speedInputTween.Value);
            }
        }
        #endif
    }
}
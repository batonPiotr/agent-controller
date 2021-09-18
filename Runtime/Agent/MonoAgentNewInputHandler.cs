namespace HandcraftedGames.AgentController
{
    using HandcraftedGames.AgentController.Abilities;
    using HandcraftedGames.Utils;
    using UnityEngine;
    #if ENABLE_INPUT_SYSTEM
    using UnityEngine.InputSystem;
    #endif

    public class MonoAgentNewInputHandler: MonoBehaviour
    {
        public MonoAgent target;

        public IMoveAbility moveAbility;
        public IRotateAbility rotateAbility;
        public IChangeSpeedAbility changeSpeedAbility;

        private FloatTween forwardInputTween = new FloatTween(0.0f);
        private FloatTween backwardInputTween = new FloatTween(0.0f);

        private FloatTween leftInputTween = new FloatTween(0.0f);
        private FloatTween rightInputTween = new FloatTween(0.0f);

        private FloatTween speedInputTween = new FloatTween(1.0f);

        float maxValue = 1.0f;
        float delay = 0.2f;

        #if ENABLE_INPUT_SYSTEM
        public void OnRun(InputAction.CallbackContext context)
        {
            if(changeSpeedAbility != null)
            {
                float minSpeed = 1.0f;
                float maxSpeed = 2.0f;
                if(context.phase == InputActionPhase.Started)
                {
                    speedInputTween.Stop();
                    speedInputTween.TweenTo(maxSpeed, (maxSpeed - Mathf.Abs((speedInputTween.Value / maxSpeed))) * delay);
                }
                else if(context.phase == InputActionPhase.Canceled)
                {
                    speedInputTween.Stop();
                    speedInputTween.TweenTo(minSpeed, Mathf.Abs((speedInputTween.Value / maxSpeed)) * delay);
                }
                changeSpeedAbility.SetSpeedMultiplier(speedInputTween.Value);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if(moveAbility == null)
                return;
            var inputVector = (Vector2)context.ReadValueAsObject();
            if(context.phase == InputActionPhase.Canceled)
            {
                forwardInputTween.Stop();
                backwardInputTween.Stop();
                leftInputTween.Stop();
                rightInputTween.Stop();

                forwardInputTween.TweenTo(0.0f, Mathf.Abs(forwardInputTween.Value / maxValue) * delay);
                backwardInputTween.TweenTo(0.0f, Mathf.Abs(backwardInputTween.Value / maxValue) * delay);
                leftInputTween.TweenTo(0.0f, Mathf.Abs(leftInputTween.Value / maxValue) * delay);
                rightInputTween.TweenTo(0.0f, Mathf.Abs(rightInputTween.Value / maxValue) * delay);
            }
            else if(context.phase == InputActionPhase.Performed)
            {
                if(inputVector.y == 0.0f)
                {
                    forwardInputTween.Stop();
                    backwardInputTween.Stop();

                    forwardInputTween.TweenTo(0.0f, Mathf.Abs(forwardInputTween.Value / maxValue) * delay);
                    backwardInputTween.TweenTo(0.0f, Mathf.Abs(backwardInputTween.Value / maxValue) * delay);
                }
                else
                {
                    if(inputVector.y > 0)
                    {
                        forwardInputTween.Stop();
                        forwardInputTween.TweenTo(Mathf.Abs(inputVector.y), (1.0f - Mathf.Abs(forwardInputTween.Value / maxValue)) * delay);
                    }
                    else
                    {
                        backwardInputTween.Stop();
                        backwardInputTween.TweenTo(Mathf.Abs(inputVector.y), (1.0f - Mathf.Abs(backwardInputTween.Value / maxValue)) * delay);
                    }
                }


                if(inputVector.x == 0.0f)
                {
                    leftInputTween.Stop();
                    rightInputTween.Stop();

                    leftInputTween.TweenTo(0.0f, Mathf.Abs(leftInputTween.Value / maxValue) * delay);
                    rightInputTween.TweenTo(0.0f, Mathf.Abs(rightInputTween.Value / maxValue) * delay);
                }
                else
                {
                    if(inputVector.x < 0)
                    {
                        leftInputTween.Stop();
                        leftInputTween.TweenTo(Mathf.Abs(inputVector.x), (1.0f - Mathf.Abs(leftInputTween.Value / maxValue)) * delay);
                    }
                    else
                    {
                        rightInputTween.Stop();
                        rightInputTween.TweenTo(Mathf.Abs(inputVector.x),(1.0f - Mathf.Abs(rightInputTween.Value / maxValue)) * delay);
                    }
                }
            }
        }
        
        private void Update()
        {
            if (moveAbility == null /*|| rotateAbility == null*/)
            {
                moveAbility = target.agent.GetAbility<IMoveAbility>();
                changeSpeedAbility = target.agent.GetAbility<IChangeSpeedAbility>();
                rotateAbility = target.agent.GetAbility<IRotateAbility>();
            }
            else
            {
                moveAbility.SetInputVector(new Vector2(rightInputTween.Value - leftInputTween.Value, forwardInputTween.Value - backwardInputTween.Value));
            }

            if(changeSpeedAbility != null)
            {
                changeSpeedAbility.SetSpeedMultiplier(speedInputTween.Value);
            }
        }
        #endif
    }
}
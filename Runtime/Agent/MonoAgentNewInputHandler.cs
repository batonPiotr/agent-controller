namespace HandcraftedGames.AgentController.Abilities
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
        public IStrafeAbility strafeAbility;
        public IRotateAbility rotateAbility;
        public IChangeSpeedAbility changeSpeedAbility;

        [SerializeField]
        private InputHandlingAbility inputHandlingAbility;

        private Vector2 lastInputVector;
        private float lastStrafeValue;

        float maxValue = 1.0f;
        float delay = 0.0f;

        private void OnEnable()
        {
            inputHandlingAbility = new InputHandlingAbility();
        }

        private void OnDisable()
        {
            if(inputHandlingAbility != null && inputHandlingAbility.Agent != null)
                inputHandlingAbility.Agent.RemoveAbility(inputHandlingAbility);
            inputHandlingAbility = null;
        }

        #if ENABLE_INPUT_SYSTEM
        public void OnRun(InputAction.CallbackContext context)
        {
            if(changeSpeedAbility != null)
            {
                float minSpeed = 1.0f;
                float maxSpeed = 2.0f;
                if(context.phase == InputActionPhase.Started)
                    changeSpeedAbility.SetSpeedMultiplier(maxSpeed);
                if(context.phase == InputActionPhase.Canceled)
                changeSpeedAbility.SetSpeedMultiplier(minSpeed);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if(moveAbility == null)
                return;
            lastInputVector = (Vector2)context.ReadValueAsObject();
        }

        public void OnStrafe(InputAction.CallbackContext context)
        {
            if(strafeAbility == null)
                return;
            lastStrafeValue = (float)context.ReadValueAsObject();
        }

        private void Update()
        {
            if (moveAbility == null /*|| rotateAbility == null*/)
            {
                moveAbility = target.agent.GetAbility<IMoveAbility>();
                changeSpeedAbility = target.agent.GetAbility<IChangeSpeedAbility>();
                rotateAbility = target.agent.GetAbility<IRotateAbility>();
                strafeAbility = target.agent.GetAbility<IStrafeAbility>();
            }
            else
            {
                if(inputHandlingAbility.Agent != this.target.agent)
                {
                    if(inputHandlingAbility.Agent != null)
                        inputHandlingAbility.Agent.RemoveAbility(inputHandlingAbility);
                    this.target.agent.AddAbility(inputHandlingAbility);
                }

                if(!inputHandlingAbility.IsActive && !((IAbility)inputHandlingAbility).TryToActivate())
                    return;

                moveAbility.SetInputVector(lastInputVector);
                strafeAbility.SetInput(lastStrafeValue);
            }
        }
        #endif
    }
}
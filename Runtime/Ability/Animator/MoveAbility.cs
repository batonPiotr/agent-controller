namespace HandcraftedGames.AgentController.Abilities.Animator
{
    using System;
    using UnityEngine;
    [Serializable]
    [Ability("Animator/Move")]
    public class MoveAbility : Ability, IMoveAbility
    {
        public override string Name => "Move Ability";
        private Animator animator;
        [SerializeField]
        private string forwardParameterName;
        [SerializeField]
        private float forwardMinValue;
        [SerializeField]
        private float forwardMaxValue;
        
        private int forwardParameterHash;


        [SerializeField]
        private string sidewardParameterName;
        private int sidewardParameterHash;

        [SerializeField]
        private float sidewardMinValue;
        [SerializeField]
        private float sidewardMaxValue;
        [SerializeField]
        private string isMovingParameterName;
        private int isMovingHash;

        private float speedMultiplier = 1.0f;
        public float SpeedMultiplier { get => speedMultiplier; set => speedMultiplier = value; }

        public MoveAbility()
        {

        }

        public MoveAbility(
            string forwardParameterName, float forwardMinValue, float forwardMaxValue,
            string sidewardParameterName, float sidewardMinValue, float sidewardMaxValue,
            string isMovingParameter
        )
        {
            this.forwardParameterName = forwardParameterName;
            this.forwardMinValue = forwardMinValue;
            this.forwardMaxValue = forwardMaxValue;
            forwardParameterHash = Animator.StringToHash(forwardParameterName);

            this.sidewardParameterName = sidewardParameterName;
            this.sidewardMinValue = sidewardMinValue;
            this.sidewardMaxValue = sidewardMaxValue;
            sidewardParameterHash = Animator.StringToHash(sidewardParameterName);
            isMovingHash = Animator.StringToHash(isMovingParameter);
        }

        public void SetInputVector(Vector2 input)
        {
            if(!IsActive && !Agent.ActivateAbility(this))
            {
                Debug.LogWarning("Deactivate " + this);
                return;
            }

            var inputNormalized = (input + Vector2.one) * 0.5f;
            var isForwardMovement = Mathf.Abs(inputNormalized.y) > 0.001f;
            var isSidewardMovement = Mathf.Abs(inputNormalized.x) > 0.001f;
            var isMovement = isForwardMovement || isSidewardMovement;

            // if(isForwardMovement)
                animator.SetFloat(forwardParameterHash, Mathf.Lerp(forwardMinValue * speedMultiplier, forwardMaxValue * speedMultiplier, inputNormalized.y));
            // if(isSidewardMovement)
                animator.SetFloat(sidewardParameterHash, Mathf.Lerp(sidewardMinValue * speedMultiplier, sidewardMaxValue * speedMultiplier, inputNormalized.x));

            var isMoving = input.sqrMagnitude > 0.001;
            animator.SetBool(isMovingHash, isMoving);
            if(!isMoving)
                Stop();
        }

        protected override bool ValidateAgent(IAgent agent)
        {
            animator = agent.GameObject.GetComponent<Animator>();
            return animator != null;
        }

        public override void Stop()
        {
            animator.SetBool(isMovingHash, false);
            animator.SetFloat(forwardParameterHash, 0.0f);
            animator.SetFloat(sidewardParameterHash, 0.0f);
        }
    }
}
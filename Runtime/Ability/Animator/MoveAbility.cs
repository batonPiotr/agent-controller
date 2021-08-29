namespace HandcraftedGames.AgentController.Abilities.Animator
{
    using UnityEngine;
    public class MoveAbility : Ability, IMoveAbility
    {
        private Animator animator;
        private string forwardParameterName;
        private float forwardMinValue;
        private float forwardMaxValue;
        private int forwardParameterHash;


        private string sidewardParameterName;
        private float sidewardMinValue;
        private float sidewardMaxValue;
        private int sidewardParameterHash;
        private int isMovingHash;

        private float speedMultiplier = 1.0f;
        public float SpeedMultiplier { get => speedMultiplier; set => speedMultiplier = value; }

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
            var inputNormalized = (input + Vector2.one) * 0.5f;
            var isForwardMovement = Mathf.Abs(inputNormalized.y) > 0.001f;
            var isSidewardMovement = Mathf.Abs(inputNormalized.x) > 0.001f;
            var isMovement = isForwardMovement || isSidewardMovement;
            // if(!isMovement)
            //     return;
            if(!IsActive && !Agent.ActivateAbility(this))
            {
                Debug.LogWarning("Deactivate " + this);
                return;
            }

            // if(isForwardMovement)
                animator.SetFloat(forwardParameterHash, Mathf.Lerp(forwardMinValue * speedMultiplier, forwardMaxValue * speedMultiplier, inputNormalized.y));
            // if(isSidewardMovement)
                animator.SetFloat(sidewardParameterHash, Mathf.Lerp(sidewardMinValue * speedMultiplier, sidewardMaxValue * speedMultiplier, inputNormalized.x));
            animator.SetBool(isMovingHash, input.sqrMagnitude > 0.001);
        }

        protected override bool ValidateAgent(IAgent agent)
        {
            animator = agent.GameObject.GetComponent<Animator>();
            return animator != null;
        }
    }
}
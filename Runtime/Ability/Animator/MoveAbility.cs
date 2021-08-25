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

        public MoveAbility(
            string forwardParameterName, float forwardMinValue, float forwardMaxValue,
            string sidewardParameterName, float sidewardMinValue, float sidewardMaxValue)
        {
            this.forwardParameterName = forwardParameterName;
            this.forwardMinValue = forwardMinValue;
            this.forwardMaxValue = forwardMaxValue;
            forwardParameterHash = Animator.StringToHash(forwardParameterName);

            this.sidewardParameterName = sidewardParameterName;
            this.sidewardMinValue = sidewardMinValue;
            this.sidewardMaxValue = sidewardMaxValue;
            sidewardParameterHash = Animator.StringToHash(sidewardParameterName);
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
                animator.SetFloat(forwardParameterHash, Mathf.Lerp(forwardMinValue, forwardMaxValue, inputNormalized.y));
            // if(isSidewardMovement)
                animator.SetFloat(sidewardParameterHash, Mathf.Lerp(sidewardMinValue, sidewardMaxValue, inputNormalized.x));
        }

        protected override bool ValidateAgent(IAgent agent)
        {
            animator = agent.GameObject.GetComponent<Animator>();
            return animator != null;
        }
    }
}
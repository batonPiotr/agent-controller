namespace HandcraftedGames.AgentController.Abilities.Animator
{
    using System;
    using System.Collections.Generic;
    using HandcraftedGames.AgentController.Properties;
    using HandcraftedGames.Utils;
    using UnityEngine;
    [Serializable]
    [Ability("Animator/Move")]
    public class MoveAbility : Ability, IMoveAbility
    {
        public override string Name => "Move Ability";
        private MovementProperties movementProperties;
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

        [SerializeField]
        private int EMASize = 5;

        private List<double> forwardValues = new List<double>();
        private List<double> sidewardValues = new List<double>();

        int lastFrameSetInputVector = 0;

        public MoveAbility()
        {

        }

        public void SetInputVector(Vector2 input)
        {
            if(!IsActive && !Agent.ActivateAbility(this))
            {
                Debug.LogWarning("Deactivate " + this);
                return;
            }

            var inputNormalized = (input + Vector2.one) * 0.5f;

            lastFrameSetInputVector = Time.frameCount;
            Add(Mathf.Lerp(forwardMinValue * movementProperties.MovementSpeed, forwardMaxValue * movementProperties.MovementSpeed, inputNormalized.y), forwardValues);
            Add(Mathf.Lerp(sidewardMinValue * movementProperties.MovementSpeed, sidewardMaxValue * movementProperties.MovementSpeed, inputNormalized.x), sidewardValues);


            var forwardValue = (float)forwardValues.ExponentialMovingAverage(EMASize);
            var sidewardValue = (float)sidewardValues.ExponentialMovingAverage(EMASize);

            animator.SetFloat(forwardParameterHash, forwardValue);
            animator.SetFloat(sidewardParameterHash, sidewardValue);

            var isMoving = new Vector2(forwardValue, sidewardValue).sqrMagnitude > 0.001;
            animator.SetBool(isMovingHash, isMoving);
            if(!isMoving)
                Complete();
        }

        protected override bool ShouldBeAddedToAgent(IAgent agent)
        {
            animator = agent.GameObject.GetComponent<Animator>();
            UpdateHashes();
            movementProperties = agent.GetProperties<MovementProperties>();
            return animator != null && movementProperties != null;
        }

        protected override void OnStop(StopReason reason)
        {
            animator.SetBool(isMovingHash, false);
            animator.SetFloat(forwardParameterHash, 0.0f);
            animator.SetFloat(sidewardParameterHash, 0.0f);
        }

        private void UpdateHashes()
        {
            forwardParameterHash = Animator.StringToHash(forwardParameterName);
            sidewardParameterHash = Animator.StringToHash(sidewardParameterName);
            isMovingHash = Animator.StringToHash(isMovingParameterName);
        }

        private void Add(double value, List<double> to)
        {
            while(to.Count >= EMASize)
                to.RemoveAt(0);
            to.Add(value);
        }
    }
}
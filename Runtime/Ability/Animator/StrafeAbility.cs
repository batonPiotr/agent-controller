namespace HandcraftedGames.AgentController.Abilities.Animator
{
    using System;
    using System.Collections.Generic;
    using HandcraftedGames.AgentController.Properties;
    using HandcraftedGames.Utils;
    using UnityEngine;
    [Serializable]
    [Ability("Animator/Strafe")]
    public class StrafeAbility : Ability, IStrafeAbility
    {
        public override string Name => "Strafe Ability";
        private MovementProperties movementProperties;
        private Animator animator;


        [SerializeField]
        private string strafingParameterName;
        private int strafingParameterHash;

        [SerializeField]
        private float sidewardMinValue;
        [SerializeField]
        private float sidewardMaxValue;
        [SerializeField]
        private string isStrafingParameterName;
        private int isStrafingHash;

        [SerializeField]
        private int EMASize = 5;
        private List<double> values = new List<double>();

        public StrafeAbility()
        {

        }

        public void SetInput(float value)
        {
            if(!IsActive && !Agent.ActivateAbility(this))
            {
                Debug.LogWarning("Deactivate " + this);
                return;
            }

            var inputNormalized = (value + 1.0f) * 0.5f;
            Add(Mathf.Lerp(sidewardMinValue * movementProperties.MovementSpeed, sidewardMaxValue * movementProperties.MovementSpeed, inputNormalized), values);

            var sidewardValue = (float)values.ExponentialMovingAverage(EMASize);

            animator.SetFloat(strafingParameterHash, sidewardValue);

            var isStrafing = Mathf.Abs(sidewardValue) > 0.01;
            animator.SetBool(isStrafingHash, isStrafing);
            if(!isStrafing)
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
            animator.SetBool(isStrafingHash, false);
            animator.SetFloat(strafingParameterHash, 0.0f);
        }

        private void UpdateHashes()
        {
            strafingParameterHash = Animator.StringToHash(strafingParameterName);
            isStrafingHash = Animator.StringToHash(isStrafingParameterName);
        }

        private void Add(double value, List<double> to)
        {
            while(to.Count >= EMASize)
                to.RemoveAt(0);
            to.Add(value);
        }
    }
}
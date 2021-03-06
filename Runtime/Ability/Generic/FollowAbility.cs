namespace HandcraftedGames.AgentController.Abilities
{
    using System;
    using HandcraftedGames.Utils;
    using UnityEngine;

    [Serializable]
    [Ability("Generic/Follow")]
    public class FollowAbility : Ability, IFollowAbility, IFixedUpdate
    {
        public override string Name => "Follow Ability";

        [SerializeField]
        private bool stopWhenReached = true;
        public bool StopWhenReached { get => stopWhenReached; set => stopWhenReached = value; }

        [SerializeField]
        private float updateTargetPositionInterval = 1.0f;
        public float UpdateTargetPositionInterval
        {
            get => updateTargetPositionInterval;
            set
            {
                updateTargetPositionInterval = value;
                ScheduleTaskIfNeeded();
            }
        }

        private GameObject target;
        public GameObject Target => target;

        private IGoToAbility goToAbility;

        private object currentScheduledTask = null;

        override protected bool ShouldBeAddedToAgent(IAgent agent)
        {
            goToAbility = agent.GetAbility<IGoToAbility>();

            if(goToAbility == null)
                Debug.LogError("Couldn't add FollowAbility: IGoToAbility not found!");

            return goToAbility != null;
        }

        public void Follow(GameObject target)
        {
            if(!IsActive && !Agent.ActivateAbility(this))
                return;

            this.target = target;

            ScheduleTaskIfNeeded();
        }

        public void FixedUpdate()
        {
            if(!IsActive)
                return;

            if(StopWhenReached && !goToAbility.IsActive && (target.transform.position - Agent.GameObject.transform.position).magnitude < 1.0f)
            {
                Complete();
            }
        }

        private void ScheduleTaskIfNeeded()
        {
            if(currentScheduledTask != null)
            {
                TaskDispatcher.Shared.Stop(currentScheduledTask);
                currentScheduledTask = null;
            }
            if(target == null || !IsActive)
                return;

            goToAbility.OnDidStop += OnGoToStop;

            currentScheduledTask = TaskDispatcher.Shared.Schedule(() => {
                if(!target)
                    return false;
                goToAbility.GoTo(target.transform.position);
                return true;
            }, 0.0f, updateTargetPositionInterval);
        }

        private void OnGoToStop(IAbility ability, StopReason reason)
        {
            if(!IsActive)
                return;

            if(reason != StopReason.Completion)
            {
                Fail();
            }

            if(StopWhenReached && (target.transform.position - Agent.GameObject.transform.position).magnitude < 1.0f)
            {
                Complete();
            }
        }

        protected override void OnStop(StopReason reason)
        {
            if(currentScheduledTask != null)
            {
                TaskDispatcher.Shared.Stop(currentScheduledTask);
                currentScheduledTask = null;
            }
            target = null;
            goToAbility.OnDidStop -= OnGoToStop;
            if(reason != StopReason.Completion)
                goToAbility.Stop();
        }
    }
}
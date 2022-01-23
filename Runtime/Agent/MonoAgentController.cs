namespace HandcraftedGames.AgentController
{
    using System;
    using System.Collections.Generic;
    using HandcraftedGames.AgentController.Abilities;
    using UnityEngine;
    using System.Linq;
    using HandcraftedGames.Utils;
    using HandcraftedGames.AgentController.Abilities.Animator;
    using HandcraftedGames.AgentController.Abilities.Rigidbody;
    using HandcraftedGames.AgentController.Properties;
    using HandcraftedGames.Common;

    [Serializable]
    public class MonoAgentController : MonoBehaviour, IAgentController
    {
        private IAgentController agentController;

        [SerializeReference]
        private List<IAbility> abilities = new List<IAbility>();

        [SerializeReference]
        private List<IProperties> properties = new List<IProperties>();

        private void Awake()
        {
            agentController = new AgentController(gameObject);
            gameObject.GetGODependencyInjection().Register(agentController);
            foreach(var prop in properties)
                AddProperties(prop);
            var abilitiesToEnable = abilities.Where(i => i.IsEnabled);
            foreach(var ability in abilities)
            {
                if(!AddAbility(ability, abilitiesToEnable.Contains(ability)))
                {
                    Debug.LogError("Couldn't Add Ability [" + ability + "]." + ability.Name);
                }
            }
        }

        public GameObject GameObject => agentController.GameObject;

        public List<IAbility> Abilities => agentController.Abilities;

        public event Action<IAgentController> OnDestroy;

        public bool ActivateAbility(IAbility ability)
        {
            return agentController.ActivateAbility(ability);
        }

        public bool AddAbility(IAbility ability, bool enableOnAdd = true)
        {
            return agentController.AddAbility(ability, enableOnAdd);
        }

        public void AddProperties(IProperties properties)
        {
            agentController.AddProperties(properties);
        }

        public void Dispose()
        {
            agentController.Dispose();
        }

        public void FixedUpdate()
        {
            agentController.FixedUpdate();
        }

        public void RemoveAbility(IAbility ability)
        {
            agentController.RemoveAbility(ability);
        }

        public void Update()
        {
            agentController.Update();
        }

        public T GetAbility<T>() where T: class, IAbility
        {
            return agentController.GetAbility<T>();
        }

        public T GetProperties<T>() where T: class, IProperties
        {
            return agentController.GetProperties<T>();
        }
    }
}
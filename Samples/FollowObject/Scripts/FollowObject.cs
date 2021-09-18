using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandcraftedGames.AgentController;
using HandcraftedGames.AgentController.Abilities;

namespace HandcraftedGames.AgentController.Samples
{
    public class FollowObject : MonoBehaviour
    {
        public MonoAgent agent;
        public GameObject objectToFollow;
        private IFollowAbility followAbility;
        
        void Update()
        {
            if(followAbility == null)
            {
                followAbility = agent.agent.GetAbility<IFollowAbility>();
                if(followAbility == null)
                {
                    Debug.LogError("Couldn't find IFollowAbility in the agent!");
                    return;
                }
                followAbility.Follow(objectToFollow);
            }
        }
    }
}
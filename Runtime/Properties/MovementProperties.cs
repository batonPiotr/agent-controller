using System;
using UnityEngine;

namespace HandcraftedGames.AgentController.Properties
{
    [Properties("Movement Properties")]
    [Serializable]
    public class MovementProperties: IProperties
    {
        public string Name => "Movement Properties";

        [SerializeField]
        public float MovementSpeed = 1.0f;
    }
}
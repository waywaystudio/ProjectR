using System;
using UnityEngine;

namespace Common.Character.Operation.Combat.Projector
{
    public class ProjectorEvent : MonoBehaviour
    {
        protected void OnTriggerEnter(Collider other)
        {
            Debug.Log("Colliding!");
        }
    }
}

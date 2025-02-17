﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace HexTecGames.Basics
{
	/// <summary>
    /// Sets this objects position equal to the target's position + offset.
    /// </summary>
    public class FollowTarget : MonoBehaviour
	{
        public Transform Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }
        [SerializeField, FormerlySerializedAs("Target")] private Transform target = default;
        [SerializeField, FormerlySerializedAs("Offset")] private Vector3 offset = default;


        private void Reset()
        {
            offset = transform.position;
        }

        private void Update()
        {
            if (target != null)
            {
                transform.position = target.transform.position + offset;
            }           
        }
    }
}
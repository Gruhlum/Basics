using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
	public class FollowTarget : MonoBehaviour
	{
        public Transform Target;
        public Vector3 Offset;
        private void Update()
        {
            if (Target != null)
            {
                transform.position = Target.transform.position + Offset;
            }           
        }
    }
}
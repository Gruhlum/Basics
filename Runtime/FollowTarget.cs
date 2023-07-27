using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
	public class FollowTarget : MonoBehaviour
	{
        public Transform Target;

        private void Update()
        {
            if (Target != null)
            {
                transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
            }           
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{
    [DisallowMultipleComponent]
    public class SkyDivingMovement : MovementType
	{
        public bool CanSkyDive()
		{
            RaycastHit hit;
			if (Physics.Raycast(transform.position, Vector3.down, out hit, 3000f, 17))
			{
				if (hit.distance < 50)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHitbox : MonoBehaviour
{
	public float damageMultiplier;

	private TankController tankController;

	public void SetTankController(TankController tankController)
	{
		this.tankController = tankController;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Shell")
		{
			if (tankController != null)
			{
				tankController.RecieveDamage((int)(15 * damageMultiplier));
			}
		}
	}
}

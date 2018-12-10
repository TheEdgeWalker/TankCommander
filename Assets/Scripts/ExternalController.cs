using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalController : MonoBehaviour
{
	protected TankController tankController;

	protected void Awake()
	{
		tankController = GetComponent<TankController>();
	}

	protected void Fire(Vector3 target)
	{
		tankController.RotateTurretTo(target);
		tankController.Fire();
	}
}

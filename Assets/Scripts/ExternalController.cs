using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalController : MonoBehaviour
{
	protected TankController tankController;

	private void Awake()
	{
		tankController = GetComponent<TankController>();
	}

	protected void SetDestination(Vector3 destination)
	{
		tankController.SetDestination(destination);
	}

	protected void Fire(Vector3 target)
	{
		tankController.RotateTurretTo(target);
		tankController.Fire();
	}
}

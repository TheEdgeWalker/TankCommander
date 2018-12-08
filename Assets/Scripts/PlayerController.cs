using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private TankController tankController;

	private void Awake()
	{
		tankController = GetComponent<TankController>();
	}

	private void Start()
	{
		CameraController.instance.SetTarget(transform);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.tag == "Enemy")
				{
					tankController.RotateTurretTo(hit.point);
					tankController.Fire();
				}
				else
				{
					tankController.SetDestination(hit.point);
				}
			}
		}
	}
}

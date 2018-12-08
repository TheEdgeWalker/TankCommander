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
		if (Input.GetMouseButtonUp(0))
		{
			Vector3 destination;
			if (GetClickPosition(out destination))
			{
				tankController.SetDestination(destination);
			}
		}
	}

	private bool GetClickPosition(out Vector3 position)
	{
		position = Vector3.zero;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			position = hit.point;
			return true;
		}

		return false;
	}
}

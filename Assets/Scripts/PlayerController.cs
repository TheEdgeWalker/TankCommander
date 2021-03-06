﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : ExternalController
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (Input.GetKey(KeyCode.A))
				{
					Fire(hit.point);
				}
				else
				{
					tankController.SetDestination(hit.point);
				}
			}
		}
	}
}

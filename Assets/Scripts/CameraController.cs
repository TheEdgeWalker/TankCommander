using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	static public CameraController instance;

	private Transform target;
	private Vector3 positionOffset;

	private void Awake()
	{
		positionOffset = transform.position;

		instance = this;
	}

	private void Update()
	{
		if (target != null)
		{
			transform.position = target.position + positionOffset;
		}
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public void ClearTarget()
	{
		target = null;
	}
}

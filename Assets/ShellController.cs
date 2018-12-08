using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
	public float speed;

	private void Update()
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}

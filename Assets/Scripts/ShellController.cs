using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
	public float speed;
	public ParticleSystem explosion;

	private void Update()
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	private void OnDisable()
	{
		explosion.transform.position = transform.position;
		explosion.Play();
	}
}

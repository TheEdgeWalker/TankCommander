using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
	public float speed = 20;
	public float range = 20;
	public ParticleSystem explosion;

	private float distance = 0f;

	private void Update()
	{
		Vector3 velocity = Vector3.forward * speed * Time.deltaTime;
		transform.Translate(velocity);

		distance += velocity.magnitude;
		if (distance >= range)
		{
			gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		distance = 0f;
	}

	private void OnDisable()
	{
		explosion.transform.position = transform.position;
		explosion.Play();
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Shell collision: " + other.tag);
		gameObject.SetActive(false);
	}
}

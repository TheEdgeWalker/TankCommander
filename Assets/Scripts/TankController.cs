using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : MonoBehaviour
{
	public Transform turret;

	private NavMeshAgent agent;
	private TankHitbox[] hitboxes;

	private int health = 100;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		hitboxes = GetComponentsInChildren<TankHitbox>();

		foreach(TankHitbox hitbox in hitboxes)
		{
			hitbox.SetTankController(this);
		}
	}

	private void Update()
	{
		if (health <= 0)
		{
			gameObject.SetActive(false);
		}
	}

	public void SetDestination(Vector3 destination)
	{
		if (agent.velocity.magnitude > 0f)
		{
			Debug.Log("Agent is moving, cannot set new destination");
			return;
		}

		agent.SetDestination(destination);
	}

	public void RotateTurretTo(Vector3 target)
	{
		target.y = turret.position.y;
		turret.LookAt(target);
		
	}

	public void Fire()
	{
		ShellManager.instance.Fire(turret);
	}

	public void RecieveDamage(int damage)
	{
		health -= damage;
	}
}

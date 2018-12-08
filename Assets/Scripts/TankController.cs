using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : MonoBehaviour
{
	public Transform turret;

	public int hitPoint = 100;
	public int actionPoint = 100;

	private NavMeshAgent agent;
	private TankHitbox[] hitboxes;

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
		if (hitPoint <= 0)
		{
			gameObject.SetActive(false);
		}
	}

	public bool IsBusy()
	{
		return agent.pathPending || agent.velocity.magnitude > 0f;
	}

	public void SetDestination(Vector3 destination)
	{
		if (IsBusy())
		{
			Debug.Log("Agent is busy, cannot set new destination");
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
		if (IsBusy())
		{
			Debug.Log("Agent is busy, cannot fire");
			return;
		}

		ShellManager.instance.Fire(turret);
	}

	public void RecieveDamage(int damage)
	{
		hitPoint -= damage;
	}
}

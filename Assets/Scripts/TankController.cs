using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : MonoBehaviour
{
	public Transform turret;

	public int hitPoint = 100;
	public int actionPoint = 100;

	const int MOVE_COST = 2;
	const int FIRE_COST = 40;

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

	private void Start()
	{
		prevPosition = transform.position;
	}

	private Vector3 prevPosition;
	private float distanceMoved = 0f;
	private void Update()
	{
		if (actionPoint < agent.remainingDistance * MOVE_COST)
		{
			Debug.Log("Not enough action points to move");
			agent.ResetPath();
			return;
		}

		distanceMoved += Vector3.Distance(prevPosition, transform.position);
		while (distanceMoved >= 1f)
		{
			actionPoint -= MOVE_COST;
			distanceMoved -= 1f;
		}

		prevPosition = transform.position;
	}

	private bool IsMoving()
	{
		return agent.velocity.magnitude > 0f;
	}

	private bool IsBusy()
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

		if (actionPoint < FIRE_COST)
		{
			Debug.Log("Not enough action points to fire");
			return;
		}

		actionPoint -= FIRE_COST;
		ShellManager.instance.Fire(turret);
	}

	public void RecieveDamage(int damage)
	{
		hitPoint -= damage;

		if (hitPoint <= 0)
		{
			gameObject.SetActive(false);
		}
	}
}

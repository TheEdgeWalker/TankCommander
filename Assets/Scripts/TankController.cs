using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : MonoBehaviour
{
	public Transform turret;
	public Transform muzzle;

	public TankResource hitPoint = new TankResource(100);
	public TankResource actionPoint = new TankResource(100);

	public int moveCost = 2;
	public int fireCost = 40;

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
		if (actionPoint.Value < moveCost)
		{
			agent.ResetPath();
			return;
		}

		distanceMoved += Vector3.Distance(prevPosition, transform.position);
		while (distanceMoved >= 1f)
		{
			actionPoint.Subtract(moveCost);
			distanceMoved -= 1f;
		}

		prevPosition = transform.position;
	}

	public bool IsMoving()
	{
		return agent.velocity.magnitude > 0f;
	}

	public bool IsBusy()
	{
		return agent.pathPending || IsMoving();
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

		if (actionPoint.Value < fireCost)
		{
			Debug.Log("Not enough action points to fire");
			return;
		}

		actionPoint.Subtract(fireCost);
		ShellManager.instance.Fire(muzzle);
	}

	public void RecieveDamage(int damage)
	{
		hitPoint.Subtract(damage);

		if (hitPoint.IsZero())
		{
			gameObject.SetActive(false);
			BattleManager.instance.CheckVictory();
		}
	}
}

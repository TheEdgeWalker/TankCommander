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
	public int fireCost = 30;

	private NavMeshAgent agent;
	private NavMeshPath path;
	private TankHitbox[] hitboxes;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		path = new NavMeshPath();
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

	public bool SetDestination(Vector3 destination, float stoppingDistance = 0.5f)
	{
		if (IsBusy())
		{
			Debug.Log("Agent is busy, cannot set new destination");
			return false;
		}

		agent.stoppingDistance = stoppingDistance;
		agent.ResetPath();
		return agent.SetDestination(destination);
	}

	public bool CalculateAndSetPath(Vector3 destination, int maximumCost, float stoppingDistance = 0.5f)
	{
		agent.CalculatePath(destination, path);

		if (path.status != NavMeshPathStatus.PathInvalid)
		{
			if (GetPathLength() * moveCost <= maximumCost)
			{
				agent.stoppingDistance = stoppingDistance;
				agent.ResetPath();
				return agent.SetPath(path);
			}
		}

		return false;
	}

	private float GetPathLength()
	{
		float length = 0f;

		if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
		{
			for (int i = 1; i < path.corners.Length; ++i)
			{
				length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
			}
		}

		return length;
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

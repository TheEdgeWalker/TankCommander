using BTAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : ExternalController
{
	private Root aiRoot;
	private GameObject target;

	private NavMeshAgent agent;
	private NavMeshPath path;

	private new void Awake()
	{
		base.Awake();

		agent = GetComponent<NavMeshAgent>();
		path = new NavMeshPath();
	}

	private void Start()
	{
		aiRoot = BT.Root();
		aiRoot.OpenBranch(
			BT.If(HasTarget).OpenBranch(
				BT.If(tankController.IsBusy).OpenBranch(
					BT.Call(() => { Debug.Log("Busy"); }),
					BT.Wait(1f)
				),
				BT.If(CanTakeBehind).OpenBranch(
					BT.Wait(5f),
					BT.Call(FireAtTarget),
					BT.Wait(1f)
				),
				BT.If(CanFireAtTarget).OpenBranch(
					BT.Call(FireAtTarget),
					BT.Wait(1f)
				),
				BT.If(CanMoveToTarget).OpenBranch(
					BT.Call(MoveToTarget),
					BT.Wait(1f)
				),
				BT.If(IsNotMoving).OpenBranch(
					BT.Wait(1f),
					BT.Call(EndTurn)
				)
			),
			BT.If(HasNoTarget).OpenBranch(
				BT.Call(() => { Debug.Log("Honestly, this should not happen"); }),
				BT.Call(EndTurn)
			)
		);
	}

	private void OnEnable()
	{
		ChooseTarget();
	}

	private void Update()
	{
		aiRoot.Tick();
	}

	private void ChooseTarget()
	{
		target = BattleManager.instance.GetClosestTank(transform.position);
	}

	private bool HasTarget()
	{
		return target != null;
	}

	private bool HasNoTarget()
	{
		return target == null;
	}

	private bool CanFireAtTarget()
	{
		if (tankController.IsBusy())
		{
			return false;
		}

		if (tankController.actionPoint.Value < tankController.fireCost)
		{
			return false;
		}

		// check distance
		if (!IsInCannonRange())
		{
			return false;
		}

		// raytrace from muzzle
		Vector3 targetPos = target.transform.position;
		targetPos.y = tankController.muzzle.position.y;

		RaycastHit hit;
		if (Physics.Linecast(tankController.muzzle.position, targetPos, out hit))
		{
			if (hit.collider.tag != "Player")
			{
				return false;
			}
		}

		return true;
	}

	private void FireAtTarget()
	{
		Fire(target.transform.position);
	}

	private bool CanMoveToTarget()
	{
		if (tankController.IsBusy())
		{
			return false;
		}

		if (tankController.actionPoint.Value < tankController.moveCost)
		{
			return false;
		}

		return true;
	}

	private void MoveToTarget()
	{
		agent.stoppingDistance = 15f;
		SetDestination(target.transform.position);
	}

	private void EndTurn()
	{
		BattleManager.instance.EndTurn();
	}

	private bool IsNotMoving()
	{
		return !tankController.IsMoving();
	}

	private bool CanTakeBehind()
	{
		Vector3 targetDir = transform.position - target.transform.position;
		float angle = Vector3.Angle(target.transform.forward, targetDir);
		if (angle > 135f && angle < 225f && IsInCannonRange())
		{
			Debug.Log("I already have your six");
			return false;
		}

		Vector3 targetBehind = target.transform.position - (4f * target.transform.forward);
		if (agent.CalculatePath(targetBehind, path) &&
			path.status == NavMeshPathStatus.PathComplete &&
			GetPathLength(path) * tankController.moveCost < tankController.actionPoint.Value - tankController.fireCost
		)
		{
			Debug.Log("Attempting to take back: " + targetBehind);
			agent.stoppingDistance = 0.5f;
			agent.ResetPath();
			return agent.SetPath(path);
		}

		return false;
	}

	private bool IsInCannonRange()
	{
		ShellController shell = ShellManager.instance.shell.GetComponent<ShellController>();
		return shell.range >= Vector3.Distance(transform.position, target.transform.position);
	}

	public static float GetPathLength(NavMeshPath path)
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
}

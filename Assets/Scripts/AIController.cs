using BTAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : ExternalController
{
	private Root aiRoot;
	private GameObject target;

	private void Start()
	{
		aiRoot = BT.Root();
		aiRoot.OpenBranch(
			BT.If(HasTarget).OpenBranch(
				BT.If(CanFireAtTarget).OpenBranch(
					BT.Call(FireAtTarget),
					BT.Wait(2f)
				),
				BT.If(CanMoveToTarget).OpenBranch(
					BT.Call(MoveToTarget),
					BT.Wait(1f)
				),
				BT.Sequence().OpenBranch(
					BT.Wait(2f),
					BT.Call(EndTurn)
				)
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

	private bool CanFireAtTarget()
	{
		if (tankController.actionPoint.Value < tankController.fireCost)
		{
			return false;
		}

		// check distance
		ShellController shell = ShellManager.instance.shell.GetComponent<ShellController>();
		if (shell.range < Vector3.Distance(transform.position, target.transform.position))
		{
			return false;
		}

		// raytrace from turret
		Vector3 targetPos = target.transform.position;
		targetPos.y = tankController.turret.position.y;

		RaycastHit hit;
		if (Physics.Linecast(tankController.turret.position, targetPos, out hit))
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
		if (tankController.actionPoint.Value < tankController.moveCost)
		{
			return false;
		}

		return true;
	}

	private void MoveToTarget()
	{
		SetDestination(target.transform.position);
	}

	private void EndTurn()
	{
		BattleManager.instance.EndTurn();
	}
}

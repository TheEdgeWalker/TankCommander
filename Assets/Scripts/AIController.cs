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
					BT.Call(Fire),
					BT.Wait(5f)
				),
				BT.Sequence().OpenBranch(
					BT.Call(MoveToTarget),
					BT.Wait(1f)
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
		return true;
	}


	private void Fire()
	{
		Debug.Log("Fire");
		Fire(target.transform.position);
	}

	private void MoveToTarget()
	{
		Debug.Log("MoveToTarget");
		SetDestination(target.transform.position);
	}
}

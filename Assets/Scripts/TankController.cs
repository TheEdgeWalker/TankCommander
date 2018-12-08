using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : MonoBehaviour
{
	private NavMeshAgent agent;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
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
}

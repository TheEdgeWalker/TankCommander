using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
	public static BattleManager instance;

	public Transform uiCanvas;
	public GameObject playerTankPrefab;
	public GameObject enemyTankPrefab;
	public GameObject tankStatusBarsPrefab;
	public Transform[] playerTankSpawnPoints;
	public Transform[] enemyTankSpawnPoints;
	public Button endTurnButton;

	private List<GameObject> tanks = new List<GameObject>();
	private int currentTankIndex = 0;

	private void Awake()
	{
		instance = this;

		for (int i = 0; i < 3; ++i)
		{
			InstantiateTank(playerTankPrefab, playerTankSpawnPoints);
			InstantiateTank(enemyTankPrefab, enemyTankSpawnPoints);
		}
	}

	private void Start()
	{
		SetCurrentTank();
	}

	private void InstantiateTank(GameObject tankPrefab, Transform[] spawnPoints)
	{
		GameObject newTank = Instantiate(tankPrefab);
		GameObject newTankStatusBars = Instantiate(tankStatusBarsPrefab);

		TankController tankController = newTank.GetComponent<TankController>();
		ExternalController externalController = newTank.GetComponent<ExternalController>();
		UITankStatus tankStatusBars = newTankStatusBars.GetComponent<UITankStatus>();

		if (tankController != null && externalController !=null && tankStatusBars != null)
		{
			int randomIndex = Random.Range(0, spawnPoints.Length - 1);
			newTank.transform.position = spawnPoints[randomIndex].position;
			newTankStatusBars.transform.SetParent(uiCanvas);

			tankStatusBars.SetTank(tankController, externalController);

			// disable initially
			externalController.enabled = false;

			tanks.Add(newTank);
		}
		else
		{
			Debug.LogError("Components not found");
		}
	}

	private void SetCurrentTank()
	{
		GameObject tank = tanks[currentTankIndex];
		if (!tank.activeInHierarchy)
		{
			EndTurn();
			return;
		}

		endTurnButton.interactable = tank.tag == "Player";

		CameraController.instance.SetTarget(tank.transform);

		ExternalController externalController = tank.GetComponent<ExternalController>();
		if (externalController != null)
		{
			externalController.enabled = true;
		}
	}

	public void EndTurn()
	{
		GameObject tank = tanks[currentTankIndex];

		TankController tankController = tank.GetComponent<TankController>();
		if (tankController != null)
		{
			tankController.actionPoint.Add(50);
		}

		ExternalController externalController = tank.GetComponent<ExternalController>();
		if (externalController != null)
		{
			externalController.enabled = false;
		}

		currentTankIndex = currentTankIndex == tanks.Count - 1 ? 0 : currentTankIndex + 1;
		SetCurrentTank();
	}

	public GameObject GetClosestTank(Vector3 from, string tag = "Player")
	{
		// if target is null, no approporiate tank was found
		GameObject target = null;

		float minDistance = float.MaxValue;
		foreach (GameObject tank in tanks)
		{
			if (tank.tag != tag || !tank.activeInHierarchy)
			{
				continue;
			}

			float distance = Vector3.Distance(from, tank.transform.position);
			if (distance < minDistance)
			{
				minDistance = distance;
				target = tank;
			}
		}

		return target;
	}

	public void CheckVictory()
	{
		int players = 0;
		int enemies = 0;

		foreach (GameObject tank in tanks)
		{
			if (!tank.activeInHierarchy)
			{
				continue;
			}

			if (tank.tag == "Player")
			{
				players++;
			}
			else if (tank.tag == "Enemy")
			{
				enemies++;
			}
		}

		if (players == 0)
		{
			Debug.Log("Enemy Win!");
		}
		else if (enemies == 0)
		{
			Debug.Log("Player Win!");
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	public Transform uiCanvas;
	public GameObject playerTankPrefab;
	public GameObject enemyTankPrefab;
	public GameObject tankStatusBarsPrefab;
	public Transform[] playerTankSpawnPoints;
	public Transform[] enemyTankSpawnPoints;

	private List<TankController> tanks = new List<TankController>(8);

	private void Awake()
	{
		InstantiateTank(playerTankPrefab, playerTankSpawnPoints);
		InstantiateTank(enemyTankPrefab, enemyTankSpawnPoints);
	}

	private void Start()
	{
		CameraController.instance.SetTarget(tanks[0].transform);
	}

	private void InstantiateTank(GameObject tankPrefab, Transform[] spawnPoints)
	{
		GameObject newTank = Instantiate(tankPrefab);
		GameObject newTankStatusBars = Instantiate(tankStatusBarsPrefab);

		TankController tankController = newTank.GetComponent<TankController>();
		TankStatusBars tankStatusBars = newTankStatusBars.GetComponent<TankStatusBars>();

		if (tankController != null && tankStatusBars != null)
		{
			newTank.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position;
			newTankStatusBars.transform.parent = uiCanvas;

			tankStatusBars.tankController = tankController;

			tanks.Add(tankController);
		}
	}

	public void EndTurn()
	{

	}
}

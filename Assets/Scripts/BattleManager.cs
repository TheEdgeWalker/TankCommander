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

	private struct Tank
	{
		public GameObject gameObject;
		public TankController tankController;
		public ExternalController externalController;
	}

	private List<Tank> tanks = new List<Tank>();
	private int currentTankIndex = 0;

	private void Awake()
	{
		for (int i = 0; i < 3; ++i)
		{
			InstantiateTank(playerTankPrefab, playerTankSpawnPoints);
			InstantiateTank(enemyTankPrefab, enemyTankSpawnPoints);
		}
	}

	private void Start()
	{
		SetCurrentTank(currentTankIndex);
	}

	private void InstantiateTank(GameObject tankPrefab, Transform[] spawnPoints)
	{
		GameObject newTank = Instantiate(tankPrefab);
		GameObject newTankStatusBars = Instantiate(tankStatusBarsPrefab);

		TankController tankController = newTank.GetComponent<TankController>();
		ExternalController externalController = newTank.GetComponent<ExternalController>();
		TankStatusBars tankStatusBars = newTankStatusBars.GetComponent<TankStatusBars>();

		if (tankController != null && externalController !=null && tankStatusBars != null)
		{
			newTank.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position;
			newTankStatusBars.transform.SetParent(uiCanvas);

			tankStatusBars.tankController = tankController;

			Tank tank;
			tank.gameObject = newTank;
			tank.tankController = tankController;
			tank.externalController = externalController;

			// disable initially
			tank.externalController.enabled = false;

			tanks.Add(tank);
		}
		else
		{
			Debug.LogError("Components not found");
		}

		// disable player control by default
	}

	private void SetCurrentTank(int index)
	{
		Tank tank = tanks[index];
		CameraController.instance.SetTarget(tank.gameObject.transform);
		tank.externalController.enabled = true;
	}

	public void EndTurn()
	{
		Tank currentTank = tanks[currentTankIndex];
		currentTank.tankController.actionPoint += 50;
		currentTank.externalController.enabled = false;

		currentTankIndex = currentTankIndex == tanks.Count - 1 ? 0 : currentTankIndex + 1;
		SetCurrentTank(currentTankIndex);
	}
}

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

	private List<TankController> tankControllers = new List<TankController>(8);
	private int currentTankIndex = 0;

	private void Awake()
	{
		InstantiateTank(playerTankPrefab, playerTankSpawnPoints);
		InstantiateTank(enemyTankPrefab, enemyTankSpawnPoints);
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
		TankStatusBars tankStatusBars = newTankStatusBars.GetComponent<TankStatusBars>();

		if (tankController != null && tankStatusBars != null)
		{
			newTank.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position;
			newTankStatusBars.transform.SetParent(uiCanvas);

			tankStatusBars.tankController = tankController;

			tankController.enabled = false;

			tankControllers.Add(tankController);
		}
	}

	private void SetCurrentTank(int index)
	{
		tankControllers[index].enabled = true;
		CameraController.instance.SetTarget(tankControllers[index].transform);
	}

	public void EndTurn()
	{
		tankControllers[currentTankIndex].actionPoint += 50;
		tankControllers[currentTankIndex].enabled = false;

		currentTankIndex = currentTankIndex == tankControllers.Count - 1 ? 0 : currentTankIndex + 1;
		SetCurrentTank(currentTankIndex);
	}
}

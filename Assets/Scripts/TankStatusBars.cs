using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankStatusBars : MonoBehaviour
{
	public Slider hitPointSlider;
	public Slider actionPointSlider;

	private TankController tankController;

	public void SetTankController(TankController tankController)
	{
		this.tankController = tankController;

		hitPointSlider.maxValue = tankController.hitPoint.Max;
		actionPointSlider.maxValue = tankController.actionPoint.Max;
	}

	private void Update()
	{
		if (tankController == null)
		{
			return;
		}

		if (!tankController.gameObject.activeInHierarchy)
		{
			gameObject.SetActive(false);
		}

		hitPointSlider.value = tankController.hitPoint.Value;
		actionPointSlider.value = tankController.actionPoint.Value;

		Vector3 screenPoint = Camera.main.WorldToScreenPoint(tankController.transform.position);
		transform.position = screenPoint + (Vector3.up * 40f);
	}
}

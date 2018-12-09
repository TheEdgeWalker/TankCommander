using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITankStatus : MonoBehaviour
{
	public Slider hitPointSlider;
	public Slider actionPointSlider;
	public Image selectedArrow;

	private TankController tankController;
	private ExternalController externalController;

	public void SetTank(TankController tankController, ExternalController externalController)
	{
		this.tankController = tankController;
		this.externalController = externalController;

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
			return;
		}

		selectedArrow.gameObject.SetActive(externalController.enabled);

		hitPointSlider.value = tankController.hitPoint.Value;
		actionPointSlider.value = tankController.actionPoint.Value;

		Vector3 screenPoint = Camera.main.WorldToScreenPoint(tankController.transform.position);
		transform.position = screenPoint + (Vector3.up * 40f);
	}
}

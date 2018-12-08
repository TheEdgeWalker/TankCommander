using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankStatusBars : MonoBehaviour
{
	public TankController tankController;
	public Slider hitPointSlider;
	public Slider actionPointSlider;

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

		hitPointSlider.value = tankController.hitPoint;
		actionPointSlider.value = tankController.actionPoint;

		Vector3 screenPoint = Camera.main.WorldToScreenPoint(tankController.transform.position);
		transform.position = screenPoint + (Vector3.up * 40f);
	}
}

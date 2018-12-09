using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellManager : MonoBehaviour
{
	public static ShellManager instance;

	public GameObject shell;

	private void Awake()
	{
		instance = this;
		shell.SetActive(false);
	}

	public void Fire(Transform muzzle)
	{
		if (shell.activeInHierarchy)
		{
			return;
		}

		shell.transform.position = muzzle.position;
		shell.transform.rotation = muzzle.rotation;

		shell.SetActive(true);
	}
}

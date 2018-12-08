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

	public void Fire(Transform turret)
	{
		if (shell.activeInHierarchy)
		{
			return;
		}

		shell.transform.position = turret.position;
		shell.transform.rotation = turret.rotation;

		// Place the shell a little bit forward to the turret
		shell.transform.Translate(Vector3.forward * 1.3f);

		shell.SetActive(true);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankResource
{
	public int Value
	{
		get;
		private set;
	}

	public int Max
	{
		get;
		private set;
	}

	public TankResource(int max)
	{
		Max = Value = max;
	}

	public void Add(int value)
	{
		if (value < 0)
		{
			Debug.LogError("You should not add negative numbers");
			return;
		}

		Value = Mathf.Min(Value + value, Max);
	}

	public void Subtract(int value)
	{
		if (value < 0)
		{
			Debug.LogError("You should not subtract negative numbers");
			return;
		}

		Value = Mathf.Max(Value - value, 0);
	}

	public void Set(int value)
	{
		if (value < 0 || value > Max)
		{
			Debug.LogError("Invalid value");
			return;
		}

		Value = value;
	}

	public bool IsZero()
	{
		return Value == 0;
	}

	public bool IsMax()
	{
		return Value == Max;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RandomDirections
{
	[SerializeField] private AxisType axisType;

	public Vector2 ReturnDirections()
	{
		float randomValue = UnityEngine.Random.Range(-1f,1f);
		int ceiledRandomValue = RoundingHelper.InvertOnNegativeCeil(randomValue);

		Vector2 direction;

		if(axisType == AxisType.Horizontal)
		{
			direction = new Vector2(ceiledRandomValue, 0);
		}
		else
		{
			direction = new Vector2(0, ceiledRandomValue);
		}

		return direction;
	}
}
using System;
using UnityEngine;

[Serializable]
public class RandomMoves
{
	[SerializeField] private int maxValue = 6;
	[SerializeField] private int minValue = 1;

	public int ReturnValue()
	{
		int totalMoves = UnityEngine.Random.Range(minValue, maxValue);
		return totalMoves; 
	}
}
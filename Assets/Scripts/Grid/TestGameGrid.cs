using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameGrid : MonoBehaviour
{
	void Start ()
    {
        //removes the top left obstacle into a path
        Vector2 _pos = new Vector2(0, 0);
        GameGrid.Instance.removeOccupied(_pos);

        //Adds an obstacle to point (3,2)
        _pos = new Vector2(3, 2);
        GameGrid.Instance.AddOccupied(_pos, NodeType.Obstacle);

        //Checks if an obstacle is on the way
        _pos = new Vector2(4, 0);
        if (GameGrid.Instance.IsOccupied(_pos))
        {
            Debug.Log("Obstacle in the way");
        }
	}
}
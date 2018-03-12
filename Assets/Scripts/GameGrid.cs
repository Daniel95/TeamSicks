using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameGrid : MonoBehaviour
{

    public static GameGrid Instance { get { return GetInstance(); } }

    public Dictionary<Vector2, Node> NodeByGridPosition { get { return nodeByGridPosition; } }

    private static GameGrid instance;

    private Dictionary<Vector2, Node> nodeByGridPosition = new Dictionary<Vector2, Node>();

    private static GameGrid GetInstance() {
        if (instance == null) {
            instance = FindObjectOfType<GameGrid>();
        }
        return instance;
    }

}

public class Node
{


}

public class LevelExample {

    public int[,] testGrid = new int[5, 5]
    {
        { 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 },
        { 0, 2, 2, 0, 0 },
        { 0, 2, 2, 0, 0 },
        { 0, 0, 0, 0, 0 }
    };

    public void Test()
    {
        NodeType nodeType = (NodeType)testGrid[2, 2];
    }

}

public enum NodeType
{
    Empty = 0,
    Player = 1,
    Obstacle = 2,
}
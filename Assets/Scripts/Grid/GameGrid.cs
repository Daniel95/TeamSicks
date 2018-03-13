using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameGrid : MonoBehaviour
{
    #region SingleTon

    public static GameGrid Instance { get { return GetInstance(); } }
    private static GameGrid instance;

    private static GameGrid GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameGrid>();
        }
        return instance;
    }
    #endregion

    Dictionary<Vector2, Node> Grid = new Dictionary<Vector2, Node>();

    [SerializeField]    private Paths paths;
    [SerializeField]    private Obstacles obstacles;
    [SerializeField]    private Specials specials;

    [Space(5)]

    [SerializeField]    private int step;

    private void Awake()
    {
        Level level = new Level();

        Grid = CreateGrid(level.TestGrid);
    }

    private Dictionary<Vector2, Node> CreateGrid(int[,] _gridLayout)
    {
        Dictionary<Vector2, Node> _grid = new Dictionary<Vector2, Node>();

        for (int i = 0; i < _gridLayout.GetLength(0); i++)
        {
            for (int j = 0; j < _gridLayout.GetLength(1); j++)
            {
                GameObject[] _spawnList = Spawnlist((NodeType)_gridLayout[i, j]);
                Vector3 position = new Vector3(step * j, -step * i);
                Node _node = Instantiate(_spawnList[UnityEngine.Random.Range(0, _spawnList.Length - 1)], position, Quaternion.identity, transform).GetComponent<Node>();

                _grid.Add(new Vector2(j,i), _node);
            }
        }

        return _grid;
    }

    private GameObject[] Spawnlist(NodeType nodeType)
    {
        GameObject[] _spawnList;
        switch (nodeType)
        {
            case NodeType.Path:
                _spawnList = paths.PathsPrefabs;
                break;
            case NodeType.Obstacle:
                _spawnList = obstacles.ObstaclePrefabs;
                break;
            case NodeType.Special:
                _spawnList = specials.SpecialsPrefabs;
                break;
            default:
                Debug.LogError("Triggered default");
                _spawnList = paths.PathsPrefabs;
                break;
        }
        return _spawnList;
    }

    public bool IsOccupied(Vector2 _positionToCheck)
    {
        switch (Grid[new Vector2(_positionToCheck.x, _positionToCheck.y)].Type)
        {
            case NodeType.Obstacle:
                return true;
            default:
                return false;
        }
    }

    public void AddOccupied(Vector2 _positionToAdd, NodeType _nodeType)
    {
        GameObject[] _spawnList = Spawnlist(_nodeType);
        Node _node = Instantiate(_spawnList[UnityEngine.Random.Range(0, _spawnList.Length - 1)], Grid[_positionToAdd].transform.position, Quaternion.identity, transform).GetComponent<Node>();

        _node.name = "Test Object";

        Destroy(Grid[_positionToAdd].gameObject);
        Grid[_positionToAdd] = _node;

        Debug.Log("Added");
    }

    public void removeOccupied(Vector2 _positionToRemove)
    {
        Node _node = Instantiate(paths.PathsPrefabs[UnityEngine.Random.Range(0, paths.PathsPrefabs.Length - 1)], Grid[_positionToRemove].transform.position, Quaternion.identity, transform).GetComponent<Node>();

        Destroy(Grid[_positionToRemove].gameObject);
        Grid[_positionToRemove] = _node;
        Debug.Log("Removed");
    }
}

public class Level
{
    public int[,] TestGrid = new int[5, 5]
    {
        { 1, 0, 1, 1, 1 },
        { 2, 0, 1, 2, 1 },
        { 0, 1, 1, 0, 1 },
        { 0, 0, 0, 0, 1 },
        { 1, 1, 1, 1, 1 }
    };
}


[Serializable]
public class Paths
{
    public GameObject[] PathsPrefabs;
}

[Serializable]
public class Obstacles
{
    public GameObject[] ObstaclePrefabs;
}

[Serializable]
public class Specials
{
    public GameObject[] SpecialsPrefabs;
}
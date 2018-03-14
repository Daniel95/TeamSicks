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
    [SerializeField]    private Players players;

    [Space(5)]

    [SerializeField]    private int step;

    private void Awake()
    {
        Level level = new Level();

        Grid = CreateGrid(new int[][,] { level.ObstacleGrid, level.PickupGrid });
    }

    private Dictionary<Vector2, Node> CreateGrid(int[][,] _gridLayout)
    {
        Dictionary<Vector2, Node> _grid = new Dictionary<Vector2, Node>();

        foreach (int[,] _currentGrid in _gridLayout)
        {
            for (int i = 0; i < _currentGrid.GetLength(0); i++)
            {
                for (int j = 0; j < _currentGrid.GetLength(1); j++)
                {
                    if (_currentGrid[i, j] != (int)NodeType.Null)
                    {
                        GameObject[] _spawnList = Spawnlist((NodeType)_currentGrid[i, j]);
                        Vector3 position = new Vector3(step * j, -step * i);
                        GameObject _node = Instantiate(_spawnList[UnityEngine.Random.Range(0, _spawnList.Length - 1)], position, Quaternion.identity, transform);

                        if (_grid.ContainsKey(new Vector2(j, i)))
                        {
                            _grid[new Vector2(j, i)].Type.Add((NodeType)_currentGrid[i, j]);
                            _node.transform.parent = _grid[new Vector2(j, i)].transform;
                        }
                        else
                        {
                            Node _nodeType = _node.GetComponent<Node>();

                            _nodeType.Type.Add((NodeType)_currentGrid[i, j]);
                            _grid.Add(new Vector2(j, i), _nodeType);
                        }
                    }
                }
            }
        }

        #region Old Code
        /*
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
        */
        #endregion

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
            case NodeType.Player:
                _spawnList = players.PlayersPrefabs;
                break;
            default:
                Debug.LogError("Triggered default");
                _spawnList = paths.PathsPrefabs;
                break;
        }
        return _spawnList;
    }

    public Vector2 GetNodePosition(Node _searchNode)
    {
        foreach (Vector2 _position in Grid.Keys)
        {
            if (Grid[_position] == _searchNode)
            {
                return _position;
            }
        }

        Debug.LogError("Not in the keys");
        return new Vector2(0, 0);
    }

    public Node GetNode(Vector2 _position)
    {
        return Grid[_position];
    }

    public bool IsOccupied(Vector2 _positionToCheck)
    {
        if (Grid[new Vector2(_positionToCheck.x, _positionToCheck.y)].Type.Contains(NodeType.Obstacle))
        {
            return true;
        }
        return false;
    }

    //Tom: Not functional right now TOOD: fix for new methode
    /*
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
    */
}

/*
    LEVEL NUMBERS:

    Null = 0,
    Player = 1,
    Special = 2,
    Path = 3,
    Obstacle = 4,
*/

public class Level
{
    private const int WIDTH = 10;
    private const int HEIGHT = 10;

    public int[,] ObstacleGrid = new int[WIDTH, HEIGHT]
    {
        { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
        { 4, 3, 3, 3, 3, 3, 3, 3, 3, 4},
        { 4, 3, 3, 3, 3, 3, 3, 3, 3, 4},
        { 4, 3, 3, 3, 3, 3, 3, 3, 3, 4},
        { 4, 3, 3, 3, 3, 3, 3, 3, 3, 4},
        { 4, 3, 3, 3, 3, 3, 3, 3, 3, 4},
        { 4, 3, 3, 3, 3, 3, 3, 3, 3, 4},
        { 4, 3, 3, 3, 3, 3, 3, 3, 3, 4},
        { 4, 3, 3, 3, 3, 3, 3, 3, 3, 4},
        { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
    };

    public int[,] PickupGrid = new int[WIDTH, HEIGHT]
    {
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 2, 2, 0, 0, 0, 0, 0},
        { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 2, 0, 1, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };

    #region Old Code
    /*
    public int[,] TestGrid = new int[5, 5]
    {
        { 1, 0, 1, 1, 1 },
        { 2, 0, 1, 2, 1 },
        { 0, 1, 1, 0, 1 },
        { 0, 0, 0, 0, 1 },
        { 1, 1, 1, 1, 1 }
    };

    public int[,][] TestGrid0 = new int[5, 5][]
    {
        {  new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 } },
        {  new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 } },
        {  new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 } },
        {  new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 } },
        {  new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 }, new int[1] { 0 } }
    };

    public List<int>[,] TestGrid1 = new List<int>[5, 5] 
    {
        { new List<int> { 0 } , new List<int> { 0 } , new List<int> { 0 } , new List<int> {0 } , new List<int> { 0 } },
        { new List<int> { 0 } , new List<int> { 0 } , new List<int> { 0 } , new List<int> {0 } , new List<int> { 0 } },
        { new List<int> { 0 } , new List<int> { 0 } , new List<int> { 0 } , new List<int> {0 } , new List<int> { 0 } },
        { new List<int> { 0 } , new List<int> { 0 } , new List<int> { 0 } , new List<int> {0 } , new List<int> { 0 } },
        { new List<int> { 0 } , new List<int> { 0 } , new List<int> { 0 } , new List<int> {0 } , new List<int> { 0 } },
    };
    */
    #endregion
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

[Serializable]
public class Players
{
    public GameObject[] PlayersPrefabs;
}
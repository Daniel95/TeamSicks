using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityToolbag;

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
    
    [Reorderable] [SerializeField] private List<NodeObjectEntries> nodeObjectEntries;

    [Space(5)]
    [SerializeField]    private int step;

    [Space(5)]
    [SerializeField]    private GameObject node;

    private void Awake()
    {
        Level level = new Level();

        Grid = CreateGrid(new int[][,] { level.ObstacleGrid, level.PickupGrid });
    }

    private Dictionary<Vector2, Node> CreateGrid(int[][,] _gridLayout)
    {
        Dictionary<Vector2, Node> _grid = CreateEmptyNodes(10, 10);

        foreach (int[,] _currentGrid in _gridLayout)
        {
            for (int i = 0; i < _currentGrid.GetLength(0); i++)
            {
                for (int j = 0; j < _currentGrid.GetLength(1); j++)
                {
                    if (_currentGrid[i, j] != (int)NodeObjectType.Null)
                    {
                        List<GameObject> _spawnList = Spawnlist((NodeObjectType)_currentGrid[i, j]);
                        Node _currentNode = _grid[new Vector2(i, j)];

                        NodeObject _nodeObject = Instantiate(_spawnList[UnityEngine.Random.Range(0, _spawnList.Count - 1)], _currentNode.transform.position, Quaternion.identity, _currentNode.transform).GetComponent<NodeObject>();
                        _nodeObject.ParentNode = _currentNode;
                        _nodeObject.NodeObjectType = (NodeObjectType)_currentGrid[i, j];

                        _currentNode.NodeObjects.Add(_nodeObject);
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
        

                        //_grid[new Vector2(j, i)].NodeObjects.Add((NodeObjectType)_currentGrid[i, j]);
                        //_node.transform.parent = _grid[new Vector2(j, i)].transform;

                        //_nodeType.NodeObjects.Add((NodeObjectType)_currentGrid[i, j]);

        */
        #endregion

        return _grid;
    }

    private Dictionary<Vector2, Node> CreateEmptyNodes(int _width, int _height)
    {
        Dictionary<Vector2, Node> _grid = new Dictionary<Vector2, Node>();

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Vector3 _position = new Vector3(step * j, -step * i);
                GameObject _nodeObject = Instantiate(node, _position, Quaternion.identity, transform);
                _nodeObject.name = "Node["+ i + "," + j + "]";

                Node _node = _nodeObject.GetComponent<Node>();
                _node.GridPosition = new Vector2(i, j);

                _grid.Add(new Vector2(i, j), _node);
            }
        }

        return _grid;
    }

    private List<GameObject> Spawnlist(NodeObjectType nodeObjectType)
    {
        List<GameObject> prefabs = nodeObjectEntries.Find(x => x.NodeObjectType == nodeObjectType).Prefabs;
        return prefabs;
    }

    private List<GameObject> TestP(NodeObjectType nodeObjectType)
    {
        foreach (NodeObjectEntries item in nodeObjectEntries)
        {
            if (item.NodeObjectType == nodeObjectType)
            {
                return item.Prefabs;
            }
        }
        return null;
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
        foreach (NodeObject _nodeObject in Grid[_positionToCheck].NodeObjects)
        {
            if (_nodeObject.NodeObjectType == NodeObjectType.Obstacle)
            {
                return true;
            }
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
public class NodeObjectEntries
{
    public NodeObjectType NodeObjectType;
    public List<GameObject> Prefabs;
}
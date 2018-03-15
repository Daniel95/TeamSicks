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

    private Dictionary<Vector2, Node> nodeGrid = new Dictionary<Vector2, Node>();

    [Reorderable] [SerializeField] private List<NodeObjectEntries> nodeObjectEntries;

    [Space(5)]
    [SerializeField]
    private int step;

    [Space(5)]
    [SerializeField]
    private GameObject nodePrefab;

    private void Awake()
    {
        nodeGrid = SpawnNodeGrid(Levels.GetLevelGrid(1));
    }

    private Dictionary<Vector2, Node> SpawnNodeGrid(Dictionary<Vector2, List<NodeObjectType>> _grid)
    {
        Dictionary<Vector2, Node> _nodeGrid = new Dictionary<Vector2, Node>();

        foreach (KeyValuePair<Vector2, List<NodeObjectType>> _nodeObjectByGridPosition in _grid)
        {
            Vector2 _gridPosition = _nodeObjectByGridPosition.Key;
            Vector2 _localPosition = _gridPosition * step;
            Vector2 _worldPosition = (Vector2)transform.position + _localPosition;
            GameObject _nodeGameObject = Instantiate(nodePrefab, _worldPosition, Quaternion.identity, transform);
            _nodeGameObject.name = "Node[" + _gridPosition.x + "," + _gridPosition.y + "]";

            Node _node = _nodeGameObject.GetComponent<Node>();
            _node.GridPosition = _gridPosition;

            foreach (NodeObjectType nodeObjectType in _nodeObjectByGridPosition.Value)
            {
                List<GameObject> _prefabList = GetPrefabList(nodeObjectType);

                if(_prefabList.Count <= 0) { continue; }

                int randomPrefabIndex = UnityEngine.Random.Range(0, _prefabList.Count - 1);
                GameObject randomPrefab = _prefabList[randomPrefabIndex];

                GameObject _nodeObjectGameObject = Instantiate(randomPrefab, _worldPosition, Quaternion.identity, _nodeGameObject.transform);
                NodeObject _nodeObject = _nodeObjectGameObject.GetComponent<NodeObject>();
                _nodeObject.ParentNode = _node;
                _nodeObject.NodeObjectType = nodeObjectType;

                _node.NodeObjects.Add(_nodeObject);
            }

            _nodeGrid.Add(_gridPosition, _node);
        }

        return _nodeGrid;
    }

    private List<GameObject> GetPrefabList(NodeObjectType _nodeObjectType)
    {
        NodeObjectEntries _nodeObjectEntries = nodeObjectEntries.Find(x => x.NodeObjectType == _nodeObjectType);

        if(_nodeObjectEntries == null) { return new List<GameObject>(); }

        List<GameObject> prefabs = _nodeObjectEntries.Prefabs;
        return prefabs;
    }

    public Vector2 GetNodePosition(Node _searchNode)
    {
        foreach (Vector2 _position in nodeGrid.Keys)
        {
            if (nodeGrid[_position] == _searchNode)
            {
                return _position;
            }
        }

        Debug.LogError("Node " + _searchNode.name + " does not exists in grid");
        return new Vector2(0, 0);
    }

    public Node GetNode(Vector2 _position)
    {
        return nodeGrid[_position];
    }

    public bool IsOccupied(Vector2 _positionToCheck)
    {
        foreach (NodeObject _nodeObject in nodeGrid[_positionToCheck].NodeObjects)
        {
            if (_nodeObject.NodeObjectType == NodeObjectType.Obstacle)
            {
                return true;
            }
        }

        return false;
    }
}

public enum NodeObjectType
{
    Null = 0,
    Player = 1,
    Special = 2,
    Path = 3,
    Obstacle = 4,
    Finish = 5,
}

public class Levels
{
    private const int WIDTH = 12;
    private const int HEIGHT = 6;

    private static Level[] levels =
    {
        //LEVEL 1
        new Level
        {
            Height = 6,
            Width = 12,

            Grids = new int[][,] {
                //ObstacleGrid
                new int[HEIGHT, WIDTH]
                {
                    { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                    { 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4 },
                    { 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4 },
                    { 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4 },
                    { 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4 },
                    { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 }
                },
                //PickupGrid
                new int[HEIGHT, WIDTH]
                {
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 5, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                }
            },
        }
     };

    public static Dictionary<Vector2, List<NodeObjectType>> GetLevelGrid(int _levelIndex)
    {
        Dictionary<Vector2, List<NodeObjectType>> _levelGrid = new Dictionary<Vector2, List<NodeObjectType>>();

        Level _level = levels[_levelIndex - 1];

        for (int x = 0; x < _level.Width; x++)
        {
            for (int invertedY = 0; invertedY < HEIGHT; invertedY++)
            {
                List<NodeObjectType> _nodeObjectTypes = new List<NodeObjectType>();

                for (int i = 0; i < _level.Grids.Length; i++)
                {
                    int[,] _grid = _level.Grids[i];
                    int _nodeObjectIndex = _grid[invertedY, x];
                    NodeObjectType _nodeObjectType = (NodeObjectType)_nodeObjectIndex;
                    _nodeObjectTypes.Add(_nodeObjectType);
                }

                int _y = Mathf.Abs(invertedY - HEIGHT);
                Vector2 _gridPosition = new Vector2(x, _y);
                _levelGrid.Add(_gridPosition, _nodeObjectTypes);
            }
        }

        return _levelGrid;
    }

}

public class Level
{
    public int Width = 0;
    public int Height = 0;
    public int[][,] Grids;
}

[Serializable]
public class NodeObjectEntries
{
    public NodeObjectType NodeObjectType;
    public List<GameObject> Prefabs;
}
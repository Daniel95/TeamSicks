using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityToolbag;

public class GameGrid : MonoBehaviour
{

    public static GameGrid Instance { get { return GetInstance(); } }

    private static GameGrid instance;

    [Reorderable] [SerializeField] private List<NodeObjectPrefabs> nodeObjectEntries;
    [Space(5)] [SerializeField] private int step;
    [Space(5)] [SerializeField] private GameObject nodePrefab;

    private Dictionary<Vector2, Node> nodeGrid = new Dictionary<Vector2, Node>();

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

    private static GameGrid GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameGrid>();
        }
        return instance;
    }

    private void Awake()
    {
        nodeGrid = SpawnNodeGrid(Levels.GetLevelLayout(1));
    }

    private Dictionary<Vector2, Node> SpawnNodeGrid(Dictionary<Vector2, List<NodeObjectType>> _layout)
    {
        Dictionary<Vector2, Node> _nodeGrid = new Dictionary<Vector2, Node>();

        foreach (KeyValuePair<Vector2, List<NodeObjectType>> _nodeObjectByGridPosition in _layout)
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
                List<GameObject> _prefabList = GetNodeObjectTypePrefabList(nodeObjectType);

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

    private List<GameObject> GetNodeObjectTypePrefabList(NodeObjectType _nodeObjectType)
    {
        NodeObjectPrefabs _nodeObjectEntries = nodeObjectEntries.Find(x => x.NodeObjectType == _nodeObjectType);

        if(_nodeObjectEntries == null) { return new List<GameObject>(); }

        List<GameObject> prefabs = _nodeObjectEntries.Prefabs;
        return prefabs;
    }

}
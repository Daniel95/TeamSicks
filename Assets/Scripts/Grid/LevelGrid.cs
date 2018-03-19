using System;
using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

public class LevelGrid : MonoBehaviour
{

    public static Action OnLoadedEvent;

    public static LevelGrid Instance { get { return GetInstance(); } }

    private static LevelGrid instance;

    [Reorderable] [SerializeField] private List<NodeObjectEditorEntry> nodeObjectEntries;
    [Space(5)] [SerializeField] private int step;
    [Space(5)] [SerializeField] private GameObject nodePrefab;

    private Dictionary<Vector2Int, Node> nodeGrid = new Dictionary<Vector2Int, Node>();
    private int loadedLevelGridNumber;

    public void LoadLevelGrid(int _levelNumber)
    {
        int _width = 0;
        int _height = 0;
        Dictionary<Vector2Int, List<NodeObjectType>> _layout = Levels.GetLevelLayout(1, out _width, out _height);

        nodeGrid = SpawnNodeGrid(_layout, _width, _height);
        SetSpriteIndex();

        loadedLevelGridNumber = _levelNumber;

        if(OnLoadedEvent != null)
        {
            OnLoadedEvent();
        }
    }

    public Vector2Int GetNodePosition(Node _searchNode)
    {
        foreach (Vector2Int _position in nodeGrid.Keys)
        {
            if (nodeGrid[_position] == _searchNode)
            {
                return _position;
            }
        }

        Debug.LogError("Node " + _searchNode.name + " does not exists in grid");
        return new Vector2Int(0, 0);
    }

    public Node GetNode(Vector2Int _gridPosition)
    {
        return nodeGrid[_gridPosition];
    }

    public bool Contains(Vector2Int _gridPosition)
    {
        return nodeGrid.ContainsKey(_gridPosition);
    }

    public bool Contains(Vector2Int _gridPosition, NodeObjectType _nodeObjectType)
    {
        if(!nodeGrid.ContainsKey(_gridPosition)) { return false; }
        Node _node = nodeGrid[_gridPosition];

        bool nodeObjectTypeExists = _node.NodeObjects.Exists(x => x.NodeObjectType == _nodeObjectType);
        return nodeObjectTypeExists;
    }

    public Vector2Int GetSize()
    {
        Vector2Int _getSize = Levels.GetLevelSize(loadedLevelGridNumber);
        return _getSize;
    }

    public int[,] GetImpassableMap()
    {
        Vector2Int _size = Levels.GetLevelSize(loadedLevelGridNumber);

        int[,] _impassableMap = new int[_size.y, _size.x];
        for (int invertedY = 0; invertedY < _size.y; invertedY++)
        {
            for (int x = 0; x < _size.x; x++)
            {
                int _y = _size.y - invertedY;
                _impassableMap[invertedY, x] = IsImpassable(new Vector2Int(x, invertedY)) ? 1 : 0;
            }
        }

        return _impassableMap;
    }

    public bool IsImpassable(Vector2Int _gridPosition)
    {
        if (!nodeGrid.ContainsKey(_gridPosition)) { return false; }
        Node _node = nodeGrid[_gridPosition];

        bool containsImpassableNodeObject = _node.NodeObjects.Exists(x => x.Impassable);
        return containsImpassableNodeObject;
    }

    private void Awake()
    {
        LoadLevelGrid(1);
    }

    private static LevelGrid GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<LevelGrid>();
        }
        return instance;
    }

    private Dictionary<Vector2Int, Node> SpawnNodeGrid(Dictionary<Vector2Int, List<NodeObjectType>> _layout, int _width, int _height)
    {
        Dictionary<Vector2Int, Node> _nodeGrid = new Dictionary<Vector2Int, Node>();

        Vector2 _offset = new Vector2(_width, _height) / 2;

        foreach (KeyValuePair<Vector2Int, List<NodeObjectType>> _nodeObjectByGridPosition in _layout)
        {
            Vector2Int _gridPosition = _nodeObjectByGridPosition.Key;
            Vector2 _localPosition = (_gridPosition - _offset) * step;
            Vector2 _worldPosition = (Vector2)transform.position + _localPosition;
            GameObject _nodeGameObject = Instantiate(nodePrefab, _worldPosition, Quaternion.identity, transform);
            _nodeGameObject.name = "Node[" + _gridPosition.x + "," + _gridPosition.y + "]";

            Node _node = _nodeGameObject.GetComponent<Node>();
            _node.GridPosition = _gridPosition;

            foreach (NodeObjectType _nodeObjectType in _nodeObjectByGridPosition.Value)
            {
                NodeObjectEditorEntry _nodeObjectEditorEntry = GetNodeObjectEditorEntry(_nodeObjectType);
                if(_nodeObjectEditorEntry.Prefabs.Count <= 0) { continue; }

                int _randomPrefabIndex = UnityEngine.Random.Range(0, _nodeObjectEditorEntry.Prefabs.Count - 1);
                GameObject _randomPrefab = _nodeObjectEditorEntry.Prefabs[_randomPrefabIndex];
                
                GameObject _nodeObjectGameObject = Instantiate(_randomPrefab, _worldPosition, Quaternion.identity, _nodeGameObject.transform);
                NodeObject _nodeObject = _nodeObjectGameObject.GetComponent<NodeObject>();
                _nodeObject.ParentNode = _node;
                _nodeObject.NodeObjectType = _nodeObjectType;
                _nodeObject.Impassable = _nodeObjectEditorEntry.Impassable;

                _node.NodeObjects.Add(_nodeObject);
            }

            _nodeGrid.Add(_gridPosition, _node);
        }

        return _nodeGrid;
    }

    private void SetSpriteIndex()
    {
        foreach (Node _node in nodeGrid.Values)
        {
            for (int i = 0; i < _node.NodeObjects.Count; i++)
            {
                SpriteRenderer _spriteRenderer = _node.NodeObjects[i].GetComponentInChildren<SpriteRenderer>();
                if (_spriteRenderer != null)
                {
                    _spriteRenderer.sortingOrder = (1000 - 10 * (int)_node.NodeObjects[i].ParentNode.GridPosition.y) + 1000 * i;
                }
            }
        }
    }

    private NodeObjectEditorEntry GetNodeObjectEditorEntry(NodeObjectType _nodeObjectType)
    {
        NodeObjectEditorEntry _nodeObjectEditorEntry = nodeObjectEntries.Find(x => x.NodeObjectType == _nodeObjectType);

        if(_nodeObjectEditorEntry == null) {
            Debug.LogError("NodeObjectEditorEntry with nodeObjectType " + _nodeObjectType + " does not exist.");
            return new NodeObjectEditorEntry();
        }

        return _nodeObjectEditorEntry;
    }

}
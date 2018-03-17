using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

public class GameGrid : MonoBehaviour
{

    public static GameGrid Instance { get { return GetInstance(); } }

    private static GameGrid instance;

    [Reorderable] [SerializeField] private List<NodeObjectEditorEntry> nodeObjectEntries;
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

    public Node GetNode(Vector2 _gridPosition)
    {
        return nodeGrid[_gridPosition];
    }

    public bool Contains(Vector2 _gridPosition)
    {
        return nodeGrid.ContainsKey(_gridPosition);
    }

    public bool Contains(Vector2 _gridPosition, NodeObjectType _nodeObjectType)
    {
        if(!nodeGrid.ContainsKey(_gridPosition)) { return false; }
        Node _node = nodeGrid[_gridPosition];

        bool nodeObjectTypeExists = _node.NodeObjects.Exists(x => x.NodeObjectType == _nodeObjectType);
        return nodeObjectTypeExists;
    }

    public bool IsImpassable(Vector2 _gridPosition)
    {
        if (!nodeGrid.ContainsKey(_gridPosition)) { return false; }
        Node _node = nodeGrid[_gridPosition];

        bool containsImpassableNodeObject = _node.NodeObjects.Exists(x => x.Impassable);
        return containsImpassableNodeObject;
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
        int _width = 0;
        int _height = 0;
        Dictionary<Vector2, List<NodeObjectType>> _layout = Levels.GetLevelLayout(1, out _width, out _height);

        nodeGrid = SpawnNodeGrid(_layout, _width, _height);
        SetSpriteIndex();
    }

    private Dictionary<Vector2, Node> SpawnNodeGrid(Dictionary<Vector2, List<NodeObjectType>> _layout, int _width, int _height)
    {
        Dictionary<Vector2, Node> _nodeGrid = new Dictionary<Vector2, Node>();

        Vector2 _offset = new Vector2(_width, _height) / 2;

        foreach (KeyValuePair<Vector2, List<NodeObjectType>> _nodeObjectByGridPosition in _layout)
        {
            Vector2 _gridPosition = _nodeObjectByGridPosition.Key;
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

                int _randomPrefabIndex = Random.Range(0, _nodeObjectEditorEntry.Prefabs.Count - 1);
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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

/// <summary>
/// LevelGrid contains the logic used to spawn and manage the grid.
/// The LevelGrid uses Nodes to manage which GameObjects are at a grid position.
/// </summary>
public class LevelGrid : MonoBehaviour
{

    /// <summary>
    /// The LevelGridLoadedEvent is triggered when the LevelGrid is done generating and spawning the grid.
    /// </summary>
    public static Action LevelGridLoadedEvent;

    public static LevelGrid Instance { get { return GetInstance(); } }

    /// <summary>
    /// Returns the Nodes which are linked to a gridpositions.
    /// </summary>
    public Dictionary<Vector2Int, Node> NodeGrid
    {
        get
        {
            return _nodeNodeGrid;
        }
    }

    /// <summary>
    /// Step is used to indicate the amount of space between Nodes.
    /// </summary>
    public Vector2 Step
    {
        get
        {
            return new Vector2(widthStep, heightStep);
        }
    }

    /// <summary>
    /// When DebugMode is on the nodes display on which gridposition they are placed.
    /// </summary>
    public bool DebugMode
    {
        get
        {
            return debugMode;
        }
    }

    private static LevelGrid instance;

    [Reorderable] [SerializeField] private List<NodeObjectEditorEntry> nodeObjectEntries;
    [Space(5)] [SerializeField] private float widthStep;
    [Space(5)] [SerializeField] private float heightStep;
    [Space(5)] [SerializeField] private GameObject nodePrefab;
    [Space(5)] [SerializeField] private bool debugMode;

    private Dictionary<Vector2Int, Node> _nodeNodeGrid = new Dictionary<Vector2Int, Node>();
    private int loadedLevelGridNumber;

    /// <summary>
    /// Loads the Level specified by the levelNumber.
    /// </summary>
    /// <param name="_levelNumber"></param>
    public void LoadLevelGrid(int _levelNumber)
    {
        loadedLevelGridNumber = _levelNumber;

        int _width = 0;
        int _height = 0;
        Dictionary<Vector2Int, List<NodeObjectType>> _layout = Levels.GetLevelLayout(_levelNumber, out _width, out _height);

        SpawnNodeGrid(_layout, _width, _height);
        SetSpriteIndex();

        if (LevelGridLoadedEvent != null)
        {
            LevelGridLoadedEvent();
        }
    }

    /// <summary>
    /// Returns the node at the specified gridposition.
    /// </summary>
    /// <param name="_gridPosition"></param>
    /// <returns></returns>
    public Node GetNode(Vector2Int _gridPosition)
    {
        if (!_nodeNodeGrid.ContainsKey(_gridPosition))
        {
            Debug.LogError("Nodegrid does not contain gridposition " + _gridPosition);
            return null;
        }
        return _nodeNodeGrid[_gridPosition];
    }

    /// <summary>
    /// Chech there exists a node at the specified gridposition.
    /// </summary>
    /// <param name="_gridPosition"></param>
    /// <returns></returns>
    public bool Contains(Vector2Int _gridPosition)
    {
        return _nodeNodeGrid.ContainsKey(_gridPosition);
    }

    /// <summary>
    /// Chech there exists a node at the specified gridposition with the specified NodeObjectType.
    /// </summary>
    /// <param name="_gridPosition"></param>
    /// <param name="_nodeObjectType"></param>
    /// <returns></returns>
    public bool Contains(Vector2Int _gridPosition, NodeObjectType _nodeObjectType)
    {
        if (!_nodeNodeGrid.ContainsKey(_gridPosition)) { return false; }
        Node _node = _nodeNodeGrid[_gridPosition];

        bool _nodeObjectTypeExists = _node.NodeObjects.Exists(x => x.NodeObjectType == _nodeObjectType);
        return _nodeObjectTypeExists;
    }

    /// <summary>
    /// Returns the total size of the grid that is currently spawned.
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetSize()
    {
        Vector2Int _getSize = Levels.GetLevelSize(loadedLevelGridNumber);
        return _getSize;
    }

    /// <summary>
    /// Returns a map which indicate which positions are passable or impassable.
    /// </summary>
    /// <returns></returns>
    public int[,] GetImpassableMap()
    {
        Vector2Int _size = Levels.GetLevelSize(loadedLevelGridNumber);

        int[,] _impassableMap = new int[_size.y, _size.x];
        for (int y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++)
            {
                _impassableMap[y, x] = IsImpassable(new Vector2Int(x, y)) ? 1 : 0;
            }
        }

        return _impassableMap;
    }

    /// <summary>
    /// Returns if the specified gridposition is impassable or not.
    /// </summary>
    /// <param name="_gridPosition"></param>
    /// <returns></returns>
    public bool IsImpassable(Vector2Int _gridPosition)
    {
        if (!_nodeNodeGrid.ContainsKey(_gridPosition)) { return false; }
        Node _node = _nodeNodeGrid[_gridPosition];

        bool _containsImpassableNodeObject = _node.NodeObjects.Exists(x => x.Impassable);
        return _containsImpassableNodeObject;
    }

    /// <summary>
    /// Transforms the specified screenposition to a gridposition.
    /// </summary>
    /// <param name="_screenPosition"></param>
    /// <returns></returns>
    public Vector2Int ScreenToGridPosition(Vector2 _screenPosition)
    {
        Vector3 _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);
        Vector2Int _gridPosition = WorldToGridPosition(_worldPosition);

        return _gridPosition;
    }

    /// <summary>
    /// Transforms the specified worldposition to a gridposition.
    /// </summary>
    /// <param name="_worldPosition"></param>
    /// <returns></returns>
    public Vector2Int WorldToGridPosition(Vector3 _worldPosition)
    {
        Vector3 _localPosition = transform.InverseTransformPoint(_worldPosition);
        Vector2Int _gridPosition = LocalToGridPosition(_localPosition);
        return _gridPosition;
    }

    /// <summary>
    /// Transforms the specified grid localposition to a gridposition.
    /// </summary>
    /// <param name="_localPosition"></param>
    /// <returns></returns>
    public Vector2Int LocalToGridPosition(Vector3 _localPosition)
    {
        Vector2 _unroundedGridPosition = VectorHelper.Divide((Vector2)_localPosition, Step);
        Vector2 _roundedGridPosition = VectorHelper.Round(_unroundedGridPosition);
        Vector2Int _gridPosition = new Vector2Int((int)_roundedGridPosition.x, (int)_roundedGridPosition.y);
        return _gridPosition;
    }

    /// <summary>
    /// Transforms the specified gridposition to a worldposition.
    /// </summary>
    /// <param name="_gridPosition"></param>
    /// <returns></returns>
    public Vector3 GridToWorldPosition(Vector2Int _gridPosition)
    {
        Vector3 _localPosition = VectorHelper.Multiply(_gridPosition, Step);
        Vector3 _calculateWorldPos = _localPosition + transform.position;
        Vector3 _worldPosition = new Vector3(_calculateWorldPos.x, _calculateWorldPos.y);

        return _worldPosition;
    }

    /// <summary>
    /// Adds a NodeObject of a specified NodeObjectType to the grid at the specified gridposition.
    /// </summary>
    /// <param name="_nodeObjectType"></param>
    /// <param name="_gridPosition"></param>
    /// <returns></returns>
    public NodeObject AddNodeObject(NodeObjectType _nodeObjectType, Vector2Int _gridPosition)
    {
        if (!Contains(_gridPosition))
        {
            AddNode(_gridPosition);
        }

        Node _node = GetNode(_gridPosition);
        GameObject _nodeGameObject = _node.gameObject;

        NodeObjectEditorEntry _nodeObjectEditorEntry = GetNodeObjectEditorEntry(_nodeObjectType);
        if (_nodeObjectEditorEntry.Prefabs.Count <= 0) { return null; }

        int _randomPrefabIndex = UnityEngine.Random.Range(0, _nodeObjectEditorEntry.Prefabs.Count);
        GameObject _randomPrefab = _nodeObjectEditorEntry.Prefabs[_randomPrefabIndex];

        GameObject _nodeObjectGameObject = Instantiate(_randomPrefab, _nodeGameObject.transform.position, Quaternion.identity, _nodeGameObject.transform);
        NodeObject _nodeObject = _nodeObjectGameObject.GetComponent<NodeObject>();

        if (_nodeObject == null)
        {
            Debug.LogError("No NodeObject script on NodeObject of type " + _nodeObjectType);
            return null;
        }

        _nodeObject.ParentNode = _node;
        _nodeObject.NodeObjectType = _nodeObjectType;
        _nodeObject.Impassable = _nodeObjectEditorEntry.Impassable;

        _node.AddNodeObject(_nodeObject);

        return _nodeObject;
    }

    /// <summary>
    /// Removes a NodeObject of the specified NodeObjectType at the specified gridposition
    /// </summary>
    /// <param name="_nodeObjectType"></param>
    /// <param name="_gridPosition"></param>
	public void RemoveNodeObject(NodeObjectType _nodeObjectType, Vector2Int _gridPosition)
	{
		if (Contains(_gridPosition))
		{
			Node _node = GetNode(_gridPosition);
			NodeObject _nodeObject = _node.NodeObjects.Find(x => x.NodeObjectType == _nodeObjectType);
			if(_nodeObject == null)
			{
				Debug.LogError(_nodeObjectType + " doesn't exist in " + _gridPosition);
				return;
			}
			_node.RemoveNodeObject(_nodeObject);
			Destroy(_nodeObject.gameObject);

            if (_node.NodeObjects.Count == 0)
            {
                Destroy(_node.gameObject);
                _nodeNodeGrid.Remove(_gridPosition);
            }
        }
	}

	private static LevelGrid GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<LevelGrid>();
        }
        return instance;
    }

    private Node AddNode(Vector2Int _gridPosition)
    {
        Vector3 _worldPosition = GridToWorldPosition(_gridPosition);

        GameObject _nodeGameObject = Instantiate(nodePrefab, _worldPosition, Quaternion.identity, transform);
        _nodeGameObject.name = "Node[" + _gridPosition.x + "," + _gridPosition.y + "]";

        Node _node = _nodeGameObject.GetComponent<Node>();
        _node.GridPosition = _gridPosition;
        _nodeNodeGrid.Add(_gridPosition, _node);

        return _node;
    }

    private void SpawnNodeGrid(Dictionary<Vector2Int, List<NodeObjectType>> _layout, int _width, int _height)
    {
        _nodeNodeGrid = new Dictionary<Vector2Int, Node>();

        foreach (KeyValuePair<Vector2Int, List<NodeObjectType>> _nodeObjectByGridPosition in _layout)
        {
            Vector2Int _gridPosition = _nodeObjectByGridPosition.Key;
            foreach (NodeObjectType _nodeObjectType in _nodeObjectByGridPosition.Value)
            {
                AddNodeObject(_nodeObjectType, _gridPosition);
            }
        }
    }

    private void SetSpriteIndex()
    {
        foreach (Node _node in _nodeNodeGrid.Values)
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

        if (_nodeObjectEditorEntry == null)
        {
            Debug.LogError("NodeObjectEditorEntry with nodeObjectType " + _nodeObjectType + " does not exist.");
            return new NodeObjectEditorEntry();
        }

        return _nodeObjectEditorEntry;
    }
}
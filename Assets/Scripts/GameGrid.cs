using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

//Needed functionality:
//  Generate level from 2D Int array
//  Generate "random" Level
//  Return Bool IsOccupied
//  
//  

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
        Level testClass = new Level();

        //TODO: add randommised level generation
        Grid = CreateGrid(testClass.TestGrid);
    }

    private Dictionary<Vector2, Node> CreateGrid(int[,] _gridLayout)
    {
        Dictionary<Vector2, Node> _grid = new Dictionary<Vector2, Node>();

        for (int i = 0; i < _gridLayout.GetLength(0); i++)
        {
            for (int j = 0; j < _gridLayout.GetLength(1); j++)
            {
                Node _node;
                GameObject[] _spawnList;

                switch (_gridLayout[i, j])
                {
                    case 0:
                        _spawnList = paths.PathsPrefabs;
                        break;
                    case 1:
                        _spawnList = obstacles.ObstaclePrefabs;
                        break;
                    case 2:
                        _spawnList = specials.SpecialsPrefabs;
                        break;
                    default:
                        Debug.LogError("Triggered default");
                        _spawnList = paths.PathsPrefabs;
                        break;
                }
                Vector3 position = new Vector3(step * i, step * j);

                _node = Instantiate(_spawnList[UnityEngine.Random.Range(0, _spawnList.Length - 1)], position, Quaternion.identity, transform).GetComponent<Node>();
                
                _grid.Add(new Vector2(i,j), _node);
                _node.Type = ( NodeType)_gridLayout[i, j];
            }
        }

        return _grid;
    }

    private Dictionary<Vector2, Node> CreateRandomGrid(Vector2 _widthHeight)
    {
        Dictionary<Vector2, Node> _grid = new Dictionary<Vector2, Node>();
        for (int i = 0; i < _widthHeight.x; i++)
        {
            for (int j = 0; j < _widthHeight.y; j++)
            {
                //TODO: random generation
            }
        }
        return _grid;
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
        Grid[_positionToAdd].Type = _nodeType;
    }

    public void removeOccupied(Vector2 _positionToRemove)
    {
        Grid[_positionToRemove].Type = NodeType.Path;
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
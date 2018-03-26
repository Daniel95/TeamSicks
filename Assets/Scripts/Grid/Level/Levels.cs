using System.Collections.Generic;
using UnityEngine;

public class Levels
{

    //Null = 0,
    //Path = 1,
    //Obstacle = 2,
    //Special = 3,
    //Finish = 4,
    //Player = 5,
    //Enemy = 6,

   private static Level[] levels =
    {
        //LEVEL 1
        new Level
        {
            Height = 7,
            Width = 20,

            MapLayers = new int[][,] {
                //Obstacle Layer
                new int[7, 20]
                {
                    { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    { 1, 1, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    { 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1 },
                    { 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 2, 2, 1, 1 },
                    { 1, 1, 1, 1, 1, 1, 2, 1, 2, 2, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1 },
                    { 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1 },
                    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1 },
                },
                //Pickup Layer
                new int[7, 20]
                {
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
                }
            },
        },
        //LEVEL 2
        new Level
        {
            Height = 6,
            Width = 20,

            MapLayers = new int[][,] {
                //Obstacle Layer
                new int[6, 20]
                {
                    { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    { 1, 1, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    { 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1 },
                    { 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 2, 2, 1, 1 },
                    { 1, 1, 1, 1, 1, 1, 2, 1, 2, 2, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1 },
                    { 1, 1, 1, 1, 1, 1, 2, 1, 2, 2, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1 },
                },
                //Pickup Layer
                new int[6, 20]
                {
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                }
            },
        }

    };


    public static Vector2Int GetLevelSize(int _levelNumber)
    {
        Level _level = GetLevel(_levelNumber);
        Vector2Int _size = new Vector2Int(_level.Width, _level.Height);
        return _size;
    }

    public static bool LevelExists(int _levelNumber)
    {
        int levelIndex = _levelNumber - 1;
        bool levelExists = _levelNumber >= 0 || levelIndex < levels.Length;
        return levelExists;
    }

    public static Dictionary<Vector2Int, List<NodeObjectType>> GetLevelLayout(int _levelNumber, out int _width, out int _height)
    {
        Dictionary<Vector2Int, List<NodeObjectType>> _levelLayout = new Dictionary<Vector2Int, List<NodeObjectType>>();

        if(_levelNumber < 0 || _levelNumber > levels.Length)
        {
            Debug.Log("Level " + _levelNumber + " does not exist");
            _width = 0;
            _height = 0;
            return new Dictionary<Vector2Int, List<NodeObjectType>>();
        }

        Level _level = GetLevel(_levelNumber);

        for (int x = 0; x < _level.Width; x++)
        {
            for (int invertedY = 0; invertedY < _level.Height; invertedY++)
            {
                List<NodeObjectType> _nodeObjectTypes = new List<NodeObjectType>();

                for (int i = 0; i < _level.MapLayers.Length; i++)
                {
                    int[,] _layout = _level.MapLayers[i];
                    int _nodeObjectIndex = _layout[invertedY, x];
                    NodeObjectType _nodeObjectType = (NodeObjectType)_nodeObjectIndex;
                    _nodeObjectTypes.Add(_nodeObjectType);
                }

                int _y = (_level.Height - 1) - invertedY;
                Vector2Int _layoutGridPosition = new Vector2Int(x, _y);
                _levelLayout.Add(_layoutGridPosition, _nodeObjectTypes);
            }
        }

        _width = _level.Width;
        _height = _level.Height;

        return _levelLayout;
    }

    private static Level GetLevel(int _levelNumber)
    {
        Level _level = levels[_levelNumber - 1];
        return _level;
    }

}
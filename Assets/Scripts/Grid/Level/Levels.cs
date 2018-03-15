using System.Collections.Generic;
using UnityEngine;

public class Levels
{

    // Null = 0,
    // Player = 1,
    // Special = 2,
    // Path = 3,
    // Obstacle = 4,
    // Finish = 5,

    private static Level[] levels =
    {
        //LEVEL 1
        new Level
        {
            Height = 10,
            Width = 20,

            LayoutLayers = new int[][,] {
                //Obstacle Layer
                new int[10, 20]
                {
                    { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                    { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                    { 3, 3, 3, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 4, 3, 3, 3, 3, 3, 3 },
                    { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 3, 3, 3, 3, 3, 3 },
                    { 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                    { 3, 3, 3, 3, 3, 3, 3, 3, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                    { 3, 3, 3, 4, 3, 3, 3, 3, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                    { 3, 3, 3, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                    { 3, 3, 3, 4, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 3, 3, 3, 3 },
                    { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                },
                //Pickup Layer
                new int[10, 20]
                {
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 5 },
                }
            },
        }
     };

    public static Dictionary<Vector2, List<NodeObjectType>> GetLevelLayout(int _levelNumber, out int _width, out int _height)
    {
        Dictionary<Vector2, List<NodeObjectType>> _levelLayout = new Dictionary<Vector2, List<NodeObjectType>>();

        if(_levelNumber < 0 || _levelNumber > levels.Length)
        {
            Debug.Log("Level " + _levelNumber + " does not exist");
            _width = 0;
            _height = 0;
            return new Dictionary<Vector2, List<NodeObjectType>>();
        }
        
        Level _level = levels[_levelNumber - 1];

        for (int x = 0; x < _level.Width; x++)
        {
            for (int invertedY = 0; invertedY < _level.Height; invertedY++)
            {
                List<NodeObjectType> _nodeObjectTypes = new List<NodeObjectType>();

                for (int i = 0; i < _level.LayoutLayers.Length; i++)
                {
                    int[,] _layout = _level.LayoutLayers[i];
                    int _nodeObjectIndex = _layout[invertedY, x];
                    NodeObjectType _nodeObjectType = (NodeObjectType)_nodeObjectIndex;
                    _nodeObjectTypes.Add(_nodeObjectType);
                }

                int _y = Mathf.Abs(invertedY - _level.Height);
                Vector2 _layoutGridPosition = new Vector2(x, _y);
                _levelLayout.Add(_layoutGridPosition, _nodeObjectTypes);
            }
        }

        _width = _level.Width;
        _height = _level.Height;

        return _levelLayout;
    }

}
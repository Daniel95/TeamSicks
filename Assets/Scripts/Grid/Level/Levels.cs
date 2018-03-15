using System.Collections.Generic;
using UnityEngine;

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

            LayoutLayers = new int[][,] {
                //Obstacle Layer
                new int[HEIGHT, WIDTH]
                {
                    { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                    { 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4 },
                    { 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4 },
                    { 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4 },
                    { 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4 },
                    { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 }
                },
                //Pickup Layer
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

    public static Dictionary<Vector2, List<NodeObjectType>> GetLevelLayout(int _levelIndex)
    {
        Dictionary<Vector2, List<NodeObjectType>> _levelLayout = new Dictionary<Vector2, List<NodeObjectType>>();

        Level _level = levels[_levelIndex - 1];

        for (int x = 0; x < _level.Width; x++)
        {
            for (int invertedY = 0; invertedY < HEIGHT; invertedY++)
            {
                List<NodeObjectType> _nodeObjectTypes = new List<NodeObjectType>();

                for (int i = 0; i < _level.LayoutLayers.Length; i++)
                {
                    int[,] _layout = _level.LayoutLayers[i];
                    int _nodeObjectIndex = _layout[invertedY, x];
                    NodeObjectType _nodeObjectType = (NodeObjectType)_nodeObjectIndex;
                    _nodeObjectTypes.Add(_nodeObjectType);
                }

                int _y = Mathf.Abs(invertedY - HEIGHT);
                Vector2 _layoutGridPosition = new Vector2(x, _y);
                _levelLayout.Add(_layoutGridPosition, _nodeObjectTypes);
            }
        }

        return _levelLayout;
    }

}
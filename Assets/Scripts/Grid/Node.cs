using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //[HideInInspector]
    public List<NodeType> Type;
}

public enum NodeType
{
    Null = 0,
    Player = 1,
    Special = 2,
    Path = 3,
    Obstacle = 4,
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public NodeType Type;
}

public enum NodeType
{
    Path = 0,
    Obstacle = 1,
    Special = 2,
}

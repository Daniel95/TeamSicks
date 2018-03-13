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
    Special = 1,
    Obstacle = 2,
}

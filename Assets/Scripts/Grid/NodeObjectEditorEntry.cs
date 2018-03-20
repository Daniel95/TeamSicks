using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NodeObjectEditorEntry
{
    public NodeObjectType NodeObjectType;
    public bool Impassable;
    public List<GameObject> Prefabs;
}
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to set the NodeObject settings in the editor of the LevelGrid.
/// </summary>
[Serializable]
public class NodeObjectEditorEntry
{
    public NodeObjectType NodeObjectType;
    public bool Impassable;
    public List<GameObject> Prefabs;
}
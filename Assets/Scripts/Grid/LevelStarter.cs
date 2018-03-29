using UnityEngine;


/// <summary>
/// Calls a method LevelGrid whichs loads a level of a specified levelnumber.
/// </summary>
public class LevelStarter : MonoBehaviour
{

    [SerializeField] private int levelNumberToStart = 1;

    private void Start()
    {
        if(!Levels.LevelExists(levelNumberToStart))
        {
            Debug.LogError("LevelNumber assigned in LevelStarter does not exists!", this);
        }
        LevelGrid.Instance.LoadLevelGrid(levelNumberToStart);
    }

}

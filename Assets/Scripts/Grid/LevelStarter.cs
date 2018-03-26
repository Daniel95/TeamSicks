using UnityEngine;

public class LevelStarter : MonoBehaviour
{

    [SerializeField] private int levelNumberToStart = 1;

    private void Awake()
    {
        if(!Levels.LevelExists(levelNumberToStart))
        {
            Debug.LogError("LevelNumber assigned in LevelStarter does not exists!", this);
        }
        LevelGrid.Instance.LoadLevelGrid(levelNumberToStart);
    }

}

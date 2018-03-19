using UnityEngine;

public class LevelStarter : MonoBehaviour
{

    [SerializeField] private int levelNumberToStart = 1;

    private void Awake()
    {
        LevelGrid.Instance.LoadLevelGrid(levelNumberToStart);
    }

}

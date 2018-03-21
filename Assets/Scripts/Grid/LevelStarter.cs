using UnityEngine;

public class LevelStarter : MonoBehaviour
{

    [SerializeField] private int levelNumberToStart;

    private void Awake()
    {
        LevelGrid.Instance.LoadLevelGrid(levelNumberToStart);
    }

}

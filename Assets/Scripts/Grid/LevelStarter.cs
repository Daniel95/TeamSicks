using UnityEngine;

public class LevelStarter : MonoBehaviour
{

    [SerializeField] private int levelNumberToStart;

    private void Start()
    {
        LevelGrid.Instance.LoadLevelGrid(levelNumberToStart);
    }

}

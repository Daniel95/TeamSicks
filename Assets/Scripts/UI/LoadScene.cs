using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    [SerializeField]
    private string level;

    public void Load()
    {
        //We use the old function because the scenemanager doesnt function properly on mobile
        SceneManager.LoadScene(level);
    }
}

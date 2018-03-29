using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads a scene by string specified in the LoadScene editor.
/// </summary>
public class LoadScene : MonoBehaviour
{

    [SerializeField] private string level;

    /// <summary>
    /// Load a scene.
    /// We use the old function because the scenemanager doesnt function properly on mobile
    /// </summary>
    public void Load()
    {
        SceneManager.LoadScene(level);
    }

}
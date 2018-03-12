using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public static PlayerInput Instance { get { return GetInstance(); } }

    private static PlayerInput instance;

    public void EnableInputs(bool _enable)
    {
        if (_enable)
        {

        }
        else
        {

        }
    }

    private static PlayerInput GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PlayerInput>();
        }
        return instance;
    }

}

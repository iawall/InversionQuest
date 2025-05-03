using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    private static bool hasLoadedManagers = false;

    void Awake()
    {
        if (!hasLoadedManagers)
        {
            hasLoadedManagers = true;
            SceneManager.LoadScene("ManagerScene", LoadSceneMode.Additive);
        }
    }
}


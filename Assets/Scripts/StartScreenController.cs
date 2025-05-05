using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenController : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;
    public Button controlsButton;
    public GameObject controlsPanel;
    public Button closeControlsButton;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        controlsButton.onClick.AddListener(ShowControls);
        closeControlsButton.onClick.AddListener(HideControls);

        controlsPanel.SetActive(false);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("ManagerScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("level1");
    }

    public void StartGame()
    {
        LoadLevel1();
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        controlsPanel.SetActive(false);
    }

}

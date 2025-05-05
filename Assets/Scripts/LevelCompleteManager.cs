using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteManager : MonoBehaviour
{
    public static LevelCompleteManager Instance;

    [Header("UI Elements")]
    public GameObject levelCompleteUI;
    public Button nextButton;
    public Button quitButton;
    public Text completeMessage;
    public int finalLevelBuildIndex = 4;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene")
        {
            if (levelCompleteUI != null)
                levelCompleteUI.SetActive(false);
            return;
        }

        // Make sure UI is hidden at start of any new level
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false);

        // Setup listeners (only once is enough)
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(GoToNextLevel);
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(ReturnToMenu);
        }
    }

    public void ShowLevelCompleteUI()
    {
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
            Time.timeScale = 0f;

            // Check if we're on the final level
            bool isFinalLevel = SceneManager.GetActiveScene().buildIndex == finalLevelBuildIndex;

            if (isFinalLevel)
            {
                if (nextButton != null)
                    nextButton.gameObject.SetActive(false);

                if (completeMessage != null)
                    completeMessage.gameObject.SetActive(true);
            }
            else
            {
                if (nextButton != null)
                    nextButton.gameObject.SetActive(true);

                if (completeMessage != null)
                    completeMessage.gameObject.SetActive(false);
            }
        }
    }

    public void HideLevelCompleteUI()
    {
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void GoToNextLevel()
    {
        HideLevelCompleteUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void ReturnToMenu()
    {
        HideLevelCompleteUI();
        SceneManager.LoadScene("StartScene");
    }

    public bool IsLevelCompleteUIActive()
    {
        return levelCompleteUI != null && levelCompleteUI.activeSelf;
    }
}

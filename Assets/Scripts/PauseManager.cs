using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("Pause Menu References")]
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button quitButton;

    private bool isPaused = false;

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

    void Update()
    {
        if (!this.enabled || pauseMenuUI == null) return;

        // Block pause toggle if LevelComplete is active
        if (LevelCompleteManager.Instance != null && 
            LevelCompleteManager.Instance.IsLevelCompleteUIActive())
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }

        if (Input.GetKeyDown(KeyCode.R)) Resume();
        if (Input.GetKeyDown(KeyCode.Q)) QuitGame();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (scene.name == "StartScene")
        {
            // Disable pause functionality (but not the script)
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);

            isPaused = false;
            Time.timeScale = 1f;

            return;
        }

        this.enabled = true;

        // === [1] Try finding menu inside prefab
        Transform root = this.transform;
        pauseMenuUI = root.Find("pauseMenuUI")?.gameObject;

        if (pauseMenuUI == null)
        {
            Debug.LogError("PauseManager: 'pasueMenuUI' not found in prefab. Check spelling and hierarchy.");
            return;
        }

        resumeButton = pauseMenuUI.transform.Find("resumeButton")?.GetComponent<Button>();
        if (resumeButton == null)
            Debug.LogError("PauseManager: 'Resume' button not found under 'pasueMenuUI'.");

        quitButton = pauseMenuUI.transform.Find("quitButton")?.GetComponent<Button>();
        if (quitButton == null)
            Debug.LogError("PauseManager: 'Quit' button not found under 'pasueMenuUI'.");

        // === [2] TEMP enable menu to wire buttons
        bool wasMenuActive = pauseMenuUI.activeSelf;
        pauseMenuUI.SetActive(true);

        // === [3] Wire buttons
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(Resume);
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(QuitGame);
        }

        // === [4] Restore visibility
        pauseMenuUI.SetActive(false);  // always start hidden
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("StartScene");
    }

    private void DisablePauseUI()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }
}

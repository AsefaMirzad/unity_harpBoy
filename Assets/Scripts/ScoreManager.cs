using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Game Stats")]
    public int score = 0;
    public int missesAllowed = 4;
    public int missCount = 0;
    public int perfectHits = 0;
    public int goodHits = 0;
    public int normalHits = 0;
    public int obstacleHits = 0;
    public int maxObstacleHits = 20;

    [Header("UI References")]
    public Text scoreText;
    public Text multiplierText;  // Will use this for obstacle hits count
    public Text failedText;      // Will use this for missed arrows count
    public GameObject resultsPanel;

    // Results panel elements
    public Text titleText;
    public Text normalHitsValue;
    public Text goodHitsValue;
    public Text perfectHitsValue;
    public Text missedHitsValue;
    public Text percentHitValue;
    public Text rankValue;
    public Text finalScoreValue;

    [Header("Audio")]
    public AudioSource perfectSound;
    public AudioSource goodSound;
    public AudioSource missSound;
    public AudioSource gameOverSound;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Find UI elements if not assigned
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<Text>();

        if (multiplierText == null)
            multiplierText = GameObject.Find("MultiplierText")?.GetComponent<Text>();

        if (failedText == null)
            failedText = GameObject.Find("FailedText")?.GetComponent<Text>();

        if (resultsPanel == null)
            resultsPanel = GameObject.Find("Results")?.gameObject;

        // Find results panel elements if not assigned
        if (resultsPanel != null)
        {
            Transform resultsTransform = resultsPanel.transform;

            if (titleText == null)
                titleText = resultsTransform.Find("Title Text")?.GetComponent<Text>();

            if (normalHitsValue == null)
                normalHitsValue = resultsTransform.Find("Normal Hits Value")?.GetComponent<Text>();

            if (goodHitsValue == null)
                goodHitsValue = resultsTransform.Find("Good Hits Value")?.GetComponent<Text>();

            if (perfectHitsValue == null)
                perfectHitsValue = resultsTransform.Find("Perfect Hits Value")?.GetComponent<Text>();

            if (missedHitsValue == null)
                missedHitsValue = resultsTransform.Find("Missed Hits Value")?.GetComponent<Text>();

            if (percentHitValue == null)
                percentHitValue = resultsTransform.Find("Percent Hit Value")?.GetComponent<Text>();

            if (rankValue == null)
                rankValue = resultsTransform.Find("Rank Value")?.GetComponent<Text>();

            if (finalScoreValue == null)
                finalScoreValue = resultsTransform.Find("Final Score Value")?.GetComponent<Text>();
        }

        // Add restart button to Results panel if it doesn't exist
        if (resultsPanel != null && resultsPanel.transform.Find("RestartButton") == null)
        {
            CreateRestartButton();
        }

        // Hide results panel at start
        if (resultsPanel != null)
            resultsPanel.SetActive(false);

        // Initialize UI
        UpdateScoreUI();
        UpdateFailedUI();
        UpdateObstacleUI();
    }

    // Hit score methods
    public void Perfect()
    {
        score += 100;
        perfectHits++;
        UpdateScoreUI();
        Debug.Log("Perfect! Score: " + score);

        if (perfectSound != null)
            perfectSound.Play();
    }

    public void Good()
    {
        score += 50;
        goodHits++;
        UpdateScoreUI();
        Debug.Log("Good! Score: " + score);

        if (goodSound != null)
            goodSound.Play();
    }

    public void Miss()
    {
        score = Mathf.Max(0, score - 25); // Prevent negative score
        missCount++;
        UpdateScoreUI();
        UpdateFailedUI();
        Debug.Log("Miss! Score: " + score + ", Misses: " + missCount);

        if (missSound != null)
            missSound.Play();

        // Check if game over
        if (missCount >= missesAllowed)
        {
            GameOver();
        }
    }

    public void ObstacleHit()
    {
        obstacleHits++;
        score = Mathf.Max(0, score - 50);
        UpdateScoreUI();
        UpdateObstacleUI();
        Debug.Log("Obstacle Hit! Hits: " + obstacleHits);

        if (missSound != null)
            missSound.Play();

        // Check if game over due to too many obstacle hits
        if (obstacleHits >= maxObstacleHits)
        {
            GameOver();
        }
    }

    // UI update methods
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    private void UpdateFailedUI()
    {
        if (failedText != null)
            failedText.text = missCount + " / " + missesAllowed;
    }

    private void UpdateObstacleUI()
    {
        if (multiplierText != null)
            multiplierText.text = "Obstacles: " + obstacleHits + " / " + maxObstacleHits;
    }

    // Game over handling
    public void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + score);

        // Play game over sound
        if (gameOverSound != null)
            gameOverSound.Play();

        // Stop all spawners
        ArrowSpawner[] spawners = FindObjectsOfType<ArrowSpawner>();
        foreach (ArrowSpawner spawner in spawners)
        {
            spawner.enabled = false;
        }

        // Stop the game
        Time.timeScale = 0;

        // Calculate hit percentage
        int totalNotes = perfectHits + goodHits + normalHits + missCount;
        float hitPercentage = totalNotes > 0 ?
            (float)(perfectHits + goodHits + normalHits) / totalNotes * 100 : 0;

        // Determine rank
        string rank = "F";
        if (hitPercentage >= 95) rank = "S";
        else if (hitPercentage >= 90) rank = "A";
        else if (hitPercentage >= 80) rank = "B";
        else if (hitPercentage >= 70) rank = "C";
        else if (hitPercentage >= 60) rank = "D";

        // Update results panel text
        if (normalHitsValue != null) normalHitsValue.text = normalHits.ToString();
        if (goodHitsValue != null) goodHitsValue.text = goodHits.ToString();
        if (perfectHitsValue != null) perfectHitsValue.text = perfectHits.ToString();
        if (missedHitsValue != null) missedHitsValue.text = missCount.ToString();
        if (percentHitValue != null) percentHitValue.text = hitPercentage.ToString("F1") + "%";
        if (rankValue != null) rankValue.text = rank;
        if (finalScoreValue != null) finalScoreValue.text = score.ToString();

        // Show results panel
        if (resultsPanel != null)
            resultsPanel.SetActive(true);
    }

    // Create restart button for results panel
    private void CreateRestartButton()
    {
        GameObject buttonObj = new GameObject("RestartButton");
        buttonObj.transform.SetParent(resultsPanel.transform, false);

        // Set up RectTransform
        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -200);
        rectTransform.sizeDelta = new Vector2(200, 50);

        // Add Button component
        Button button = buttonObj.AddComponent<Button>();

        // Add Image component for background
        Image image = buttonObj.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f);

        // Add Text child
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        // Set up text
        Text text = textObj.AddComponent<Text>();
        text.text = "Play Again";
        // Don't specify a font - Unity will use the default font
        text.fontSize = 24;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;

        // Set up text RectTransform
        RectTransform textRectTransform = text.GetComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.offsetMin = Vector2.zero;
        textRectTransform.offsetMax = Vector2.zero;

        // Add click handler
        button.onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
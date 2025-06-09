using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

    [Header("UI Feedback")]
    public GameObject comboTextPrefab;
    public Transform uiCanvas;
    public Vector3 comboTextOffset = new Vector3(0, 100, 0);

    [Header("Screen Effects")]
    public Image screenFlashImage; // Full screen flash effect
    public float flashDuration = 0.1f;

    [Header("Feedback Colors")]
    public Color perfectFlashColor = new Color(1f, 1f, 0f, 0.3f);
    public Color goodFlashColor = new Color(0f, 1f, 0f, 0.3f);
    public Color normalFlashColor = new Color(0f, 0.5f, 1f, 0.3f);
    public Color missFlashColor = new Color(1f, 0f, 0f, 0.3f);
    public Color obstacleFlashColor = new Color(1f, 0f, 0f, 0.5f);

    [Header("Combo Feedback")]
    public int[] comboMilestones = { 10, 25, 50, 100 };
    public string[] comboMessages = { "Great!", "Awesome!", "Incredible!", "LEGENDARY!" };
    public Color[] comboColors;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Create screen flash if it doesn't exist
        if (screenFlashImage == null)
        {
            CreateScreenFlash();
        }

        // Set default combo colors if not set
        if (comboColors == null || comboColors.Length == 0)
        {
            comboColors = new Color[] {
                Color.white,
                Color.yellow,
                new Color(1f, 0.5f, 0f), // Orange
                new Color(1f, 0f, 1f)     // Purple
            };
        }
    }

    private void CreateScreenFlash()
    {
        // Find or create UI canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("FeedbackCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Create flash image
        GameObject flashObj = new GameObject("ScreenFlash");
        flashObj.transform.SetParent(canvas.transform, false);

        screenFlashImage = flashObj.AddComponent<Image>();
        screenFlashImage.color = new Color(1, 1, 1, 0);

        // Make it cover the whole screen
        RectTransform rect = flashObj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Make sure it's behind other UI
        flashObj.transform.SetAsFirstSibling();
    }

    // Called when player hits a note
    public void OnNoteHit(string hitQuality, int combo)
    {
        Color flashColor = Color.clear;

        switch (hitQuality)
        {
            case "Perfect":
                flashColor = perfectFlashColor;
                break;
            case "Good":
                flashColor = goodFlashColor;
                break;
            case "Normal":
                flashColor = normalFlashColor;
                break;
        }

        if (flashColor != Color.clear)
        {
            StartCoroutine(ScreenFlash(flashColor));
        }

        // Check for combo milestones
        CheckComboMilestone(combo);
    }

    // Called when player misses
    public void OnNoteMiss()
    {
        StartCoroutine(ScreenFlash(missFlashColor));
        StartCoroutine(ScreenShake(0.2f, 0.05f));
    }

    // Called when hit by obstacle
    public void OnObstacleHit()
    {
        StartCoroutine(ScreenFlash(obstacleFlashColor));
        StartCoroutine(ScreenShake(0.3f, 0.1f));
        ShowDamageText();
    }

    private void CheckComboMilestone(int combo)
    {
        for (int i = 0; i < comboMilestones.Length; i++)
        {
            if (combo == comboMilestones[i])
            {
                ShowComboMilestone(comboMessages[i], comboColors[i % comboColors.Length]);
                break;
            }
        }
    }

    private void ShowComboMilestone(string message, Color color)
    {
        // Create combo text in center of screen
        GameObject comboObj = new GameObject("ComboMilestone");
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            comboObj.transform.SetParent(canvas.transform, false);

            Text comboText = comboObj.AddComponent<Text>();
            comboText.text = message;
            comboText.fontSize = 60;
            comboText.color = color;
            comboText.alignment = TextAnchor.MiddleCenter;

            // Add outline
            Outline outline = comboObj.AddComponent<Outline>();
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(2, -2);

            // Position in center
            RectTransform rect = comboObj.GetComponent<RectTransform>();
            rect.anchoredPosition = comboTextOffset;

            // Animate
            StartCoroutine(AnimateComboText(comboObj));
        }
    }

    private void ShowDamageText()
    {
        GameObject damageObj = new GameObject("DamageText");
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            damageObj.transform.SetParent(canvas.transform, false);

            Text damageText = damageObj.AddComponent<Text>();
            damageText.text = "OUCH!";
            damageText.fontSize = 48;
            damageText.color = Color.red;
            damageText.alignment = TextAnchor.MiddleCenter;

            // Random position near center
            RectTransform rect = damageObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(
                Random.Range(-100f, 100f),
                Random.Range(-50f, 50f)
            );

            // Animate
            StartCoroutine(AnimateDamageText(damageObj));
        }
    }

    private IEnumerator ScreenFlash(Color flashColor)
    {
        if (screenFlashImage == null) yield break;

        screenFlashImage.color = flashColor;

        float timer = 0f;
        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(flashColor.a, 0f, timer / flashDuration);
            Color newColor = flashColor;
            newColor.a = alpha;
            screenFlashImage.color = newColor;
            yield return null;
        }

        screenFlashImage.color = new Color(1, 1, 1, 0);
    }

    private IEnumerator ScreenShake(float duration, float magnitude)
    {
        Camera mainCam = Camera.main;
        if (mainCam == null) yield break;

        Vector3 originalPos = mainCam.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCam.transform.position = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.position = originalPos;
    }

    private IEnumerator AnimateComboText(GameObject textObj)
    {
        if (textObj == null) yield break;

        Text text = textObj.GetComponent<Text>();
        RectTransform rect = textObj.GetComponent<RectTransform>();

        // Scale up animation
        float timer = 0f;
        float animDuration = 0.5f;

        while (timer < animDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / animDuration;

            // Scale effect
            float scale = Mathf.Lerp(0f, 1.2f, Mathf.SmoothStep(0, 1, progress));
            rect.localScale = Vector3.one * scale;

            yield return null;
        }

        // Hold
        yield return new WaitForSeconds(0.5f);

        // Fade out
        timer = 0f;
        animDuration = 0.3f;
        Color startColor = text.color;

        while (timer < animDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / animDuration;

            // Move up and fade
            rect.anchoredPosition += Vector2.up * Time.deltaTime * 100f;
            Color newColor = startColor;
            newColor.a = 1f - progress;
            text.color = newColor;

            yield return null;
        }

        Destroy(textObj);
    }

    private IEnumerator AnimateDamageText(GameObject textObj)
    {
        if (textObj == null) yield break;

        Text text = textObj.GetComponent<Text>();
        RectTransform rect = textObj.GetComponent<RectTransform>();
        Vector2 startPos = rect.anchoredPosition;

        // Shake and fade
        float timer = 0f;
        float animDuration = 0.5f;

        while (timer < animDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / animDuration;

            // Shake
            rect.anchoredPosition = startPos + Random.insideUnitCircle * 5f;

            // Fade
            Color newColor = text.color;
            newColor.a = 1f - progress;
            text.color = newColor;

            yield return null;
        }

        Destroy(textObj);
    }
}
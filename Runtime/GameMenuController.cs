using UnityEngine;

/// <summary>
/// Controls the game menu's visibility and animation.
/// </summary>
public class GameMenuController : MonoBehaviour
{
    [SerializeField]
    private RectTransform gameMenuRectTransform;

    [SerializeField]
    private Vector3 gameMenuShownPos;

    [SerializeField]
    private Vector3 gameMenuHiddenPos;

    [SerializeField]
    private float lerpTime = 1f;
    
    [SerializeField]
    private bool isGameMenuShown = true;

    private float currentLerpTime;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
            ToggleGameMenu();
        
        HandleMenuAnimation();
    }

    /// <summary>
    /// Handles the animation of the game menu.
    /// </summary>
    private void HandleMenuAnimation()
    {
        if (currentLerpTime >= lerpTime)
        {
            return;
        }
        currentLerpTime += Time.deltaTime;
        float t = currentLerpTime / lerpTime;
        t = SmoothStep(t);
        gameMenuRectTransform.anchoredPosition = Vector3.Lerp(
            isGameMenuShown ? gameMenuHiddenPos : gameMenuShownPos,
            isGameMenuShown ? gameMenuShownPos : gameMenuHiddenPos,
            t
        );
    }

    /// <summary>
    /// Smoothstep interpolation function.
    /// </summary>
    /// <param name="t">The interpolation factor.</param>
    /// <returns>The interpolated value.</returns>
    private float SmoothStep(float t)
    {
        return t * t * (3f - 2f * t);
    }

    /// <summary>
    /// Toggles the visibility of the game menu.
    /// </summary>
    public void ToggleGameMenu()
    {
        isGameMenuShown = !isGameMenuShown;
        currentLerpTime = 0f;
    }
}
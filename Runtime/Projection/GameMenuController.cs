using UnityEngine;


/// <summary>
/// vv UNUSED DEMO BEHAVIOUR vv
/// <br>Controls the game menu's visibility and animation.</br>
/// </summary>
public class GameMenuController : MonoBehaviour
{
    [Tooltip("The RectTransform component of the menu.")][SerializeField]
    private RectTransform gameMenuRectTransform;

    [Tooltip("The position of the menu when shown")][SerializeField]
    private Vector3 gameMenuShownPos;

    [Tooltip("The position of the menu when hidden")][SerializeField]
    private Vector3 gameMenuHiddenPos;

    [Tooltip("Time the menu takes to move")][SerializeField]
    private float lerpTime = 1f;

    [Tooltip("Whether the menu is shown")][SerializeField]
    private bool isGameMenuShown = true;

    private float currentLerpTime;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ToggleGameMenu();
        }
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
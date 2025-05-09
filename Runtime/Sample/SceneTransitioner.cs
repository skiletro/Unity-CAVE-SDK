using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Threading.Tasks;

public class SceneTransitioner : MonoBehaviour
{
    [Tooltip("Add scene names in the order you want to load")]
    [SerializeField] private string[] sceneNames;

    private CancellationTokenSource _cancellationTokenSource;


#region Public
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is null or empty");
            return;
        }

        /*
        if (System.Array.IndexOf(sceneNames, sceneName) < 0)
        {
            Debug.LogError($"Scene name '{sceneName}' not found in the list");
            return;
        }
        */

        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int index)
    {
        if (index < 0 || index >= sceneNames.Length)
        {
            Debug.LogError("Index out of range");
            return;
        }

        SceneManager.LoadScene(sceneNames[index]);
    }
#endregion




    void OnEnable()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _ = OnKeypadNumberPress(_cancellationTokenSource.Token);
    }

    void OnDisable()
    {
        _cancellationTokenSource.Cancel();
    }

    // Async listen for keypad number press
    async Task OnKeypadNumberPress(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Keypad0 + i) || Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    if (i < sceneNames.Length)
                    {
                        await SceneManager.LoadSceneAsync(sceneNames[i]);
                    }
                    else
                    {
                        Debug.LogError("Index out of range");
                    }
                    break;
                }
            }
            await Task.Yield();
        }
    }
}
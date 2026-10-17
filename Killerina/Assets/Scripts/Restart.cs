using UnityEngine;

public class Restart : MonoBehaviour
{
    public void RestartGame()
    {
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}

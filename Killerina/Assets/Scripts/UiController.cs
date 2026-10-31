using UnityEngine;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void StartTrigger()
    {
        animator.SetBool("Move", true);
    }

    public void ChangeScene()
    {
        
         SceneManager.LoadScene("Level");
    }
}

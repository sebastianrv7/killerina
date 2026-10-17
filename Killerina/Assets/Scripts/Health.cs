using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;   
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isBoss;
    [SerializeField] private GameObject winPanel;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage = 1)
    {
        currentHealth -= damage;

        print($"{gameObject.name} recibió {damage} de daño. Vidaq restante: {currentHealth}");
        if (currentHealth <= 0)
        {
            
            Die();
        }
    } 

    private void Die()
    {
        print($"{gameObject.name} ha muerto.");
        if(isPlayer)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if(isBoss)
        {
            winPanel.SetActive(true);
        }
        Destroy(gameObject);
    }
}
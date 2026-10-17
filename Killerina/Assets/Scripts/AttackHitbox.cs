using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Health life))
        {
            life.TakeDamage();
        }
    }
}

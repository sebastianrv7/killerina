using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private BoxCollider2D bodyCollider;
    [SerializeField] private BoxCollider2D damageTrigger;

    public BoxCollider2D BodyCollider => bodyCollider;
    public BoxCollider2D DamageTrigger => damageTrigger;
}

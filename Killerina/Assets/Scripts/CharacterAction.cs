using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAction : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;
    private bool grounded;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 8f;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashDuration = 20f;
    [SerializeField] private bool dashPressed;


    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float ascendingGravity = 1f;
    [SerializeField] private float yDashGravity = 0f; 
    private float defaultGravity;

    [Header("Attack")]
    [SerializeField] private bool attackPressed;
    [SerializeField] private bool isAttacking;

    [Header("References")]
    [SerializeField] private Transform visualRoot;
    [SerializeField] private Animator animator;

    [Header("Colliders")]
    [SerializeField] private PolygonCollider2D attackCollider;
    [SerializeField] private BoxCollider2D playerBodyCollider;

    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference dashAction;
    [SerializeField] private InputActionReference attackAction;


    private Rigidbody2D rb;
    private float horizontalInput;
    private bool jumpPressed;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    private void Update()
    {
        // Leer el input del jugador
        horizontalInput = moveAction.action.ReadValue<Vector2>().x;

        if (jumpAction.action.WasPressedThisFrame()) 
        {
            jumpPressed = true;
        }

        if (horizontalInput > 0 && !facingRight)
        {
            facingRight = true;
            visualRoot.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (horizontalInput < 0 && facingRight)
        {
            facingRight = false;
            visualRoot.localScale = new Vector3(-1f, 1f, 1f);
        }

        if(dashAction.action.WasPressedThisFrame() && !isDashing)
        {
            isDashing = true;
            StartCoroutine(DashCoroutine());
        }

        if (attackAction.action.WasPressedThisFrame())
        {
            attackPressed = true;
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        animator.SetBool("Grounded", grounded);

        if (isAttacking && grounded)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("AirAttack"))
            {
                EndAttack();
            }
        }

        if (rb.linearVelocityY > 0.1f && !isDashing)
        {
            rb.gravityScale = ascendingGravity;
        }
        else
        {
            if(!isDashing)
            {
                rb.gravityScale = defaultGravity;
            }            
        }


        bool canMove = !isAttacking || !grounded;
        if (canMove)
        {
            if(!isDashing)
            {
                rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
            }
            
        }
        

        if(isDashing)
        {
            print("Está deslizandose");
            
        }

        if(attackPressed)
        {
            if (!isAttacking)
            {
                isAttacking = true;

                if (grounded)
                {
                    animator.SetTrigger("Attack");
                }
                else
                {
                    animator.SetTrigger("AirAttack");
                }
            }

            attackPressed = false;
        }

        bool canJump = !isAttacking;
        if (jumpPressed)
        {
            if (canJump && grounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            // Consumir siempre el input
            jumpPressed = false;
        }

        animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);
    }
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }

    public void EndAttack()
    {
        isAttacking = false;
        DisableAttackHitbox();
    }

    public void EnableAttackHitbox()
    {
        attackCollider.enabled = true;
    }

    public void DisableAttackHitbox()
    {
        attackCollider.enabled = false;
    }

    private IEnumerator DashCoroutine()
    {
        float dashDirection;
        if(facingRight)
        {
            dashDirection =1f;
        }
        else
        {
            dashDirection = -1f;
        }

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        

        foreach(Enemy enemy in enemies)
        {
            Physics2D.IgnoreCollision(playerBodyCollider, enemy.BodyCollider, true);
            Physics2D.IgnoreCollision(playerBodyCollider, enemy.DamageTrigger, true);
        }
        
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

        if(grounded)
        {
            animator.SetBool("Dash", isDashing);
        }
        else
        {
            rb.gravityScale = yDashGravity;
            animator.SetBool("AirDash", isDashing);
        }
        yield return new WaitForSeconds(dashDuration);

        foreach (Enemy enemy in enemies)
        {
            Physics2D.IgnoreCollision(playerBodyCollider, enemy.BodyCollider, false);
            Physics2D.IgnoreCollision(playerBodyCollider, enemy.DamageTrigger, false);
        }

        rb.gravityScale = 0f;
        isDashing = false;
        animator.SetBool("Dash", isDashing);
        animator.SetBool("AirDash", isDashing);
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }
}

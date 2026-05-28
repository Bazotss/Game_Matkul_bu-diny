using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    public Collider2D swordCollider;


    Rigidbody2D rb;
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;

    void Start()
    {
        // Ambil Rigidbody2D dari Player
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    void FixedUpdate()
    {
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);
                if (!success){
                    success = TryMove(new Vector2(movementInput.x, 0));

                    if (!success){
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }
                animator.SetBool("IsMoving", success);
            } else {
                animator.SetBool("IsMoving", false);
            }

            print("IsMoving" + animator.GetBool("IsMoving"));

            if (movementInput.x < 0){
                spriteRenderer.flipX = true;
                
            }
            else if (movementInput.x > 0){
                spriteRenderer.flipX = false;
                
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime * collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
        
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        if (animator != null)
        {
            animator.SetTrigger("SwordAttack");
        }
    }

    public void SwordAttack()
    {
        LockMovement();
        Debug.Log("Attack Aktif");

        swordCollider.enabled = true;

        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }

    }

    public void EndSwordAttack()
    {
        UnlockMovement();

        rb.velocity = Vector2.zero;

        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }

        if (swordAttack != null)
        {
            swordAttack.StopAttack();
        }
    }

    public void LockMovement(){
        canMove = false;
    }

    public void UnlockMovement(){
        canMove = true;
    }
}
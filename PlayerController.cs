using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collssionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    Rigidbody2D rb;
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;

    void Awake()
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
                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));

                    if (!success)
                    {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }
                animator.SetBool("IsMoving", success);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }

            print("IsMoving" + animator.GetBool("IsMoving"));

            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
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
            moveSpeed * Time.fixedDeltaTime * collssionOffset);

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

    void OnFire(){
        //print("Fire Pressed");
        animator.SetTrigger("SwordAttack");
    }

    public void LockMovement(){
        canMove = false;
    }

    public void UnlockMovement(){
        canMove = true;
    }
}
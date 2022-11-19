using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit for Update & Move Functions: Game Dev Experiments on YouTube

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; // public variable for movement speed

    private bool isMoving; // not public bc not changing in inspector; check if player is moving
    private Vector2 input;

    private Animator animator; // reference to animator controller

    private void Awake ()
    {
        animator = GetComponent<Animator>();
    }



    private void Update() 
    {
        if (! isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal"); // input will always be 1 or -1 with GetAxisRaw
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0; // removes y component so no diagonal movement

            if (input != Vector2.zero) // if input not zero
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x; // current position + input
                targetPos.y += input.y;

                StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    // Coroutine function
    // used to do something over a period of time -- in this case, move the character from starting to ending over time
    IEnumerator Move(Vector3 targetPos) 
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) 
        // checks if the diff between the player's position & target position is greater than v small value
        {
            // moves player toward target position by v small amt
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime); 
            // stops the execution, resumes from here in the next update function
            yield return null;
        }
        // when diff b/t two positions is so small, leave loop and set the new current position
        transform.position = targetPos;

        isMoving = false;
    }
}

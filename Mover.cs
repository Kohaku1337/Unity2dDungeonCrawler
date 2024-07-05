using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    private Vector3 originalSize;
    protected BoxCollider2D boxCollider; 
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    public float ySpeed = 0.75f;
    public float xSpeed = 1.0f;
    

    protected virtual void Start()
    {
        originalSize = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        // Reset moveDelta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        // Swap sprite direction if you're going right or left
        if (moveDelta.x > 0)
            transform.localScale = originalSize;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(originalSize.x *-1,originalSize.y,originalSize.z);

        // Add push vector, if any
        moveDelta += pushDirection;

        // Reduce pushforce every frame , based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y),
            Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("ppls", "Blocking"));
        if (hit.collider == null)
        {
            // Move woohoo
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0),
            Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("ppls", "Blocking"));
        if (hit.collider == null)
        {
            // Move woohoo
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}

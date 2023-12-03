using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    public float boundaryRadius = 2f;

    public float fovAngle = 90f;
    public float detectionRadius = 10f;
    public LayerMask playerLayer;
    private Vector2 initialPosition;

    private float timeSinceLastDirectionChange;
    private Vector2 currentDirection;


    private void Start()
    {

        initialPosition = transform.position;
        ChooseRandomDirection(); // Set initial random direction

    }

    private void Update()
    {
        Move();
        CheckDirectionChangeTimer();
        ClampPositionToBoundary();
        CheckPlayerDetection();

    }


    private void OnDrawGizmos()
    {
        DrawGizmos();
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmos();
    }

    private void DrawGizmos()
    {
        // Draw a wire sphere in the editor to represent the boundary
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, boundaryRadius);

        Vector2 fovLine1 = Quaternion.AngleAxis(fovAngle * 0.5f, transform.forward) * currentDirection;
        Vector2 fovLine2 = Quaternion.AngleAxis(-fovAngle * 0.5f, transform.forward) * currentDirection;

        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, fovLine1 * detectionRadius);
        Gizmos.DrawRay(transform.position, fovLine2 * detectionRadius);
    }

    private void Move()
    {
        transform.Translate(currentDirection * moveSpeed * Time.deltaTime);
    }
    private void CheckDirectionChangeTimer()
    {
        timeSinceLastDirectionChange += Time.deltaTime;
        if (timeSinceLastDirectionChange >= changeDirectionInterval)
        {
            ChooseRandomDirection(); // Set a new random direction
            timeSinceLastDirectionChange = 0f;
        }
    }

    private void ChooseRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        currentDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }

    private void ClampPositionToBoundary()
    {

        Vector2 currentPosition = transform.position;

        // Calculate the vector from the initial position to the slime's current position
        Vector2 vectorToCurrentPosition = currentPosition - initialPosition;

        // Clamp based on the slime's position relative to its initial position
        if (vectorToCurrentPosition.magnitude > boundaryRadius)
        {
            // Normalize the vector and multiply by boundaryRadius to get the clamped position
            Vector2 clampedPosition = initialPosition + vectorToCurrentPosition.normalized * boundaryRadius;

            // Update the slime's position to the clamped position
            transform.position = new Vector2(clampedPosition.x, clampedPosition.y);
        }
    }
    void CheckPlayerDetection()
    {
        float angleIncrement = 0.1f;

        for (float angle = -fovAngle * 0.5f; angle <= fovAngle * 0.5f; angle += angleIncrement)
        {
            Vector2 direction = Quaternion.AngleAxis(angle, transform.forward) * currentDirection;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius, playerLayer);

            if (hit.collider != null)
            {
                Debug.Log("Player detected");

                // Draw the ray for visualization
                Debug.DrawRay(transform.position, direction * detectionRadius, Color.blue);

                // You can add additional logic here for what happens when the player is detected
            }
            else
            {
                // Draw the ray for visualization (optional)
                Debug.DrawRay(transform.position, direction * detectionRadius, Color.gray);

            }
        }
    }

}
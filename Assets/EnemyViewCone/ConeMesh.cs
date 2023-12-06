using System.Collections.Generic;
using UnityEngine;

public class CharacterWaypointsHandler : MonoBehaviour
{
    private const float speed = 5f; // Lower speed for demonstration purposes
    [SerializeField] private float movementRadius = 5f; // Specify the movement radius

    private Vector3 randomMoveDir;

    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 5f; // Lower view distance for demonstration purposes

    private FieldOfView fieldOfView;
    private Vector3 lastMoveDir;
    [SerializeField] private Transform pfFieldOfView;

    private void Start()
    {
        lastMoveDir = GetRandomMoveDirection();
        fieldOfView = Instantiate(pfFieldOfView, null).GetComponent<FieldOfView>();
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(viewDistance);
    }

    private void HandleMovement()
    {
        // Move in the random direction
        transform.Translate(randomMoveDir * speed * Time.deltaTime, Space.World);

        // Ensure the slime stays within the circular movement radius
        Vector3 directionToCenter = (transform.parent.position - transform.position).normalized;
        transform.Translate(directionToCenter * speed * Time.deltaTime, Space.World);

        // Update the random move direction periodically
        if (Random.Range(0f, 1f) < 0.02f)
        {
            randomMoveDir = GetRandomMoveDirection();
        }
    }

    private Vector3 GetRandomMoveDirection()
    {
        // Get a random direction within the movement radius
        float randomAngle = Random.Range(0f, 360f);
        Vector3 randomDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.right;
        return transform.position + randomDirection * movementRadius;
    }

    private void Update()
    {
        HandleMovement();
        FindTargetPlayer();

        if (fieldOfView != null)
        {
            fieldOfView.SetOrigin(transform.position);
            fieldOfView.SetAimDirection(GetAimDir());
        }

        Debug.DrawLine(transform.position, transform.position + GetAimDir() * 10f);
    }

    private void FindTargetPlayer()
    {
        // Your player detection logic remains the same
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, viewDistance);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Player detected
                Debug.Log("Player Detected!");
                // Implement your logic for attacking or following the player here
            }
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetAimDir()
    {
        return lastMoveDir;
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    public float boundaryRadius = 2f;
    private int POINTS_LIMIT = 20;

    // Reference to the object to be removed
    public GameObject _ownObj;

    public float fovAngle = 90f;
    public float detectionRadius = 10f;
    public LayerMask playerLayer;
    public LayerMask testLayer;
    private Vector2 initialPosition;
    public PlayerController _player;
    private int points = 0;

    private float timeSinceLastDirectionChange;
    private Vector2 currentDirection;
    public Transform fieldOfViewPrefab;
    private FieldOfView fieldOfViewInstance;
    private void Start()
    {

        initialPosition = transform.position;
        ChooseRandomDirection(); // Set initial random direction
        fieldOfViewInstance = Instantiate(fieldOfViewPrefab, null).GetComponent<FieldOfView>();
        fieldOfViewInstance.SetFoV(fovAngle); // Set the initial FOV value
        fieldOfViewInstance.SetViewDistance(detectionRadius);
    }

    private void Update()
    {
        Move();
        CheckDirectionChangeTimer();
        ClampPositionToBoundary();
        CheckPlayerDetection();
        CheckPoints();
        fieldOfViewInstance.SetOrigin(transform.position);
        fieldOfViewInstance.SetAimDirection(currentDirection);
    }

    private void CheckPoints()
    {
        if (points >= POINTS_LIMIT)
        {
            _player.AddPoints(points);
            points = 0;
            //Destroy(_ownObj);
        }

    }

    public void AddPoint()
    {
        points++;
    }

    public int GetPoints()
    {
        return points;
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

    private void CheckPlayerDetection()
    {
        if (fieldOfViewInstance.IsPlayerInFOV())
        {
            // Player detected
            Debug.Log("Player detected");
        }
    }
}
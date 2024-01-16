using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class SlimeMovement : MonoBehaviour
{
    public int state;
    public  float moveSpeed               = 2f;
    public  float changeDirectionInterval = 2f;
    public  float boundaryRadius          = 2f;
    public  float DIFFICULTY              = 1.3f;
    private int   POINTS_LIMIT            = 20;
    private bool  alreadyTriggered        = false;

    public  float            fovAngle         = 90f;
    public  float            detectionRadius  = 10f;
    public  LayerMask        playerLayer;
    public  LayerMask        testLayer;
    public  PlayerController _player;
    public  GameObject       DeathMenu;
    private GameObject       Canvas;
    private Vector2          initialPosition;
    private int              points           = 0;

    public  Transform   fieldOfViewPrefab;
    private FieldOfView fieldOfViewInstance;
    private float       timeSinceLastDirectionChange;
    private Vector2     currentDirection;

    private void Start()
    {
        setPlayer();
        initialPosition = transform.position;
        ChooseRandomDirection(); // Set initial random direction
        createFov();
    }

    private void setPlayer()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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

    public void createFov()
    {
        fieldOfViewInstance = Instantiate(fieldOfViewPrefab, null).GetComponent<FieldOfView>();
        fieldOfViewInstance.SetFoV(fovAngle); // Set the initial FOV value
        fieldOfViewInstance.SetViewDistance(detectionRadius);
    }

   /* private void CheckPoints()
    {
        if (points >= POINTS_LIMIT)
        {
            _player.AddPoints(points);
            points = 0;
            if (!alreadyTriggered)
            {
                fovAngle = fovAngle * DIFFICULTY;
                fieldOfViewInstance.SetFoV(fovAngle);
                alreadyTriggered = true;
            }
            //Destroy(_ownObj);
        }

    }*/
   private void CheckPoints() 
    { 
        if (points >= POINTS_LIMIT)
        {
            _player.AddPoints(points);
            points = 0;

            switch (state)
            {
                case 1:
                    fovAngle *= DIFFICULTY;
                    fieldOfViewInstance.SetFoV(fovAngle);
                    break;

                case 2:

                    moveSpeed *= DIFFICULTY;
                    break;

                case 3:

                    detectionRadius *= DIFFICULTY;
                    fieldOfViewInstance.SetViewDistance(detectionRadius);
                    break;


            }
        }

    }

    public void AddPoint()
    {
        points++;
        //Debug.Log("Points : " + points);
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
        currentDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 
                                       Mathf.Sin(randomAngle * Mathf.Deg2Rad));
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
            StartCoroutine(ActivateDeathMenuAfterDelay());
        }
    }

    private IEnumerator ActivateDeathMenuAfterDelay()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(0.5f);

        // Activate the death menu
        DeathMenu.SetActive(true);

        // Destroy slimes
        GameObject[] slimes = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var slime in slimes)
        {
            Destroy(slime);
        }

        // Destroy other objects, except those tagged 'audio'
        foreach (var obj in GetDontDestroyOnLoadObjects())
        {
            if (!obj.tag.Equals("audio"))
            {
                Destroy(obj);
            }
            // Uncomment if you want to log the destruction
            // Debug.Log(obj.name + " destroyed!");
        }
    }

    // Rest of your class code, including GetDontDestroyOnLoadObjects and other methods


public static GameObject[] GetDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            Object.DontDestroyOnLoad(temp);
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.DestroyImmediate(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if (temp != null)
                Object.DestroyImmediate(temp);
        }
    }
}
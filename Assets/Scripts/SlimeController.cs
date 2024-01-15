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
    public  float moveSpeed               = 2f;
    public  float changeDirectionInterval = 2f;
    public  float boundaryRadius          = 2f;
    public  float DIFFICULTY              = 1.3f;
    private int   POINTS_LIMIT            = 20;
    private bool  alreadyTriggered        = false;
    private bool  created                 = false;
    // Reference to the object to be removed
    //public GameObject _ownObj;

    public float            fovAngle = 90f;
    public float            detectionRadius = 10f;
    public LayerMask        playerLayer;
    public LayerMask        testLayer;
    public PlayerController _player;
    private Vector2         initialPosition;
    private int             points = 0;
    public GameObject DeathMenu;
    private GameObject Canvas;

    private float timeSinceLastDirectionChange;
    private Vector2 currentDirection;
    public Transform fieldOfViewPrefab;
    private FieldOfView fieldOfViewInstance;

    private void Start()
    {
        //DeathMenu = GameObject.FindGameObjectWithTag("Death");
        //if(DeathMenu == null) { Debug.Log("[INFO] Death Menu is null"); };
        //if(DeathMenu != null)
        //{
        //    DeathMenu.SetActive(false);
        //    Debug.Log("Falsseeeeee");
        //}
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

    private void CheckPoints()
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
            DeathMenu.SetActive(true);
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject player   = GameObject.FindGameObjectWithTag("Player");

            foreach (var slime in slimes)
            {
                Destroy(slime);
            }

            foreach (var obj in GetDontDestroyOnLoadObjects())
            {
                Destroy(obj);
                //Debug.Log(obj.name + " destroyed!");
            }

            //player.GetComponent<PlayerController>().walkingPoints = 100;
            //player.GetComponent<Transform>().position = new Vector3(-8, -7, 0);
            //SceneManager.LoadSceneAsync(1);

        }
    }

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
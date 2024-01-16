using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _playerRigidbody2D;
    public  float       _playerSpeed;
    private Vector2     _playerDirection;
    private Animator    _player_Animator;

    private SlimeMovement[] slimeObjs;
    private Transform[]     slimeTrans;
    private SpriteRenderer  spriteRenderer;

    public Transform playerTrans;

    public  int         walkingPoints;
    public  float       movementCooldown = 0.1f;
    private float       lastMovementTime;
    public  UIManager   uiManager;
    private Transform   walkingPointsTransform;

    private void Start()
    {
        setSlimeObjectsAndTrans();

        playerTrans            = GameObject.FindGameObjectWithTag("Player")
                                .GetComponent<Transform>();
        _playerRigidbody2D     = GetComponent<Rigidbody2D>();
        _player_Animator       = GetComponent<Animator>();
        walkingPoints          = 100;
        walkingPointsTransform = transform.Find("WalkingPoints");
        uiManager              = FindObjectOfType<UIManager>();
        spriteRenderer         = GetComponent<SpriteRenderer>();
    }

    private void setSlimeObjectsAndTrans()
    {
        GameObject[] slimes = GameObject.FindGameObjectsWithTag("Enemy");
        slimeObjs           = new SlimeMovement[slimes.Length];
        slimeTrans          = new Transform[slimes.Length];

        int i = 0;
        foreach(var slime in slimes)
        {
            slimeObjs[i]  = slime.GetComponent<SlimeMovement>();
            slimeTrans[i] = slime.GetComponent<Transform>();
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 
                                       Input.GetAxisRaw("Vertical")).normalized;
        Debug.Log(_playerDirection.x);

        if(_playerDirection.sqrMagnitude > 0 )
        {
            _player_Animator.SetInteger("Movement", 1);
            FlipMovement();
            walkingPointsTransform.position = transform.position;
        }
        else
        {
            //FlipMovement();
            _player_Animator.SetInteger("Movement", 0);
        }
        if (walkingPoints <= 0)
        {
            RestartScene();
        }

        // TO BE REMOVED
        setSlimeObjectsAndTrans();
        playerTrans = GameObject.FindGameObjectWithTag("Player")
                                .GetComponent<Transform>();
    }

    public void AddPoints(int points)
    {
        walkingPoints += points;
    }

    private void checkCloserEnemy()
    {
        Dictionary<SlimeMovement, float> distHashMap = new Dictionary<SlimeMovement, float>();

        try
        {
            for(int i = 0; i < slimeObjs.Length; i++)
            {
                distHashMap.Add(slimeObjs[i], Vector3.Distance(playerTrans.position, slimeTrans[i].position));
            }
        }
        catch(Exception e)
        {
            print(e);
        }

        float minValue = float.MaxValue; 
        foreach (var slime in distHashMap)
        {
            if(slime.Value < minValue)
            {
                minValue = slime.Value;
            }
        }

        // Iterating through key-value pairs
        foreach (var kvp in distHashMap)
        {
            if(kvp.Value == minValue)
            {
                //POF
                kvp.Key.AddPoint();
            }
        }


    }
    void FixedUpdate()
    {
        if (_playerDirection.sqrMagnitude > 0 && walkingPoints > 0)
        {
            if (Time.time - lastMovementTime > movementCooldown)
            {
                DeductWalkingPoints();
                lastMovementTime = Time.time;

            }
            _playerRigidbody2D.MovePosition(_playerRigidbody2D.position + _playerDirection 
                                            * _playerSpeed * Time.fixedDeltaTime);
        }
           
    }
    void RestartScene()
    {
        walkingPoints = 10;
        SceneManager.LoadScene(0);
    }
    void DeductWalkingPoints()
    {      
        walkingPoints--;
        checkCloserEnemy();
    }
    void FlipMovement()
    {
        spriteRenderer.flipX = _playerDirection.x < 0;
    }
}

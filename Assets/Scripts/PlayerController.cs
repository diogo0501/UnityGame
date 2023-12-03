using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _playerRigidbody2D;
    public float        _playerSpeed;
    private Vector2     _playerDirection;
    private Animator    _player_Animator;
    public int walkingPoints;
    public float movementCooldown = 0.5f;
    private float lastMovementTime;
    public UIManager uiManager;
    private Transform walkingPointsTransform;
    void Start()
    {
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
        _player_Animator = GetComponent<Animator>();
        walkingPoints = 10;
        walkingPointsTransform = transform.Find("WalkingPoints");
        uiManager = FindObjectOfType<UIManager>();
        Debug.Log("Walking Points: " + walkingPoints);
    }

    // Update is called once per frame
    void Update()
    {
        _playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if(_playerDirection.sqrMagnitude > 0 )
        {
            _player_Animator.SetInteger("Movement", 1);
            FlipMovement();
            Debug.Log("Walking Points: " + walkingPoints);
            walkingPointsTransform.position = transform.position;
        }
        else
        {
            _player_Animator.SetInteger("Movement", 0);
        }
        if (walkingPoints <= 0)
        {
            RestartScene();
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
            _playerRigidbody2D.MovePosition(_playerRigidbody2D.position + _playerDirection * _playerSpeed * Time.fixedDeltaTime);
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
    }
    void FlipMovement()
    {
        if(_playerDirection.x > 0)
        {
            transform.eulerAngles = new Vector3(1f, 1f, 1f);
        }
        else if(_playerDirection.x < 0)
        {
            transform.eulerAngles = new Vector3(-1f, 1f, 1f);
        }
    }
}

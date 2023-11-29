using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _playerRigidbody2D;
    public float        _playerSpeed;
    private Vector2     _playerDirection;
    private Animator    _player_Animator;
    // Start is called before the first frame update
    void Start()
    {
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
        _player_Animator = GetComponent<Animator>();
  
    }

    // Update is called once per frame
    void Update()
    {
        _playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if(_playerDirection.sqrMagnitude > 0 )
        {
            _player_Animator.SetInteger("Movement", 1);
            FlipMovement();
        }
        else
        {
            _player_Animator.SetInteger("Movement", 0);
        }
        
    }

    void FixedUpdate()
    {

        _playerRigidbody2D.MovePosition(_playerRigidbody2D.position + _playerDirection * _playerSpeed * Time.fixedDeltaTime);
    }
    void FlipMovement()
    {
        if(_playerDirection.x > 0)
        {
            transform.eulerAngles = new Vector2(0f, 0f);
        }
        else if(_playerDirection.x < 0)
        {
            transform.eulerAngles = new Vector2(0f, 180f);
        }
    }
}

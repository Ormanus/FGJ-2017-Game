using UnityEngine;
using System;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotationSlowRate;
    [SerializeField]
    private float _speedSlowRate;
    [SerializeField]
    private KeyCode _upKey;
    [SerializeField]
    private KeyCode _downKey;
    [SerializeField]
    private KeyCode _leftKey;
    [SerializeField]
    private KeyCode _rightKey;
    [SerializeField]
    private KeyCode _actionKey;

    public float _curSpeed = 0f;
    public float _curRotation = 0f;


    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKey(_leftKey))
        {
            _curRotation = -_rotationSpeed;
        }
        else if (Input.GetKey(_rightKey))
        {
            _curRotation = _rotationSpeed;
        }

        if (Input.GetKey(_downKey))
        {
            _curSpeed = -_speed;
        }
        else if (Input.GetKey(_upKey))
        {
            _curSpeed = _speed;
        }

        if (_curSpeed <= 0)
        {
            _curSpeed += Time.deltaTime;
        }
        else if (_curSpeed >= 0)
        {
            _curSpeed -= Time.deltaTime;
        }
        else if (_curSpeed >= -0.05f && _curSpeed <= 0.05f)
        {
            _curSpeed = 0;
        }

        if (_curRotation <= 0)
        {
            _curRotation += Time.deltaTime;
        }
        else if (_curRotation >= 0)
        {
            _curRotation -= Time.deltaTime;
        }
        else if (_curRotation >= -0.05f && _curRotation <= 0.05f)
        {
            _curRotation = 0;
        }

        Vector3 moveVector = new Vector3(0, 0, _curSpeed);
        transform.Translate(moveVector.normalized * _curSpeed * Time.deltaTime);
        transform.Rotate(0, _curRotation * _rotationSpeed, 0);

    }
}

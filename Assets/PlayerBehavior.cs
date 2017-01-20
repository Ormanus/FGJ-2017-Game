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
        HandleRotation();
        HandleSpeed();
        Vector3 moveVector = new Vector3(0, 0, _curSpeed);
        transform.Translate(moveVector);
        transform.Rotate(0, _curRotation * _rotationSpeed, 0);

    }

    private void HandleRotation()
    {
        if (Input.GetKey(_leftKey))
        {
            if (_curRotation <= -_rotationSpeed)
            {
                _curRotation = -_rotationSpeed;
            }
            else
            {
                _curRotation -= Math.Abs(_rotationSpeed + _curRotation) * Time.deltaTime;
            }
        }
        else if (Input.GetKey(_rightKey))
        {
            if (_curRotation >= _rotationSpeed)
            {
                _curRotation = _rotationSpeed;
            }
            else
            {
                _curRotation += Math.Abs(_rotationSpeed - _curRotation) * Time.deltaTime;
            }
        }
        else
        {
            if (_curRotation >= -0.25f && _curRotation <= 0.25f)
            {
                _curRotation = 0;
            }
            else
            {
                if (_curRotation < 0)
                {
                    _curRotation += Time.deltaTime * _rotationSlowRate;
                }
                else if (_curRotation > 0)
                {
                    _curRotation -= Time.deltaTime * _rotationSlowRate;
                }
            }
        }
    }

    private void HandleSpeed()
    {
        if (Input.GetKey(_downKey))
        {
            if (_curSpeed <= -_speed)
            {
                _curSpeed = -_speed;
            }
            else
            {
                _curSpeed -= _speed * Time.deltaTime; ;
            }
        }
        else if (Input.GetKey(_upKey))
        {
            if (_curSpeed >= _speed)
            {
                _curSpeed = _speed;
            }
            else
            {
                _curSpeed += _speed * Time.deltaTime; ;
            }
        }
        else
        {
            if (_curSpeed >= -1.5f && _curSpeed <= 1.5f)
            {
                _curSpeed = 0;
            }
            else
            {
                if (_curSpeed < 0)
                {
                    _curSpeed += Time.deltaTime * _speedSlowRate;
                }
                else if (_curSpeed > 0)
                {
                    _curSpeed -= Time.deltaTime * _speedSlowRate;
                }
            }
        }
    }
}

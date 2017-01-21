﻿using UnityEngine;
using System;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

    [SerializeField]
    private Transform _playerModel;
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private float _radius;
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

    public int minRotation;
    public int maxRotation;

    public float _curSpeed = 0f;
    public float _curRotation = 0f;
    public bool _isDoingAction = false;

    private Vector3 _startPosition;
    private Quaternion _startRotation;


    // Use this for initialization
    void Start ()
    {
        _playerModel.localPosition = _offset;
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if(_playerModel.position != _offset)
        {
            _playerModel.localPosition = _offset;
        }
        HandleRotation();
        HandleSpeed();
        Vector3 moveVector = new Vector3(0, 0, _curSpeed);
        _playerModel.Translate(moveVector);
        _playerModel.localEulerAngles = new Vector3(0, -_curRotation * 25, 0);
        transform.Rotate(0, _curRotation * _rotationSpeed, 0);

        if (transform.eulerAngles.y < minRotation)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, minRotation, transform.eulerAngles.z);
            _curRotation = 0;
        }
        if (transform.eulerAngles.y > maxRotation)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, maxRotation, transform.eulerAngles.z);
            _curRotation = 0;
        }

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
                _curRotation -= _rotationSpeed * _rotationSlowRate * Time.deltaTime;
            }
            /*
            if(transform.position.x <= _startPosition.x - _radius)
            {
                transform.position = new Vector3(_startPosition.x - _radius, transform.position.y, transform.position.z);
            }
            else
            {
                transform.Translate(_startPosition.x - _rotationSpeed, 0,0);
            }
            */
        }
        else if (Input.GetKey(_rightKey))
        {
            if (_curRotation >= _rotationSpeed)
            {
                _curRotation = _rotationSpeed;
            }
            else
            {
                _curRotation += _rotationSpeed * _rotationSlowRate * Time.deltaTime;
            }
            /*
            if (transform.position.x >= _startPosition.x + _radius)
            {
                transform.position = new Vector3(_startPosition.x + _radius, transform.position.y, transform.position.z);
            }
            else
            {
                transform.Translate(_startPosition.x + _rotationSpeed, 0, 0);
            }
            */
        }
        else
        {
            if (_curRotation >= (-_rotationSpeed/10) && _curRotation <= (_rotationSpeed / 10))
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

        if (Input.GetKey(_actionKey))
        {
            HandleAction();
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
                _curSpeed -= _speed * Time.deltaTime;
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
                _curSpeed += _speed * Time.deltaTime;
            }
        }
        else
        {
            if (_curSpeed >= (-_speed/15) && _curSpeed <= (_speed / 15))
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

    private void HandleAction()
    {
        if(!_isDoingAction)
        {
            StartCoroutine("action");
        }
    }

    IEnumerator action()
    {
        _isDoingAction = true;
        Animator animator = _playerModel.gameObject.GetComponent<Animator>();
        animator.SetBool("Animate", true);
        yield return new WaitForSeconds(2.25f);
        GameObject.Find("Water").GetComponent<Water>().SpawnWave(new Vector2(transform.FindChild("WaveGun").transform.position.x + 15.0f, transform.FindChild("WaveGun").transform.position.z + 15.0f));
        animator.SetBool("Animate", false);
        _isDoingAction = false;
        yield return null;
    }
}

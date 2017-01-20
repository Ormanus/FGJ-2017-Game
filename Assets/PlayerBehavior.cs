using UnityEngine;
using System;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

    [SerializeField]
    private TeamUtility.IO.PlayerID _playerID;
    [SerializeField]
    private float _speed;
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
        float horDir = 0f;
        float verDir = 0f;
        if (Input.GetKey(_leftKey))
        {
            horDir = -1;
        }
        else if (Input.GetKey(_rightKey))
        {
            horDir = 1;
        }
        else
        {
            horDir = TeamUtility.IO.InputManager.GetAxis("Left Stick Horizontal", _playerID);
        }

        if (Input.GetKey(_downKey))
        {
            verDir = -1;
        }
        else if (Input.GetKey(_upKey))
        {
            verDir = 1;
        }
        else
        {
            TeamUtility.IO.InputManager.GetAxis("Left Stick Vertical", _playerID);
        }

        Vector3 moveVector = new Vector3(0, 0, verDir);
        transform.Translate(moveVector.normalized * _speed * Time.deltaTime);
        transform.Rotate(0, horDir*10, 0);

    }
}

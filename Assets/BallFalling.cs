﻿using UnityEngine;
using System.Collections;

public class BallFalling : MonoBehaviour {

    int p1Points, p2Points;
    float timer;

	// Use this for initialization
	void Start () {
        p1Points = 0;
        p2Points = 0;

        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                Respawn();
            }
            else
                return;
        }

        if (transform.position.y < -5.0f)
        {
            if(transform.position.z > 61)
            {
                p1Points++;
                timer = 3.0f;
                GameObject.Find("Water").GetComponent<Water>().Reset(3.0f);
                Debug.Log("P1 points:" + p1Points);
                if (p1Points > 5)
                {
                    //p1 win
                    Debug.Log("P1 Win");
                }
            }
            else
            {
                p2Points++;
                timer = 3.0f;
                GameObject.Find("Water").GetComponent<Water>().Reset(3.0f);
                Debug.Log("P2 points:" + p2Points);
                if (p2Points > 5)
                {
                    //p2 win
                    Debug.Log("P2 Win");
                }
            }
        }
	}
    void Respawn()
    {
        Water water = GameObject.Find("Water").GetComponent<Water>();
        float x = water.areaSize.x / 2;
        float y = water.areaSize.y / 2;

        transform.position = new Vector3(x, 20, y);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3();
        GameObject plat = GameObject.FindGameObjectWithTag("Platform");
        plat.transform.eulerAngles = new Vector3(0, 0, 0);
        plat.transform.position = new Vector3(x, 3, y);
        plat.GetComponent<Rigidbody>().velocity = new Vector3();
        plat.GetComponent<Rigidbody>().angularVelocity = new Vector3();
    }
}
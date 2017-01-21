using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallFalling : MonoBehaviour {

    int p1Points, p2Points;
    float timer;
    public Text P1Points;
    public Text P2Points;
	// Use this for initialization
	void Start () {
        p1Points = 0;
        p2Points = 0;

        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        P1Points.text = "Player 1 Points: " + p1Points;
        P2Points.text = "Player 2 Points: " + p2Points;
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                GameObject.Find("WinnerText").GetComponent<Text>().text = "";
                Respawn();
            }
            else
                return;
        }

        if (transform.position.y < 0f && transform.position.y > -1f)
        {
            if(!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
        }

            if (transform.position.y < -5.0f)
        {
            if(transform.position.x > 100)
            {
                p1Points++;
                timer = 3.0f;
                GameObject.Find("Water").GetComponent<Water>().Reset(3.0f);
                GameObject.Find("WinnerText").GetComponent<Text>().text = "P1 SCORES";
                if (p1Points > 5)
                {
                    //p1 win
                    GameObject.Find("WinnerText").GetComponent<Text>().text = "P1 WINS";
                }
            }
            else
            {
                p2Points++;
                timer = 3.0f;
                GameObject.Find("Water").GetComponent<Water>().Reset(3.0f);
                GameObject.Find("WinnerText").GetComponent<Text>().text = "P2 SCORES";
                if (p2Points > 5)
                {
                    //p2 win
                    GameObject.Find("WinnerText").GetComponent<Text>().text = "P2 WINS";
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

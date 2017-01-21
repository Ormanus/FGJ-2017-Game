using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {
    private Vector3 startLocation;
    public int player1Score;
    public int player2Score;
    
	// Use this for initialization
	void Start () {
        startLocation = gameObject.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
	if(transform.position.y < -50)
        {
            addScore();returnToStartLocation();
        }
	}
    void returnToStartLocation()
    {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        gameObject.transform.position = startLocation;
       
    }
    void addScore()
    {
        if (transform.position.z < startLocation.z)
        {
            player1Score++;

        }
        else player2Score++;
    }

}

using UnityEngine;
using System.Collections.Generic;

struct Wave
{
    public Vector2 center;
    public float radius;
    public float strength;
}

public class Water : MonoBehaviour {

    public GameObject cube;

    private List<Wave> waves;

    private int width;
    private int height;

    private float direction;

	// Use this for initialization
	void Start () {
        waves = new List<Wave>();

        width = 256;
        height = 256;

	    for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(cube);
                go.transform.position = new Vector3(i, 0, j);
                go.tag = "Cube";
            }
        }
	}

    void Update()
    {
        float dt = Time.deltaTime * 2.0f;

        direction += dt;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Wave w = new Wave();
            w.center = new Vector2(width / 2 + Mathf.Cos(direction) * (width / 2), height / 2 + Mathf.Sin(direction) * (width / 2));
            w.strength = 10.0f;
            w.radius = 0.0f;
            waves.Add(w);
            Debug.Log("Wave Created");

            Wave w2 = new Wave();
            w2.center = new Vector2(width / 2 + Mathf.Cos(direction + Mathf.PI / 2) * (width / 2), height / 2 + Mathf.Sin(direction + Mathf.PI / 2) * (width / 2));
            w2.strength = 10.0f;
            w2.radius = 0.0f;
            waves.Add(w2);
            Debug.Log("Wave2 Created");
        }

        for(int i = 0; i < waves.Count; i++)
        {
            Wave w = waves[i];
            w.radius += dt;
            waves[i] = w;

            if(waves[i].radius > width+width)
            {
                Debug.Log("Wave Removed");
                waves.RemoveAt(i);
            }
        }

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        foreach(GameObject cube in cubes)
        {
            float y = 0;
            Vector3 pos = cube.transform.position;
            foreach (Wave w in waves)
            {
                Vector2 delta = w.center - new Vector2(pos.x, pos.z); //delta
                float distance = delta.magnitude;//distance squared
                float distanceToCircle = Mathf.Abs(distance - w.radius);
                if (distanceToCircle < 4)
                {
                    y += 4 - distanceToCircle;
                }
            }
            pos.y = y;
            cube.transform.position = pos;
        }

    }
}

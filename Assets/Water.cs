using UnityEngine;
using System.Collections.Generic;

struct Wave
{
    public Vector2 center;
    public float radius;
    public float strength;
}

public class Water : MonoBehaviour
{

    public Material mat;
    public float platformRadius;

    public int width; //squares on x axis
    public int height;

    public Vector2 areaSize;

    public float forcePerVertex;


    private Vector2 squareSize;
    private List<Wave> waves;
    private float[,] y;

    private float direction;

    private float resetTimer;
    private float resetTimerMax;
    private float platResetDist;

    public void SpawnWave(Vector2 position)
    {
        Wave w = new Wave();
        w.center = position;
        w.strength = 10.0f;
        w.radius = 0.0f;
        waves.Add(w);
    }

    public void Reset(float time)
    {
        resetTimer = time;
        resetTimerMax = time;
        GameObject plat = GameObject.FindGameObjectWithTag("Platform");
        platResetDist = plat.transform.position.y - 3;
    }

    // Use this for initialization
    void Start()
    {
        GameObject plat = GameObject.FindGameObjectWithTag("Platform");

        resetTimer = 0;

        squareSize = new Vector2(areaSize.x / width, areaSize.y / height);

        waves = new List<Wave>();

        y = new float[width,height];

        Vector3[] vertices = new Vector3[width * height * 4];

        int[] indices = new int[width * height * 6];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                y[i, j] = 0.0f;

                vertices[(i * width + j) * 4 + 0] = new Vector3((i - 0.5f)*squareSize.x, 0, (j - 0.5f) * squareSize.y);
                vertices[(i * width + j) * 4 + 1] = new Vector3((i - 0.5f)*squareSize.x, 0, (j + 0.5f) * squareSize.y);
                vertices[(i * width + j) * 4 + 2] = new Vector3((i + 0.5f)*squareSize.x, 0, (j + 0.5f) * squareSize.y);
                vertices[(i * width + j) * 4 + 3] = new Vector3((i + 0.5f)*squareSize.x, 0, (j - 0.5f) * squareSize.y);

                indices[i * width * 6 + j * 6 + 0] = (i * width + j) * 4 + 0;
                indices[i * width * 6 + j * 6 + 1] = (i * width + j) * 4 + 1;
                indices[i * width * 6 + j * 6 + 2] = (i * width + j) * 4 + 2;
                indices[i * width * 6 + j * 6 + 3] = (i * width + j) * 4 + 2;
                indices[i * width * 6 + j * 6 + 4] = (i * width + j) * 4 + 3;
                indices[i * width * 6 + j * 6 + 5] = (i * width + j) * 4 + 0;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "watermesh";
        mesh.vertices = vertices;
        mesh.triangles = indices;

        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = mat;

        plat.transform.position = new Vector3(areaSize.x / 2, 3, areaSize.y / 2);
        GameObject.Find("Sphere").transform.position = new Vector3(areaSize.x / 2, 10, areaSize.y / 2);
        GameObject.Find("Arena").transform.position = new Vector3(areaSize.x / 2, 0, areaSize.y / 2);
        //gameObject.AddComponent<MeshCollider>();

        //GameObject.FindGameObjectWithTag("Platform").GetComponent<Rigidbody>().
    }

    void FixedUpdate()
    {
        GameObject plat = GameObject.FindGameObjectWithTag("Platform");

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //check collisions
                Vector3 vertex = new Vector3(i * squareSize.x, y[i, j], j * squareSize.y);
                Vector3 n = plat.transform.up; //normal of the plane
                float d = Vector3.Dot((plat.transform.position - vertex), n) / Vector3.Dot(new Vector3(0, 1, 0), n);
                Vector3 collision = new Vector3(0, d, 0) + vertex;
                Vector3 radius = plat.transform.position - collision;
                if (d < 0 && radius.magnitude < platformRadius)
                {
                    plat.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(0, 1.1f, 0) * -d * forcePerVertex, collision);
                }
            }
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;
        GameObject plat = GameObject.FindGameObjectWithTag("Platform");

        if (resetTimer > 0)
        {
            resetTimer -= dt;

            plat.transform.position = new Vector3(areaSize.x / 2, platResetDist * (resetTimer / resetTimerMax) + 3, areaSize.y / 2);

            if(resetTimer < 0)
            {
                for(int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; ++j)
                    {
                        y[i,j] = 0;
                        }
                }
                waves.Clear();
                return;
            }
        }

        direction += dt;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float step = 3.14159265f * 2.0f / 5.0f / 10.0f;
            for(int i = 0; i < 5; i++)
            {
                SpawnWave(new Vector2(width / 2 + Mathf.Cos(direction + (step * i)) * (width / 2), height / 2 + Mathf.Sin(direction + (step * i)) * (width / 2)));
            }
        }

        for (int i = 0; i < waves.Count; i++)
        {
            Wave w = waves[i];
            w.radius += dt * 8;
            waves[i] = w;

            if (waves[i].radius > width + width)
            {
                waves.RemoveAt(i);
            }
        }

        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                
                if (resetTimer > 0)
                {
                    y[i, j] *= (1.0f - dt);
                }
                else
                {
                    y[i, j] = 0.0f;

                    foreach (Wave w in waves)
                    {
                        Vector2 delta = w.center - new Vector2(i * squareSize.x, j * squareSize.y); //delta
                        float distance = delta.magnitude;
                        float distanceToCircle = Mathf.Abs(w.radius - distance);
                        if (distanceToCircle < 4)
                        {
                            y[i, j] += 4 - distanceToCircle;
                        }
                    }
                }
            }
        }

        Vector3[] vertices = new Vector3[width * height];

        int[] indices = new int[(width - 1) * (height - 1) * 6];

        Vector2[] uvs = new Vector2[width * height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float y1 = y[i, j];

                //check collisions
                Vector3 vertex = new Vector3(i * squareSize.x, y[i, j], j * squareSize.y);
                Vector3 n = plat.transform.up; //normal of the plane
                float d = Vector3.Dot((plat.transform.position - vertex), n) / Vector3.Dot(new Vector3(0, 1, 0), n);
                Vector3 collision = new Vector3(0, d, 0) + vertex;
                Vector3 radius = plat.transform.position - collision;
                if (d < 0 && radius.magnitude < platformRadius)
                {
                    y1 = collision.y;
                    if(y1 < 0)
                    {
                        //y1 = 0.0f; //it's not a bug, it's a feature!
                    }
                }

                vertices[i * width + j] = new Vector3((i - 0.5f) * squareSize.x, y1, (j - 0.5f) * squareSize.y);

                uvs[i * width + j] = new Vector2((float)i / width, (float)j / height);
            }
        }
        for(int i = 0; i < width - 1; i++)
        {
            for(int j = 0; j < height - 1; j++)
            {
                indices[i * (width - 1) * 6 + j * 6 + 0] = (i * width + j);
                indices[i * (width - 1) * 6 + j * 6 + 1] = (i * width + j + 1);
                indices[i * (width - 1) * 6 + j * 6 + 2] = ((i + 1) * width + j + 1);
                indices[i * (width - 1) * 6 + j * 6 + 3] = ((i + 1) * width + j + 1);
                indices[i * (width - 1) * 6 + j * 6 + 4] = ((i + 1) * width + j);
                indices[i * (width - 1) * 6 + j * 6 + 5] = (i * width + j);
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "watermesh";
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.uv = uvs;

        gameObject.GetComponent<MeshFilter>().mesh = mesh;

        //gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}

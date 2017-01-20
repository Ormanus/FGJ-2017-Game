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

    private List<Wave> waves;

    private float[] grid;

    private int width;
    private int height;

    private float direction;

    // Use this for initialization
    void Start()
    {
        waves = new List<Wave>();

        width =  122;
        height = 122;

        //grid = new float[width * height];


        Vector3[] vertices = new Vector3[width * height * 4];

        int[] indices = new int[width * height * 6];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                vertices[(i * width + j) * 4 + 0] = new Vector3(i - 0.5f, 0, j - 0.5f);
                vertices[(i * width + j) * 4 + 1] = new Vector3(i - 0.5f, 0, j + 0.5f);
                vertices[(i * width + j) * 4 + 2] = new Vector3(i + 0.5f, 0, j + 0.5f);
                vertices[(i * width + j) * 4 + 3] = new Vector3(i + 0.5f, 0, j - 0.5f);

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
        gameObject.AddComponent<MeshCollider>();
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

        for (int i = 0; i < waves.Count; i++)
        {
            Wave w = waves[i];
            w.radius += dt * 4;
            waves[i] = w;

            if (waves[i].radius > width + width)
            {
                Debug.Log("Wave Removed");
                waves.RemoveAt(i);
            }
        }

        float[] y = new float[width * height];

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                foreach (Wave w in waves)
                {
                    Vector2 delta = w.center - new Vector2(i, j); //delta
                    float distance = delta.magnitude;//distance squared
                    float distanceToCircle = Mathf.Abs(distance - w.radius);
                    if (distanceToCircle < 4)
                    {
                        y[i * height + j] += 4 - distanceToCircle;
                    }
                }
            }
        }

        //GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        //foreach (GameObject cube in cubes)
        //{
        //    Vector3 pos = cube.transform.position;
        //    foreach (Wave w in waves)
        //    {
        //        Vector2 delta = w.center - new Vector2(pos.x, pos.z); //delta
        //        float distance = delta.magnitude;//distance squared
        //        float distanceToCircle = Mathf.Abs(distance - w.radius);
        //        if (distanceToCircle < 4)
        //        {
        //            y += 4 - distanceToCircle;
        //        }
        //    }
        //    pos.y = y;
        //    cube.transform.position = pos;
        //}


        Vector3[] vertices = new Vector3[width * height];

        int[] indices = new int[(width - 1) * (height - 1) * 6];

        Vector2[] uvs = new Vector2[width * height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                vertices[i * width + j] = new Vector3(i - 0.5f, y[i * height + j], j - 0.5f);

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

        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}

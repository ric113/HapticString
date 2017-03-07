using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCurvedEffect : MonoBehaviour {

    public GameObject sphere;
    public float button_length = 1, button_height = 1;
    public float angle_horizontal = 0, angle_vertical = 90;

    GameObject pivot_point;
    float sphere_radius;
    Renderer renderer;


    private void Start()
    {
        renderer = transform.GetComponent<Renderer>();
    }

    // Use this for initialization
    public void setButton () {
        //create the pivot point (for rotate the button horizontally)
        pivot_point = new GameObject();
        pivot_point.transform.position = sphere.transform.position;

        //Attach the Plane on the Sphere
        Mesh newmesh = new Mesh();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] newvertices;
        newvertices = new Vector3[vertices.Length];
        int vertex_eachside = (int)Mathf.Sqrt(vertices.Length);
        sphere_radius = sphere.transform.localScale.x / 2.0f - 0.003f;

        Vector3 center = calculatepoint(angle_vertical, 0, sphere_radius);
        Vector2 ori_angles = calculateangle(-0.5f * button_length, 0.5f * button_height, angle_vertical, 0f, sphere_radius);

        for (int i = 0; i < vertex_eachside; i++)
        {
            for(int j = 0 ; j < vertex_eachside ; j++)
            {
                float vertangles = ori_angles.x - (float)i / vertex_eachside * 2.0f * Mathf.Abs(ori_angles.x - angle_vertical);
                float horiangles = ori_angles.y + (float)j / vertex_eachside * 2.0f * Mathf.Abs(ori_angles.y);
                Vector3 position = calculatepoint(vertangles, horiangles, sphere_radius);
                
                newvertices[i * vertex_eachside + j].x = position.x;
                newvertices[i * vertex_eachside + j].y = position.y;
                newvertices[i * vertex_eachside + j].z = position.z;
            }
        }

        mesh.vertices = newvertices;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        mesh.RecalculateBounds();
        this.transform.parent = pivot_point.transform;
        pivot_point.transform.parent = sphere.transform;
        this.GetComponent<MeshRenderer>().enabled = true;
        pivot_point.transform.Rotate(Vector3.up, angle_horizontal, Space.World);
	}

    //calculate the position with the angle
    Vector3 calculatepoint(float vertical,float horizontal,float radius)
    {
        float angle_theta = 90.0f - vertical;
        float angle_phi = horizontal;
        Vector3 center = new Vector3(0, 0, 0);
        Vector3 sphere_center = sphere.transform.position;

        center.x = sphere_center.x + radius * Mathf.Sin(angle_theta * Mathf.Deg2Rad) * Mathf.Cos(angle_phi * Mathf.Deg2Rad);
        center.z = sphere_center.z + radius * Mathf.Sin(angle_theta * Mathf.Deg2Rad) * Mathf.Sin(angle_phi * Mathf.Deg2Rad);
        center.y = sphere_center.y + radius * Mathf.Cos(angle_theta * Mathf.Deg2Rad);
        return center;
    }

    //calculate the corresponding angle with the relative position (to center)
    //x : vertical angle, y : horizontal angle
    Vector2 calculateangle(float length,float height,float vertical,float horizontal,float radius)
    {
        Vector2 result_angle = new Vector2(0, 0);

        //vertical angle
        float circle_perimeter = 2.0f * Mathf.PI * radius;
        result_angle.x = vertical + height / circle_perimeter;

        //horizontal angle
        float subradius = radius * Mathf.Sin((90 - result_angle.x) * Mathf.Deg2Rad);
        float subperimeter = 2.0f * Mathf.PI * subradius;
        result_angle.y = horizontal + length / subperimeter;

        return result_angle;
    }

    void OnCollisionEnter(Collision collision)
    {
        print("collide!");
        renderer.material.SetColor("_EmissionColor", Color.red);
    }

    void OnCollisionExit(Collision collision)
    {
        print("collide!");
        renderer.material.SetColor("_EmissionColor", Color.white);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))] // make sure components exsist
public class SurfaceCreator : MonoBehaviour
{
    private Mesh mesh;
    private int currentResolution;

    [Range(1, 200)]
	public int resolution = 10;

    

    private void OnEnable() { //make sure mesh exsists or make a new one
        if (mesh == null) {
            mesh = new Mesh();
            mesh.name = "Surface Mesh";
            GetComponent<MeshFilter>().mesh = mesh;
        }
        Refresh();
    }
    
   
    public void Refresh() {

        if (resolution != currentResolution) {
			CreateGrid();
		}
        
    }

    private void CreateGrid(){
        //set resolution. //more res = more quads.
        currentResolution = resolution;
        mesh.Clear(); // clear to reset array sizes.
        //if n^2 is a grid of quads, with n being resolution. then we need. (n + 1)^2  vertices.
        //vertices are on the edge of a quad. so offset entire quad by 1/2 to center it
        Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        
        //shader stuff
        Vector3[] normals = new Vector3[vertices.Length]; //normal vectors
        Vector2[] uv = new Vector2[vertices.Length]; //UV coords for shader
        Color[] colors = new Color[vertices.Length];

		float stepSize = 1f / resolution;
        for (int v = 0, y = 0; y <= resolution; y++) {
			for (int x = 0; x <= resolution; x++, v++) {
				vertices[v] = new Vector3(x * stepSize - 0.5f, y * stepSize - 0.5f);
                
                //shaders
                normals[v] = Vector3.back;
                uv[v] = new Vector2(x * stepSize, y * stepSize);
                colors[v] = Color.black;
			}
		}
        //2 triangles for a square. 3 vertices for a triangle. = 6n^2 per quad
        int[] triangles = new int[resolution * resolution * 6];
        //loop per quad.
        //but think of it as points of a triangle.
		for (int t = 0, v = 0, y = 0; y < resolution; y++, v++) {
			for (int x = 0; x < resolution; x++, v++, t += 6) {
				triangles[t] = v;
				triangles[t + 1] = v + resolution + 1;
                //2 and 3;   are 0 and 1 of the next row. so n + 1.
				triangles[t + 2] = v + 1; 
				triangles[t + 3] = v + 1;
				triangles[t + 4] = v + resolution + 1;
				triangles[t + 5] = v + resolution + 2;
			}
		}

        //OLD used an array to set it manually.
        //lines (VERTICES)
        mesh.vertices = vertices;  
        //shader stuff
        mesh.uv = uv;
        mesh.normals = normals;
        mesh.colors = colors; //vertext colours have to be used by a shader
        //TRIANGLES/QUADS using the lines
		mesh.triangles = triangles;
    }
}

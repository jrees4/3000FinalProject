using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))] // make sure components exsist
public class SurfaceCreator : MonoBehaviour
{
    //vars 
    private Mesh mesh;
    private int currentResolution;
    //for transforming.
    public Vector3 offset; 
    public Vector3 rotation;
    //(mostly for create grid function)
    private Vector3[] vertices;
	private Vector3[] normals;
	private Color[] colors;

    


    [Range(1, 200)]
	public int resolution = 10;

    public float frequency = 1f;
	
	[Range(1, 8)]
	public int octaves = 1;
	
	[Range(1f, 4f)]
	public float lacunarity = 2f;
	
	[Range(0f, 1f)]
	public float persistence = 0.5f;
	
	[Range(1, 3)]
	public int dimensions = 3;
	
	public NoiseMethodType type;
	
	public Gradient coloring;

    //public MeshCollider collider;
    public GameObject SurfaceThing;

    

    private void OnEnable() { //make sure mesh exsists or make a new one
        if (mesh == null) {
            mesh = new Mesh();
            mesh.name = "Surface Mesh";
            GetComponent<MeshFilter>().mesh = mesh;

            //mesh collider stuff that works in a weird way.
            DestroyImmediate(SurfaceThing.GetComponent<MeshCollider>());
            Debug.Log("Deleted.");
            SurfaceThing.AddComponent<MeshCollider>().sharedMesh = mesh;

            /*
              Doesnt work. HAD TO BE DONE IN REFRESH.  AND IT HAS TO BE DELETED THEN REMADE. unity reports it as a bug.
            DestroyImmediate(SurfaceThing.GetComponent<MeshCollider>());
            SurfaceThing.AddComponent<MeshCollider>().sharedMesh = mesh;
            Debug.Log("Deleted.");

            var collider = SurfaceThing.GetComponent<MeshCollider>();
            collider.enabled = false;
            collider.enabled = true;    

             var mc = SurfaceThing.GetComponent<MeshCollider>();
             if(mc==null)
                 mc = (MeshCollider) SurfaceThing.AddComponent (typeof(MeshCollider));
             else
             {
                 mc.sharedMesh = null;
                 mc.sharedMesh = mesh;
             }*/
        }
        Refresh();
    }
    
   
    
    public void Refresh() {   // This mimics the filltexture function in  TextureCreator.

        if (resolution != currentResolution) {
			CreateGrid();
		}
        
        Quaternion q = Quaternion.Euler(rotation); //two direct lines in a 3d space. or division of two numbers OF two vectors.
        Vector3 point00 = q * new Vector3(-0.5f,-0.5f)+ offset;
		Vector3 point10 = q * new Vector3( 0.5f,-0.5f)+ offset;
		Vector3 point01 = q * new Vector3(-0.5f, 0.5f)+ offset;
		Vector3 point11 = q * new Vector3( 0.5f, 0.5f)+ offset;

        NoiseMethod method = Noise.noiseMethods[(int)type][dimensions - 1]; //call method in noise class
        float stepSize = 1f / resolution;
        for (int v = 0, y = 0; y <= resolution; y++) {
            Vector3 point0 = Vector3.Lerp(point00, point01, y * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, y * stepSize);
            for (int x = 0; x <= resolution; x++, v++) {
                Vector3 point = Vector3.Lerp(point0, point1, x * stepSize);
                float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);
                /* if (type != NoiseMethodType.Value) {
                    sample = sample * 0.5f + 0.5f;
                } */
                sample = type == NoiseMethodType.Value ? (sample - 0.5f) : (sample * 0.5f);
                vertices[v].y = sample; //actually moving the vertices alone the z axis
                colors[v] = coloring.Evaluate(sample + 0.5f);
            }
        }
        mesh.vertices = vertices;
        mesh.colors = colors;

        //mesh collider stuff that works in a weird way.
            DestroyImmediate(SurfaceThing.GetComponent<MeshCollider>());
            Debug.Log("Deleted.");
            SurfaceThing.AddComponent<MeshCollider>().sharedMesh = mesh;
        
    }

    
    private void CreateGrid(){
        //set resolution. //more res = more quads.
        currentResolution = resolution;
        mesh.Clear(); // clear to reset array sizes.
        //if n^2 is a grid of quads, with n being resolution. then we need. (n + 1)^2  vertices.
        //vertices are on the edge of a quad. so offset entire quad by 1/2 to center it
        vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        
        //shader stuff
        colors = new Color[vertices.Length];
        normals = new Vector3[vertices.Length]; //normal vectors
        Vector2[] uv = new Vector2[vertices.Length]; //UV coords for shader
        

		float stepSize = 1f / resolution;
        for (int v = 0, z = 0; z <= resolution; z++) {
			for (int x = 0; x <= resolution; x++, v++) {
				vertices[v] = new Vector3(x * stepSize - 0.5f, 0f ,z * stepSize - 0.5f);
                
                //shaders
                colors[v] = Color.black;
                normals[v] = Vector3.up; //old = .back
                uv[v] = new Vector2(x * stepSize, z * stepSize);
                
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

        //collider.sharedMesh = mesh;
        

    }
}

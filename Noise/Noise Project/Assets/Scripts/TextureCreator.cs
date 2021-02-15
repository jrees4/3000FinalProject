using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCreator : MonoBehaviour
{
    [Range (2, 512)]
    public int resolution = 256;  //size. 16 * 16

    public float frequency = 1f; //for Noise.cs

    [Range (1,8)]
    public int octaves = 1; //samples
    
    [Range(1, 3)]
	public int dimensions = 3; // 1D, 2D, 3D

    [Range (1f, 4f)]
    public float lacunarity =2f; // frequency changes

    [Range (0f, 1f)]
    public float persistence = 0.5f; //amplitude or gain.

    public NoiseMethodType type; //noise type

    public Gradient colouring;  //colours
    
    //vars
    private Texture2D texture;
    
    // AWAKE is called when the instance is being loaded. (before app starts)
    void Awake(){
        
    }

    // On Enable called when the component is activated.
    void OnEnable() {
        if (texture == null) {
        texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
        texture.name = "Procedural Texture";
        texture.wrapMode = TextureWrapMode.Clamp; // Stops edges being fucky
        texture.filterMode = FilterMode.Trilinear; //Point filtering instead of defeault (bilinear) OR Point OR Trilinear
        texture.anisoLevel = 9;
        GetComponent<MeshRenderer>().material.mainTexture = texture;
        }
        FillTexture();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //make sure its up to date while transforming,  this allows to changing the shape during play.
		if (transform.hasChanged) {
			transform.hasChanged = false;
			FillTexture();
		}
	
    }

    public void FillTexture() {
        if (texture.width != resolution){
            texture.Resize(resolution, resolution);
        }

        //Four corners.  pointX,Y
        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f,-0.5f));
		Vector3 point10 = transform.TransformPoint(new Vector3( 0.5f,-0.5f));
		Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
		Vector3 point11 = transform.TransformPoint(new Vector3( 0.5f, 0.5f));

        //Random.seed = 42; //Seed for the random, so its not too different each time. (JUST FOR TESTING ATM);
        NoiseMethod method = Noise.noiseMethods[(int)type][dimensions - 1]; //use selected dimension  + noise type
        float stepSize = 1f / resolution;
        for (int y = 0; y < resolution; y++){
            // Interpolate (insert) points between points.  The .lerp function does this.
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize); //left
            //Debug.Log("Rotation: " + y + " Point 0: -- " + point0);
			Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize); //right
            //Debug.Log("Rotation: " + y + "Point 1: -- " + point1);
            for (int x = 0; x < resolution; x++){ //for
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize); // point between left and right.
                // OLD float sample = method(point, frequency);
                float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence); //pass selected method into the sum method in the noise class. + the number of samples needed (octaves)
				if (type != NoiseMethodType.Value) { //at this point, maybe theres a way to do this without this many loops
					sample = sample * 0.5f + 0.5f;
				}

                //Debug.Log("Rotation: " + y + "version:  " + x + "Point : -- " + point);
                texture.SetPixel(x , y , colouring.Evaluate(sample));  //OLD: Sets pixel colour for each point using noise.method   dimension.
                //old white * random.value
                //OLD: new Color(point.x, point.y, point.z)  --   OLD OLD: ((x + 0.5f) * stepSize % 0.1f, (y + 0.5f) * stepSize % 0.1f, 0f) * 10f )
            }
        }
        texture.Apply();
    }

    
}

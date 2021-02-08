using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCreator : MonoBehaviour
{
    [Range (2, 512)]
    public int resolution = 256;  //size. 16 * 16
    private Texture2D texture;
    
    // AWAKE is called when the instance is being loaded. (before app starts)
    void Awake(){
        
    }

    // On Enable called when the component is activated.
    void OnEnable() {
        texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
        texture.name = "Procedural Texture";
        texture.wrapMode = TextureWrapMode.Clamp; // Stops edges being fucky
        texture.filterMode = FilterMode.Point; //Point filtering instead of defeault (bilinear)
        GetComponent<MeshRenderer>().material.mainTexture = texture;
        FillTexture();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillTexture() {
        if (texture.width != resolution){
            texture.Resize(resolution, resolution);
        }
        float stepSize = 1f / resolution;
        for (int y = 0; y < resolution; y++){
            for (int x = 0; x < resolution; x++){
                texture.SetPixel(x , y , new Color((x + 0.5f) * stepSize, (y + 0.5f) * stepSize, 0f));
            }
        }
        texture.Apply();
    }
}

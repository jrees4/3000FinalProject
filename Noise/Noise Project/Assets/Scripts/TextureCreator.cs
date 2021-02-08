using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCreator : MonoBehaviour
{
    
    public int resolution = 256;  //size. 16 * 16
    private Texture2D texture;
    
    // AWAKE is called when the instance is being loaded. (before app starts)
    void Awake(){
        texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
        texture.name = "Procedural Texture";
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

    private void FillTexture() {
        for (int y = 0; y < resolution; y++){
            for (int x = 0; x < resolution; x++){
                texture.SetPixel(x , y , Color.red);
            }
        }
        texture.Apply();
    }
}

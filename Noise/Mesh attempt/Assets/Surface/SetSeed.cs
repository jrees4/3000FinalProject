using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSeed : MonoBehaviour
{

    public string stringSeed = "Seed Here";
    public bool useStringSeed;
    public int seed;
    public bool randomizeSeed;

    //x postition
    public float x = 0;
    //y
    public float y = 0;
    //z
    public float z = 0;
    //Seed.
    public int RSeed;

    public Vector3 XPos;
   

    void Awake(){
        if (useStringSeed) {
            seed = stringSeed.GetHashCode();
        }

        if (randomizeSeed) {
            seed = Random.Range(0 , 99999);
        }

        Random.InitState(seed);

        //XPos = Noise.offset ;

        //XPos = GameObject.Find("Surface").GetComponent<SurfaceCreator>().offset;

        x = Random.Range(0, 100);
        y = Random.Range(0, 100);
        z = Random.Range(0, 100);

        XPos = new Vector3(x,y,z);

        GameObject.Find("Surface").GetComponent<SurfaceCreator>().offset = XPos;

        stringSeed = ("" + x + "" + y + "" + z);
        // Generates a random number within a range.      
        RSeed = Random.Range(0, 100);  
        //Logs output
        Debug.Log("Random number: "   + RSeed);
        Debug.Log("offset : " + XPos);
        Debug.Log(stringSeed);
    } 
}

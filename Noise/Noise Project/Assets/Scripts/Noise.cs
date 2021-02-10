using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static int[] hash = {  //premuatation array as a hashing method.
		0, 1, 2, 3, 4, 5, 6, 7
	};

    public static float Value (Vector3 point, float frequency) {  //give 3d space point, return random value.
        point *= frequency ;
        int i = Mathf.FloorToInt(point.x); //OLD discard fractional. cast int.
        i &= 7; //limit i to array index
		return hash[i] / 7f; //scaled result from hash.    //i & 2; //bitwise AND operator. logic: 101 - 010 = 0      || OLD return i % 2  when odd?, only right side shows.
	}
}

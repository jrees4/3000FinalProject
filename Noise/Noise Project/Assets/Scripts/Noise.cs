using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float NoiseMethod (Vector3 point, float frequency); //delegate for noise methods

public enum NoiseMethodType {
	Value,
	Perlin
}

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

    private static int[] hash = {  //premuatation array as a hashing method.    256
		151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
		140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
		247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
		 57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
		 74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
		 60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
		 65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
		200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
		 52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
		207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
		119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
		129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
		218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
		 81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
		184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
		222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180,

		151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
		140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
		247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
		 57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
		 74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
		 60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
		 65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
		200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
		 52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
		207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
		119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
		129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
		218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
		 81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
		184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
		222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
	};

    private const int hashMask = 255;

	//maths for a divide by √1/2
	private static float sqr2 = Mathf.Sqrt(2f);

    // VALUE NOISE
    public static float Value1D (Vector3 point, float frequency) {  //give 3d space point, return random value.
        point *= frequency ;
        int i0 = Mathf.FloorToInt(point.x); //store cord. //OLD: discard fractional. cast int.
		float t = point.x - i0; //the distance from point to point FOR interpolation //value noise
        i0 &= hashMask; //limit i to array index
		int i1 = i0 + 1; //point next to the poit we want

		int h0 = hash[i0];
		int h1 = hash[i1];
		t = Smooth(t); //smooth those blurred lines
		return Mathf.Lerp(h0, h1, t) * (1f/ hashMask); // use lerp to find point between. // hash[i]  //scaled result from hash.    //i & 2; //bitwise AND operator. logic: 101 - 010 = 0      || OLD return i % 2  when odd?, only right side shows.
	}

    public static float Value2D (Vector3 point, float frequency) {
		point *= frequency;
		int ix0 = Mathf.FloorToInt(point.x); //x cord
        int iy0 = Mathf.FloorToInt(point.y); // y cord
		float tx = point.x - ix0; //interpolation points
		float ty = point.y - iy0; // ( the distance between points). this isnt a set value cause transformable object scaling
		ix0 &= hashMask;
        iy0 &= hashMask;
		int ix1 = ix0 + 1;
		int iy1 = iy0 + 1;

		//pre hash
		int h0 = hash[ix0];
		int h1 = hash[ix1];
		int h00 = hash[h0 + iy0]; 
		int h10 = hash[h1 + iy0];
		int h01 = hash[h0 + iy1];
		int h11 = hash[h1 + iy1];

		tx = Smooth(tx); //smoothing~
		ty = Smooth(ty);
		
		return Mathf.Lerp(
			Mathf.Lerp(h00, h10, tx),
			Mathf.Lerp(h01, h11, tx),
			ty) * (1f / hashMask); // hash[(hash[ix] + iy) & hashMask] // hash both cords. convert max range
	}

	public static float Value3D (Vector3 point, float frequency) {
		point *= frequency;
		int ix0 = Mathf.FloorToInt(point.x); //x cord
        int iy0 = Mathf.FloorToInt(point.y); // y cord
		int iz0 =Mathf.FloorToInt(point.z); // z cord
		float tx = point.x - ix0;
		float ty = point.y - iy0;
		float tz = point.z - iz0;
		ix0 &= hashMask;
        iy0 &= hashMask;
		iz0 &= hashMask;
		int ix1 = ix0 + 1;
		int iy1 = iy0 + 1;
		int iz1 = iz0 + 1;

		//pre baked hashbrowns. more dimensions scale ridiculously
		int h0 = hash[ix0];
		int h1 = hash[ix1];
		int h00 = hash[h0 + iy0];
		int h10 = hash[h1 + iy0];
		int h01 = hash[h0 + iy1];
		int h11 = hash[h1 + iy1];
		int h000 = hash[h00 + iz0];
		int h100 = hash[h10 + iz0];
		int h010 = hash[h01 + iz0];
		int h110 = hash[h11 + iz0];
		int h001 = hash[h00 + iz1];
		int h101 = hash[h10 + iz1];
		int h011 = hash[h01 + iz1];
		int h111 = hash[h11 + iz1];

		//smoothing distance
		tx = Smooth(tx);
		ty = Smooth(ty);
		tz = Smooth(tz);
		return  Mathf.Lerp(
			Mathf.Lerp(Mathf.Lerp(h000, h100, tx), Mathf.Lerp(h010, h110, tx), ty),
			Mathf.Lerp(Mathf.Lerp(h001, h101, tx), Mathf.Lerp(h011, h111, tx), ty),
			tz) * (1f / hashMask); //NO HASHING NEEDED. Already done//hash[(hash[(hash[ix] + iy) & hashMask] + iz) & hashMask] // hash all cords. convert max range  
	}

	public static NoiseMethod[] valueMethods = {  //contain reference to valuemethods
		Value1D,
		Value2D,
		Value3D
	};

	//PERLINE NOISE
	public static NoiseMethod[] perlinMethods = {
		Perlin1D,
		Perlin2D,
		Perlin3D
	};

	public static NoiseMethod[][] noiseMethods = {
		valueMethods,
		perlinMethods
	};

	public static float Perlin1D (Vector3 point, float frequency) {  
        point *= frequency ;
        int i0 = Mathf.FloorToInt(point.x); //store point as x coordinate
		
		//the distance from point to point FOR interpolation //gradient
		float t0 = point.x - i0; 
		float t1 = t0 - 1f;
        i0 &= hashMask; //limit
		int i1 = i0 + 1; 

		//convert to gradient functions so length isnt an isssue
		float g0 = gradients1D[hash[i0] & gradientsMask1D];
		float g1 = gradients1D[hash[i1] & gradientsMask1D];

		//values at current point
		float v0 = g0 * t0;
		float v1 = g1 * t1;


		float t = Smooth(t0);
		return Mathf.Lerp(v0, v1, t) * 2f; //lerp for space between //double for darker darks and whiter whites
	}

    public static float Perlin2D (Vector3 point, float frequency) {
		point *= frequency;
		//cords
		int ix0 = Mathf.FloorToInt(point.x); 
        int iy0 = Mathf.FloorToInt(point.y); 
		//interpolation 
		float tx0 = point.x - ix0; 
		float ty0 = point.y - iy0; 
		float tx1 = tx0 - 1f;
		float ty1 = ty0 - 1f;
		//
		ix0 &= hashMask;
        iy0 &= hashMask;
		int ix1 = ix0 + 1;
		int iy1 = iy0 + 1;

		//hashing
		int h0 = hash[ix0];
		int h1 = hash[ix1];
		Vector2 g00 = gradients2D[hash[h0 + iy0] & gradientsMask2D];
		Vector2 g10 = gradients2D[hash[h1 + iy0] & gradientsMask2D];
		Vector2 g01 = gradients2D[hash[h0 + iy1] & gradientsMask2D];
		Vector2 g11 = gradients2D[hash[h1 + iy1] & gradientsMask2D];

		//find angles
		float v00 = Dot(g00, tx0, ty0);
		float v10 = Dot(g10, tx1, ty0);
		float v01 = Dot(g01, tx0, ty1);
		float v11 = Dot(g11, tx1, ty1);

		//smoothing
		float tx = Smooth(tx0);
		float ty = Smooth(ty0);
		
		return Mathf.Lerp( //(start, end, distance)
			Mathf.Lerp(v00, v10, tx),
			Mathf.Lerp(v01, v11, tx),
			ty) * sqr2;  //square root of 2 is basically 1/2
	}

	public static float Perlin3D (Vector3 point, float frequency) {
		point *= frequency;
		//cords
		int ix0 = Mathf.FloorToInt(point.x);
		int iy0 = Mathf.FloorToInt(point.y);
		int iz0 = Mathf.FloorToInt(point.z);
		//points between points
		float tx0 = point.x - ix0;
		float ty0 = point.y - iy0;
		float tz0 = point.z - iz0;
		float tx1 = tx0 - 1f;
		float ty1 = ty0 - 1f;
		float tz1 = tz0 - 1f;
		ix0 &= hashMask; //this has to be done after that to apply properly
		iy0 &= hashMask;
		iz0 &= hashMask;
		int ix1 = ix0 + 1;
		int iy1 = iy0 + 1;
		int iz1 = iz0 + 1;
		//hashing
		int h0 = hash[ix0];
		int h1 = hash[ix1];
		int h00 = hash[h0 + iy0];
		int h10 = hash[h1 + iy0];
		int h01 = hash[h0 + iy1];
		int h11 = hash[h1 + iy1];
		Vector3 g000 = gradients3D[hash[h00 + iz0] & gradientsMask3D];
		Vector3 g100 = gradients3D[hash[h10 + iz0] & gradientsMask3D];
		Vector3 g010 = gradients3D[hash[h01 + iz0] & gradientsMask3D];
		Vector3 g110 = gradients3D[hash[h11 + iz0] & gradientsMask3D];
		Vector3 g001 = gradients3D[hash[h00 + iz1] & gradientsMask3D];
		Vector3 g101 = gradients3D[hash[h10 + iz1] & gradientsMask3D];
		Vector3 g011 = gradients3D[hash[h01 + iz1] & gradientsMask3D];
		Vector3 g111 = gradients3D[hash[h11 + iz1] & gradientsMask3D];
		//maths
		float v000 = Dot(g000, tx0, ty0, tz0);
		float v100 = Dot(g100, tx1, ty0, tz0);
		float v010 = Dot(g010, tx0, ty1, tz0);
		float v110 = Dot(g110, tx1, ty1, tz0);
		float v001 = Dot(g001, tx0, ty0, tz1);
		float v101 = Dot(g101, tx1, ty0, tz1);
		float v011 = Dot(g011, tx0, ty1, tz1);
		float v111 = Dot(g111, tx1, ty1, tz1);
		float tx = Smooth(tx0);
		float ty = Smooth(ty0);
		float tz = Smooth(tz0);

		return Mathf.Lerp(
			Mathf.Lerp(Mathf.Lerp(v000, v100, tx), Mathf.Lerp(v010, v110, tx), ty),
			Mathf.Lerp(Mathf.Lerp(v001, v101, tx), Mathf.Lerp(v011, v111, tx), ty),
			tz);
	}

	//--- GRADIENT METHODS---
	private static float[] gradients1D = { //two possible directions/movement  odds and evens
		1f, -1f
	};
	private const int gradientsMask1D = 1;

	private static Vector2[] gradients2D = { //4 directions. up down left right //OLD diagonal horizontal and vertical alignment
		//new normalized diagonal
		new Vector2( 1f, 0f),
		new Vector2(-1f, 0f),
		new Vector2( 0f, 1f),
		new Vector2( 0f,-1f),
		new Vector2( 1f, 1f).normalized,
		new Vector2(-1f, 1f).normalized,
		new Vector2( 1f,-1f).normalized,
		new Vector2(-1f,-1f).normalized
	};
	private const int gradientsMask2D = 3;

	private static Vector3[] gradients3D = {//12 pointing to cube center
		new Vector3( 1f, 1f, 0f),
		new Vector3(-1f, 1f, 0f),
		new Vector3( 1f,-1f, 0f),
		new Vector3(-1f,-1f, 0f),
		new Vector3( 1f, 0f, 1f),
		new Vector3(-1f, 0f, 1f),
		new Vector3( 1f, 0f,-1f),
		new Vector3(-1f, 0f,-1f),
		new Vector3( 0f, 1f, 1f),
		new Vector3( 0f,-1f, 1f),
		new Vector3( 0f, 1f,-1f),
		new Vector3( 0f,-1f,-1f),
		
		new Vector3( 1f, 1f, 0f),
		new Vector3(-1f, 1f, 0f),
		new Vector3( 0f,-1f, 1f),
		new Vector3( 0f,-1f,-1f)
	};
	private const int gradientsMask3D = 15;

	// Math functions
	private static float Dot (Vector2 g, float x, float y) { //dot / scalar product.  operation between two vectors.  more maths. tldr: angle between them.  for 2d
		return g.x * x + g.y * y;
	}

	private static float Dot (Vector3 g, float x, float y, float z) { //for 3d
		return g.x * x + g.y * y + g.z * z;
	}

	private static float Smooth (float t) {
		return t * t * t * (t * (t * 6f - 15f) + 10f); //0s on each end.   Maths.
	}
}

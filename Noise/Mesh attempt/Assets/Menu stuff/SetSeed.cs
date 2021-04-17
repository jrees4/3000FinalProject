using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SetSeed : MonoBehaviour
{

    public TextMeshProUGUI text;


    public string stringSeed = "Seed Here";
    public bool useStringSeed;
    //public int seed;
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
   
    private string allPositions;
    private string[] Result;

    void Awake(){
        /* if (useStringSeed) {
            seed = stringSeed.GetHashCode();
        }

        if (randomizeSeed) {
            seed = Random.Range(0 , 99999);
        } */

        //Random.InitState(seed);

        //XPos = Noise.offset ;

        //XPos = GameObject.Find("Surface").GetComponent<SurfaceCreator>().offset;

        x = Random.Range(0, 100);
        y = Random.Range(0, 100);
        z = Random.Range(0, 100);

        XPos = new Vector3(x,y,z);

        GameObject.Find("Surface").GetComponent<SurfaceCreator>().offset = XPos;

        stringSeed = (  x.ToString()  + y.ToString()  + z.ToString());
        // Generates a random number within a range.      
        // RSeed = Random.Range(0, 100);  
        //Logs output
        //Debug.Log("Random number: "   + RSeed);
        Debug.Log("offset : " + XPos);
        Debug.Log(stringSeed);

        //allPositions = stringSeed;
        //string[] code = allPositions.Split('-');

        Result = StringExtensions.CanSplit(stringSeed,3);
        /* if (Result == !null){
            x = Result[0];
            y = Result[1];
            z = Result[2];

              XPos = new Vector3(x,y,z);

              GameObject.Find("Surface").GetComponent<SurfaceCreator>().offset = XPos;
        } */
         
        Debug.Log(Result[0] + Result[1] + Result[2]);

        //Debug.Log(code[0] + code[1] + code[2]);
        text.text = stringSeed;
    } 

    void FixedUpdate() {
        text.text = stringSeed;
    }

    public void SeedInputBox (string seed){
        Debug.Log(seed);

        /* if (seed.Length >= 9){
            seed = "11-11-11";
            text.text = seed;
        } */

         // custom length  3
        foreach (var stringChunk in seed.SplitEveryN(3))
        {
            Debug.Log("String chunk: " + stringChunk);
        } 

        //Debug.Log(seed);
    }

    /* static class StringExtensions {

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength) {
        
            if (s == null){
            throw new ArgumentNullException(nameof(s));
            }
            if (partLength <= 0){
            throw new ArgumentException("Part length has to be positive.", nameof(partLength));
            }
            for (var i = 0; i < s.Length; i += partLength) {
            yield return s.Substring(i, Math.Min(partLength, s.Length - i));
            }
        }

        string parts = "(x + y + z)".SplitInParts(3);
        Debug.Log(String.Join("" , parts));
    } */

        /* function Split(text : String, charCount : int) : String[]
    {
        if (text.length == 0)
            return new String[0];
        var arrayLength = (text.length-1) / charCount +1;
        var result = new String[arrayLength];
        for(var i = 0; i < arrayLength; i++)
        {
            var tmp = "";
            for (var n = 0; n < charCount; n++)
            {
                var index = i*charCount+n;
                if (index >= text.length)   //important if last "part" is smaller
                    break;
                tmp += text[index];
            }
            result[i] = tmp;
        }
        return result;
    } */


    

     

}

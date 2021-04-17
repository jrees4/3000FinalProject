using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 

public static class StringExtensions 
{
    public static List<string> SplitEveryN(this string str, int n = 3)
     {
         List<string> ret = new List<string>();
 
         int chunkIterator = 0;
         while (chunkIterator < str.Length)
         {
             int currentChunkSize = Mathf.Min(n, str.Length - chunkIterator);
             ret.Add(str.Substring(chunkIterator, currentChunkSize));
             // Debug.Log(str.Substring(chunkIterator, currentChunkSize));
             chunkIterator += currentChunkSize;
         }
         return ret;
     }

     public static string[] CanSplit(string str, int n)  
    {  
        //The string
        if (str == null){
        str = "aaaabbbbcccc"; 
        } 
        
        //Stores the length of the string  
        int len = str.Length;  
                      
        //n determines the variable that divide the string in 'n' equal parts  
        if (n == 0) {
            n = 3;  
        }
        
        int temp = 0, chars = len/n;  
        
        //Stores the array of string  
        String[] equalStr = new String [n];   
        
        //Check whether a string can be divided into n equal parts  
        if(len % n != 0) {  
            Debug.Log("Sorry this string cannot be divided into "+ n +" equal parts.");  
        }  
            else {  
            for(int i = 0; i < len; i = i+chars) {  
                  
                //Dividing string in n equal part using substring()  
                string part = str.Substring(i, chars);  
                equalStr[temp] = part;  
                temp++;  
            }  
           Debug.Log(n + " equal parts of given string are ");  
            for(int i = 0; i < equalStr.Length; i++) {  
                Debug.Log(equalStr[i]);  
            }  
        }  
        return equalStr;
    }  
}

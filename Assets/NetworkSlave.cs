using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class NetworkSlave : MonoBehaviour
{
    // Start is called before the first frame update

    public Dictionary<string,Component> myComponents = new Dictionary<string,Component>();
    public List<string> meassages = new List<string>();
    void Start()
    {
        Component[] all = GetComponents(typeof(Component));
        foreach (Component myComp in all)
         {
             try{
             myComponents.Add(Convert.ToString(myComp.GetType()),myComp);
         }catch{}}
    }
    public void process_message(string message){
       // Debug.Log("got the message");
       meassages.Add(message);
    }
    // Update is called once per frame
    void Update()
    {
        
        while(meassages.Count > 0){
            //Debug.Log(meassages.Count);
        
        try{
            
        string[] words = meassages[meassages.Count-1].Split(':');

       // Debug.Log(words[0]);
       // Debug.Log(words[1]);
        //Debug.Log(words[2]);
        //Debug.Log(words[3]);

        Component myComp;
            if (myComponents.TryGetValue(words[1], out   myComp)){
                foreach (var thisVar in myComp.GetType().GetProperties())
                {
                        if(thisVar.Name == words[2]){
                            try{
                            if(Convert.ToString(thisVar.PropertyType) == "UnityEngine.Vector3"){
                                //Debug.Log(thisVar.PropertyType);
                                //thisVar.SetValue(thisVar,StringToVector3(words[3]));
                                thisVar.SetValue(myComp,StringToVector3(words[3]));
                            }
                            }catch{
                                Debug.Log("failed to convert ");
                                Debug.Log(StringToVector3(words[3]));
                            }
                        }
                // lastsents.Add(myComp.name + ":" + thisVar.Name  +  ":" + thisVar.GetValue(myComp,null));
                }
            }else{
                Debug.Log("cant find conponent" +words[1] );
            }
        
        //Type myObjectType = myComp.GetType();
       }catch{

           // drop message
       }
       meassages.RemoveAt(meassages.Count-1);
        
        }
    }
    public static Vector3 StringToVector3(string sVector)
     {
        Vector3 result = new Vector3(0,0,0);
         try{
         // Remove the parentheses
         //if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
             sVector = sVector.Replace("("," ");
              sVector = sVector.Replace(")"," ");
              sVector = sVector.Replace(" ","");
              sVector = sVector.Replace(".",".");
             //sVector = sVector.Remove(sVector.Length -2);
         //}
        Debug.Log(sVector);
         // split the items
         string[] sArray = sVector.Split(',');
         Debug.Log(sArray[0]);
         Debug.Log(sArray[1]);
         Debug.Log(sArray[2]);
         float a = float.Parse(sArray[0],CultureInfo.InvariantCulture);
         Debug.Log(a);
         float b = float.Parse(sArray[1],CultureInfo.InvariantCulture);
         Debug.Log(b);
         float c = float.Parse(sArray[2],CultureInfo.InvariantCulture);
         Debug.Log(c);
         // store as a Vector3
         result = new Vector3(
             float.Parse(sArray[0]),
             float.Parse(sArray[1]),
             float.Parse(sArray[2]));
         }
         catch(Exception e){
             Debug.Log("failed to convert string to vector" + e);
         }
         return result;
     }
     public static Quaternion StringToQuaternion(string sQuaternion)
{
    // Remove the parentheses
    if (sQuaternion.StartsWith("(") && sQuaternion.EndsWith(")"))
    {
        sQuaternion = sQuaternion.Substring(1, sQuaternion.Length - 2);
    }

    // split the items
    string[] sArray = sQuaternion.Split(',');

    // store as a Vector3
    Quaternion result = new Quaternion(
        float.Parse(sArray[0]),
        float.Parse(sArray[1]),
        float.Parse(sArray[2]),
        float.Parse(sArray[3]));

    return result;
}
 public static Matrix4x4 StringToMatrix4x4(string sMatrix4x4)
{
    // Remove the parentheses
    if (sMatrix4x4.StartsWith("(") && sMatrix4x4.EndsWith(")"))
    {
        sMatrix4x4 = sMatrix4x4.Substring(1, sMatrix4x4.Length - 2);
    }

    // split the items
    string[] sArray = sMatrix4x4.Split(' ');


     Vector3 t = new Vector3(
             float.Parse(sArray[0]),
             float.Parse(sArray[1]),
             float.Parse(sArray[2]));
     Quaternion r = new Quaternion(
        float.Parse(sArray[4]),
        float.Parse(sArray[5]),
        float.Parse(sArray[6]),
        float.Parse(sArray[7]));
     Vector3 s = new Vector3(
             float.Parse(sArray[0]),
             float.Parse(sArray[1]),
             float.Parse(sArray[2]));


    // store as a Vector3
    Matrix4x4 result = Matrix4x4.TRS(t,r,s);
    


    return result;
}
}

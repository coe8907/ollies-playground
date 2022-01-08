using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkSlave : MonoBehaviour
{
    // Start is called before the first frame update

    public Dictionary<string,Component> myComponents = new Dictionary<string,Component>();
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
        string[] words = message.Split(':');

       // Debug.Log(words[0]);
       // Debug.Log(words[1]);
        //Debug.Log(words[2]);
        //Debug.Log(words[3]);

        Component myComp;
            if (myComponents.TryGetValue(words[1], out   myComp)){
                foreach (var thisVar in myComp.GetType().GetProperties())
                {
                        if(thisVar.Name == words[2]){
                            
                            if(Convert.ToString(thisVar.PropertyType) == "UnityEngine.Vector3"){
                                Debug.Log(thisVar.PropertyType);
                                thisVar.SetValue(myComp,StringToVector3(words[3]));
                            }
                        }
                // lastsents.Add(myComp.name + ":" + thisVar.Name  +  ":" + thisVar.GetValue(myComp,null));
                }
            }else{
                Debug.Log("cant find conponent" +words[1] );
            }
        
        //Type myObjectType = myComp.GetType();
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public static Vector3 StringToVector3(string sVector)
     {
         // Remove the parentheses
         if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
             sVector = sVector.Substring(1, sVector.Length-2);
         }
 
         // split the items
         string[] sArray = sVector.Split(',');
 
         // store as a Vector3
         Vector3 result = new Vector3(
             float.Parse(sArray[0]),
             float.Parse(sArray[1]),
             float.Parse(sArray[2]));
 
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

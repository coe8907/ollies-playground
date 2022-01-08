using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
public class networked_object : MonoBehaviour
{
    // Start is called before the first frame update
    NetworkMaster networkmaster;
    string obj_name = "null";
    int id = -1;
    List<Component> myComponents = new List<Component>(); 
    List<string> lastsents = new List<string>();
    
    void Start()
    {
        Component[] myComponent = GetComponents(typeof(Component));
        foreach (Component myComp in myComponent)
         {
             myComponents.Add(myComp);
             Type myObjectType = myComp.GetType();
             foreach (var thisVar in myComp.GetType().GetProperties())
             {
                  try
                 {
                    lastsents.Add(myComp.name + ":" + thisVar.Name  +  ":" + thisVar.GetValue(myComp,null));
                 }
                 catch (Exception e)
                 {
                 }
             }
         }
        obj_name = gameObject.name;
        networkmaster = GameObject.Find("Network_master").GetComponent<NetworkMaster>();
        id = networkmaster.new_object(this);
        if(id == -1){
           Debug.Log("networked object failed to get vaild ID");
        }
    }
    public string get_name(){
        return obj_name;
    }
    // Update is called once per frame
    string lastsent;
    string current_stat;
    void Update()
    {
        int i =0;
         foreach (Component myComp in myComponents)
         {
             Type myObjectType = myComp.GetType();
             foreach (var thisVar in myComp.GetType().GetProperties())
             {
                 try
                 {
                    
                     current_stat = ( myComp.GetType() + ":" + thisVar.Name  +  ":" + thisVar.GetValue(myComp,null) );
                     if(lastsents[i] != current_stat){
                        networkmaster.Send_message(id, current_stat);
                        lastsents[i] = current_stat;
                     }
                      i++;
                 }
                 catch (Exception e)
                 {
            
                 }
             }
         }
      
    }
    void Quit(){
        
    }
}

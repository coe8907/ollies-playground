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
    void Start()
    {
        name = gameObject.name;
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
        current_stat = gameObject.name;
        current_stat += ((int)this.transform.rotation.eulerAngles.z).ToString()
         + ((int)this.transform.rotation.eulerAngles.y).ToString()
         + ((int)this.transform.rotation.eulerAngles.x).ToString()
         + ((int)this.transform.position.x).ToString()
         + ((int)this.transform.position.y).ToString()
         + ((int)this.transform.position.z).ToString();

        if(lastsent != current_stat){
          networkmaster.Send_message(id, current_stat);
        }
    }
    void Quit(){
        
    }
}

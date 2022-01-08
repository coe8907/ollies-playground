using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
public class NetworkMaster : MonoBehaviour
{
    private Dictionary<int,networked_object> gameobjects = new Dictionary<int,networked_object>();
    private Dictionary<int,NetworkSlave> slaves = new Dictionary<int,NetworkSlave>();
    const int MAX_OBJECTS = 100;
    UdpClient udpClient;

    private Thread _t1;
    bool running = false;
    bool locked = false;
    List<string> messages = new List<string>();
    void Awake(){
         udpClient = new UdpClient(2556);
        try{
            udpClient.Connect("127.0.0.1", 25565);

            Byte[] sendBytes = Encoding.ASCII.GetBytes("new client...");

            udpClient.Send(sendBytes, sendBytes.Length);

            running = true;
            _t1 = new Thread(_Threadednetwork);
            _t1.Start();
        }
        catch (Exception e ) {
            Console.WriteLine(e.ToString());
        }
    } 
    void Start()
    {
       
    }
    public void Send_message(int id,string message){
        Byte[] sendBytes = Encoding.ASCII.GetBytes(id+":"+message);
        udpClient.Send(sendBytes, sendBytes.Length);
    }
    public int new_object(networked_object obj){
        for(int i = 0; i < MAX_OBJECTS; i ++){
            if(!gameobjects.ContainsKey(i)){
                Send_message(i,"NewObject:"+obj.get_name());
                gameobjects.Add(i,obj);
                return i;
            }
        }
        Debug.Log(" Out of objects for networking ");
        return -1;
        
    }
    void remove_object(networked_object obj){
        //gameobjects.Remove(obj);
    }
    public void remove_object(int id){
        gameobjects.Remove(id);
    }

    void Update()
    {
        if(locked){
            try{
            if(messages.Count > 0){
                
                foreach(string message in messages){
                    process_message(message);
                    //Debug.Log(message);
                }
                messages.Clear();
            }
            else{
                locked = false;
            }
            }catch{
                locked = false;
            }
        }
       
    }
    private void process_message(string message){
        string[] words = message.Split(':');
        //Debug.Log(words[1]);
        if(words[1] == "NewObject"){
            Debug.Log(words[2]);
            // create a new object 
            GameObject instance = Instantiate(Resources.Load(words[2], typeof(GameObject))) as GameObject;
            slaves.Add(Int32.Parse(words[0]),instance.GetComponent<NetworkSlave>());
            Debug.Log("Slave " + words[0] + "  created ");
        }else{
            NetworkSlave target;
            if (slaves.TryGetValue(Int32.Parse(words[0]), out  target)){
                 target.process_message(message);
                 
            }else{
                Debug.Log("I cant find the slave  " +words[0] );
            }
            
        }

    }
    
    private void _Threadednetwork()
    {
        List<string> Temp_messages = new List<string>();
        while(running){
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);
            Temp_messages.Add(returnData.ToString());
        
            if(!locked){
                messages = Temp_messages;
                Temp_messages.Clear();
                locked = true;
            }
        }
    }
    void Quit(){
        running = false;
        udpClient.Close();
    }
}

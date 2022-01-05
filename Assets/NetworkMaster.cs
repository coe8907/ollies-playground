using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class NetworkMaster : MonoBehaviour
{
    private Dictionary<int,networked_object> gameobjects = new Dictionary<int,networked_object>();
    const int MAX_OBJECTS = 100;
    UdpClient udpClient;
    void Start()
    {
        udpClient = new UdpClient(2556);
    try{
        
        udpClient.Connect("127.0.0.1", 25565);

         // Sends a message to the host to which you have connected.
        Byte[] sendBytes = Encoding.ASCII.GetBytes("new client...");

        udpClient.Send(sendBytes, sendBytes.Length);

         // Sends a message to a different host using optional hostname and port parameters.
         //UdpClient udpClientB = new UdpClient();
         //udpClientB.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", 25565);
         //IPEndPoint object will allow us to read datagrams sent from any source.
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

         // Blocks until a message returns on this socket from a remote host.
         Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
         string returnData = Encoding.ASCII.GetString(receiveBytes);

         // Uses the IPEndPoint object to determine which of these two hosts responded.
        Debug.Log("This is the message you received " +
                                      returnData.ToString());
        Debug.Log("This message was sent from " +
                                     RemoteIpEndPoint.Address.ToString() +
                                     " on their port number " +
                                     RemoteIpEndPoint.Port.ToString());

         
          //udpClientB.Close();
          }
       catch (Exception e ) {
                  Console.WriteLine(e.ToString());
        }
    }
    public void Send_message(int id,string message){
        Byte[] sendBytes = Encoding.ASCII.GetBytes(message);
        udpClient.Send(sendBytes, sendBytes.Length);
    }
    public int new_object(networked_object obj){
        for(int i = 0; i < MAX_OBJECTS; i ++){
            if(!gameobjects.ContainsKey(i)){
                Send_message(-100,"NewObject:"+obj.get_name());
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
       
    }
    void Quit(){
        udpClient.Close();
    }
}

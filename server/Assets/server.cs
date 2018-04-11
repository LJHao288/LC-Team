using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class server : MonoBehaviour {
    public Button start;
	// Use this for initialization
	void Start ()
    {
	//开启服务器
        start.onClick.AddListener(Start1);
        clients();
        //显示已连接的clients
        //刷新聊天记录
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Start1()
    {
        //启动服务器
        Text server = GameObject.Find("serverstart").GetComponent<Text>();
        Text breaks = GameObject.Find("break").GetComponent<Text>();
        if (breaks.text.Equals("break server"))
        {
            
            breaks.text = "start server";           
            server.text = "";
        }
        else
        {
            server.text = "server started.";      
            breaks.text = "break server";
        }
        

        //breaked server
       
        
    }
    public void clients()
    {   /*
        if( ){
            Text clients = GameObject.Find("clients").GetComponent<Text>();
            clients.text = "clienr address";
        }*/
    }
    public void massage()
    {
        /*
         if(){
              Text massages = GameObject.Find("massages").GetComponent<Text>();
            clients.text = "chat massages"; 
         }
         */
    }
}

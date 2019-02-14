using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class Helper : MonoBehaviour
{
    public static AMQP.Settings stagingServer = new AMQP.Settings("54.80.99.15")
                                                    .VirtualHost("/")
                                                    .Protocol(Protocols.AMQP_0_9_1)
                                                    .Port(5672)
                                                    .Username("110")
                                                    .Password("kYToKJfKG");
    float lastPress = 0f;



    protected void Awake()
    {
        //StartCoroutine( "updateTime");
    }


    void Update() {
        AMQP.Client.GetTime = Time.time;
        if (Input.GetKey(KeyCode.Return) && (Time.time - lastPress > 0.5f))
        {
      
                lastPress = Time.time;
                

            for (int i=0; i< 200 ; i++)
            {

            
                new AMQP.Request("1001", null, "{ \"payload\": { \"value1\":123, \"value2\":456 } }", null)
                    .OnReply((request, reply) => {
                        Debug.Log("Reply");
                    })
                        .OnSent((message) => {
                            Debug.Log("Sent");
                        })
                        .Send();
            }
          
            Debug.Log("Msg Sent");

        }
//        if(Input.GetKey(KeyCode.R)  && (Time.time - lastPress > 0.5f))
//        {
//            lastPress = Time.time;
//            if(AMQP.Client.GetReplies)
//                AMQP.Client.GetReplies = false;
//            else
//                AMQP.Client.GetReplies = true;
//
//            Debug.Log("GETREPLIES "+AMQP.Client.GetReplies);
//        }
//        if(Input.GetKey(KeyCode.S) && (Time.time - lastPress > 0.5f))
//        {
//            lastPress = Time.time;    
//            if(AMQP.Client.SendMSG)
//                AMQP.Client.SendMSG = false;
//            else
//                AMQP.Client.SendMSG = true;
//
//
//            Debug.Log("SENDMSG "+AMQP.Client.SendMSG);
//        }

    }

//    IEnumerator updateTime()
//    {
//        while(true)
//        {
//
//            yield return new WaitForSeconds(1f);
//        }
//    }


    protected void OnMouseDown()
    {

//		Debug.LogError("OnMouseDown connecting again - ??");
//            AMQP.Client.Connect(stagingServer, () => {
//                Debug.Log("Connected");
//
//                AMQP.Client.On("1", (AMQP.InboundMessage message) => {
//                    Debug.Log("Idle Check" + message.GetType().ToString());
//                    new AMQP.OutboundMessage("1004", null, "{ }").Send();
//                });
//
//            });

    }

    public void OnDestroy()
    {
       // AMQP.Client.Disconnect();
    }
}
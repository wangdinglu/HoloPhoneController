using System;
using System.Collections;
using System.Collections.Generic;
//using System.Net;
//using System.Net.Sockets;
using System.Text;
//using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP;
using BestHTTP.WebSocket;

namespace MixOne
{
    public class PhoneClient : MonoBehaviour
    {

        private Button button;
        public Text log;

        //Socket socketSend;
        private WebSocket webSocket;
        private string ip;
        private string port;

        private void init(string _ip, string _port)
        {
            webSocket = new WebSocket(new Uri("ws://"+_ip+":"+_port+"/"));
            Debug.Log("ws://" + _ip + ":" + _port + "/");
            webSocket.Send("Message to the Server");
            webSocket.OnOpen += OnOpen;
            webSocket.OnMessage += OnMessageReceived;
            webSocket.OnError += OnError;
            webSocket.OnClosed += OnClosed;
        }

        private void antiInit()
        {
            webSocket.OnOpen = null;
            webSocket.OnMessage = null;
            webSocket.OnError = null;
            webSocket.OnClosed = null;
            webSocket = null;
        }

        private void setConsoleMsg(string msg)
        {
            log.text =  msg;
        }

        private byte[] getBytes(string message)
        {

            byte[] buffer = Encoding.Default.GetBytes(message);
            return buffer;
        }

        public void Send(string str)
        {
            if (webSocket != null)
            {
                webSocket.Send(str);
                //Debug.Log("Send: " + str);

            }

        }

        public void Close()
        {
            webSocket.Close();
        }

        #region WebSocket Event Handlers

        /// <summary>
        /// Called when the web socket is open, and we are ready to send and receive data
        /// </summary>
        void OnOpen(WebSocket ws)
        {
            Debug.Log("connected");
            setConsoleMsg("Connected");
        }

        /// <summary>
        /// Called when we received a text message from the server
        /// </summary>
        void OnMessageReceived(WebSocket ws, string message)
        {
            //Debug.Log(message);
            //setConsoleMsg(message);
        }

        /// <summary>
        /// Called when the web socket closed
        /// </summary>
        void OnClosed(WebSocket ws, UInt16 code, string message)
        {
            Debug.Log(message);
            setConsoleMsg(message);
            antiInit();
            init(ip,port);
        }

        private void OnDestroy()
        {
            if (webSocket != null && webSocket.IsOpen)
            {
                webSocket.Close();
                antiInit();
            }
        }

        /// <summary>
        /// Called when an error occured on client side
        /// </summary>
        void OnError(WebSocket ws, Exception ex)
        {
            string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
            if (ws.InternalRequest.Response != null)
                errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
#endif
            Debug.Log(errorMsg);
            setConsoleMsg(errorMsg);
            antiInit();
            init(ip, port);
        }

        #endregion


        public void Connect(string _ip, string _port)
        {
            ip = _ip;
            port = _port;
            init(ip,port);

            webSocket.Open();
            //try
            //{
            //    //Pull up a server service
            //    Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //    IPAddress ip = IPAddress.Parse(_ip);
            //    //Create new ip port
            //    IPEndPoint point = new IPEndPoint(ip, port);
            //    Debug.Log(point);
            //    socketWatch.Bind(point);//bond teh ip
            //    Debug.Log("Server success!");
            //    socketWatch.Listen(2);//Listen to 2 devices max

            //    //create thread for listen
            //    Thread thread = new Thread(Listen);
            //    thread.IsBackground = true;
            //    thread.Start(socketWatch);
            //}
            //catch (Exception)
            //{
            //    Debug.Log("False IP address...");
            //    log.text = "False IP address...";
            //}

        }

        ///// <summary>
        ///// Recieve from server
        ///// </summary>
        //void Received(object o)
        //{
        //    try
        //    {
        //        Socket socketSend = o as Socket;
        //        while (true)
        //        {
        //            //recieve message from client
        //            byte[] buffer = new byte[1024 * 1024 * 3];
        //            //efficent message
        //            int len = socketSend.Receive(buffer);
        //            if (len == 0)
        //            {
        //                break;
        //            }
        //            string str = Encoding.UTF8.GetString(buffer, 0, len);
        //            Debug.Log("Server：" + str);
        //            //sc.ClientTask(str);
        //            //cl.pushOperation(str);
        //            //Debug.Log("Server2：" + socketSend.RemoteEndPoint + ":" + str);
        //            Send(str);
        //        }
        //    }
        //    catch { }
        //}

        //public void Listen(object o)
        //{
        //    try
        //    {
        //        Socket socketWatch = o as Socket;
        //        while (true)
        //        {
        //            socketSend = socketWatch.Accept();//wait for client connection
        //            Debug.Log(socketSend.RemoteEndPoint.ToString() + ":" + "server connection success!");
        //            //new thread for recieve message

        //            Thread r_thread = new Thread(Received);
        //            r_thread.IsBackground = true;
        //            r_thread.Start(socketSend);
        //        }
        //    }
        //    catch { }
        //}

        ///// <summary>
        ///// Send to server
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //public void Send(string str)
        //{
        //    try
        //    {
        //        string msg = str;
        //        byte[] buffer = new byte[1024 * 1024 * 3];
        //        buffer = Encoding.UTF8.GetBytes(msg);
        //        socketSend.Send(buffer);
        //    }
        //    catch { }
        //}

    }
}
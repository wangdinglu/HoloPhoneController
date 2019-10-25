using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MixOne
{
    public class ControlManager : MonoBehaviour
    {
        private PhoneClient pc;
        private InputField ip;
        private InputField port;


        public void SendConnect()
        {
            //Debug.Log(ip.text);
            pc.Connect(ip.text,port.text);
        }
        // Start is called before the first frame update
        void Start()
        {
            ip = GameObject.Find("IP").GetComponent<InputField>();
            port = GameObject.Find("Port").GetComponent<InputField>();

            pc = GameObject.Find("ClientManager").GetComponent<PhoneClient>();
        }

    }
}


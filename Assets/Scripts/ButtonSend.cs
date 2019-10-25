using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MixOne
{
    public class ButtonSend: MonoBehaviour
    {
        private PhoneClient pc;
        private Button button;

        private void Start()
        {
            pc = GameObject.Find("ClientManager").GetComponent<PhoneClient>();
            button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(sendInfo);
        }

        public void sendInfo()
        {
            string name = button.name;
            Debug.Log(button.name);
            pc.Send(name);
        } 
    }
}
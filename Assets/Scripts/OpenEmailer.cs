using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.ComponentModel;

public class OpenEmailer : MonoBehaviour 
{


    public InputField Mensaje;
    public InputField Asunto;
    public void SendEmailRequest() 
    {
        Emailer.SendEmail(Mensaje.text, Asunto.text);
    }
}
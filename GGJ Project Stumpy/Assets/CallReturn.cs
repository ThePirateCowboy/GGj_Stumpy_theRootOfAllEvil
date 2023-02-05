using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallReturn : MonoBehaviour
{
    public Initiation init;


    public void callbackFromCut()
    {
        init.ReturnToGame();
    }
}

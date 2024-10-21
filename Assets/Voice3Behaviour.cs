using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice3Behaviour : MonoBehaviour
{
    public GameObject carAI;
    private Voice3 MusicScript;
    private GameObject HMIStatic;
    private Transform dashWarning;
    private Transform onScreenTakeOver;
    private void OnTriggerEnter(Collider other)
    {
        carAI = GameObject.Find("DrivableSmartCommon-no_driver(Clone)");
        MusicScript = carAI.GetComponent<Voice3>();
        MusicScript.playVoice3();
    }
}
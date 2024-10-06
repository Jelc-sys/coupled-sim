using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeepBehaviour : MonoBehaviour
{
    public GameObject carAI;
    private Beep BeepScript;
    private GameObject HMIStatic;
    private Transform dashWarning;
    private void OnTriggerEnter(Collider other)
    {
        HMIStatic = GameObject.Find("StaticHMI_dash(Clone)");
        carAI = GameObject.Find("DrivableSmartCommon-no_driver(Clone)");
        dashWarning = carAI.transform.Find("Dash_Warning");
        SpriteRenderer spriteRenderer = dashWarning.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        //SpeedSettings.speed = 0;
        BeepScript = carAI.GetComponent<Beep>();
        BeepScript.playBeep();
        SpriteHMI_DASH HMIScript = HMIStatic.GetComponent<SpriteHMI_DASH>();
        HMIScript.Display(HMIState.DASH);
    }
}
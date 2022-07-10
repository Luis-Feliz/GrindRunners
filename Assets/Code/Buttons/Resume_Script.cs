using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume_Script : MonoBehaviour
{
    private GameObject pause;
    
    public void OnButtonPress()
    {
        pause = GameObject.Find("Pause_Menu");
        pause.SetActive(false);
        Time.timeScale = 1;
    }
}

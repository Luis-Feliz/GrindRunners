using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Script : MonoBehaviour
{
    public void OnButtonPress()
    {
        SceneManager.LoadScene(0);
    }
}

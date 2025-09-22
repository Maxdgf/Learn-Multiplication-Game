/*
=======================================
| Script for game start panel control |
=======================================
 */

using UnityEngine;
using UnityEngine.UI;

public class StartPanelControl : MonoBehaviour
{
    [SerializeField] private Button startButton; //continue button

    void Start()
    {
        Time.timeScale = 0f; //stop game time

        startButton.onClick.AddListener(Continue); //adding onclick listener to continue button
    }
    
    //continue button function
    private void Continue()
    {
        Handheld.Vibrate(); //vibration

        Time.timeScale = 1f; //continue game time
        gameObject.SetActive(false); //hide game start panel object on scene
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the HomeScreenNavigator
/// </summary>
public class HomeScreenNavigator : MonoBehaviour
{
    public GameObject MainCanvas;
    public GameObject HowToCanvas;
    public GameObject RoomsCanvas;

    /// <summary>
    /// Method to navigate to main.
    /// </summary>
    public void goToMain() {
        MainCanvas.SetActive(true);
        RoomsCanvas.SetActive(false);
        HowToCanvas.SetActive(false);
    }

    /// <summary>
    /// Method to navigate to rooms.
    /// </summary>
    public void goToRooms() {
        RoomsCanvas.SetActive(true);
        HowToCanvas.SetActive(false);
        MainCanvas.SetActive(false);
    }

    /// <summary>
    /// Method to navigate to HowToPlay.
    /// </summary>
    public void goToHowTo() {
        HowToCanvas.SetActive(true);
        RoomsCanvas.SetActive(false);
        MainCanvas.SetActive(false);
    }
}

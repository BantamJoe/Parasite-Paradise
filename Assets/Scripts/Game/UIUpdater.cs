﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIUpdater : BasicObject {

    void Awake () {
        base.Start();
        HealthBar = GameObject.Find("Health Bar");
        StealthOverlay = GameObject.Find("Stealth Overlay");
        PauseOverlay = GameObject.Find("Paused UI");
        PauseOverlay.SetActive(false);
    }

    // updates health UI on screen to given number
    public static void UpdateHealthBar(int currentHealth)
    {
        HealthBar.GetComponent<Slider>().value = currentHealth;
    }

    // change overlay color based on given detection
    public static void UpdateStealthOverlay(Enemy.BehaviorState behaviorState)
    {
        switch (behaviorState) {
            case Enemy.BehaviorState.Alerted:
            case Enemy.BehaviorState.Restless:
        StealthOverlay.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                break;
            case Enemy.BehaviorState.Passive:
            default:
        StealthOverlay.GetComponent<Image>().color = new Color(0, 0, 0, 90);
                break;
        }
    }

    // toggle pause overlay
    public static void TogglePauseOverlay()
    {
        if(PauseOverlay.activeSelf)
        {
            PauseOverlay.SetActive(false);
        } else
        {
            PauseOverlay.SetActive(true);
        }
    }
 
    /** References to UI objects for one-time, fast access **/
    /** Set references in editor **/

    private static GameObject HealthBar;
    private static GameObject StealthOverlay;
    private static GameObject PauseOverlay;

}

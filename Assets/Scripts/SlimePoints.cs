using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UISlimeManager : MonoBehaviour
{
    public TextMeshProUGUI slimePointsText;
    public SlimeMovement slimeController;

    void Start()
    {
        try
        {
            slimePointsText.text = "Points: " + slimeController.GetPoints();
        }
        catch (NullReferenceException e) { }
    }

    void Update()
    {
        try
        {
            slimePointsText.text = "Points: " + slimeController.GetPoints();
        }
        catch(NullReferenceException e) { }
    }
}

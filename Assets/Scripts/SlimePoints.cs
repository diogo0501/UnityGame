using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UISlimeManager : MonoBehaviour
{
    public TextMeshProUGUI slimePointsText;
    public SlimeMovement   slimeController;

    private int LIMIT_POINTS = 20;

    void Start()
    {
        try
        {
            slimePointsText.text = slimeController.GetPoints() + "/" + LIMIT_POINTS;
        }
        catch (NullReferenceException e) { _ = e; }
    }

    void Update()
    {
        try
        {
            slimePointsText.text = slimeController.GetPoints() + "/" + LIMIT_POINTS;
        }
        catch(NullReferenceException e) { _ = e; }
    }
}

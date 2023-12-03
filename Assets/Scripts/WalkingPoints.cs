using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI walkingPointsText;
    public PlayerController playerController;

    void Start()
    {
        walkingPointsText.text = "Walking Points: " + playerController.walkingPoints;
    }

    void Update()
    {
        walkingPointsText.text = "Walking Points: " + playerController.walkingPoints;
    }
}


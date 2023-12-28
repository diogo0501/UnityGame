using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    // Singleton instance
    public static SlimeManager Instance;

    // Data or references to objects you want to persist
    public int playerScore = 0;
    public Dictionary<int,GameObject[]> scenesSlimes = new Dictionary<int, GameObject[]> {};

    void Awake()
    {
        // Ensure only one instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroySlime : MonoBehaviour
{
    private static int NUMBER_SLIMES = 10;
    public int index;
    private static GameObject[] persistentObjects = new GameObject[NUMBER_SLIMES];
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (persistentObjects[index] == null)
        {
            persistentObjects[index] = gameObject;
            DontDestroyOnLoad(gameObject);
        }

        else if(persistentObjects[index] != gameObject)
        {
            Destroy(gameObject);
            persistentObjects[index].SetActive(true);
            persistentObjects[index].GetComponent<SlimeMovement>().createFov();
        }
    }
}

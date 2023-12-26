using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CNScript : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineVirtualCamera cn;
    void Start()
    {
        cn = GetComponent<CinemachineVirtualCamera>();
        cn.Follow = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

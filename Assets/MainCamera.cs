using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera _inst;
    public CinemachineVirtualCamera menuCam;
    // Start is called before the first frame update
    private void Awake()
    {
        _inst = this;
    }
    void Start()
    {
        
    }

    public void onGameStarted()
    {
        menuCam.Priority = 9;
    }

}

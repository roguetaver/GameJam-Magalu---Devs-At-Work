using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera1;
    [SerializeField] private CinemachineVirtualCamera virtualCamera2;
    private GameObject centerPoint;

    void Start()
    {
        centerPoint = this.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if(centerPoint.GetComponent<CenterPoint>().onTrigger){
            virtualCamera1.Priority = 10;
            virtualCamera2.Priority = 11;
        }
        else{
            virtualCamera1.Priority = 11;
            virtualCamera2.Priority = 10;
        }
    }
}

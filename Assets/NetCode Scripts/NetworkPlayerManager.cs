using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerManager : MonoBehaviour
{
    NetworkObject networkObject;
    public Camera PlayerCamera;
    public Canvas CanvasUI;
    void Awake()
    {
        networkObject = GetComponent<NetworkObject>();
    }

    void Start()
    {
        if (!networkObject.IsOwner)
        {
            PlayerCamera.gameObject.GetComponent<AudioListener>().enabled = false;
            PlayerCamera.enabled = false;
            GetComponent<PlayerHud>().enabled = false;
            CanvasUI.gameObject.SetActive(false);
            GetComponent<CameraMovement>().enabled = false;
        }
        else
        {
            print(transform.name + "Is the Owner");
        }
    }

    void Update()
    {
        
    }
}
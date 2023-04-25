using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickOfBridge : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshBridge;
    
    public bool isActiveMesh { get => meshBridge.enabled;}

    public void SetActiveMesh(bool isActive)
    {
        meshBridge.enabled = isActive;
    }
    public void ActiveMesh()
    {
        meshBridge.enabled = true;
    }
    public void DeActiveMesh()
    {
        meshBridge.enabled = false;
    }
}

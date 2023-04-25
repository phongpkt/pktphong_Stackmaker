using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    
    public bool isActiveMesh { get => mesh.enabled;}

    public void SetActiveMesh(bool isActive)
    {
        mesh.enabled = isActive;
    }
    public void DeActiveMesh()
    {
        mesh.enabled = false;
    }
    public void ActiveMesh()
    {
        mesh.enabled = true;
    }

    // private void Update() 
    // {
    //     setActive();
    // }

    // private void setActive()
    // {
    //     if(Player.FindObjectOfType && mesh.active == true)
    //     {
    //         mesh.SetActive(false);
    //     }
    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFinish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            for(int i = player.numberofbrick; i > 0; i --)
            {
                GameObject obj = GameObject.Find("Brick " + i);
                Destroy(obj);
                player.modelTransform.position += Vector3.down * 0.3f;
                //player.numberofbrick --;
            }
            player.moveDirection = Vector3.zero;
            UIManager.Ins.OpenUI(UIID.Win);
        }    
    }
}

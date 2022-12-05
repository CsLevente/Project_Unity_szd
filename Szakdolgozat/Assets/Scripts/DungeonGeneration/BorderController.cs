using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{
    public GameObject virtualCam;

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player") && !other.isTrigger){
            virtualCam.SetActive(true);
            GetComponentInParent<Room>().playerIn = true;
            GetComponentInParent<Room>().UpdateRoom();
            if(GetComponentInParent<Room>().hasBG){
                GetComponentInChildren<_MiniMap>().removeBG();
                GetComponentInParent<Room>().hasBG = false;
            }
            AstarPath.active.Scan();
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player") && !other.isTrigger){
            virtualCam.SetActive(false);
            GetComponentInParent<Room>().playerIn = false;
            GetComponentInParent<Room>().UpdateRoom();
        }
    }

}

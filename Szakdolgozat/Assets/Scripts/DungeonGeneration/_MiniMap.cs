using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MiniMap : MonoBehaviour
{
    public void removeBG(){
        //bool playerInRoom = GetComponentInParent<Room>().playerIn;
        Destroy(gameObject);
        Debug.Log("Terem BG torolve");
    }
}

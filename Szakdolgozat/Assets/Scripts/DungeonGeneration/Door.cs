using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    void OnMouseDown(){
        Destroy(gameObject);
    }
    public void Death(){
        Destroy(gameObject);
    }
}

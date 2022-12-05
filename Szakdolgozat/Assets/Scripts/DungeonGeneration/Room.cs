using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int X;
    public int Y;
    public int type = 1;
    public Vector2 pos;
    public bool playerIn = false;
    private bool updatedDoors = false;
    public GameObject BG;
    public bool hasBG = true;
    public Room(int x, int y){
        X = x;
        Y = y;
    }

    public void UpdateRoom(){
        if(playerIn == false){
			EnemyController[] enemies = GetComponentsInChildren<EnemyController>();
				if(enemies != null){
					foreach(EnemyController enemy in enemies){
						enemy.notInRoom = true;
				    }
			    }
		}else{
			EnemyController[] enemies = GetComponentsInChildren<EnemyController>();
			if(enemies != null){
				foreach(EnemyController enemy in enemies){
					enemy.notInRoom = false;
				}
			}
		}
    }

}

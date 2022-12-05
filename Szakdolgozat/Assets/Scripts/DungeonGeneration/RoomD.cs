using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomD{
    public Vector2 gridPos;
    public int type;

    public bool doorUp, doorDown, doorLeft, doorRight;

    public RoomD(Vector2 _gridPos, int _type){
        gridPos = _gridPos;
        type = _type;
    }
}

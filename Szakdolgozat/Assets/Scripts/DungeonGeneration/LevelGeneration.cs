using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class LevelGeneration : MonoBehaviour
{
    Vector2 worldSize = new Vector2(4,4);
    RoomD[,] rooms;
	NavGraph graphToScan;
	List<GameObject> placedRooms = new List<GameObject>();
    List<Vector2> takenPositions = new List<Vector2>();
    int gridSizeX, gridSizeY, numberOfRooms = 20;

	public GameObject spU, spD, spR, spL,
			spUD, spLR, spUR, spUL, spDR, spDL,
			spUDL, spULR, spUDR, spDLR, spUDLR;

	public GameObject gift;
			
	void Awake(){
		 if(SceneManager.GetActiveScene().name=="Floor1"){
			if(GameObject.Find("SettingManager").GetComponent<SettingManager>().mapsize==0){
				worldSize = new Vector2(4,4);
				numberOfRooms = 10;
				Debug.Log("Floor1, kicsi");
			}else if(GameObject.Find("SettingManager").GetComponent<SettingManager>().mapsize==1){
				worldSize = new Vector2(4,4);
				numberOfRooms = 20;
				Debug.Log("Floor1, nagy");
			}
		 }else if(SceneManager.GetActiveScene().name=="Floor2"){
			if(GameObject.Find("SettingManager").GetComponent<SettingManager>().mapsize==0){
				worldSize = new Vector2(5,5);
				numberOfRooms = 30;
				Debug.Log("Floor2, kicsi");
			}else if(GameObject.Find("SettingManager").GetComponent<SettingManager>().mapsize==1){
				worldSize = new Vector2(5,5);
				numberOfRooms = 45;
				Debug.Log("Floor2, nagy");
			}
		 }
	}

	void Start () {
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2)){
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(worldSize.x);
        gridSizeY = Mathf.RoundToInt(worldSize.y);
        CreateRooms();
        SetRoomDoors();
        DrawMap();
		Scan();
    }

    void CreateRooms(){
        rooms = new RoomD[gridSizeX*2, gridSizeY*2];
        rooms[gridSizeX, gridSizeY] = new RoomD (Vector2.zero, 1);
        takenPositions.Insert(0,Vector2.zero);
        Vector2 checkPos = Vector2.zero;

        float randC = 0.2f, randCS = 0.2f, randCE = 0.01f;

        for (int i = 0; i < numberOfRooms -1; i++){
            float randPct = ((float) i) /(((float)numberOfRooms - 1));
            randC = Mathf.Lerp(randCS, randCE, randPct);

            checkPos = NewPosition();

            if (NumberOfNeighbours(checkPos, takenPositions) > 1 && Random.value > randC){
                int iterations = 0;
                do{
                    checkPos = SelectiveNewPosition();
                    iterations++;
                }while(NumberOfNeighbours(checkPos, takenPositions) > 1 && iterations < 100);
                if (iterations >= 50)
                    print("error: túlfutottunk " + NumberOfNeighbours(checkPos, takenPositions));
            }
            rooms[(int) checkPos.x + gridSizeX, (int) checkPos.y + gridSizeY] = new RoomD(checkPos, 0);
            takenPositions.Insert(0,checkPos);
        }
    }

    Vector2 NewPosition(){
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;
		do{
			int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // kiválasztunk egy random room-ot
			x = (int) takenPositions[index].x;//eltároljuk a pozícióját
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);//véletlenszerűen kiválasztjuk a tengelyt
			bool positive = (Random.value < 0.5f);//véletlenszerűen kiválasztjuk az irányt
			if (UpDown){ //kiválasztjuk az adott pozíciót
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //biztosra megyünk, hogy a pozíció valid
		return checkingPos;
	}

    Vector2 SelectiveNewPosition(){ 
		int index = 0, inc = 0;
		int x =0, y =0;
		Vector2 checkingPos = Vector2.zero;
		do{
			inc = 0;
			do{ 
				index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				inc ++;
			}while (NumberOfNeighbours(takenPositions[index], takenPositions) > 1 && inc < 100);
			x = (int) takenPositions[index].x;
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);
			bool positive = (Random.value < 0.5f);
			if (UpDown){
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
		if (inc >= 100){ // kilépünk ha túl sokáig tart a loop
			print("Error: could not find position with only one neighbor");
		}
		return checkingPos;
	}
    int NumberOfNeighbours(Vector2 checkingPos, List<Vector2> usedPositions){
        int ret = 0;
        if(usedPositions.Contains(checkingPos + Vector2.right)){
            ret++;
        }
        if(usedPositions.Contains(checkingPos + Vector2.left)){
            ret++;
        }
        if(usedPositions.Contains(checkingPos + Vector2.up)){
            ret++;
        }
        if(usedPositions.Contains(checkingPos + Vector2.down)){
            ret++;
        }
        return ret;
    }
    void DrawMap(){

		GameObject helper;
		GameObject actGift;
		int actNum = 0;
		int bossRooNum = BossRoomNumber();
		foreach (RoomD room in rooms){
			if (room == null){
				continue; 
			}
			Vector2 drawPos = room.gridPos;
			drawPos.x *= 19;//távolságok a map spawnpoint-ok között
			drawPos.y *= 11;
			//megjelenítjök az adott szobát, azaz lerakjuk az adott game objekt-et
			
			helper = Instantiate(PickGameObj(room), drawPos, Quaternion.identity);

			helper.GetComponent<Room>().Width = 19;
			helper.GetComponent<Room>().Height = 11;
			helper.GetComponent<Room>().pos = room.gridPos;
			
			if(drawPos == Vector2.zero){
				helper.GetComponent<Room>().type = 0;
			}else{
				if(PickGameObj(room)== spD|| PickGameObj(room)==spU||PickGameObj(room)== spL||PickGameObj(room)== spR){
					if(actNum == bossRooNum){
						helper.GetComponentInChildren<GridController>().GenerateGrid();
						helper.GetComponent<Room>().type = 2;
						actNum++;
					}else{
						actGift = Instantiate(gift, drawPos, Quaternion.identity);
						actNum++;
					}
				}else{
					helper.GetComponentInChildren<GridController>().GenerateGrid();
				}	
			}
			placedRooms.Add(helper);
			
		}
	}

	public void Scan(){
		AstarPath.active.Scan();
	}

	int BossRoomNumber(){
		int cout=0;
		
		foreach (RoomD room in rooms){
			if (room == null){
				continue; 
			}
			if(PickGameObj(room)== spD|| PickGameObj(room)==spU||PickGameObj(room)== spL||PickGameObj(room)== spR){
				cout++;
			}
		}
		int outnum = Random.Range(0,cout);
		return outnum;
	}


    void SetRoomDoors(){
		for (int x = 0; x < ((gridSizeX * 2)); x++){
			for (int y = 0; y < ((gridSizeY * 2)); y++){
				if (rooms[x,y] == null){
					continue;
				}
				Vector2 gridPosition = new Vector2(x,y);
				if (y - 1 < 0){ //check above
					rooms[x,y].doorDown = false;
				}else{
					rooms[x,y].doorDown = (rooms[x,y-1] != null);
				}
				if (y + 1 >= gridSizeY * 2){ //check bellow
					rooms[x,y].doorUp = false;
				}else{
					rooms[x,y].doorUp = (rooms[x,y+1] != null);
				}
				if (x - 1 < 0){ //check left
					rooms[x,y].doorLeft = false;
				}else{
					rooms[x,y].doorLeft = (rooms[x - 1,y] != null);
				}
				if (x + 1 >= gridSizeX * 2){ //check right
					rooms[x,y].doorRight = false;
				}else{
					rooms[x,y].doorRight = (rooms[x+1,y] != null);
				}
			}
		}
	}

	public GameObject PickGameObj(RoomD room)
	{ //kivalasztjuk az kellő szobát az ajtó értékrk alapján

		if(room.doorUp && room.doorDown && room.doorLeft && room.doorRight)
        {
			return spUDLR;
		}
		else if (room.doorUp && room.doorDown && room.doorLeft && !room.doorRight)
        {
			return spUDL;
		}
		else if (room.doorUp && room.doorDown && !room.doorLeft && room.doorRight)
		{
			return spUDR;
		}
		else if (room.doorUp && !room.doorDown && room.doorLeft && room.doorRight)
		{
			return spULR;
		}
		else if (!room.doorUp && room.doorDown && room.doorLeft && room.doorRight)
		{
			return spDLR;
		}
		else if (room.doorUp && room.doorDown && !room.doorLeft && !room.doorRight)
		{
			return spUD;
		}
		else if (room.doorUp && !room.doorDown && room.doorLeft && !room.doorRight)
		{
			return spUL;
		}
		else if (!room.doorUp && room.doorDown && room.doorLeft && !room.doorRight)
		{
			return spDL;
		}
		else if (room.doorUp && !room.doorDown && !room.doorLeft && room.doorRight)
		{
			return spUR;
		}
		else if (!room.doorUp && room.doorDown && !room.doorLeft && room.doorRight)
		{
			return spDR;
		}
		else if (!room.doorUp && !room.doorDown && room.doorLeft && room.doorRight)
		{
			return spLR;
		}
		else if (room.doorUp && !room.doorDown && !room.doorLeft && !room.doorRight)
		{
			return spU;
		}
		else if (!room.doorUp && room.doorDown && !room.doorLeft && !room.doorRight)
		{
			return spD;
		}
		else if (!room.doorUp && !room.doorDown && room.doorLeft && !room.doorRight)
		{
			return spL;
		}
		else if (!room.doorUp && !room.doorDown && !room.doorLeft && room.doorRight)
		{
			return spR;
        }
        else
        {
			Debug.Log("Not working PickGameObj()");
			return spUDLR;
        }
	}
}

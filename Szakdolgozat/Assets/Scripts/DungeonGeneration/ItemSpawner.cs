using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable] 
    public struct Spawnable{
        public GameObject gameObject;
        public float weight;

    }
    public List<Spawnable> items = new List<Spawnable>();
    float totalWeight;

    void Awake(){
        totalWeight = 0;
        foreach(var spawnabl in items){
            totalWeight += spawnabl.weight;
        }
    }
    
    void Start()
    {
        float pick = Random.value * totalWeight;
        int index = 0;
        float actWeight = items[0].weight;
        while(pick> actWeight && index<items.Count -1){
            index++;
            actWeight += items[index].weight;
        }
        GameObject i = Instantiate(items[index].gameObject,transform.position, Quaternion.identity,transform) as GameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            Destroy(gameObject);
        }
    }
}

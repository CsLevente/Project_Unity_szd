using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Item{
    public string name;
    public string description;
    public Sprite itemImage;
}

public class CollectController : MonoBehaviour
{
    public Item item;
    public float healthChange;
    public float moveSpeedChange;
    public float attackSpeedChange;
    public float bulletSizeChange;
    public bool nextLevel;


    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            PlayerController.collectedAmount++;
            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(attackSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            if(nextLevel){
                if(SceneManager.GetActiveScene().name=="Floor1"){
                    GameController.ResetStats();
                    SceneManager.LoadScene("Floor2");   
                }
                if(SceneManager.GetActiveScene().name=="Floor2"){
                    GameController.ResetStats();
                    SceneManager.LoadScene("Menu");   
                
                }
            }
            Destroy(gameObject);
        }
    }
}

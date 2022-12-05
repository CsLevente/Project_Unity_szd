 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum EnemyState
{
    Idle,
    Wander,
    Follow,
    Die,
    Attack
};
public enum EnemyType{
    Melee,
    Ranged,
    Boss
};

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState currState = EnemyState.Idle;
    public EnemyType enemyType;

    public float health ;
    public float range;
    public float speed;
    public float attackRange;
    public float bulletSpeed;
    public float coolDown;
    private bool chooseDir = false;
    private bool dead = false;
    private bool coolDownAttack = false;
    private Vector3 randomDir;
    public GameObject bulletPrefab;
    public GameObject Item;
    public GameObject Itemrand;
    public bool notInRoom = true;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(SceneManager.GetActiveScene().name=="Floor1"){
            switch(enemyType){
                case(EnemyType.Melee):
                    health = 2;
                    break;
                case(EnemyType.Ranged):
                    health = 1;
                    break;
                case(EnemyType.Boss):
                    health = 3;
                    break;
            }
        }else if(SceneManager.GetActiveScene().name=="Floor2"){
            switch(enemyType){
                case(EnemyType.Melee):
                    health = 4;
                    break;
                case(EnemyType.Ranged):
                    health = 2;
                    break;
                case(EnemyType.Boss):
                    health = 6;
                    break;
            }
        }
    }

    void Update()
    {
        switch(currState){
            case(EnemyState.Idle):
                Idle();
                break;
            case(EnemyState.Wander):
                Wander();
                break;
            case(EnemyState.Follow):
                Follow();
                break;
            case(EnemyState.Die):
                break;
            case(EnemyState.Attack):
                Attack();
                break;
        }
        if(!notInRoom){
            if(IsPlayerInRange(range)&& currState!=EnemyState.Die){
                currState = EnemyState.Follow;
            }else if(!IsPlayerInRange(range)&& currState != EnemyState.Die){
                currState = EnemyState.Wander;
            }
            if(Vector3.Distance(transform.position, player.transform.position) <= attackRange){
                currState = EnemyState.Attack;
            }
        }else{
            currState = EnemyState.Idle;
        }
        
    }

    private bool IsPlayerInRange(float range){
        if(player == null){
            return false;
        }
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }
    
   
    

    void Wander(){
        
        if(IsPlayerInRange(range)){
            currState = EnemyState.Follow;
        }
    }

    void Follow(){
        if(player == null){
            return;
        }
        switch(enemyType){
            case(EnemyType.Ranged):
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                break;
            case(EnemyType.Boss):
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                break;
        }
        
    }
    void Idle(){}

    void Attack(){
        if(!coolDownAttack){
            switch(enemyType){
                case(EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown());
                break;
                case(EnemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    StartCoroutine(CoolDown());
                break;
                case(EnemyType.Boss):
                    Debug.Log(transform.position);
                    Vector2 pos = new Vector2(transform.position.x-3,transform.position.y);
                    Vector2 pos2 = new Vector2(transform.position.x-3,transform.position.y+3);
                    Vector2 pos3 = new Vector2(transform.position.x,transform.position.y+3);
                    Vector2 pos4 = new Vector2(transform.position.x+3,transform.position.y+3);
                    Vector2 pos5 = new Vector2(transform.position.x+3,transform.position.y);
                    Vector2 pos6 = new Vector2(transform.position.x+3,transform.position.y-3);
                    Vector2 pos7 = new Vector2(transform.position.x,transform.position.y-3);
                    Vector2 pos8 = new Vector2(transform.position.x-3,transform.position.y-3);
                    GameObject _bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    StartBullet(_bullet,pos);
                    GameObject _bullet2 = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    StartBullet(_bullet2,pos2);
                    GameObject _bullet3 = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    StartBullet(_bullet3,pos3);
                    GameObject _bullet4 = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    StartBullet(_bullet4,pos4);
                    GameObject _bullet5 = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    StartBullet(_bullet5,pos5);
                    GameObject _bullet6 = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    StartBullet(_bullet6,pos6);
                    GameObject _bullet7 = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    StartBullet(_bullet7,pos7);
                    GameObject _bullet8 = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    StartBullet(_bullet8,pos8);

                    StartCoroutine(CoolDown());
                
                break;
            };
        }
    }

    private IEnumerator CoolDown(){
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    private void StartBullet(GameObject bullet, Vector2 pos){
        bullet.GetComponent<BulletController>().GetPos(pos);
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<BulletController>().isEnemyBullet = true;
    }

    public void DamageEnemy(float num){
        health -= num;
        if(health <= 0){
            Death();
            return;
        }
        switch(enemyType){
            case(EnemyType.Boss):
                FindObjectOfType<SettingManager>().Play("boss_damaged");
                break;
            case(EnemyType.Melee):
                FindObjectOfType<SettingManager>().Play("enemy_damaged");
                Debug.Log("enemy_damaged played" );
                break;
            case(EnemyType.Ranged):
                FindObjectOfType<SettingManager>().Play("enemy_damaged");
                Debug.Log("enemy_damaged played" );
                break;
        }
    }

    public void Death(){
        float rand = Random.Range(0f,1f);
        switch(enemyType){
            case(EnemyType.Boss):
                FindObjectOfType<SettingManager>().Play("boss_dead");
                GameObject droppedItem = Instantiate(Item, transform.position, Quaternion.identity);
                if(SceneManager.GetActiveScene().name=="Floor2"){
                    GameObject.Find("GO").transform.Find("GameEndedScreen").GetComponent<OverScreen>().Setup();
                }
                break;
            case(EnemyType.Melee):
                FindObjectOfType<SettingManager>().Play("enemy_dead");
                Debug.Log("enemy_dead played" );
                if(rand<=.1){
                    GameObject droppedItemrand = Instantiate(Itemrand, transform.position, Quaternion.identity);
                }
                
                break;
            case(EnemyType.Ranged):
                FindObjectOfType<SettingManager>().Play("enemy_dead");
                Debug.Log("enemy_dead played" );
                if(rand<=.3){
                    GameObject droppedItemrand2 = Instantiate(Itemrand, transform.position, Quaternion.identity);
                }
                break;
                
        };
        Destroy(gameObject);
    }
}

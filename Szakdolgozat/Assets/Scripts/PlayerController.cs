using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float moveSpeed = 5f;
    Vector2 movement;
    Rigidbody2D rigid_body;
    public Text collectedText;
    public static int collectedAmount = 0;

    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;
    private bool facingRight = true;
    public Animator animator;

    void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal",movement.x);
        animator.SetFloat("Vertical",movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);


        float shootHor = Input.GetAxis("ShootH");
        float shootVert = Input.GetAxis("ShootV");

        if((shootHor != 0 || shootVert != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHor, shootVert);
            lastFire = Time.time;
        }
        

        rigid_body.velocity = new Vector3(horizontal*speed, vertical*speed,0);
        
    }

    void Shoot(float x, float y){
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
        );
        FindObjectOfType<SettingManager>().Play("Shoot");
    }
}



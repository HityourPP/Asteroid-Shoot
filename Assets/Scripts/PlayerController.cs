using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float speed,turnSpeed;
    private bool isMoving;
    private float turnDir = 0;
    [Header("射击参数设置")]
    public float bulletSpeed;
    public float fireCoolTime;//射击间隔
    private float nextFire;
    public float maxLifeTime;

    public GameObject bullet;
    public GameObject bulletPos;
    public GameManager gameManager;
    //[Header("生命设置")]
    //public float maxHealth = 100f;
    //public float currentHealth;
    //public float damage = 20f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //CheckInput();
    }
    private void FixedUpdate()
    {
        //Movement();
        NewMove();
        Shoot();
    }
    //之前通过AD进行旋转，W向前移动，这种移动很不舒服
    private void CheckInput()
    {
        isMoving = Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow);
        if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow))
        {
            turnDir = 1.0f;
        }
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDir = -1.0f;
        }
        else
        {
            turnDir = 0.0f;
        }

    }
    private void Movement()
    {
        if(isMoving)
        {
            rb.AddForce(transform.up * speed);
        }
        if (turnDir != 0)
        {
            rb.AddTorque(turnDir * turnSpeed);
        }
    }
    //使用下面方式进行移动
    private void NewMove()
    {
        float cameraRotate = transform.localEulerAngles.y / 180 * Mathf.PI;
        float v = Input.GetAxis("Vertical");//获取水平与垂直方向的输入
        float h = Input.GetAxis("Horizontal");
        if (Mathf.Abs(v) > 0.1f || Mathf.Abs(h) > 0.1f)//轻敲键盘不做移动
        {
            float sr = Mathf.Sin(cameraRotate);//计算其在相机角度的水平二维方向的两个值
            float cr = Mathf.Cos(cameraRotate);
            rb.velocity = new Vector3((v * sr + h * cr) * speed, (v * cr - h * sr) * speed, 0);
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
    private void Shoot()
    {
        nextFire += Time.fixedDeltaTime;
        if (Input.GetMouseButton(0) && nextFire > fireCoolTime)
        {
            // 获取鼠标坐标并转换成世界坐标
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // 因为是2D，用不到z轴。使将z轴的值为0，这样鼠标的坐标就在(x,y)平面上了
            mousePosition.z = 0;
            // 子弹角度
            float fireAngle = Vector2.Angle(mousePosition - transform.position, Vector2.up);
            
            if (mousePosition.x > transform.position.x)
            {
                fireAngle = -fireAngle;
            }
            // 计时器归零
            nextFire = 0;
            //让角色与射击方向一致
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, fireAngle), 1f);//设置插值
            // 生成子弹
            GameObject bulletGenerate = Instantiate(bullet, bulletPos.transform.position, Quaternion.identity);
            // 速度
            bulletGenerate.GetComponent<Rigidbody2D>().velocity = ((mousePosition - transform.position).normalized * bulletSpeed);
            // 角度
            bulletGenerate.transform.eulerAngles = new Vector3(0, 0, fireAngle);
            //设置其生存时间
            Destroy(bulletGenerate, maxLifeTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0.0f;//angularVelocity为角速度矢量
            gameObject.SetActive(false);

            gameManager.PlayerDied();
            //print("Die");
        }
    }

}

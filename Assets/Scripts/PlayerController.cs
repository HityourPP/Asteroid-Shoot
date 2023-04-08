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
    [Header("�����������")]
    public float bulletSpeed;
    public float fireCoolTime;//������
    private float nextFire;
    public float maxLifeTime;

    public GameObject bullet;
    public GameObject bulletPos;
    public GameManager gameManager;
    //[Header("��������")]
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
    //֮ǰͨ��AD������ת��W��ǰ�ƶ��������ƶ��ܲ����
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
    //ʹ�����淽ʽ�����ƶ�
    private void NewMove()
    {
        float cameraRotate = transform.localEulerAngles.y / 180 * Mathf.PI;
        float v = Input.GetAxis("Vertical");//��ȡˮƽ�봹ֱ���������
        float h = Input.GetAxis("Horizontal");
        if (Mathf.Abs(v) > 0.1f || Mathf.Abs(h) > 0.1f)//���ü��̲����ƶ�
        {
            float sr = Mathf.Sin(cameraRotate);//������������Ƕȵ�ˮƽ��ά���������ֵ
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
            // ��ȡ������겢ת������������
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // ��Ϊ��2D���ò���z�ᡣʹ��z���ֵΪ0�����������������(x,y)ƽ������
            mousePosition.z = 0;
            // �ӵ��Ƕ�
            float fireAngle = Vector2.Angle(mousePosition - transform.position, Vector2.up);
            
            if (mousePosition.x > transform.position.x)
            {
                fireAngle = -fireAngle;
            }
            // ��ʱ������
            nextFire = 0;
            //�ý�ɫ���������һ��
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, fireAngle), 1f);//���ò�ֵ
            // �����ӵ�
            GameObject bulletGenerate = Instantiate(bullet, bulletPos.transform.position, Quaternion.identity);
            // �ٶ�
            bulletGenerate.GetComponent<Rigidbody2D>().velocity = ((mousePosition - transform.position).normalized * bulletSpeed);
            // �Ƕ�
            bulletGenerate.transform.eulerAngles = new Vector3(0, 0, fireAngle);
            //����������ʱ��
            Destroy(bulletGenerate, maxLifeTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0.0f;//angularVelocityΪ���ٶ�ʸ��
            gameObject.SetActive(false);

            gameManager.PlayerDied();
            //print("Die");
        }
    }

}

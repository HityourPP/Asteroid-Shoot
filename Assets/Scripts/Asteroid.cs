using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Asteroid : MonoBehaviour
{
    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;

    public Sprite[] sprites;//设置一个数组存放各种不同的精灵，以随机生成不同的物体
    public float speed = 10f;

    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    public float maxLifeTime = 30.0f;//生存时间
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];//设置其随机的精灵
        //Random.value返回一个随机的浮点数，在0~1之间
        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);//设置其随机的角度
        transform.localScale = Vector3.one * size;

        rb.mass = size;//设置重量作为衡量物体大小
    }
    public void SetTrajectory(Vector2 dir)//设置轨迹
    {
        rb.AddForce(dir * speed);//为其添加速度

        Destroy(gameObject, maxLifeTime);//一段时间摧毁物体
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (size * 0.5f > minSize)//如果比较大，拆分这个物体
            {
                CreateSplit();
                CreateSplit();
            }
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);
            Destroy(gameObject);
        }
    }
    private void CreateSplit()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;//在一个圈内随机选择位置

        Asteroid half = Instantiate(this, position, transform.rotation);
        half.size = size * 0.6f;
        half.SetTrajectory(Random.insideUnitCircle.normalized);//随机选择一个方向移动
    }

}

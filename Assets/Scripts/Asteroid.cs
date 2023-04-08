using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Asteroid : MonoBehaviour
{
    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;

    public Sprite[] sprites;//����һ�������Ÿ��ֲ�ͬ�ľ��飬��������ɲ�ͬ������
    public float speed = 10f;

    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    public float maxLifeTime = 30.0f;//����ʱ��
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];//����������ľ���
        //Random.value����һ������ĸ���������0~1֮��
        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);//����������ĽǶ�
        transform.localScale = Vector3.one * size;

        rb.mass = size;//����������Ϊ���������С
    }
    public void SetTrajectory(Vector2 dir)//���ù켣
    {
        rb.AddForce(dir * speed);//Ϊ������ٶ�

        Destroy(gameObject, maxLifeTime);//һ��ʱ��ݻ�����
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (size * 0.5f > minSize)//����Ƚϴ󣬲���������
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
        position += Random.insideUnitCircle * 0.5f;//��һ��Ȧ�����ѡ��λ��

        Asteroid half = Instantiate(this, position, transform.rotation);
        half.size = size * 0.6f;
        half.SetTrajectory(Random.insideUnitCircle.normalized);//���ѡ��һ�������ƶ�
    }

}

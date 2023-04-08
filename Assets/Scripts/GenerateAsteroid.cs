using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAsteroid : MonoBehaviour
{
    public float spawnRate = 1.0f;

    public int spawnAmount = 1;
    public Asteroid asteroidPrefab;
    public float spawnDistance = 25.0f;
    public float trajectoryVariance = 15.0f;//�켣�������켣�ھ����ĵ�һ����Χ�����ѡ��

    void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);//��spawnRate���ÿ��spawnRate�����һ��Spawn����
    }
    private void Spawn()//��������
    {
        for (int i = 0; i < spawnRate; i++)
        {
            //Random.insideUnitSphere���ذ뾶Ϊ 1 �������ڵ�����㣨ֻ����
            Vector3 spawnDir;
            float spawnX,spawnY;
            //spawnDir = new Vector3(Random.Range(10.0f,20.0f),Random.Range(8.0f,16.0f),0)
            do
            {
                spawnDir = Random.insideUnitCircle.normalized * spawnDistance;//������һ����ΧȦ�ı�Ե����
                spawnX = Mathf.Abs(spawnDir.x);
                spawnY = Mathf.Abs(spawnDir.y);
            } while (spawnX < 15.0f && spawnY < 10.0f);

            Vector3 spawnPoint = transform.position + spawnDir;//��������Ϊ����
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);//��������ķ���
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);//����һ��Χ��Vector3.forward��תvariance�ȵ���ת

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);//��������
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);//�������С
            asteroid.SetTrajectory(rotation * -spawnDir);//������켣
        }
    }
}

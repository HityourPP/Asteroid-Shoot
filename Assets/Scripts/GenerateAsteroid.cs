using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAsteroid : MonoBehaviour
{
    public float spawnRate = 1.0f;

    public int spawnAmount = 1;
    public Asteroid asteroidPrefab;
    public float spawnDistance = 25.0f;
    public float trajectoryVariance = 15.0f;//轨迹方差，将其轨迹在据中心的一个范围内随机选择

    void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);//在spawnRate秒后，每隔spawnRate秒调用一次Spawn函数
    }
    private void Spawn()//生成物体
    {
        for (int i = 0; i < spawnRate; i++)
        {
            //Random.insideUnitSphere返回半径为 1 的球体内的随机点（只读）
            Vector3 spawnDir;
            float spawnX,spawnY;
            //spawnDir = new Vector3(Random.Range(10.0f,20.0f),Random.Range(8.0f,16.0f),0)
            do
            {
                spawnDir = Random.insideUnitCircle.normalized * spawnDistance;//设置在一个范围圈的边缘生成
                spawnX = Mathf.Abs(spawnDir.x);
                spawnY = Mathf.Abs(spawnDir.y);
            } while (spawnX < 15.0f && spawnY < 10.0f);

            Vector3 spawnPoint = transform.position + spawnDir;//以生成器为中心
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);//设置随机的方差
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);//创建一个围绕Vector3.forward旋转variance度的旋转

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);//生成物体
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);//设置其大小
            asteroid.SetTrajectory(rotation * -spawnDir);//设置其轨迹
        }
    }
}

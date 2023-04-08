using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerController playController;
    public ParticleSystem explosion;

    public int lives = 3;//������������
    public int score = 0;

    public float respawnTime = 3.0f;
    public float invincibleTime = 3.0f;//�޵�ʱ��

    public TextMeshProUGUI scoreText;
    public GameObject pause;

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        explosion.transform.position = asteroid.transform.position;
        explosion.Play();

        if (asteroid.size < 0.6f)//���մ�С�ӷ�
        {
            score += 30;
        }
        else if (asteroid.size < 1.0f)
        {
            score += 20;
        }
        else
        {
            score += 10;
        }
        scoreText.text = "Score:" + score.ToString("0000");
    }
    public void PlayerDied()
    {
        explosion.transform.position = playController.transform.position;
        explosion.Play();
        lives--;
        if(lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), respawnTime);//��һ��ʱ�������
        }
    }
    private void Respawn()
    {
        playController.gameObject.transform.position = Vector3.zero;//��ʼ����λ��
        playController.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");//����ʱת����ͼ�㣬�����������ײ
        playController.gameObject.SetActive(true);

        Invoke(nameof(TurnOnCollision), invincibleTime);//һ��ʱ���ͼ��ת������
    }
    private void TurnOnCollision()//������ɫ����ײ
    {
        playController.gameObject.layer = LayerMask.NameToLayer("Player");//����Layer����ΪPlayer
    }

    private void GameOver()
    {//TODO
        score = 0;
        lives = 3;
        Time.timeScale = 0;
        pause.SetActive(true);
    }
    public void Restart()
    {
        Invoke(nameof(Respawn), respawnTime);
        Time.timeScale = 1;
        pause.SetActive(false);
    }
}

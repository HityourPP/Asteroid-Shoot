using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerController playController;
    public ParticleSystem explosion;

    public int lives = 3;//设置其生命数
    public int score = 0;

    public float respawnTime = 3.0f;
    public float invincibleTime = 3.0f;//无敌时间

    public TextMeshProUGUI scoreText;
    public GameObject pause;

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        explosion.transform.position = asteroid.transform.position;
        explosion.Play();

        if (asteroid.size < 0.6f)//按照大小加分
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
            Invoke(nameof(Respawn), respawnTime);//过一段时间后重生
        }
    }
    private void Respawn()
    {
        playController.gameObject.transform.position = Vector3.zero;//初始化其位置
        playController.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");//重生时转换其图层，设置其忽视碰撞
        playController.gameObject.SetActive(true);

        Invoke(nameof(TurnOnCollision), invincibleTime);//一段时间后将图层转换回来
    }
    private void TurnOnCollision()//开启角色的碰撞
    {
        playController.gameObject.layer = LayerMask.NameToLayer("Player");//将其Layer重设为Player
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

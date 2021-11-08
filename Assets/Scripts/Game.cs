using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Game : MonoBehaviour
{

    private bool isGameStarted = false;
    public static float PipeSpeed = 1f;         // 水管移动速度
    public float PipeSpeedSet = 1.3f;
    public bool isDisableTouch = false;

    public GameObject StartCanvas;
    public GameObject GameOverCanvas;
    public GameObject PipePrefab;
    public GameObject ScoreCanvas;
    public GameObject Bird;
    public GameObject BestScore;
    public GameObject LastScore;
    public GameObject NewLabel;

    public Rigidbody2D BirdRigidbody;
  

    
    public GameObject NumberText;
    public Animator animator;
    public static int Score = 0;
    
    public float PipeTimer = 0f;
    private int record = 0;


    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }



    // Update is called once per frame
    void Update()
    {
/*        Debug.Log(birdRigidbody.velocity);*/

            if (isGameStarted)
            {
                if (Input.GetMouseButtonUp(0) || Input.touchCount == 1)
                {
                // 游戏操作逻辑
                    BirdRigidbody.velocity = transform.TransformDirection(Vector2.up * 2);
                    BirdRigidbody.transform.rotation = Quaternion.Euler(0, 0, 40);
                }

                PipeSpeed = PipeSpeedSet;
                NumberText.GetComponent<UnityEngine.UI.Text>().text = Score.ToString(); // 更新分数

                // 每隔1秒生成一组水管
                PipeTimer += Time.deltaTime;

                if (PipeTimer >= PipeSpeed)
                {
                    GameObject pipe = Instantiate(PipePrefab);
                    pipe.transform.position = transform.position + new Vector3(0, Random.Range(-100, 100) / 100f, 0);
                    //Destroy(pipe, 5); //这样写无法满足死亡后水管不消失的功能，满足游戏死亡的时候，水管就不会执行Destory

                    // 手动实现异步删除
                    Task.Run(async delegate
                    {
                        await Task.Delay(2000);

                        // 如果游戏没结束 则执行删除
                        if (isGameStarted.Equals(true))
                        {
                            Destroy(pipe);
                        }
                    });


                    PipeTimer = 0f;
                }

                //旋转鸟的角度
                BirdRigidbody.transform.rotation = Quaternion.Lerp(BirdRigidbody.transform.rotation, Quaternion.Euler(0, 0, -90), Time.deltaTime * PipeSpeed);
            } else if ((Input.GetMouseButtonUp(0) || Input.touchCount == 1) && isDisableTouch.Equals(false))
            {
                StartGame();
            }   
    }


    public void StartGame()
    {
        GameObject[] existPipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (GameObject tmpPipe in existPipes)
        {
            Destroy(tmpPipe);       // 移除Pipe
        }

        Score = 0;
        
        PipeTimer = PipeSpeedSet;

        // 开始游戏

        StartCanvas.SetActive(false);
        ScoreCanvas.SetActive(true);
        GameOverCanvas.SetActive(false);
        PipeSpeed = PipeSpeedSet;
        BirdRigidbody.simulated = true;

        animator.SetFloat("isPlaying", 1);

        BirdRigidbody.GetComponent<CapsuleCollider2D>().isTrigger = true;
        BirdRigidbody.GetComponent<Rigidbody2D>().freezeRotation = false;

        BirdRigidbody.transform.rotation = Quaternion.Euler(0, 0, 40);

        GameObject.FindGameObjectWithTag("Ground").GetComponent<Animator>().enabled = true;

        isGameStarted = true;
    }

    public void ResetGame()
    {
        GameObject.FindGameObjectWithTag("Ground").GetComponent<Animator>().enabled = true;
        
        StartCanvas.SetActive(true);
        ScoreCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        isGameStarted = false;
        isDisableTouch = false;

        Debug.Log("准备开始游戏");
    }

    public void EndGame()
    {
        // 游戏结束了
     

        PipeSpeed = 0;      // 水管停止移动
        isDisableTouch = true;
        isGameStarted = false;

        animator.SetFloat("isPlaying", 2);      // 死亡动画模式
        BirdRigidbody.GetComponent<CapsuleCollider2D>().isTrigger = false;  // 默认碰撞检测接管
        BirdRigidbody.transform.rotation = Quaternion.Euler(0, 0, -90f);
        BirdRigidbody.GetComponent<Rigidbody2D>().freezeRotation = true;        // 冻结碰撞旋转

        // 记录写入score
        int bestScore = PlayerPrefs.GetInt("BestScore");
        if (Score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", Score);     // 更新最新记录
            bestScore = Score;
            NewLabel.SetActive(true);
        } else
        {
            NewLabel.SetActive(false);
        }

        BestScore.GetComponent<UnityEngine.UI.Text>().text = bestScore.ToString();
        LastScore.GetComponent<UnityEngine.UI.Text>().text = Score.ToString();

        GameObject.FindGameObjectWithTag("Ground").GetComponent<Animator>().enabled = false;

        // 取消水管的碰撞属性  使鸟可以往下掉落
        GameObject[] existPipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (GameObject tmpPipe in existPipes)
        {
            tmpPipe.GetComponent<BoxCollider2D>().enabled = false;
        }

        GameOverCanvas.SetActive(true);
        StartCanvas.SetActive(false);
        ScoreCanvas.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Game : MonoBehaviour
{

    public static bool isGameStarted = false;
    public static bool isGameEnded = false;
    public static float PipeSpeed = 1f;         // 水管移动速度
    public float PipeSpeedSet = 1f;

    public GameObject StartCanvas;
    public GameObject GameOverCanvas;
    public GameObject PipePrefab;
    public GameObject ScoreCanvas;
    public GameObject Bird;
    public Rigidbody2D birdRigidbody;
    
    public GameObject NumberText;
    public Animator animator;
    public static int Score = 0;
    
    public float PipeTimer = 0f;
    private int record = 0;


    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Ground").GetComponent<Animator>().enabled = false;
        StartCanvas.GetComponent<Canvas>().enabled = true;
        ScoreCanvas.GetComponent<Canvas>().enabled = false;
        GameOverCanvas.GetComponent<Canvas>().enabled = false;
    }



    // Update is called once per frame
    void Update()
    {
/*        Debug.Log(birdRigidbody.velocity);*/

        if (isGameEnded)
        {
            EndGame();
        } else
        {
            if (Input.GetMouseButtonUp(0) || Input.touchCount == 1)
            {
                if (isGameStarted)
                {
                    // 游戏操作逻辑
                    birdRigidbody.velocity = transform.TransformDirection(Vector2.up * 3);
                    birdRigidbody.transform.rotation = Quaternion.Euler(0, 0, 40);
                }
                else
                {
                    StartGame();
                }
            }

            if (isGameStarted)
            {

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
                        if (isGameEnded.Equals(false))
                        {
                            Destroy(pipe);
                        }
                    });


                    PipeTimer = 0f;
                }

                //旋转鸟的角度
                birdRigidbody.transform.rotation = Quaternion.Lerp(birdRigidbody.transform.rotation, Quaternion.Euler(0, 0, -90), Time.deltaTime * PipeSpeed);
            }

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
        
        StartCanvas.GetComponent<Canvas>().enabled = false;
        ScoreCanvas.GetComponent<Canvas>().enabled = true;
        GameOverCanvas.GetComponent<Canvas>().enabled = false;
        PipeSpeed = PipeSpeedSet;
        birdRigidbody.simulated = true;

        animator.SetFloat("isPlaying", 1);

        birdRigidbody.GetComponent<CapsuleCollider2D>().isTrigger = true;
        birdRigidbody.GetComponent<Rigidbody2D>().freezeRotation = false;

        birdRigidbody.transform.rotation = Quaternion.Euler(0, 0, 40);

        GameObject.FindGameObjectWithTag("Ground").GetComponent<Animator>().enabled = true;

        isGameStarted = true;
    }

    public void EndGame()
    {
        // 游戏结束了

        GameOverCanvas.GetComponent<Canvas>().enabled = true;
        StartCanvas.GetComponent<Canvas>().enabled = false;
        ScoreCanvas.GetComponent<Canvas>().enabled = false;

        PipeSpeed = 0;      // 水管停止移动


        animator.SetFloat("isPlaying", 2);      // 死亡动画模式
        birdRigidbody.GetComponent<CapsuleCollider2D>().isTrigger = false;  // 默认碰撞检测接管
        birdRigidbody.transform.rotation = Quaternion.Euler(0, 0, -90f);
        birdRigidbody.GetComponent<Rigidbody2D>().freezeRotation = true;        // 冻结碰撞旋转

        GameObject.FindGameObjectWithTag("Ground").GetComponent<Animator>().enabled = false;

        // 取消水管的碰撞属性  使鸟可以往下掉落
        GameObject[] existPipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (GameObject tmpPipe in existPipes)
        {
            tmpPipe.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Game : MonoBehaviour
{

    private bool isGameStarted = false;
    public static float PipeSpeed = 1f;         // ˮ���ƶ��ٶ�
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
                // ��Ϸ�����߼�
                    BirdRigidbody.velocity = transform.TransformDirection(Vector2.up * 2);
                    BirdRigidbody.transform.rotation = Quaternion.Euler(0, 0, 40);
                }

                PipeSpeed = PipeSpeedSet;
                NumberText.GetComponent<UnityEngine.UI.Text>().text = Score.ToString(); // ���·���

                // ÿ��1������һ��ˮ��
                PipeTimer += Time.deltaTime;

                if (PipeTimer >= PipeSpeed)
                {
                    GameObject pipe = Instantiate(PipePrefab);
                    pipe.transform.position = transform.position + new Vector3(0, Random.Range(-100, 100) / 100f, 0);
                    //Destroy(pipe, 5); //����д�޷�����������ˮ�ܲ���ʧ�Ĺ��ܣ�������Ϸ������ʱ��ˮ�ܾͲ���ִ��Destory

                    // �ֶ�ʵ���첽ɾ��
                    Task.Run(async delegate
                    {
                        await Task.Delay(2000);

                        // �����Ϸû���� ��ִ��ɾ��
                        if (isGameStarted.Equals(true))
                        {
                            Destroy(pipe);
                        }
                    });


                    PipeTimer = 0f;
                }

                //��ת��ĽǶ�
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
            Destroy(tmpPipe);       // �Ƴ�Pipe
        }

        Score = 0;
        
        PipeTimer = PipeSpeedSet;

        // ��ʼ��Ϸ

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

        Debug.Log("׼����ʼ��Ϸ");
    }

    public void EndGame()
    {
        // ��Ϸ������
     

        PipeSpeed = 0;      // ˮ��ֹͣ�ƶ�
        isDisableTouch = true;
        isGameStarted = false;

        animator.SetFloat("isPlaying", 2);      // ��������ģʽ
        BirdRigidbody.GetComponent<CapsuleCollider2D>().isTrigger = false;  // Ĭ����ײ���ӹ�
        BirdRigidbody.transform.rotation = Quaternion.Euler(0, 0, -90f);
        BirdRigidbody.GetComponent<Rigidbody2D>().freezeRotation = true;        // ������ײ��ת

        // ��¼д��score
        int bestScore = PlayerPrefs.GetInt("BestScore");
        if (Score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", Score);     // �������¼�¼
            bestScore = Score;
            NewLabel.SetActive(true);
        } else
        {
            NewLabel.SetActive(false);
        }

        BestScore.GetComponent<UnityEngine.UI.Text>().text = bestScore.ToString();
        LastScore.GetComponent<UnityEngine.UI.Text>().text = Score.ToString();

        GameObject.FindGameObjectWithTag("Ground").GetComponent<Animator>().enabled = false;

        // ȡ��ˮ�ܵ���ײ����  ʹ��������µ���
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

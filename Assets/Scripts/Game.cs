using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Game : MonoBehaviour
{

    public static bool isGameStarted = false;
    public static bool isGameEnded = false;
    public static float PipeSpeed = 1f;         // ˮ���ƶ��ٶ�
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
                    // ��Ϸ�����߼�
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
                        if (isGameEnded.Equals(false))
                        {
                            Destroy(pipe);
                        }
                    });


                    PipeTimer = 0f;
                }

                //��ת��ĽǶ�
                birdRigidbody.transform.rotation = Quaternion.Lerp(birdRigidbody.transform.rotation, Quaternion.Euler(0, 0, -90), Time.deltaTime * PipeSpeed);
            }

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
        // ��Ϸ������

        GameOverCanvas.GetComponent<Canvas>().enabled = true;
        StartCanvas.GetComponent<Canvas>().enabled = false;
        ScoreCanvas.GetComponent<Canvas>().enabled = false;

        PipeSpeed = 0;      // ˮ��ֹͣ�ƶ�


        animator.SetFloat("isPlaying", 2);      // ��������ģʽ
        birdRigidbody.GetComponent<CapsuleCollider2D>().isTrigger = false;  // Ĭ����ײ���ӹ�
        birdRigidbody.transform.rotation = Quaternion.Euler(0, 0, -90f);
        birdRigidbody.GetComponent<Rigidbody2D>().freezeRotation = true;        // ������ײ��ת

        GameObject.FindGameObjectWithTag("Ground").GetComponent<Animator>().enabled = false;

        // ȡ��ˮ�ܵ���ײ����  ʹ��������µ���
        GameObject[] existPipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (GameObject tmpPipe in existPipes)
        {
            tmpPipe.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}

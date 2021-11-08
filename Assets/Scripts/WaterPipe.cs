using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPipe : MonoBehaviour
{
    public GameObject PipeTop;
    public GameObject PipeDown;
    public GameObject PipeWrap;

    // Start is called before the first frame update
    void Start()
    {
        /*float roundY = Random.Range(-100, 300) / 100f;*/

     /*   Debug.Log(roundY);*/

       /* PipeTop.GetComponent<Renderer>().transform.position = new Vector2(3.4f, roundY);
        PipeDown.GetComponent<Renderer>().transform.position = new Vector2(3.4f, roundY - PipeGap);*/
    }

    // Update is called once per frame
    void Update()
    {

        /* Vector2 pipeTopPosition = PipeTop.GetComponent<Renderer>().transform.position;
         pipeTopPosition.x -= Time.deltaTime * 1f;

         Vector2 pipeDownPosition = PipeDown.GetComponent<Renderer>().transform.position;
         pipeDownPosition.x -= Time.deltaTime * 1f;

         PipeTop.GetComponent<Renderer>().transform.position = pipeTopPosition;
         PipeDown.GetComponent<Renderer>().transform.position = pipeDownPosition;*/
        /*
                if (Time.deltaTime * time  <= 1)
                {
                   *//* Destroy(PipeWrap);*//*
                }*/

        transform.position += Vector3.left * Game.PipeSpeed * Time.deltaTime;

    }


    private void FixedUpdate()
    {
        
       

    }
}

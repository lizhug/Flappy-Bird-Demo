using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    public Game game;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Ground" || collision.name == "PipeTop" || collision.name == "PipeDown" || collision.name == "SkyBoxCollision")
        {

            Debug.Log("×²µ½ÁË" + collision.name);

            game.EndGame();
        }
    }
}

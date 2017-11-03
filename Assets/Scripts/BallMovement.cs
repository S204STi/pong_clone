using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour {

	public float speed = 30;

	private Rigidbody2D rigidBody;

	private AudioSource audioSource;


	// Use this for initialization
	void Start () 
	{

		rigidBody = GetComponent<Rigidbody2D>();
		rigidBody.velocity = Vector2.right * speed;
		
	}
	
	void OnCollisionEnter2d(Collision2D col)
	{
		//check what we collided with
		//left or right paddle
		if((col.gameObject.name == "paddleLeft") || (col.gameObject.name == "paddleRight"))
		{
			HandlePaddleHit(col);
		}

		//top or bottom wall
		if((col.gameObject.name == "wallTop") || (col.gameObject.name == "wallBottom"))
		{
			SoundManager.Instance.PlayOneShot(SoundManager.Instance.wallBloop);
		}

		//left or right goal
		if((col.gameObject.name == "goalLeft") ||(col.gameObject.name == "goalRight"))
		{
			SoundManager.Instance.PlayOneShot(SoundManager.Instance.goalBloop);

            if(col.gameObject.name == "goalLeft")
            {
                IncreaseTextUIScore("leftScoreUI");
            }
            else if (col.gameObject.name == "goalRight")
            {
                IncreaseTextUIScore("rightScoreUI");
            }

            transform.position = new Vector2(0,0);
		}
	}

	void HandlePaddleHit(Collision2D col)
	{

		float y = BallHitPaddleWhere(transform.position, col.transform.position, col.collider.bounds.size.y);

		Vector2 dir = new Vector2();

		if(col.gameObject.name == "paddleLeft")
		{
			dir = new Vector2(1, y).normalized;
        }

		if(col.gameObject.name == "paddleRight")
		{
			dir = new Vector2(-1, y).normalized;
		}

		rigidBody.velocity = dir * speed;

		SoundManager.Instance.PlayOneShot(SoundManager.Instance.hitPaddleBloop);

	}

	float BallHitPaddleWhere(Vector2 ball, Vector2 paddle, float paddleHeight)
	{
		return (ball.y - paddle.y) / paddleHeight;
	}

    void IncreaseTextUIScore(string textUIName)
    {
        var textUIComp = GameObject.Find(textUIName).GetComponent<Text>();

        int score = int.Parse(textUIComp.text);

        score++;

        textUIComp.text = score.ToString();
    }
}

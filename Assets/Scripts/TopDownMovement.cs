using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
	float accelerationTime;
	float moveSpeed;

	Vector3 velocity;
	float velocityXSmoothing;
    float velocityYSmoothing;

	private Controller2D controller;

	private float targetVelocityX;

    private float targetVelocityY;

	private float latestInputX;

    private float latestInputY;

	private bool hasHitHorizontal;
    public bool hasHitVertical;

    public bool powerUp, powerRight;

	public bool canMove;

	public AudioClip landSound;

	private bool playOnce1;

	private AudioSource audioSource;


	void Start()
	{
		controller = GetComponent<Controller2D>();

		moveSpeed = 20;
		accelerationTime = 0.1f;
		latestInputX = 0;
        latestInputY = 0;
		hasHitHorizontal = true;
		hasHitVertical = true;
		canMove = true;
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		
		//vetor de velocidade com o input do teclado
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if(input.x != 0 && hasHitHorizontal && hasHitVertical ){
            if(!powerRight && input.x < 0){
                latestInputX = -1;
			    hasHitHorizontal = false;
            }
            else if(powerRight){
                latestInputX = input.x;
			    hasHitHorizontal = false;
            }
		}
        else if(input.y != 0 && hasHitVertical && hasHitHorizontal && powerUp){
			latestInputY = input.y;
			hasHitVertical = false;
		}
		
        targetVelocityX = latestInputX * moveSpeed;
        targetVelocityY = latestInputY * moveSpeed;

		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
        velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTime);

		if(controller.collisions.right || controller.collisions.left){
			hasHitHorizontal = true;
			latestInputX = 0;
			velocity.x = 0;
		}
        else if(controller.collisions.above || controller.collisions.below){
			hasHitVertical = true;
			latestInputY = 0;
			velocity.y = 0;
		}

        if(!powerUp){
            latestInputY = 0;
            velocity.y = 0;
        }
		gameObject.GetComponent<Animator>().SetFloat("VelX", velocity.x);
		gameObject.GetComponent<Animator>().SetFloat("VelY", velocity.y);
		if (canMove) controller.Move(velocity * Time.deltaTime);
	}

	private void playLandSound()
	{
		audioSource.clip = landSound;
		audioSource.Play();
	}
    
}

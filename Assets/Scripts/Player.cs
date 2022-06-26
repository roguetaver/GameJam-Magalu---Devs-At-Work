using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float maxJumpHeight;
	public float minJumpHeight;
	public float timeToJumpApex;
	float accelerationTimeAirborne;
	float accelerationTimeGrounded;
	float moveSpeed;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	public float hangTime;
	private float hangCounter;
	public float jumpBufferLength;
	private float jumBufferCount;

	private Controller2D controller;

	private float targetVelocityX;

	public bool hasBreak;

	private float latestInput;

	private bool hasHit;

	public bool canMove;

	private AudioSource audioSource;

	public AudioClip jumpSound;

	public AudioClip landSound;

	private bool playOnce;

	private bool playOnce1;

	void Start()
	{
		controller = GetComponent<Controller2D>();

		moveSpeed = 11f;
		maxJumpHeight = 20;
		minJumpHeight = 0.3f;
		timeToJumpApex = 0.48f;
		accelerationTimeAirborne = 0.2f;
		accelerationTimeGrounded = 0.1f;
		hangTime = 0.1f;
		jumpBufferLength = 0.1f;
		latestInput = 0;
		hasHit = true;
		canMove = true;
		audioSource = GetComponent<AudioSource>();

		//equações para que com as variáveis de jump height e time to jump apex, seja possível chegar na gravidade e velocidade 
		//de pulo, já que esses não são números fáceis de se imaginar
		//gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);

		//valor de gravidade testado 
		gravity = -55;
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		print("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);
		
	}

	void Update()
	{
		if (controller.collisions.above || controller.collisions.below || controller.collisions.left || controller.collisions.right)
		{
			if(!playOnce1){
				playLandSound();
				playOnce1 = true;
			}
		}
		else{
			playOnce1 = false;
		}
		
		//se o jogador estiver colidindo com alguma coisa em seu eixo y, setamos a velocidade em y para 0, para que quando ele caia, a velocidade em y não acumule
		if (controller.collisions.above || controller.collisions.below)
		{
			velocity.y = 0;
		}
		//coyote time
        if (controller.collisions.below)
        {
			hangCounter = hangTime;
		}
        else
        {
			hangCounter -= Time.deltaTime;
        }

		//jump buffer

		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{		
			jumBufferCount = jumpBufferLength;
		}
		else
		{
			jumBufferCount -= Time.deltaTime;
		}


		//vetor de velocidade com o input do teclado
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		//se o jogador estiver no chão e se apertar espaço, o jogador pula
		if (jumBufferCount >= 0 && hangCounter > 0f && velocity.y <= 0)
		{
			velocity.y = maxJumpVelocity;
			if(!playOnce && canMove){
				playJumpSound();
				playOnce = true;
			}
			jumBufferCount = 0;
		}
		else{
			playOnce = false;
		}
		if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) ) && velocity.y > 0)
		{
			//para consertar bug do double jump causado pelo jump buffer
			hangCounter = -1;
			//adjustable jump
			if (velocity.y > minJumpVelocity)
			{
				velocity.y = minJumpVelocity;
			}
		}
		else{
			playOnce = false;
		}

		//para que a velocidade não mude repentinamente, usamos smoothdamp para que a velocidade atual mude para a nova de maneira suave
		//definimos a velocidade final que se quer alcançar na variavel abaixo

		if(input.x != 0 && hasHit){
			latestInput = input.x;
			hasHit = false;
		}
		
		if(hasBreak){
			targetVelocityX = input.x * moveSpeed;
		}
		else{
			targetVelocityX = latestInput * moveSpeed;

		}

		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		if(controller.collisions.right || controller.collisions.left){
			hasHit = true;
			//latestInput = 0;
			//velocity.x = 0;
		}
		
		//no primeiro parametro esta a velocidade atual, no segundo a velocidade que se quer atingir, e em quarto a aceleração
		//nesse caso, ? seria um if e : seria um else, desse modo, se o jogador esta colidindo com o chao, a aceleração será a do chão, e caso contrario, será a do ar

		velocity.y += gravity * Time.deltaTime;
		
		if (canMove) controller.Move(velocity * Time.deltaTime);
		gameObject.GetComponent<Animator>().SetFloat("VelX", velocity.x);
	}

	private void playJumpSound()
	{
		audioSource.clip = jumpSound;
		audioSource.Play();
	}

	private void playLandSound()
	{
		audioSource.clip = landSound;
		audioSource.Play();
	}
}

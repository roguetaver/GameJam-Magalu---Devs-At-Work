using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int powerNumber;
    private TopDownMovement td;
    private Player playerScript;
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject grid1, grid2, grid3, powerX, powerGravity, cam, music, fade;
    private AudioSource audioSource;
    public DialogTrigger dialogue1, dialogue2, dialogue3, dialogue4; 

    void Start()
    {
        music = GameObject.Find("Music");
        td = GameObject.Find("Player").GetComponent<TopDownMovement>();
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        animator = GameObject.Find("Player").GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        dialogue1 = this.transform.GetChild(0).gameObject.GetComponent<DialogTrigger>();
        dialogue2 = this.transform.GetChild(1).gameObject.GetComponent<DialogTrigger>();
        dialogue3 = this.transform.GetChild(2).gameObject.GetComponent<DialogTrigger>();
        dialogue4 = this.transform.GetChild(3).gameObject.GetComponent<DialogTrigger>();
    }

    public void PowerUp(){
        audioSource.Play();
        powerNumber += 1;
        if(powerNumber == 1){
            dialogue1.TriggerDialogue();
            td.powerRight = true;
            music.GetComponent<MusicScript>().NextMusic();
        }else if(powerNumber == 2){
            StartCoroutine(Stage3());
        }else if(powerNumber == 3){
            StartCoroutine(Stage4());
        }else if(powerNumber == 4){
            dialogue4.TriggerDialogue();
            playerScript.hasBreak = true;
            music.GetComponent<MusicScript>().NextMusic();
        }
    }

    private IEnumerator Stage3(){
        fade.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(.4f);
        td.powerUp = true;
        td.hasHitVertical = true;
        animator.SetBool("Stage2", true);
        grid1.SetActive(false);
        grid2.SetActive(true);
        powerX.GetComponent<Animator>().SetTrigger("Stage2");
        powerGravity.GetComponent<Animator>().SetTrigger("Stage2");
        cam.GetComponent<Camera>().backgroundColor = new Color32(0x62, 0xc3, 0xd4, 0xFF);
        yield return new WaitForSeconds(.3f);
        fade.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.8f);
        dialogue2.TriggerDialogue();
    }

    private IEnumerator Stage4(){
        fade.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(.4f);
        td.enabled = false;
        playerScript.enabled = true;
        powerX.GetComponent<Animator>().SetTrigger("Stage3");
        grid2.SetActive(false);
        grid3.SetActive(true);
        yield return new WaitForSeconds(.3f);
        fade.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.8f);
        dialogue3.TriggerDialogue();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTriggerWithArea : MonoBehaviour
{
    public Dialog dialogue;
    private bool doItOnce;
    public bool hasHit;
    private int dialogueLength;
    private int count;
    private GameObject player;

    private void Start(){
        dialogueLength = dialogue.sentences.Length;
        count = 0;
        player = GameObject.Find("Player");
    }

    public void Update(){
        if(Input.GetKeyDown(KeyCode.Space) && hasHit){
            NextDialogue();
            count ++;
        }

        if(count > dialogueLength){
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {   
        if(col.gameObject.tag == "Player" && !doItOnce){
            doItOnce = true;
            hasHit = true;
            count ++;

        if (player.TryGetComponent(out Player scriptSide))
        {
            scriptSide.canMove = false;
        }

        if (player.TryGetComponent(out TopDownMovement scriptTopDown))
        {
            scriptTopDown.canMove = false;
        }

            Invoke("TriggerDialogue", 2f);
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogManager>().StartDialogue(dialogue);
    }

    public void NextDialogue()
    {
        FindObjectOfType<DialogManager>().DisplayNextSentence();
    }
}

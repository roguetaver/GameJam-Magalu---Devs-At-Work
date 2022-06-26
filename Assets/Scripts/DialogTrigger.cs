using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialogue;
    private bool hasStarted;
    private int dialogueLength;
    private int count;

    private void Start(){
        dialogueLength = dialogue.sentences.Length;
        count = 0;
    }

    public void Update(){
        if(Input.GetKeyDown(KeyCode.Space) && hasStarted){
            NextDialogue();
            count ++;
        }

        if(count > dialogueLength){
            Destroy(this.gameObject);
        }
    }
    
    public void TriggerDialogue()
    {
        hasStarted = true;
        count ++;
        FindObjectOfType<DialogManager>().StartDialogue(dialogue);
    }

    public void NextDialogue()
    {
        FindObjectOfType<DialogManager>().DisplayNextSentence();
    }

}

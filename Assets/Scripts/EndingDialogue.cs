using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingDialogue : MonoBehaviour
{
    public Dialog dialogue;
    private bool hasStarted;
    private int dialogueLength;
    public int count;
    private GameObject fade;

    private void Start(){
        fade = GameObject.Find("Fade");
        dialogueLength = dialogue.sentences.Length;
        Invoke("TriggerDialogue", 2.0f);
        count = 0;
    }

    public void Update(){
        if(Input.GetKeyDown(KeyCode.Space) && hasStarted){
            NextDialogue();
            count ++;
        }

        if(count > dialogueLength){
            StartCoroutine(LoadNextScene());
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

    public IEnumerator LoadNextScene()
    {
        fade.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    //private Queue<string> sentences;
    public List<string> sentences;
    public Text dialogueText;
    public Animator animator, animatorPlayer;
    public AudioClip[] dialogueSounds;
    private AudioSource audioSource;
    private GameObject player;
    private GameManager GM;

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = this.GetComponent<AudioSource>();
        //sentences = new Queue<string>();
        sentences = new List<string>();
        player = GameObject.Find("Player");
        animatorPlayer = player.GetComponent<Animator>();
    }

    public void StartDialogue(Dialog dialogue)
    {

        if (player.TryGetComponent(out Player scriptSide))
        {
            scriptSide.canMove = false;
        }

        if (player.TryGetComponent(out TopDownMovement scriptTopDown))
        {
            scriptTopDown.canMove = false;
        }


        animator.SetBool("IsOpen", true);

        sentences.Clear();

        foreach(string sentence in dialogue.sentences){
            //sentences.Enqueue(sentence);
            sentences.Add(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence(){
        if(sentences.Count == 0){
            EndDialogue();
            return;
        }
        StopAllCoroutines();
        //string sentence = sentences.Dequeue();
        string sentence = sentences[0];
        sentences.RemoveAt(0);
        Debug.Log(sentence);
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        float timePerLetter = 0.05f;
        dialogueText.text = "";
        int maxCharPerLine = 75;
        int maxChar = maxCharPerLine;
        int currentChar = 0;
        string[] words = sentence.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if(currentChar + words[i].Length > maxChar)
            {
                dialogueText.text += "\n";
                maxChar = dialogueText.text.Length + maxCharPerLine;
            }
            //able to fit in line
            for (int j = 0; j < words[i].Length; j++)
            {
                //every letter in word adds to text and waits
                audioSource.clip = dialogueSounds[Random.Range(0,dialogueSounds.Length - 1)];
                audioSource.Play();
                dialogueText.text += words[i].ToCharArray().GetValue(j);
                yield return new WaitForSecondsRealtime(timePerLetter);
            }
            dialogueText.text += ' ';
            currentChar = dialogueText.text.Length;
        }
    }

    private void EndDialogue(){
        if(GM.powerNumber != 3){
            if (player.TryGetComponent(out Player scriptSide))
            {
                scriptSide.canMove = true;
            }

            if (player.TryGetComponent(out TopDownMovement scriptTopDown))
            {
                scriptTopDown.canMove = true;
            }
        }
        if(GM.powerNumber == 3){
            StartCoroutine(ChangeCone());
        }

        animator.SetBool("IsOpen", false);
    }

    private IEnumerator ChangeCone(){
        yield return new WaitForSeconds(.5f);
        animatorPlayer.SetBool("Stage3", true);
        if (player.TryGetComponent(out Player scriptSide))
        {
            scriptSide.canMove = true;
        }

        if (player.TryGetComponent(out TopDownMovement scriptTopDown))
        {
            scriptTopDown.canMove = true;
        }
    }
}

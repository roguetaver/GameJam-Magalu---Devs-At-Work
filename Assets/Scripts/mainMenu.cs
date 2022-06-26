using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject fade;

    public void play()
    {
        StartCoroutine(PlayFade());
    }

    public void quit()
    {
        Application.Quit(); 
    }

    public void Menu(){
        StartCoroutine(MenuFade());
    }

    private IEnumerator PlayFade(){
        fade.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator MenuFade(){
        fade.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Menu");
    }
}

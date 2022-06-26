using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerFinal : MonoBehaviour
{
    private GameObject fade;
    // Start is called before the first frame update
    void Start()
    {
        fade = GameObject.Find("Fade");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player"){
            print("END");
            StartCoroutine(Final());
        }
    }
    private IEnumerator Final(){
        fade.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Final");
    }
}

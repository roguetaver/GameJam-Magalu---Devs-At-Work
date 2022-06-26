using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player"){
            if (other.TryGetComponent(out Player scriptSide))
            {
                 scriptSide.canMove = false;
            }

            if (other.TryGetComponent(out TopDownMovement scriptTopDown))
            {
                 scriptTopDown.canMove = false;
            }
            GM.PowerUp();
            Destroy(gameObject);
        }
    }
}

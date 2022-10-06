using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepScript : MonoBehaviour
{
    [SerializeField] private DayCountController dcController;
    public bool sleeping = true;
    [SerializeField] private PlayerScript player;
    void Update()
    {//WAKEUP
        if(Input.GetKeyDown(KeyCode.E)){
            if(dcController.gameObject.GetComponent<CanvasGroup>().alpha == 1 && sleeping){
                StartCoroutine(dcController.FadeScreen(0));
                sleeping = false;
                player.animator.Play("IdleLeft");
                player.canMove = !player.canMove;
            }
        }
    }
    public void OnTriggerStay2D(Collider2D other)
    {//SLEEP
        if(Input.GetKeyDown(KeyCode.E)){
            if(other.TryGetComponent<PlayerScript>(out PlayerScript player) && !sleeping){
                if (dcController.gameObject.GetComponent<CanvasGroup>().alpha == 0)
                {
                    player.canMove = !player.canMove;
                    dcController.Sleep();
                    StartCoroutine(dcController.FadeScreen(1));
                    sleeping = true;
                }
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

}

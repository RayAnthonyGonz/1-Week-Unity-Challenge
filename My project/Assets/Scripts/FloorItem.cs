using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorItem : MonoBehaviour
{
    public GameObject parent;
    [SerializeField] public ItemClass item;
    private Collider2D other;
    private bool inside;

    public void Kill(){
        Destroy(this.parent);
    }
    private void OnTriggerEnter2D(Collider2D o)
    {
        other = o;
        inside = true;
        this.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D o)
    {
        other = o;
        inside = false;
        this.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && inside){ 
            if (other.gameObject.name == "Character" && !other.GetComponent<PlayerScript>().inInventory){
                other.GetComponent<PlayerScript>().inventory.Add(item, 1);
                Kill();
            }  
        }
    }
}

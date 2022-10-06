using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overheadMask : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Character"){
            GameObject.Find("Mask").GetComponent<SpriteMask>().enabled = true;
            StartCoroutine(ChangeCameraView(1.4f));
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "Character"){
            GameObject.Find("Mask").GetComponent<SpriteMask>().enabled = false;
            StartCoroutine(ChangeCameraView(3.2f));
        }
    }

    public IEnumerator ChangeCameraView(float targetSize){
        float oldSize = cam.orthographicSize;
        float elapsed = 0;
        while (elapsed <= 0.5f)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / 0.5f);
            cam.orthographicSize = Mathf.Lerp(oldSize, targetSize, t);
            yield return null;
        }
        cam.orthographicSize = targetSize;
    }
}

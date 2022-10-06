using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    public CanvasGroup blackScreen;
    
    public void NextScene(){
        StartCoroutine(FadeScreen(1f));
        Debug.Log("we go");
    }

    IEnumerator FadeScreen(float alpha){
        float timeElapsed = 0;
        float lerpDuration = 1.5f;
        float lerpAlpha = alpha; 
        float startAlpha = blackScreen.alpha;
        
        while (timeElapsed < lerpDuration)
        {
            
            blackScreen.alpha = Mathf.Lerp(startAlpha, lerpAlpha, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        blackScreen.alpha = alpha;
        SceneManager.LoadScene("GameScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;

public class DayCountController : MonoBehaviour
{
    [SerializeField] private HoeFunction HoeFunctions;
    [SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private SleepScript sleepScript;
    [SerializeField] private Transform wakeLocation;
    [SerializeField] private PlayerScript player;
    [SerializeField] private int day;
    [SerializeField] private Tilemap mushroomGrid;
    private void Awake()
    {
        if (this.gameObject.GetComponent<CanvasGroup>().alpha == 0){
            this.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }
        StartCoroutine(FadeBlack(0));
        player.canMove = false;
        player.moving = false;
    }

    public void Sleep(){
        day += 1;
        HoeFunctions.setNewShroomGrid(mushroomGrid);
    }
    
    IEnumerator FadeBlack(float alpha){
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
    }

    public IEnumerator FadeScreen(float alpha){
        this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Day "+day;
        float timeElapsed = 0;
        float lerpDuration = 1.5f;
        float lerpAlpha = alpha; 
        float startAlpha = this.gameObject.GetComponent<CanvasGroup>().alpha;
        while (timeElapsed < lerpDuration)
        {
            this.gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, lerpAlpha, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        this.gameObject.GetComponent<CanvasGroup>().alpha = alpha;
        StartCoroutine(FadeButton(alpha));
    }

    public IEnumerator FadeButton(float alpha){
        if(sleepScript.sleeping){
            player.gameObject.transform.position = wakeLocation.position;
        }
        float timeElapsed = 0;
        float lerpDuration = 1f;
        float lerpAlpha = alpha; 
        float startAlpha = this.transform.GetChild(1).GetComponent<CanvasGroup>().alpha;
        while (timeElapsed < lerpDuration)
        {
            this.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, lerpAlpha, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        this.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = alpha;
    }
}

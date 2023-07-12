using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class WaveTimerUI : MonoBehaviour, IPointerClickHandler
{
    public Text timerText;

    private void Update()
    {
        timerText.text = "Next wave in: " + Mathf.Round(GameManager.instance.waveTimer) + "s";
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.StartWave();
    }
}

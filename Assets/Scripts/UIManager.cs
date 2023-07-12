using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI waveCountdownText;

    void Update()
    {
        healthText.text = "Health: " + Player.instance.health.ToString();
        goldText.text = "Gold: " + Player.instance.gold.ToString();
        waveCountdownText.text = "Next wave in: " + Mathf.Round(GameManager.instance.waveTimer) + "s";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.StartWave();
    }
}

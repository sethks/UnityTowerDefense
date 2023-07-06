using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI waveCountdownText;

    void Update()
    {
        healthText.text = "Health: " + Player.instance.health.ToString();
        goldText.text = "Gold: " + Player.instance.gold.ToString();
        // waveCountdownText.text = "Next wave in: " + WaveManager.instance.timTo 
    }
}

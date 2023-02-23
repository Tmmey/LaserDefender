using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDisplay : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Slider healthSlider;
    [SerializeField] HealthScript playerHealth;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Powerup")]
    [SerializeField] Image tripleShootPowerupImage;
    [SerializeField] Image shieldPowerupImage;
    [SerializeField] TextMeshProUGUI powerupDuration;
    Player player;
    // Image closestpowerupImage;
    // float closestpowerupDuration;
    // Dictionary<Image, float> powerups;


    private void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        player = FindObjectOfType<Player>();
        //playerHealth = FindObjectsOfType<HealthScript>().FirstOrDefault(x => x.isPlayer);
    }

    private void Start()
    {
        healthSlider.maxValue = playerHealth.GetHealth();
        tripleShootPowerupImage.enabled = false;
        shieldPowerupImage.enabled = false;
        powerupDuration.enabled = false;
    }

    void Update()
    {
        healthSlider.value = playerHealth.GetHealth();
        scoreText.text = scoreKeeper.GetCurrentScore().ToString("000000000");

        ShowLowestPowerupDuration();
    }

    void ShowLowestPowerupDuration()
    {
        var tsPowerupDuration = player.GetTripleShootDuration();
        var sPowerupDuration = playerHealth.GetShieldDuration();

        if (tsPowerupDuration > Mathf.Epsilon && sPowerupDuration <= Mathf.Epsilon)
        {
            ShowTripleShoot(tsPowerupDuration);
        }
        else if (sPowerupDuration > Mathf.Epsilon && tsPowerupDuration <= Mathf.Epsilon)
        {
            ShowShield(sPowerupDuration);
        }
        else if (tsPowerupDuration > Mathf.Epsilon && sPowerupDuration > Mathf.Epsilon)
        {
            if (tsPowerupDuration > sPowerupDuration)
            {
                ShowShield(sPowerupDuration);
            }
            else
            {
                ShowTripleShoot(tsPowerupDuration);
            }
        }
        else
        {
            tripleShootPowerupImage.enabled = false;
            shieldPowerupImage.enabled = false;
            powerupDuration.enabled = false;
        }
    }

    void ShowTripleShoot(float tsPowerupDuration)
    {
        tripleShootPowerupImage.enabled = true;
        shieldPowerupImage.enabled = false;
        powerupDuration.text = tsPowerupDuration.ToString("0");
        powerupDuration.enabled = true;
    }

    void ShowShield(float sPowerupDuration)
    {
        shieldPowerupImage.enabled = true;
        tripleShootPowerupImage.enabled = false;
        powerupDuration.text = sPowerupDuration.ToString("0");
        powerupDuration.enabled = true;
    }
}

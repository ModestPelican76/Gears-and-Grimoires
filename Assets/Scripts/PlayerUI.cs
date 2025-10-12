//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public float healthFraction;
    public GameObject player;
    public Playerv2 playerScript;
    public HealthStats playerHealth;
    public UnityEngine.UI.Image healthFillImage;
    public UnityEngine.UI.Image healthPotion1;
    public UnityEngine.UI.Image healthPotion2;
    public UnityEngine.UI.Image healthPotion3;
    public UnityEngine.UI.Image apple;
    public UnityEngine.UI.Image seeds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Playerv2>();
        playerHealth = player.GetComponent<HealthStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthFillImage != null)
        {
            healthFraction = playerHealth.currentHealth / playerHealth.maxHealth;
            healthFillImage.fillAmount = Mathf.Clamp01(healthFraction);
        }
        if (healthPotion1 != null)
        {
            if (playerScript.currentHealthPotions >= 1)
            {
                healthPotion1.gameObject.SetActive(true);
            }
            else
            {
                healthPotion1.gameObject.SetActive(false);
            }
        }
        if (healthPotion2 != null)
        {
            if (playerScript.currentHealthPotions >= 2)
            {
                healthPotion2.gameObject.SetActive(true);
            }
            else
            {
                healthPotion2.gameObject.SetActive(false);
            }
        }
        if (healthPotion3 != null)
        {
            if (playerScript.currentHealthPotions >= 3)
            {
                healthPotion3.gameObject.SetActive(true);
            }
            else
            {
                healthPotion3.gameObject.SetActive(false);
            }
        }
        if (seeds != null)
        {
            if (playerScript.hasSeeds)
            {
                seeds.gameObject.SetActive(true);
            }
            else
            {
                seeds.gameObject.SetActive(false);
            }
        }
        if (apple != null)
        {
            if (playerScript.hasApple)
            {
                apple.gameObject.SetActive(true);
            }
            else
            {
                apple.gameObject.SetActive(false);
            }
        }
    }
}

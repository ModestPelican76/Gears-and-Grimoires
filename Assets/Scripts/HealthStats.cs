using System.Data.Common;
using DialogueEditor;
using UnityEngine;
using System.Collections;

public class HealthStats : MonoBehaviour
{
    public float maxHealth = 50f;
    public float currentHealth;
    public bool isAlive = true;
    public NPCConversation defeatConversation;
    public bool hasDefeatConversation;
    public float drunkHealth = 75f;
    public Color mutilatedColor;
    public Sprite grave;
    public SpriteRenderer SR;
    public float mutilationDuration = 2f;
    public ImportantValues valueBank;
    public TimeManager timeManager;
    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        if (hasDefeatConversation)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Defeat Conversation"))
                {
                    defeatConversation = child.GetComponent<NPCConversation>();
                    break;
                }
            }
        }
        valueBank = GameObject.FindGameObjectWithTag("Manager").GetComponent<ImportantValues>();
        mutilatedColor = valueBank.mutilatedColor;
        grave = valueBank.burySprite;
        timeManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<TimeManager>();
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && isAlive == true)
        {
            isAlive = false;
            Debug.Log(this.name + " has been defeated.");
            if (this.CompareTag("Enemy"))
            {
                if (this.name == "Frog")
                {
                    this.GetComponent<Frog>().associatedBattleZone.EndBattle();
                    Debug.Log("Battle Ended");
                    Destroy(gameObject);
                }
                if (this.name == "Drunk_Witch")
                {
                    this.GetComponent<DrunkWitch>().associatedBattleZone.EndBattle();
                    Debug.Log("Battle Ended");
                    ConversationManager.Instance.StartConversation(defeatConversation);
                    this.gameObject.SetActive(false);
                }
                if (this.name == "Xator")
                {
                    this.GetComponent<DrunkWitch>().associatedBattleZone.EndBattle();
                    Debug.Log("Battle Ended");
                    ConversationManager.Instance.StartConversation(defeatConversation);
                    this.gameObject.SetActive(false);
                }
                if (this.name == "Tutorial_Fighter")
                {
                    this.GetComponent<TutorialFighter>().associatedBattleZone.EndBattle();
                    Debug.Log("Battle Ended");
                    ConversationManager.Instance.StartConversation(defeatConversation);
                    this.gameObject.SetActive(false);
                }
                if (this.name == "Witch")
                {
                    this.GetComponent<TutorialFighter>().associatedBattleZone.EndBattle();
                    Debug.Log("Battle Ended");
                    ConversationManager.Instance.StartConversation(defeatConversation);
                    this.gameObject.SetActive(false);
                }
                if (this.name == "Robot")
                {
                    this.GetComponent<TutorialFighter>().associatedBattleZone.EndBattle();
                    Debug.Log("Battle Ended");
                    ConversationManager.Instance.StartConversation(defeatConversation);
                    this.gameObject.SetActive(false);
                }
                if (this.name == "Snail")
                {
                    this.GetComponent<Snail>().associatedBattleZone.EndBattle();
                    Debug.Log("Battle Ended");
                    ConversationManager.Instance.StartConversation(defeatConversation);
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
    public void HealDamage(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void bury()
    {
        if (grave != null)
        {
            GetComponent<SpriteRenderer>().sprite = grave;
        }
    }
    public void mutilate()
    {
        StartCoroutine(LerpColorOverTime(mutilatedColor, mutilationDuration));
    }
    public IEnumerator LerpColorOverTime(Color targetColor, float duration)
    {
        Color startColor = SR.color;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            SR.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        // Ensure final color is exactly the target
        SR.color = targetColor;
    }
}

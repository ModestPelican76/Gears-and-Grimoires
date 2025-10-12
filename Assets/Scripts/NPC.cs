using DialogueEditor;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCConversation goodKarmaConversation;
    public NPCConversation badKarmaConversation;
    public NPCConversation presentConversation;
    public NPCConversation futureConversation;
    public NPCConversation positiveConversation;
    public NPCConversation negativeConversation;
    public NPCConversation extraConversation;
    public bool isPersonNPC;
    public bool isTimeTravelNPC;
    public bool isSeedBox;
    public bool isPlantPot;
    public bool isAppleTree;
    public bool isStatue;
    public bool isButton;
    public bool isDeity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isPersonNPC)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Good Karma Conversation"))
                {
                    goodKarmaConversation = child.GetComponent<NPCConversation>();
                }
                if (child.CompareTag("Bad Karma Conversation"))
                {
                    badKarmaConversation = child.GetComponent<NPCConversation>();
                }
            }
        }
        else if (isTimeTravelNPC)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Present Conversation"))
                {
                    presentConversation = child.GetComponent<NPCConversation>();
                }
                if (child.CompareTag("Future Conversation"))
                {
                    futureConversation = child.GetComponent<NPCConversation>();
                }
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Positive Conversation"))
                {
                    positiveConversation = child.GetComponent<NPCConversation>();
                }
                if (child.CompareTag("Negative Conversation"))
                {
                    negativeConversation = child.GetComponent<NPCConversation>();
                }
                if (child.CompareTag("Extra Conversation"))
                {
                    extraConversation = child.GetComponent<NPCConversation>();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

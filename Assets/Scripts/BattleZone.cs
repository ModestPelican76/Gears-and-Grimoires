using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using JetBrains.Annotations;
//using UnityEditor.SearchService;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    public float battleWaitTime = 2f;
    public EdgeCollider2D wallCollider;
    public BoxCollider2D interactionCollider;
    public bool battleActive = false;
    public bool battleCompleted = false;
    public bool hasConversation = false;
    public bool battleHasStarted = false;
    public bool dataUpdated = false;
    public bool isSnailArena = false;
    public NPCConversation goodKarmaConversation;
    public NPCConversation badKarmaConversation;
    public NPCConversation regularConversation;
    public bool dependsOnKarma = false;
    public Playerv2 currentPlayer;
    void Awake()
    {
        interactionCollider = this.GetComponentInChildren<BoxCollider2D>();
        wallCollider = GetComponent<EdgeCollider2D>();
        if (hasConversation && dependsOnKarma)
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
        battleHasStarted = false;
        dataUpdated = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!battleCompleted)
        {
            if (battleActive == true)
            {
                wallCollider.enabled = true;
                interactionCollider.enabled = false;
                //this.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                wallCollider.enabled = false;
                interactionCollider.enabled = true;
                //this.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        if (battleCompleted)
        {
            wallCollider.enabled = false;
            interactionCollider.enabled = false;
            //this.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (battleCompleted)
        {
            battleHasStarted = false;
        }
    }
    public void OnChildTriggerEnter(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentPlayer = collision.GetComponent<Playerv2>();
            Debug.Log("Player entered Battle Zone");
            if (!dataUpdated)
            {
                if (!hasConversation)
                {
                    battleHasStarted = true;
                    StartCoroutine(StartBattle());
                    collision.GetComponent<Playerv2>().currentBattleZone = this;
                    dataUpdated = true;
                }
                else
                {
                    if (dependsOnKarma)
                    {
                        if (collision.GetComponent<Playerv2>().karma < 0)
                        {
                            if (!battleCompleted)
                            {
                                ConversationManager.Instance.StartConversation(badKarmaConversation);
                                hasConversation = false;
                                dataUpdated = true;
                            }
                        }
                        else
                        {
                            if (!battleCompleted)
                            {
                                ConversationManager.Instance.StartConversation(goodKarmaConversation);
                                hasConversation = false;
                                dataUpdated = true;
                            }
                        }
                    }
                    else
                    {
                        ConversationManager.Instance.StartConversation(regularConversation);
                        if(isSnailArena)
                        {
                            ConversationManager.Instance.SetInt("Karma", (int)currentPlayer.karma);
                        }
                        hasConversation = false;
                        dataUpdated = true;
                    }
                }
            }
        }
    }
    public void startBattle()
    {
        StartCoroutine(StartBattle());
        currentPlayer.currentBattleZone = this;
    }
    IEnumerator StartBattle()
    {
        if (!battleCompleted)
        {
            yield return new WaitForSeconds(battleWaitTime);
            battleHasStarted = true;
            battleActive = true;
        }
    }
    public void EndBattle()
    {
        battleActive = false;
        battleCompleted = true;
        Debug.Log("Battle Ended");
        currentPlayer.inBattle = false;
    }
}

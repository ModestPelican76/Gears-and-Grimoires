using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using JetBrains.Annotations;
using UnityEngine;

public class BattleZone_Remake : MonoBehaviour
{
    public float battleWaitTime = 2f;
    public EdgeCollider2D wallCollider;
    public BoxCollider2D interactionCollider;
    public bool battleActive = false;
    public bool battleCompleted = false;
    public bool hasConversation = false;
    public bool battleHasStarted = false;
    public bool dataUpdated = false;
    public NPCConversation goodKarmaConversation;
    public NPCConversation badKarmaConversation;
    public Playerv2 currentPlayer;
    void Awake()
    {
        interactionCollider = this.GetComponentInChildren<BoxCollider2D>();
        wallCollider = GetComponent<EdgeCollider2D>();
        if (hasConversation)
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
            }
            else
            {
                wallCollider.enabled = false;
                interactionCollider.enabled = true;
            }
        }
        if (battleCompleted)
        {
            wallCollider.enabled = false;
            interactionCollider.enabled = false;
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
                    //collision.GetComponent<Playerv2>().currentBattleZone = this;
                    dataUpdated = true;
                }
                else
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
            }
        }
    }
    public void startBattle()
    {
        StartCoroutine(StartBattle());
        //currentPlayer.currentBattleZone = this;
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

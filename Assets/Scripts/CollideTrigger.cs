using System;
using DialogueEditor;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

public class CollideTrigger : MonoBehaviour
{
    public GameObject timeManager;
    public Light2D drunkLight;
    public GameObject llama;
    public GameObject llamaBattleArena;
    public Playerv2 player;
    public float maxIntensity = 34f;
    public float lightTime = 0f;
    public float lightSpeed = 3f;
    public bool isTutorial = false;
    public bool isLight = false;
    public bool isLlama = false;
    public bool isHeaven = false;
    public bool isCameraZoomer = false;
    public bool isSoundChanger = false;
    public bool controlsConversation = false;
    public bool conversationPlayed = false;
    public NPCConversation conversation;
    public Camera mainCamera;
    public AudioClip snailTrack;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isTutorial)
        {
            timeManager.GetComponent<TutorialScript>().startCutscene();
        }
        if (collision.gameObject.CompareTag("Player") && isLight)
        {
            if (lightTime <= 1f)
            {
                lightTime += Time.deltaTime * lightSpeed;
                drunkLight.intensity = Mathf.Lerp(0, maxIntensity, lightTime);
            }
        }
        if (collision.gameObject.CompareTag("Player") && isLlama)
        {
            if (player.plantedTree && llama != null)
            {
                llama.SetActive(true);
                llamaBattleArena.SetActive(true);
            }
        }
        if (collision.gameObject.CompareTag("Player") && controlsConversation && !conversationPlayed)
        {
            ConversationManager.Instance.StartConversation(conversation);
            conversationPlayed = true;
        }
        if (collision.gameObject.CompareTag("Player") && isCameraZoomer)
        {
            mainCamera.orthographicSize = 20;
        }
        if (collision.gameObject.CompareTag("Player") && isHeaven)
        {
            ConversationManager.Instance.StartConversation(conversation);
            player.preventInput();
        }
        if (collision.gameObject.CompareTag("Player") && isSoundChanger)
        {
            player.gameObject.GetComponent<AudioSource>().clip = snailTrack;
            player.gameObject.GetComponent<AudioSource>().Play();
        }
    }
    void OTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isCameraZoomer)
        {
            mainCamera.orthographicSize = 10;
        }
    }
}

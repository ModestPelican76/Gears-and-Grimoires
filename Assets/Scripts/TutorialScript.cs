using DialogueEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TutorialScript : MonoBehaviour
{
    public TimeManager timeManager;
    public GameObject cutsceneBeginCollider;
    public NPCConversation cutsceneConvo;
    public NPCConversation rFinalConvo;
    public NPCConversation wFinalConvo;
    public bool choseRobot;
    public bool choseWitch;
    public bool inTutorial;
    public bool cutsceneStarted;
    public Transform tpPoint_Enemy;
    public Transform tpPoint_Player;
    public GameObject witchEnemy;
    public GameObject robotEnemy;
    public GameObject witchPlayer;
    public GameObject robotPlayer;
    public GameObject orbPlayer;
    public BattleZone robotBattleArena;
    public BattleZone witchBattleArena;
    public Scene r_castle;
    public Scene w_castle;

    public GameObject loadingScreen;
    public Image fillImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeManager = GetComponent<TimeManager>();
        inTutorial = true;
        cutsceneStarted = false;
        timeManager.player = orbPlayer.gameObject.GetComponent<HealthStats>();
    }

    public void startCutscene()
    {
        if (!cutsceneStarted)
        {
            ConversationManager.Instance.StartConversation(cutsceneConvo);
            cutsceneStarted = true;
        }
    }
    public void chooseFaction(int choice)
    {
        if (choice == 0)
        {
            choseRobot = true;
            choseWitch = false;
        }
        if (choice == 1)
        {
            choseRobot = false;
            choseWitch = true;
        }
    }
    public void setupBattle()
    {
        if (choseRobot)
        {
            robotPlayer.transform.position = new Vector3(tpPoint_Player.transform.position.x, tpPoint_Player.transform.position.y, -1);
            robotPlayer.SetActive(true);
            orbPlayer.SetActive(false);
            timeManager.player = robotPlayer.gameObject.GetComponent<HealthStats>();
            witchEnemy.GetComponent<TutorialFighter>().reassignPlayer();
            witchEnemy.transform.position = new Vector3(tpPoint_Enemy.transform.position.x, tpPoint_Enemy.transform.position.y, -1);
            robotEnemy.SetActive(false);
            robotBattleArena.gameObject.SetActive(true);


        }
        else if (choseWitch)
        {
            witchPlayer.transform.position = new Vector3(tpPoint_Player.transform.position.x, tpPoint_Player.transform.position.y, -1);
            witchEnemy.SetActive(false);
            witchPlayer.SetActive(true);
            orbPlayer.SetActive(false);
            timeManager.player = witchPlayer.gameObject.GetComponent<HealthStats>();
            robotEnemy.GetComponent<TutorialFighter>().reassignPlayer();
            robotEnemy.transform.position = new Vector3(tpPoint_Enemy.transform.position.x, tpPoint_Enemy.transform.position.y, -1);
            witchBattleArena.gameObject.SetActive(true);
        }
    }
    public void playFinalDialogue()
    {
        if (choseRobot)
        {
            Debug.Log("Playing Robot ending");
            ConversationManager.Instance.StartConversation(rFinalConvo);
            ConversationManager.Instance.SetInt("Karma", (int)timeManager.player.gameObject.GetComponent<Playerv2>().karma);
        }
        else if (choseWitch)
        {
            Debug.Log("Playing Witch ending");
            ConversationManager.Instance.StartConversation(wFinalConvo);
        }
    }
    public void endTutorial()
    {
        if (choseRobot)
        {
            //SceneManager.LoadScene("r_castle");
            StartCoroutine(LoadAsynchronously("r_castle"));
        }
        else if (choseWitch)
        {
            //SceneManager.LoadScene("w_castle");
            StartCoroutine(LoadAsynchronously("w_castle"));
        }
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (fillImage != null)
                fillImage.fillAmount = progress;

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f); // Optional delay
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
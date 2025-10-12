using System.Collections;
using System.Data.Common;
using DialogueEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public bool isPresent = true;
    public ScreenFade screenFade;
    public GameObject presentMap;
    public GameObject futureMap;
    public GameObject puzzleGate;
    public GameObject bossGate;
    public GameObject deathUI;
    public HealthStats player;

    [Header("Quick time event")]
    public GameObject quickTimeUI;
    public float currentQteTime;
    public float totalQuickTime = 1f;
    public float qteTimeFraction;
    public bool qteHappening;
    public bool qteClicked = false;
    public NPCConversation positiveQteConvo;
    public NPCConversation negativeQteConvo;
    public UnityEngine.UI.Image qteBar;
    public GameObject xator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPresent = true;
        if (futureMap != null && presentMap != null)
        {
            presentMap.SetActive(true);
            futureMap.SetActive(false);
        }
        if (bossGate != null)
        {
            bossGate.SetActive(true);
        }
        screenFade = GetComponent<ScreenFade>();
        quickTimeUI.SetActive(false);
        qteClicked = false;
        qteHappening = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (qteHappening)
        {
            currentQteTime -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.X))
            {
                qteClicked = true;
            }
        }
        qteTimeFraction = currentQteTime / totalQuickTime;
        qteBar.fillAmount = Mathf.Clamp01(qteTimeFraction);
        if (!player.isAlive)
        {
            deathUI.SetActive(true);
        }
    }
    public void ChangeTime()
    {
        if (isPresent)
        {
            Debug.Log("Time changed to Future");
            isPresent = false;
            screenFade.StartFade();
            if (futureMap != null && presentMap != null)
            {
                Debug.Log("Set map to future");
                presentMap.SetActive(false);
                futureMap.SetActive(true);
            }
        }
        else if (!isPresent)
        {
            Debug.Log("Time changed to Past");
            isPresent = true;
            screenFade.StartFade();
            if (futureMap != null && presentMap != null)
            {
                Debug.Log("Set map to present");
                futureMap.SetActive(false);
                presentMap.SetActive(true);
            }
        }
    }
    public void startQTE()
    {
        Debug.Log("Starting QTE");
        StartCoroutine(qteTimer());
    }
    public IEnumerator qteTimer()
    {
        //DontDestroyOnLoad(this);
        quickTimeUI.SetActive(true);
        currentQteTime = totalQuickTime;
        qteHappening = true;
        yield return new WaitForSeconds(totalQuickTime);
        quickTimeUI.SetActive(false);
        qteHappening = false;
        if (qteClicked)
        {
            ConversationManager.Instance.StartConversation(positiveQteConvo);
        }
        else if (!qteClicked)
        {
            ConversationManager.Instance.StartConversation(negativeQteConvo);
            xator.SetActive(false);
        }
    }
    public void DestroyPuzzleGate()
    {
        if (bossGate != null)
        {
            Debug.Log("Opened Puzzle Gate");
            puzzleGate.SetActive(false);
        }
    }
    public void DestroyBossGate()
    {
        if (bossGate != null)
        {
            Debug.Log("Opened Boss Gate");
            bossGate.SetActive(false);
        }
    }
    public void endCastle()
    {
        SceneManager.LoadScene("Heaven");
    }
}

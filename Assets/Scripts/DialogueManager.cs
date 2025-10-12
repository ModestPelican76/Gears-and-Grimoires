using DialogueEditor;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public bool xatorDead = false;
    public TimeManager timeManager;
    [Header("Drunk witch fight")]
    public NPCConversation spareDead;
    public NPCConversation spareAlive;
    public NPCConversation killDrunk;
    void Start()
    {
        GetComponent<TimeManager>();
    }
    void Update()
    {
        if (timeManager.qteClicked)
        {
            xatorDead = false;
        }
        else
        {
            xatorDead = true;
        }
    }
    public void playDrunkSpareConvo()
    {
        if (xatorDead)
        {
            ConversationManager.Instance.StartConversation(spareDead);
        }
        else
        {
            ConversationManager.Instance.StartConversation(spareAlive);
        }
    }
    public void playDrunkKillConvo()
    {
        Debug.Log("Playing kill convo");
        ConversationManager.Instance.StartConversation(killDrunk);
    }
}

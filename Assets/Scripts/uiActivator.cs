using UnityEngine;

public class uiActivator : MonoBehaviour
{
    public GameObject UI;
    public bool shouldActivate = false;
    void Update()
    {
        if (shouldActivate)
        {
            UI.SetActive(true);
        }
        else
        {
            UI.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shouldActivate = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shouldActivate = false;
        }
    }
}

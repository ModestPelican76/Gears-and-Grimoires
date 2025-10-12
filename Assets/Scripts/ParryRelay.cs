using UnityEngine;

public class ParryRelay : MonoBehaviour
{
    public Playerv2 parentPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentPlayer = GetComponentInParent<Playerv2>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        parentPlayer.parryChildHit(collision);
    }
}

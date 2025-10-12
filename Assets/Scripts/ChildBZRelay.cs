using UnityEngine;

public class ChildBZRelay : MonoBehaviour
{
    public BattleZone parentBattleZone;

    void Start()
    {
      parentBattleZone = GetComponentInParent<BattleZone>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
      parentBattleZone.OnChildTriggerEnter(collision);
    }
}

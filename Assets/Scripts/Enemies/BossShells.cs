using UnityEngine;

public class BossShells : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 5f;
    public Vector2 direction;
    public GameObject player;
    public Playerv2 playerScript;
    public BattleZone associatedBattleZone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Playerv2>();
        direction = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.inBattle == true && playerScript.currentBattleZone == associatedBattleZone && associatedBattleZone.battleHasStarted)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<HealthStats>().TakeDamage(damage);
        }
        if(collision.gameObject.CompareTag("Battle Area"))
        {
            direction=player.transform.position - transform.position;
            direction.Normalize();
        }
    }
}

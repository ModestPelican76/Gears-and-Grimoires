using UnityEngine;
using System.Collections;
using DialogueEditor;

public class TutorialFighter : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;
    public float moveSpeed = 2f;
    public float movesMade = 0f;
    public float moveLimit = 2f;
    public float damage = 10f;
    public float attackCooldown = 2f;
    public float comboCooldown = 5f;
    public float burstSeperation = 0.5f;
    public float attackTimer;
    public GameObject spellPrefab;
    public Transform spellSpawnPoint;
    public bool canAttack = true;
    public bool comboOver;
    public float projectileSpeed;
    public Playerv2 player;
    private bool isAlive;
    public bool canFight = true;
    public HealthStats thisEnemyStats;
    public BattleZone associatedBattleZone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisEnemyStats = GetComponent<HealthStats>();
        isAlive = thisEnemyStats.isAlive;
        maxHealth = thisEnemyStats.maxHealth;
        currentHealth = thisEnemyStats.currentHealth;
        projectileSpeed = spellPrefab.GetComponent<ProjectileProperties>().speed;
        damage = spellPrefab.GetComponent<ProjectileProperties>().damage;
        attackCooldown = spellPrefab.GetComponent<ProjectileProperties>().cooldown;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Playerv2>();
        comboOver = false;
        canFight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canFight)
        {
            if (player.inBattle == true && isAlive == true && player.currentBattleZone == associatedBattleZone && associatedBattleZone.battleHasStarted)
            {
                if (movesMade < moveLimit && canAttack)
                {
                    Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);

                    if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
                    {
                        //transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                        transform.position = new Vector3(newPosition.x, newPosition.y, -1f);
                    }
                    else
                    {
                        movesMade += 1;
                        comboOver = false;
                        StartCoroutine(AttackCooldown());
                    }
                }
                else if (movesMade == moveLimit && canAttack == true)
                {
                    StartCoroutine(burstGap());
                    movesMade = 0;
                }
                if (player == null)
                {
                    return;
                }
            }
        }
        isAlive = thisEnemyStats.isAlive;
        maxHealth = thisEnemyStats.maxHealth;
        currentHealth = thisEnemyStats.currentHealth;

        if (!isAlive)
        {
            canFight = false;
        }
        if (player == null)
        {
            return;
        }
    }
    void spellCast(GameObject spell_pref, Vector2 direction)
    {
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject newProjectile = Instantiate(spell_pref, transform.position, rotation);
        newProjectile.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * projectileSpeed;
        StartCoroutine(AttackCooldown());
    }
    IEnumerator AttackCooldown()
    {
        if (comboOver)
        {
            canAttack = false;
            yield return new WaitForSeconds(comboCooldown);
            canAttack = true;
        }
        else
        {
            canAttack = false;
            yield return new WaitForSeconds(attackCooldown);
            canAttack = true;
        }
    }
    public IEnumerator burstGap()
    {
        Vector2 direction = player.transform.position - this.transform.position;
        spellCast(spellPrefab, direction);
        yield return new WaitForSeconds(burstSeperation);
        spellCast(spellPrefab, direction);
        yield return new WaitForSeconds(burstSeperation);
        spellCast(spellPrefab, direction);
    }
    public void allowInput()
    {
        canFight = true;
    }
    public void stopInput()
    {
        canFight = false;
    }
    public void reassignPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Playerv2>();
    }
}

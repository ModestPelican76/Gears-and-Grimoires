using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class Frog : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;
    public float moveSpeed = 2f;
    public float movesMade = 0f;
    public float moveLimit = 2f;
    public float damage = 10f;
    public float tongueRange = 1.5f;
    public float attackCooldown = 2f;
    public float comboCooldown = 5f;
    public float attackTimer;
    public GameObject tonguePrefab;
    public Transform tongueSpawnPoint;
    public bool canAttack = true;
    public bool comboOver;
    public float projectileSpeed;
    public Playerv2 player;
    public float phase = 1f;
    private bool isAlive;
    public bool canfight = true;
    public HealthStats thisEnemyStats;
    public BattleZone associatedBattleZone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        thisEnemyStats = GetComponent<HealthStats>();
        isAlive = thisEnemyStats.isAlive;
        maxHealth = thisEnemyStats.maxHealth;
        currentHealth = thisEnemyStats.currentHealth;
        projectileSpeed = tonguePrefab.GetComponent<ProjectileProperties>().speed;
        damage = tonguePrefab.GetComponent<ProjectileProperties>().damage;
        attackCooldown = tonguePrefab.GetComponent<ProjectileProperties>().cooldown;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Playerv2>();
        comboOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.inBattle == true && isAlive == true && player.currentBattleZone == associatedBattleZone && associatedBattleZone.battleHasStarted)
        {
            if (movesMade < moveLimit && canAttack)
            {
                Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);

                if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
                {
                    // Move toward the player smoothly
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                }
                else
                {
                    // Only count the move once we reach the position
                    movesMade += 1;
                    comboOver = false;
                    StartCoroutine(AttackCooldown());
                }
            }
            else if (movesMade == moveLimit && canAttack == true)
            {
                if (phase == 1)
                {
                    tongueAttack(tonguePrefab);
                    movesMade = 0;
                }
                if (phase == 2)
                {
                    Vector2 direction = player.transform.position - this.transform.position;
                    tongueAttackPhase2(tonguePrefab, direction);
                    movesMade = 0;
                }
            }
        }
        if (phase == 1)
        {
            if (currentHealth <= maxHealth / 2)
            {
                phase = 2;
                moveLimit -= 1;
                moveSpeed += 1;
                attackCooldown -= 0.25f;
                comboCooldown -= 1f;
                projectileSpeed += 2f;
            }
        }
        isAlive = thisEnemyStats.isAlive;
        maxHealth = thisEnemyStats.maxHealth;
        currentHealth = thisEnemyStats.currentHealth;

        if (!isAlive)
        {
            canfight = false;
        }
        if (player == null)
        {
            return;
        }
    }
    void tongueAttack(GameObject tongue_pref)
    {
        GameObject tongue = Instantiate(tongue_pref, tongueSpawnPoint.position, tongueSpawnPoint.rotation);
        tongue.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -1) * projectileSpeed;
        comboOver = true;
        StartCoroutine(AttackCooldown());
    }
    void tongueAttackPhase2(GameObject tongue_pref, Vector2 direction)
    {
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject newProjectile = Instantiate(tongue_pref, transform.position, rotation);
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
    public void allowInput()
    {
        canfight = true;
    }
    public void stopInput()
    {
        canfight = false;
    }
}

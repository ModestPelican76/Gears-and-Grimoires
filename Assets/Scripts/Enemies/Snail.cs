using UnityEngine;

public class Snail : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;
    public float moveSpeed = 2f;
    public Playerv2 player;
    public float phase = 1f;
    private bool isAlive;
    public bool canFight = true;
    public bool inShell = true;
    public float vulnerableDuration = 3f;
    private float vulnerableTimer = 0f;
    private float shellTimer = 0f;
    public float vulnerableCooldown = 10f;
    public float projectileSpeed = 5f;
    public Color vulnerableColor;
    public Color shellColor;
    public HealthStats thisEnemyStats;
    public BattleZone associatedBattleZone;
    public BoxCollider2D shellCollider;
    public BoxCollider2D vulnerableCollider;
    public SpriteRenderer spriteRenderer;
    public GameObject projectile;
    public Transform attackSpawnPoint;
    public bool canShoot;
    public Sprite outShellSprite;
    public Sprite inShellSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        thisEnemyStats = GetComponent<HealthStats>();
        isAlive = thisEnemyStats.isAlive;
        maxHealth = thisEnemyStats.maxHealth;
        currentHealth = thisEnemyStats.currentHealth;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Playerv2>();
        shellTimer = 0f;
        vulnerableTimer = 0f;
        phase = 1f;
        canFight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.inBattle == true && isAlive == true && player.currentBattleZone == associatedBattleZone && associatedBattleZone.battleHasStarted)
        {
            if (!inShell)
            {
                vulnerableTimer += Time.deltaTime;
                if (vulnerableTimer >= vulnerableDuration)
                {
                    inShell = true;
                    shellCollider.enabled = true;
                    vulnerableCollider.enabled = false;
                    spriteRenderer.sprite = inShellSprite;
                    vulnerableTimer = 0f;
                }
                canShoot = true;
            }
            else
            {
                shellTimer += Time.deltaTime;
                if (shellTimer >= vulnerableCooldown)
                {
                    inShell = false;
                    Shoot();
                    shellCollider.enabled = false;
                    vulnerableCollider.enabled = true;
                    spriteRenderer.sprite = outShellSprite;
                    shellTimer = 0f;
                }
                canShoot = false;

            }
        }
    }
    public void Shoot()
    {
        GameObject spell = Instantiate(projectile, attackSpawnPoint.position, attackSpawnPoint.rotation);
        spell.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -1) * projectileSpeed;
    }
}

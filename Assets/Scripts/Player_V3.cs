using UnityEngine;
using System.Collections;
using DialogueEditor;

public class Player_V3 : MonoBehaviour
{
    public float karma = 0f;
    private float maxHealth = 100f;
    private float currentHealth;
    private float drunkHealth = 75f;
    [Header("Player Movement")]
    public float currentSpeed;
    public float moveSpeed = 3f;
    public float battleSpeed = 5f;
    [Header("Player State")]
    private bool isAlive = true;
    public bool playerInput = true;
    public bool inBattle;
    public bool canAttack = true;
    public bool isDrunk = false;
    public float drunkTimer = 5f;
    public BattleZone currentBattleZone;
    [Header("Puzzle States")]
    public bool hasSeeds = false;
    public bool hasApple = false;
    public bool plantedTree = false;
    public bool activatedStatue = false;
    public bool brokeBossDoor = false;
    public bool brokePuzzleDoor = false;
    [Header("Projectile Settings")]
    public float projectileSpeed;
    [Header("Parry Settings")]
    public float parryCooldown = 3f;
    public float parryDuration = 0.5f;
    private bool canParry = true;
    private bool parryLanded = false;

    [Header("Camera Settings")]
    public float cameraMoveSpeed = 5f;
    private Vector3 targetCameraPosition;
    private bool movingCamera = false;
    [Header("Health Potion Settings")]
    public int currentHealthPotions = 3;
    public float healthPotionsHealAmount = 25f;
    public float healthPotionsCooldown = 1f;
    public float healthPotionRegenTime = 12f;
    private bool canUseHealthPotion = true;
    public int maxHealthPotions = 3;
    [Header("References")]
    public Camera mainCamera;
    public GameObject initialCameraPosition;
    public TimeManager timeManager;
    public GameObject projectilePrefab;
    public HealthStats playerHealthStats;
    public BoxCollider2D parryCollider;
    public ScreenFade screenFade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHealthStats = GetComponent<HealthStats>();
        currentHealth = playerHealthStats.currentHealth;
        maxHealth = playerHealthStats.maxHealth;
        isAlive = playerHealthStats.isAlive;
        drunkHealth = playerHealthStats.drunkHealth;
        currentHealth = maxHealth;
        setCameraLocation(initialCameraPosition);
        projectileSpeed = projectilePrefab.GetComponent<ProjectileProperties>().speed;
        var colliders = this.GetComponentsInChildren<BoxCollider2D>();
        foreach (var col in colliders)
        {
            if (col.tag == "Parry Collider")
            {
                parryCollider = col;
                parryCollider.enabled = false;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerHealthStats.currentHealth;
        maxHealth = playerHealthStats.maxHealth;
        isAlive = playerHealthStats.isAlive;

        Inputs();
        setSpeed();

        if (movingCamera == true)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, Time.deltaTime * cameraMoveSpeed);
            if (Vector3.Distance(mainCamera.transform.position, targetCameraPosition) < 0.01f)
            {
                mainCamera.transform.position = targetCameraPosition;
                movingCamera = false;
            }
        }

        if (isDrunk)
        {
            StartCoroutine(soberUp());
        }
    }

    void Inputs()
    {
        if (playerInput)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector2(1, 1).normalized * Time.deltaTime * currentSpeed);
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector2(-1, 1).normalized * Time.deltaTime * currentSpeed);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector2(1, -1).normalized * Time.deltaTime * currentSpeed);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector2(-1, -1).normalized * Time.deltaTime * currentSpeed);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector2.up * Time.deltaTime * currentSpeed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector2.down * Time.deltaTime * currentSpeed);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.left * Time.deltaTime * currentSpeed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * Time.deltaTime * currentSpeed);
            }
            if (!inBattle && Input.GetKeyDown(KeyCode.E))
            {
                // Check for nearby NPCs to interact with
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("NPC"))
                    {
                        interactwithNPC(hitCollider.GetComponentInParent<NPC>());
                        break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (currentHealthPotions > 0 && currentHealth < maxHealth && canUseHealthPotion)
                {
                    StartCoroutine(healthpotionCooldown());
                    currentHealthPotions -= 1;
                    playerHealthStats.HealDamage(healthPotionsHealAmount);
                    StartCoroutine(healthPotionRegen());
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (canParry)
                {
                    parryCollider.enabled = true;
                    StartCoroutine(ParryCooldown());
                    Debug.Log("Parry Activated");
                }
            }
        }
    }
    public void setSpeed()
    {
        if (inBattle)
        {
            currentSpeed = battleSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
    }
    public void setCameraLocation(GameObject cameraPosition)
    {
        Debug.Log("Camera Position Set");
        targetCameraPosition = new Vector3(cameraPosition.transform.position.x, cameraPosition.transform.position.y, mainCamera.transform.position.z);
        movingCamera = true;
    }

    public void spawnProjectile(GameObject projectile, Vector2 direction)
    {
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject newProjectile = Instantiate(projectile, transform.position, rotation);
        newProjectile.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * projectileSpeed;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            Debug.Log("Floor Triggered");
            setCameraLocation(collision.transform.Find("Camera point").gameObject);
        }

        if (collision.CompareTag("Battle Area"))
        {
            Debug.Log("Battle Zone Triggered");
            inBattle = true;
        }
    }

    public void interactwithNPC(NPC npc)
    {
        Debug.Log("Interacting with NPC");
        if (npc.isPersonNPC)
        {
            if (karma >= 0)
            {
                ConversationManager.Instance.StartConversation(npc.goodKarmaConversation);
            }
            else
            {
                ConversationManager.Instance.StartConversation(npc.badKarmaConversation);
            }
        }
        else if (npc.isTimeTravelNPC)
        {
            if (timeManager.isPresent)
            {
                ConversationManager.Instance.StartConversation(npc.presentConversation);
            }
            else
            {
                ConversationManager.Instance.StartConversation(npc.futureConversation);
            }
        }
        else if (npc.isSeedBox)
        {
            if (!hasSeeds)
            {
                ConversationManager.Instance.StartConversation(npc.positiveConversation);
                hasSeeds = true;
            }
            else if (hasSeeds || plantedTree)
            {
                ConversationManager.Instance.StartConversation(npc.negativeConversation);
            }
        }
        else if (npc.isPlantPot)
        {
            if (hasSeeds)
            {
                ConversationManager.Instance.StartConversation(npc.positiveConversation);
                hasSeeds = false;
                plantedTree = true;
            }
            else if (!hasSeeds)
            {
                if (plantedTree)
                {
                    ConversationManager.Instance.StartConversation(npc.extraConversation);
                }
                else
                {
                    ConversationManager.Instance.StartConversation(npc.negativeConversation);
                }
            }
        }
        else if (npc.isAppleTree)
        {
            if (plantedTree)
            {
                if (!hasApple)
                {
                    ConversationManager.Instance.StartConversation(npc.positiveConversation);
                    hasApple = true;
                }
                else
                {
                    ConversationManager.Instance.StartConversation(npc.extraConversation);
                }
            }
            else
            {
                ConversationManager.Instance.StartConversation(npc.negativeConversation);
            }
        }
        else if (npc.isStatue)
        {
            if (hasApple)
            {
                if (!brokeBossDoor)
                {
                    ConversationManager.Instance.StartConversation(npc.positiveConversation);
                    activatedStatue = true;
                    timeManager.DestroyBossGate();
                    hasApple = false;
                    brokeBossDoor = true;
                }
            }
            else
            {
                if (activatedStatue)
                {
                    ConversationManager.Instance.StartConversation(npc.extraConversation);
                }
                else
                {
                    ConversationManager.Instance.StartConversation(npc.negativeConversation);
                }
            }
        }
        else if (npc.isButton)
        {
            if (!brokePuzzleDoor)
            {
                ConversationManager.Instance.StartConversation(npc.positiveConversation);
                timeManager.DestroyPuzzleGate();
                brokePuzzleDoor = true;

            }
            else
            {
                ConversationManager.Instance.StartConversation(npc.positiveConversation);
            }
        }
    }
    public void ModifyKarma(float amount)
    {
        karma += amount;
        Debug.Log("Karma modified by " + amount + ". New karma: " + karma);
    }
    public void preventInput()
    {
        playerInput = false;
    }
    public void allowInput()
    {
        playerInput = true;
    }
    [ContextMenu("Make Drunk")]
    public void becomeDrunk()
    {
        isDrunk = true;
        battleSpeed = -battleSpeed;
        playerHealthStats.currentHealth = drunkHealth;
    }
    public IEnumerator soberUp()
    {
        yield return new WaitForSeconds(drunkTimer);
        isDrunk = false;
        battleSpeed = -battleSpeed;
        playerHealthStats.currentHealth = maxHealth;
    }
    public IEnumerator healthpotionCooldown()
    {
        canUseHealthPotion = false;
        yield return new WaitForSeconds(healthPotionsCooldown);
        canUseHealthPotion = true;
    }
    public IEnumerator healthPotionRegen()
    {
        yield return new WaitForSeconds(healthPotionRegenTime);
        currentHealthPotions++;
    }
    public IEnumerator ParryCooldown()
    {
        canParry = false;
        yield return new WaitForSeconds(parryDuration);
        parryCollider.enabled = false;
        if (parryLanded)
        {
            canParry = true;
            parryLanded = false;
        }
        else
        {
            yield return new WaitForSeconds(parryCooldown);
            canParry = true;
            parryLanded = false;
        }
        canParry = false;
        do
        {
            parryLanded = false;
            parryCollider.enabled = true;
            yield return new WaitForSeconds(parryDuration);
            parryCollider.enabled = false;

            // If a parry happened during this window, loop again
        }
        while (parryLanded);

        // If loop breaks (no parry during last window), do cooldown
        yield return new WaitForSeconds(parryCooldown);
        canParry = true;
    }
    public void parryChildHit(Collider2D collision)
    {
        if (collision.CompareTag("Enemy Projectile"))
        {
            parryLanded = true;
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - (Vector2)transform.position;
            spawnProjectile(projectilePrefab, direction);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Non-Attack Parry"))
        {
            parryLanded = true;
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - (Vector2)transform.position;
            direction.Normalize();
            collision.GetComponent<BossShells>().direction = direction;
        }
    }
}

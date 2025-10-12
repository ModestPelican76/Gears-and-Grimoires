using System.Collections;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    public float explosionDamage = 10f;
    public float lifetime = 5f;
    public float timeleft;
    public float cooldown = 1f;
    public bool isExplosive = false;
    private bool hasExploded = false;
    public BoxCollider2D explosionCollider;
    public ParticleSystem explosionEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        timeleft = lifetime;
        if (isExplosive)
        {
            var colliders = this.GetComponentsInChildren<BoxCollider2D>();
            foreach (var col in colliders)
            {
                if (col.tag != "Enemy Projectile")
                {
                    explosionCollider = col;
                    explosionCollider.enabled = false;
                    break;
                }
            }
            explosionEffect = this.GetComponentInChildren<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;

        if (timeleft <= 0)
        {
            if (!isExplosive)
            {
                Destroy(gameObject);
            }
            else
            {
                if (!hasExploded)
                {
                    hasExploded = true;
                    explosiveProjectile();
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.tag == "Player Projectile" && collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Debug.Log("Hit Enemy");
            collision.GetComponent<HealthStats>().TakeDamage(damage);
        }
        /*if (this.tag == "Non-attack parry" && collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Debug.Log("Hit Enemy");
            collision.GetComponent<HealthStats>().TakeDamage(damage);
        }*/
        if (this.tag == "Enemy Projectile" && collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Hit Player");
            collision.GetComponent<HealthStats>().TakeDamage(damage);
        }
    }
    void explosiveProjectile()
    {
        explosionCollider.enabled = true;
        StartCoroutine(DestroyAfterExplosion());
    }
    IEnumerator DestroyAfterExplosion()
    {
        explosionEffect.gameObject.SetActive(true);
        explosionEffect.Play();
        while (explosionEffect.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}

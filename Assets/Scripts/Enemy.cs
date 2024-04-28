using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Player player;
    [SerializeField] private GameObject deathDropPrefab;
    [SerializeField] private LayerMask moveLayerMask;

    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private float attackChance = 0.5f;
    [SerializeField] private float moveChance = 0.5f;

    public GameObject DeathDropPrefab { get => deathDropPrefab; set => deathDropPrefab = value; }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// Handles dealing damage to the enemy, dropping an item, and starting the damage animation.
    /// </summary>
    /// <param name="damageToTake"></param>
    public void TakeDamage(int damageToTake)
    {
        health -= damageToTake;

        if(health <= 0)
        {
            if(DeathDropPrefab != null)
            {
                Instantiate(DeathDropPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

        StartCoroutine(DamageFlash());

        if(Random.value > attackChance)
        {
            player.TakeDamage(damage);
        }
    }

    /// <summary>
    /// Coroutine for animating the damage flash.
    /// </summary>
    /// <returns></returns>
    IEnumerator DamageFlash()
    {
        Color defaultColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = defaultColor;
    }

    /// <summary>
    /// Handles moving the enemy. Enemy has a 50% chance to move when the Player moves.
    /// </summary>
    public void Move()
    {
        // This creates a 50% chance that the Move() method will continue.
        if(Random.value < moveChance)
        {
            return;
        }

        Vector3 direction = Vector3.zero;
        bool canMove = false;

        while (canMove == false)
        {
            direction = GetRandomDirection();

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, moveLayerMask);

            if(hit.collider == null)
            {
                canMove = true;
            }
        }

        transform.position += direction;
    }

    /// <summary>
    /// Returns a random direction(up, down, left, or right).
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomDirection()
    {
        int random = Random.Range(0, 4);

        if(random == 0)
        {
            return Vector3.up;
        }
        else if(random == 1)
        {
            return Vector3.down;
        }
        else if(random == 2)
        {
            return Vector3.left;
        }
        else if(random == 3)
        {
            return Vector3.right;
        }

        return Vector3.zero;
    }
}

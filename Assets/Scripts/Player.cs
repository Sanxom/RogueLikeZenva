using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int coins;
    [SerializeField] private bool hasKey;

    [Header("Player References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LayerMask moveLayerMask;

    public bool HasKey { get => hasKey; set => hasKey = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Coins { get => coins; set => coins = value; }

    /// <summary>
    /// Handles moving the player.
    /// </summary>
    /// <param name="direction"></param>
    private void Move(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, moveLayerMask);

        if(hit.collider == null)
        {
            transform.position += new Vector3(direction.x, direction.y, 0);
            EnemyManager.instance.OnPlayerMove();
            Generation.instance.OnPlayerMove();
        }
    }

    public void OnMoveUp(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.up);
        }
    }
    public void OnMoveDown(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.down);
        }
    }
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.left);
        }
    }
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.right);
        }
    }

    /// <summary>
    /// Handles trying to attack in a direction.
    /// </summary>
    /// <param name="direction"></param>
    private void TryAttack(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, 1 << 7);

        if(hit.collider != null)
        {
            hit.transform.GetComponent<Enemy>().TakeDamage(1);
        }
    }

    public void OnAttackUp(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            TryAttack(Vector2.up);
        }
    }
    public void OnAttackDown(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            TryAttack(Vector2.down);
        }
    }
    public void OnAttackLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            TryAttack(Vector2.left);
        }
    }
    public void OnAttackRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            TryAttack(Vector2.right);
        }
    }

    /// <summary>
    /// Handles dealing damage to the player and starting the damage animation.
    /// </summary>
    /// <param name="damageToTake"></param>
    public void TakeDamage(int damageToTake)
    {
        CurrentHealth -= damageToTake;

        UIManager.instance.UpdateHealth(CurrentHealth);

        StartCoroutine(DamageFlash());

        if(CurrentHealth <= 0)
        {
            SceneManager.LoadScene(0);
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
    /// Handles adding dropped coins to the Player.
    /// </summary>
    /// <param name="amount"></param>
    public void AddCoins(int amount)
    {
        Coins += amount;
        UIManager.instance.UpdateCoinText(Coins);
    }

    /// <summary>
    /// Handles adding dropped health packs to the Player.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool AddHealth(int amount)
    {
        if(CurrentHealth + amount <= MaxHealth)
        {
            CurrentHealth += amount;
            UIManager.instance.UpdateHealth(CurrentHealth);
            return true;
        }

        return false;
    }
}

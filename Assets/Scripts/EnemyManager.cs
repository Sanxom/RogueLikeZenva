using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    public static EnemyManager instance;

    public List<Enemy> Enemies { get => enemies; set => enemies = value; }

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Handles moving the enemy when the player moves.
    /// </summary>
    public void OnPlayerMove()
    {
        StartCoroutine(MoveEnemies());
    }

    /// <summary>
    /// Coroutine for making sure the enemies move. Makes sure the enemies cannot move into the same spot as the Player.
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveEnemies()
    {
        yield return new WaitForFixedUpdate();

        foreach (Enemy enemy in Enemies)
        {
            if(enemy != null)
            {
                enemy.Move();
            }
        }
    }
}

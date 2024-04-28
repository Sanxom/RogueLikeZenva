using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Coin,
    Health
}

public class Pickup : MonoBehaviour
{
    [SerializeField] private PickupType type;
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (type)
            {
                case PickupType.Coin:
                    collision.GetComponent<Player>().AddCoins(value);
                    Destroy(gameObject);
                    break;
                case PickupType.Health:
                    if (collision.GetComponent<Player>().AddHealth(value))
                    {
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Damageable : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 15;
    
    private int _currentHealth;    
    
    private bool IsAlive => _currentHealth > 0;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isServer) return;
        
        if (other.CompareTag("Projectile"))
        {
            var damage = other.GetComponent<Projectile>().ProjectileDamage;
            TakeDamage(damage);
            NetworkServer.Destroy(other.gameObject);
        }
    }

    private void TakeDamage(int amount) 
    {
        _currentHealth -= amount;
        if (!IsAlive)
        {
            DieOnClient();
        }
    }

    [ClientRpc]
    private void DieOnClient()
    {
        NetworkServer.Destroy(gameObject);
    }
}

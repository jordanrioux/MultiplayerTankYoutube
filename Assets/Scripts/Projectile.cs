using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEditor;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float destroyAfter = 3.5f;    
    [SerializeField] private float speed = 5f;
    [SerializeField] private int projectileDamage = 3;

    public int ProjectileDamage => projectileDamage;

    private void Start()
    {
        Invoke(nameof(DestroySelf), destroyAfter); 
    }

    private void Update()
    {
        if (!isServer) return;
        
        var y = speed * Time.deltaTime;
        transform.Translate(0f, y, 0f);
    }

    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isServer) return;
        
        DestroySelf();
    }
}

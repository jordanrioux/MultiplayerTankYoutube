using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Mirror;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Tank : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 125f;
    [SerializeField] private float movementSpeed = 5f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fireRatePerSecond = 0.5f;

    private float _timeUntilNextFire;
    private float _horizontalVelocity;
    private float _verticalVelocity;
    
    private void Update()
    {
        if (!isLocalPlayer) return;
        
        _horizontalVelocity = Input.GetAxisRaw("Horizontal");
        _verticalVelocity = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && _timeUntilNextFire <= 0f)
        {            
            ShootCommand();
            _timeUntilNextFire = fireRatePerSecond;
        }

        _timeUntilNextFire -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        
        UpdateTankMovement();
    }

    private void UpdateTankMovement()
    {
        MoveTank();
        RotateTank();
    }

    private void MoveTank()
    {
        var speed = (_verticalVelocity > 0 ? movementSpeed : movementSpeed/2);
        var y = _verticalVelocity * speed * Time.deltaTime;
        transform.Translate(0f, y, 0f);
    }

    private void RotateTank()
    {
        var zAngle = (rotationSpeed * Time.deltaTime * -1f * _horizontalVelocity);
        transform.Rotate(0f, 0f, zAngle);
    }

    [Command]
    private void ShootCommand()
    {
        var projectile = Instantiate(projectilePrefab, spawnPoint.position, transform.rotation);
        NetworkServer.Spawn(projectile);
    }
}

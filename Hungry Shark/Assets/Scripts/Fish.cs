using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fish : MonoBehaviour
{
    public string _name = "";

    private Rigidbody2D _rb;

    public int _eatenValue = 100;

    public float _fleeDist = 3.0f;
    public float _speed = 3.0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public bool OnConsumed()
    {
        return GameManager._instance.CheckConsumeTarget(_name);
    }

    private void Update()
    {
        if (CheckIfCloseToFlee())
        {
            FleePlayer();
        }
        else
        {
            _rb.velocity = new Vector2(0.0f, 0.0f);
        }
    }

    private bool CheckIfCloseToFlee()
    {
        Vector3 vectorToPlayer = GameManager._instance._player.transform.position - transform.position;

        return (vectorToPlayer.magnitude >= _fleeDist);
    }

    private void FleePlayer()
    {
        Vector3 vectorToPlayer = GameManager._instance._player.transform.position - transform.position;
        Vector3 directionFromPlayer = -vectorToPlayer.normalized;

        _rb.velocity = new Vector2(directionFromPlayer.x * _speed, directionFromPlayer.y * _speed);
    }
}

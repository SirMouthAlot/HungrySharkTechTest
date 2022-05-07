using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //Rigidbody
    Rigidbody2D _rb;

    float _playerHealth = 100.0f;

    public AnimationCurve _speedCurve = new AnimationCurve();
    public float _maxDistanceSpeed = 20.0f;
    public float _playerSpeedMod = 5.0f;

    public float _positionMargin = 0.1f;
    public Vector3 _targetPosition = new Vector3(0.0f, 0.0f, 0.0f);
    Vector2 _currentPosition2D = new Vector2(0.0f, 0.0f);
    Vector2 _targetPosition2D = new Vector2(0.0f, 0.0f);
    Vector2 _movementVector2D = new Vector2(0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        //Get rigidbody and store it
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //If left click
        if (Input.GetMouseButton(0))
        {
            _targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        //Update current and target position 2D (truncates Z as this is a 2D game)
        _currentPosition2D = transform.position;
        _targetPosition2D = _targetPosition;

        _movementVector2D = _targetPosition2D - _currentPosition2D;
    }

    private void LateUpdate()
    {
        //Move and rotate towards points
        MoveTowardsTargetPoint();
        RotateTowardsMouse();

        CheckIfDead();
    }

    void MoveTowardsTargetPoint()
    {
        //If the length between current and target is greater than margine of error
        //Move the player towards the point
        if (_movementVector2D.magnitude >= _positionMargin)
        {
            float movementFactor = Mathf.Clamp(_movementVector2D.magnitude / _maxDistanceSpeed, 0.0f, 1.0f);

            //Use curve and evaluate factor of distance
            float playerSpeed = _speedCurve.Evaluate(movementFactor) * _playerSpeedMod;
            _rb.velocity = _movementVector2D.normalized * playerSpeed;
        }
    }

    void RotateTowardsMouse()
    {
        Vector2 lookAtDir;

        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        lookAtDir = Input.mousePosition - new Vector3(playerScreenPoint.x, playerScreenPoint.y, 0.0f);
        lookAtDir.Normalize();

        float angle = Mathf.Atan2(-lookAtDir.x, lookAtDir.y) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle + 90.0f, Vector3.forward);
    }

    void ConsumeFish(Collider2D other)
    {
        //Grab fish component
        Fish fish = other.gameObject.GetComponent<Fish>();

        //Run on consumed
        bool correctTarget = fish.OnConsumed();

        //Update game manager with data
        GameManager._instance.RemoveFishOfType(fish._name);

        //If your chosen target, then we just give points
        if (correctTarget)
        {
            GameManager._instance.OnConsumeTarget();
            ScoreManager.AddToScore(fish._eatenValue);
        }
        else
        {
            //Make fish sick
            ReduceFishHealth();
        }

        //Clean up
        Destroy(other.gameObject);

        GameManager._instance.PopulateSea();
    }

    void ReduceFishHealth()
    {
        //Reduce fish health
        _playerHealth -= 25.0f;
    }

    void CheckIfDead()
    {
        if (_playerHealth <= 0.0f)
        {
            GameManager._instance.QuitGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            //Eat the fish
            ConsumeFish(collision);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Speed Variable
    [SerializeField]
    private float _speed = 5f;
    private float _speedBoost = 1f;
    [SerializeField]
    private float _speedBoostMultiplier = 2f;

    [SerializeField]
    private int _score = 0;
   
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldsPrefab;

    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0f, 0.8f, 0f);

    [SerializeField]
    private bool _hasTripleShotPowerUp = false;
    [SerializeField]
    private bool _hasShieldPowerUp = false;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    private float _maxVertical = 5f;
    private float _minVertical = -3.5f;

    private float _maxHorizontal = 11.5f;
    private float _minHorizontal = -11.5f;

    private SpawnManager _spawnManager;
    private UIManager _uIManager;

    [SerializeField]
    private int _lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Assign a starting position to the player object = (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        // Find for the SpawnManager Component
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        // Find the UI Manager 
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        // No Damage
        _leftEngine.SetActive(false);
        _rightEngine.SetActive(false);

        // Errors:-
        if (_spawnManager == null)
            Debug.LogError("Spawn Manager doesn't exist");

        if (_uIManager == null)
            Debug.LogError("UI Manager is NULL");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        BoundCheck();
        Wrap();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)  FireLaser();

    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        GameObject shootLaser = _laserPrefab;
        Vector3 spawnPosition = transform.position;

        if (_hasTripleShotPowerUp)
            shootLaser = _tripleShotPrefab;

        else
            spawnPosition += _laserOffset;


        Instantiate(shootLaser, spawnPosition, Quaternion.identity);
    }

    void Wrap()
    {
        // Player Wrap
        if (transform.position.x > _maxHorizontal) transform.position = new Vector3(_minHorizontal, transform.position.y, 0);
        else if (transform.position.x < _minHorizontal) transform.position = new Vector3(_maxHorizontal, transform.position.y, 0);
    }

    void BoundCheck()
    {
        // Player Bounds
        if (transform.position.y > _maxVertical) transform.position = new Vector3(transform.position.x, _maxVertical, 0);
        else if (transform.position.y < _minVertical) transform.position = new Vector3(transform.position.x, _minVertical, 0);
    }

    void PlayerMovement()
    {
        // Translate the Player Model

        // Finding the Inputs using the Unity Input Mapping
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Using the Translate Function to Move the Player Model
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * _speed * _speedBoost);
    }

    public void Damage()
    {
        if(_hasShieldPowerUp)
        {
            _hasShieldPowerUp = false;
            _shieldsPrefab.SetActive(false);
            return;
        }

        _lives--;
        if (_lives == 2)
            _rightEngine.SetActive(true);

        else if (_lives == 1)
            _leftEngine.SetActive(true);

        _uIManager.ChangeLives(_lives);

        if (_lives <= 0)
        {
            _uIManager.GameOver();
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }

    public void UpdateScore(int amount)
    {
        _score += amount;
        _uIManager.UpdateScore(_score);
    }

    public void pickedUp(int powerup)
    {
        switch(powerup)
        {
            case 0:
                _hasTripleShotPowerUp = true;
                StartCoroutine(Cooldown(powerup));
                break;

            case 1:
                _speedBoost = _speedBoostMultiplier;
                StartCoroutine(Cooldown(powerup));
                break;

            case 2:
                _hasShieldPowerUp = true;
                // StartCoroutine(Cooldown(powerup));
                _shieldsPrefab.SetActive(true);
                break;
      
        }
    }

    IEnumerator Cooldown(int powerup)
    {
        yield return new WaitForSeconds(5f);
        switch (powerup)
        {
            case 0:
                _hasTripleShotPowerUp = false;
                break;

            case 1:
                _speedBoost = 1;
                break;

            case 2:
                // _hasShieldPowerUp = false;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;

    [SerializeField]
    private Animator _explosion;

    private float _minVertical = -13f;
    private float _maxVertical = 10f;

    private float _maxHorizontal = 9f;
    private float _minHorizontal = -9f;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _explosion = gameObject.GetComponent<Animator>();

        if (_explosion == null)
            Debug.LogError("Explosion Animator is NULL");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= _minVertical)
            transform.position = new Vector3(Random.Range(_minHorizontal, _maxHorizontal), _maxVertical, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Laser":
                            _player.UpdateScore(10);
                            Destroy(other.gameObject);

                            _explosion.SetTrigger("isEnemyDead");
                            _enemySpeed = 0;
                            Destroy(this.gameObject, 2.8f);
                            break;

            case "Player":                            
                            if (_player != null)
                                _player.Damage();

                            _explosion.SetTrigger("isEnemyDead");

                            Destroy(this.gameObject, 2.8f);
                            break;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _gameOver;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private Image _lives;

    [SerializeField]
    private Sprite[] _livesSprites;

    private bool _canRestart = false;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _scoreText.text = "Score: " + 0;
        _lives.sprite = _livesSprites[3];
        _gameOver.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }
    
    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void ChangeLives(int lives)
    {
        _lives.sprite = _livesSprites[lives];
    }

    public void GameOver()
    {
        _gameManager.GameOver();
        _gameOver.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(TextFlicker(_gameOver.gameObject));
    }

    IEnumerator TextFlicker(GameObject obj)
    {
        bool val = true;
        while(true)
        {
            obj.SetActive(val);
            yield return new WaitForSeconds(0.5f);
            val = !val;
        }
    }
}

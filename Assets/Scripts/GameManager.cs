using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Canvas _titleUI;
    [SerializeField] private TextMeshProUGUI _titleText;

    [SerializeField] private Canvas _HUD;
    [SerializeField] private TextMeshProUGUI _timerText;

    [SerializeField] private Canvas _finalUI;
    [SerializeField] private TextMeshProUGUI _timerText_onFinalUI;

    [SerializeField] private GameObject _tilelineObj;
    [SerializeField] private GameObject _player;
    [SerializeField] private CinemachineFreeLook _playerCinemachineCamera;

    private bool _isStarted = false, _isPaused =  false;
    private float _time = 0;

    [HideInInspector] public Vector3 playerInitPosition;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Update()
    {
        if (_isStarted && !_isPaused)
        {
            _time += Time.deltaTime;

            float mins = _time / 60;
            float secs = _time % 60;

            _timerText.text = mins.ToString("00") + ":" + secs.ToString("00");
        }
    }

    public void StartGame()
    {
        if (!_isStarted)
        {
            playerInitPosition = _player.transform.position;

            _titleText.GetComponent<BlinkText>().enabled = false;
            _titleText.color = new Color(_titleText.color.r, _titleText.color.g, _titleText.color.b, 1);
            _titleUI.gameObject.SetActive(false);
            _tilelineObj.SetActive(false);
            _HUD.gameObject.SetActive(true);
            _player.GetComponent<PlayerController>().enabled = true;
            _playerCinemachineCamera.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _isStarted = true;
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        _titleUI.gameObject.SetActive(true);
        _titleText.text = "- PAUSE! Click to Resume -";
        _playerCinemachineCamera.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _isPaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _titleUI.gameObject.SetActive(false);
        _tilelineObj.SetActive(false);
        _playerCinemachineCamera.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _isPaused = false;
        Time.timeScale = 1;
    }

    public void FinishGame()
    {
        _finalUI.gameObject.SetActive(true);
        _HUD.gameObject.SetActive(false);
        _player.GetComponent<PlayerController>().enabled = false;
        _player.GetComponent<Rigidbody>().isKinematic = true;
        _playerCinemachineCamera.enabled = false;
        _isStarted = false;
        _timerText_onFinalUI.text = "Used Time: " + _timerText.text;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResetGame()
    {
        Time.timeScale = 1;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}

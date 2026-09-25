using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool _muted;
    private AudioManager _audioManager;

    [SerializeField] TMP_Text _muteButtonText;

    void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        pauseMenu.SetActive(false);
        _muted = _audioManager.GetMuteStatus();
        SetMuteButtonText();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScreen");
    }

    public void MuteOrUnmute()
    {
        if (!_muted) _audioManager.Mute();
        else _audioManager.Unmute();

        _muted = !_muted;
        SetMuteButtonText();
    }

    private void SetMuteButtonText()
    {
        string text = _muted ? "UNMUTE" : "MUTE";
        _muteButtonText.SetText(text);
    }
}

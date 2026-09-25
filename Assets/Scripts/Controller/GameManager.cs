using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

class BonusItem
{
    public int nextLevel;
    public float timeout;

    public BonusItem(int nextLevel, float timeout)
    {
        this.nextLevel = nextLevel;
        this.timeout = timeout;
    }
}

public class GameManager : MonoBehaviour
{

    private AudioManager audioManager;

    [SerializeField]
    private PointCounterController _pointCounter;


    public GameObject floatingPoints;

    // BONUS ELEMENTS
    [SerializeField]
    private Animator _bonusAnimator;

    [SerializeField]
    private Slider _bonusSlider;

    [SerializeField]
    private GameObject _bonusTextObject;

    private Animator _bonusTextAnimator;
    private TMP_Text _bonusText;
    private static int MAX_BONUS_LEVEL = 9;
    private int _currentBonusLevel;
    private float _activeTimeout;
    private int _accumulatedPoints;
    private Dictionary<int, BonusItem> _bonusMap = new Dictionary<int, BonusItem>();



    void Awake()
    {
        GameObject audioGameObject = GameObject.Find("AudioManager");
        if (audioGameObject)
        {
            audioManager = audioGameObject.GetComponent<AudioManager>();
        }

        if (_bonusTextObject)
        {
            _bonusTextAnimator = _bonusTextObject.GetComponent<Animator>();
            _bonusText = _bonusTextObject.GetComponent<TMP_Text>();
        }

        InitializeBonusMap();
    }
    void Start()
    {
        if (!audioManager) return;
        audioManager.PlayBackground(audioManager.bg_gameScene);
    }

    void Update()
    {
        ManageBonusState();
    }

    private void ManageBonusState()
    {

        // If activeTimer is up to 0f, decrease it;
        if (_activeTimeout > 0f)
        {
            _activeTimeout -= Time.deltaTime;

            // If bonus level is up to 2, show manage slider
            if (_currentBonusLevel > 1)
            {
                _bonusSlider.value = _activeTimeout;
            }

            Debug.Log("Tiempo restante: " + _activeTimeout);
            // If timer is zero, restore bonus to default state
            if (_activeTimeout <= 0f)
            {
                _accumulatedPoints = 0;
                if (_currentBonusLevel > 1)
                {
                    ResetBonus();
                }
                return;
            }
        }

        BonusItem _currentBonusData = _bonusMap[_currentBonusLevel];

        if (_accumulatedPoints >= _currentBonusData.nextLevel)
        {
            int nextLevel = Math.Min(_currentBonusLevel + 1, MAX_BONUS_LEVEL);
            SetBonusLevel(nextLevel);
        }
    }

    public void SetBonusLevel(int level)
    {
        // Set point counter and reset accumulated points
        _currentBonusLevel = level;
        _pointCounter.SetBonusLevel(_currentBonusLevel);
        _accumulatedPoints = 0;
        BonusItem _currentBonusData = _bonusMap[_currentBonusLevel];

        // If timeout exists, start the timer
        if (_currentBonusData.timeout > 0f)
        {
            _activeTimeout = _currentBonusData.timeout;
            _bonusSlider.maxValue = _activeTimeout;
            _bonusSlider.value = _activeTimeout;
        }

        // Display the bonus animator
        _bonusAnimator.SetTrigger("UpdateBonus");
        _bonusText.text = "BONUS \n X" + _currentBonusLevel;
        _bonusTextAnimator.SetTrigger("Blink");

        // Play the audio
        if (audioManager)
        {
            audioManager.PlaySFX(audioManager.sfx_bonus);
        }
    }

    public void ResetBonus()
    {
        _currentBonusLevel = 1;
        _accumulatedPoints = 0;
        _pointCounter.SetBonusLevel(_currentBonusLevel);
        _bonusAnimator.SetTrigger("UpdateBonus");

        if (audioManager)
        {
            audioManager.PlaySFX(audioManager.sfx_lostBonus);
        }

        _bonusText.text = "BONUS LOST";
        _bonusTextAnimator.SetTrigger("Blink");
    }

    private void InitializeBonusMap()
    {
        _bonusMap.Add(1, new BonusItem(800, 15f));
        _bonusMap.Add(2, new BonusItem(1000, 10f));
        _bonusMap.Add(3, new BonusItem(2000, 8f));
        _bonusMap.Add(4, new BonusItem(3500, 8f));
        _bonusMap.Add(5, new BonusItem(5000, 6f));
        _bonusMap.Add(6, new BonusItem(6000, 6f));
        _bonusMap.Add(7, new BonusItem(9000, 6f));
        _bonusMap.Add(8, new BonusItem(12000, 4f));
        _bonusMap.Add(9, new BonusItem(17000, 2f));

        _currentBonusLevel = 1;
        _activeTimeout = 0f;
        _accumulatedPoints = 0;

    }

    public void AddPoints(int reward, Vector3 position)
    {

        int rewardPlusBonus = reward * _currentBonusLevel;

        // Set FLoating Points text
        floatingPoints.GetComponentInChildren<TextMesh>().text = "+" + rewardPlusBonus;

        // Display floating Points
        Instantiate(floatingPoints, position, Quaternion.identity);

        // Add the points to the score counter
        if (_pointCounter != null)
        {
            _pointCounter.AddPoints(rewardPlusBonus);
            _accumulatedPoints += rewardPlusBonus;
        }

        // If a timer is active, add some time to the player as an advantage
        if (_activeTimeout > 0f)
        {
            _activeTimeout += Math.Min(reward / 100, 1f); // TODO: Check values for the reward
        }
        // In other case, if the currentBonusLevel is the first, start again the timer
        else if (_currentBonusLevel == 1)
        {
            _activeTimeout = _bonusMap[1].timeout;
        }
    }


}

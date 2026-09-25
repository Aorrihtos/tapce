using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmPlayerID : MonoBehaviour
{

    [SerializeField]
    private TMP_Text _firstLetter;

    [SerializeField]
    private TMP_Text _secondLetter;
    [SerializeField]
    private TMP_Text _thirdLetter;

    public void ConfirmIDAndStart()
    {
        // Get the ID and store it
        string currentID = _firstLetter.text + _secondLetter.text + _thirdLetter.text;
        PlayerPrefs.SetString("playerID", currentID);

        // Start the game
        SceneManager.LoadScene("GameScene");
    }
}

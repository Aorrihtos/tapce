using System.Linq;
using TMPro;
using UnityEngine;

public class ChangeIDLetter : MonoBehaviour
{

    char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();



    private TMP_Text _letter;

    void Awake()
    {
        var letterObj = transform.Find("Letter");
        _letter = letterObj.GetComponent<TMP_Text>();

        // Set the initial of the last player ID introduced
        string _currentID = PlayerPrefs.GetString("playerID");
        Debug.Log(_currentID);
        Debug.Log(letterObj.tag);
        if (_currentID != null && _currentID != "")
        {
            switch (letterObj.tag)
            {
                case "FirstLetter": _letter.SetText(_currentID.ToArray()[0].ToString()); break;
                case "SecondLetter": _letter.SetText(_currentID.ToArray()[1].ToString()); break;
                case "ThirdLetter": _letter.SetText(_currentID.ToArray()[2].ToString()); break;
            }
        }

    }

    public void PreviousLetter()
    {
        int index = alpha.ToList().FindIndex(0, letter => letter.ToString() == _letter.text);
        int newLetterIndex = index - 1 < 0 ? alpha.Length - 1 : index - 1;
        _letter.SetText(alpha[newLetterIndex].ToString());
    }

    public void NextLetter()
    {
        int index = alpha.ToList().FindIndex(0, letter => letter.ToString() == _letter.text);
        int newLetterIndex = index + 1 > alpha.Length - 1 ? 0 : index + 1;
        _letter.SetText(alpha[newLetterIndex].ToString());
    }
}

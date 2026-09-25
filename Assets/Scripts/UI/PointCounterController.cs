using TMPro;
using UnityEngine;

public class PointCounterController : MonoBehaviour
{

    [SerializeField]
    private TMP_Text _counterUI;

    [SerializeField]
    private TMP_Text _bonusUI;
    private static int _points;

    void Awake()
    {
        _counterUI.SetText(_points.ToString().PadLeft(4, '0'));
    }

    public void AddPoints(int reward)
    {
        _points += reward;
        _counterUI.SetText(_points.ToString().PadLeft(4, '0'));
    }

    public void ResetPoints()
    {
        _points = 0;
        _counterUI.SetText(_points.ToString().PadLeft(4, '0'));
    }

    public int GetPoints()
    {
        return _points;
    }

    public void SetBonusLevel(int bonusLevel)
    {
        _bonusUI.SetText("x" + bonusLevel.ToString());
    }
}

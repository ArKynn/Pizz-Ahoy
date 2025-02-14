using Main_Assets.Scripts;
using TMPro;
using UnityEngine;

public class CalendaryText : MonoBehaviour
{
    private DayManager _dayManager;
    private TMP_Text _text;
    private int _day;

    private void Start()
    {
        _dayManager = FindFirstObjectByType<DayManager>();
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (_dayManager.day != _day)
        {
            _text.text = $"Day {(_day = _dayManager.day).ToString()}\n {_dayManager.nextQuotaDay - _day} days until quota";
        }
    }
}
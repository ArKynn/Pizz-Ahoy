using System;
using Main_Assets.Scripts;
using TMPro;
using UnityEngine;

public class BalanceText : MonoBehaviour
{
    private GameManager _gameManager;
    private TMP_Text _text;
    private int _money = -1; 

    private void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (_gameManager.money != _money)
        {
            _text.text = $"{(_money = _gameManager.money).ToString()} Gold";
        }
    }
}

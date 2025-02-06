using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class Timer : MonoBehaviour
{
    [Header("Countdown")]
    [Tooltip("here you can change the starting time")]
    [SerializeField] float _startingTime = 10f;//the first time
    [SerializeField] TextMeshProUGUI _contdowntext;
    const float TIMESET = .5f;
  [HideInInspector] public float _currentTime = 0f;//current time
    PlayerController _player;
    event  EventHandler SendDie;
    void Start()
    {
        _player= FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        _currentTime = _startingTime + TIMESET;
        SendDie += PlayerRequest;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player._isMove)
        {
            if (_currentTime > 0)
                _currentTime -= 1 * Time.deltaTime;
            else
            {
                _currentTime = 0;
                SendDie?.Invoke(this, EventArgs.Empty);
                SendDie -= PlayerRequest;
            }
        }
      

        int second = (int)(_currentTime % 60);
        int minute = (int)(_currentTime / 60) % 60;
        int hour = (int)(_currentTime / 3600) % 24;
        string timerString = string.Format("{0:0}:{1:00}", minute, second);
        _contdowntext.text = timerString;

       
    }
    private void PlayerRequest(object sender, EventArgs e)=> _player.PlayerDie();
}

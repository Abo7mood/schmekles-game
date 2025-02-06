using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Chest : MonoBehaviour
{
    PlayerController player;
    public GameObject _p;
    public TextMeshProUGUI[] _coinsUI;
    const string CANVAS = "Canvas", COINSAMOUNT = "CoinsAmount", HEALTHAMOUNT = "HealthAmount";
    [SerializeField] int _scoreAdd;
    private void Awake()
    {
         player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
    }
    private void Start()
    {
        if (_coinsUI[0] != null)
            _coinsUI[1].text = _coinsUI[0].text;

    }
    public void Increase()
    {
        var CoinObject = Instantiate(_p, transform.position, Quaternion.identity, null);
        Destroy(CoinObject, 30);
        CoinObject.transform.Find(CANVAS).transform.Find(COINSAMOUNT).GetComponent<TextMeshProUGUI>().text = _scoreAdd.ToString();
        player._currentScore+= _scoreAdd;
        player._coins.text = player._currentScore.ToString();
        SoundManager.instance.SoundPlayer(12);
      
    }
}

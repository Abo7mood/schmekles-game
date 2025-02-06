using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Coin : MonoBehaviour
{
    public GameObject _p;
    public TextMeshProUGUI[] _coinsUI;

    private void Start()
    {
        if (_coinsUI[0] != null)
            _coinsUI[1].text = _coinsUI[0].text;
      
    }

    
}

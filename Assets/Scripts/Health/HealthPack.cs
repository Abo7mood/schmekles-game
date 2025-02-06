using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HealthPack : MonoBehaviour
{
    public GameObject _p;
    public TextMeshProUGUI[] _healthUI;

    private void Start()
    {
        if (_healthUI[0]!= null)       
        _healthUI[1].text = _healthUI[0].text;
        
    }
}

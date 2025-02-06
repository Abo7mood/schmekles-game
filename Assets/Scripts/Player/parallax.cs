using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    private float _lengthX, _startPosX;
    [SerializeField] GameObject _cam;
    [SerializeField] float _parallaxEffect;
    private void Start()
    {
        _startPosX = transform.position.x;
        _lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
    

    }
    private void Update()
    {

        float tempx = (_cam.transform.position.x * (1 - _parallaxEffect));
        float distx = (_cam.transform.position.x * _parallaxEffect);
        transform.position = new Vector3(_startPosX + distx, transform.position.y, transform.position.z);

        if (tempx > _startPosX + _lengthX) _startPosX += _lengthX;
        else if (tempx < _startPosX - _lengthX) _startPosX -= _lengthX;
      

    }
}


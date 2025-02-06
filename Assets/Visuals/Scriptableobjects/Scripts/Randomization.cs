using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomization : MonoBehaviour
{
    [SerializeField] RectTransform _canvas;//the canvas position
    public Vector2 _preview;//vector 2 to preview the positon
   [SerializeField] float _randXMin, _randXMax;// the minimum and maximum for x
   [SerializeField] float _randYMin, _randYMax;// the minimum and maximum for y
    void Start()
    {
        float rand1 = Random.Range(_randXMin, _randXMax);//random value between x min and x max
        float rand2 = Random.Range(_randYMin, _randYMax);//random value between y min and y max
        _preview = new Vector2(rand1, rand2);//set the preview posisition
        _canvas.localPosition= _preview;//set the canvas posisition
    }
   
   
}

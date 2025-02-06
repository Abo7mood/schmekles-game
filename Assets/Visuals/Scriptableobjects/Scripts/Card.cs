using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Bullet")]
public class Card : ScriptableObject
{//varibles for the bullet
    public new string name;
    public Sprite bulletSprite;
    public float bulletSpeed;
    public float bulletDestroyTime;
    public bool isEnemyTarget;
    public GameObject particle;
}

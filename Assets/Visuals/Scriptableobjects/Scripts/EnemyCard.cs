using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Enemy Card", menuName = "Cards/Enemy")]
public class EnemyCard : ScriptableObject
{//varibles for the enemy
    public new string name;
    public float enemySpeed;
    public float enemyDistance;
    public float enemyAttackRange;
    public float enemyAttackCooldown;
    public int enemyMaxHealth;
    public int enemyCoinsAmount;
    public int enemyDamageMeele;
    public int enemyDamageBody;

}

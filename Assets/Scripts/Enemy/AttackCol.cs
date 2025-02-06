using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCol : MonoBehaviour
{
    [SerializeField] EnemyController _enemy;// the enemy script
    const string PLAYER = "Player";// names
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
    }
   
}

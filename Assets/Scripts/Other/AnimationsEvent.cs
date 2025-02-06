using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsEvent : MonoBehaviour
{
    //this script to call function from another scripts, animationevent, you have to put it on the gameobject animator
    [Header("Constructer")]
   [SerializeField] EnemyController _enemy; // enemy controller reference , you have to drag and drop the enemy here
    [SerializeField] PlayerController _player; // enemy controller reference , you have to drag and drop the enemy here
    [SerializeField] Punch _punch; // Punch reference , you have to drag and drop the Punch here
    [SerializeField] Punch _punch2; // Punch reference , you have to drag and drop the Punch here
    [SerializeField] float _deathTime;//the time to enemy death brr
    [SerializeField] Chest _chest;
    public void AttackEvent2Enemy()//attack func
    {
       
        _enemy._direction = 1;//change the direction
        _enemy._speed = _enemy._realSpeed;//freeze the enemy speed
        _enemy.GetComponentInChildren<Animator>().ResetTrigger("Attack");//Reset The Animation
    }
    public void AttackEvent1Enemy()
    {
        _enemy.RealAttack();
    }
    public void PlayerLive()//call the playerlive func from the playercontroller
    {
        _player.PlayerLive();
    }
    public void playerthrow() => _player.RockThrow();
    public void EnemyDie() => Destroy(_enemy.gameObject, _deathTime);
    public void Punch1() => _punch.EnableTheCollider();
    public void Punch2() => _punch2.EnableTheCollider();
    public void MoveChanger() => _player.MoveChanger();
}

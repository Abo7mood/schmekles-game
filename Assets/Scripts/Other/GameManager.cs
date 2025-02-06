using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    #region Constrcuters
    public KeyCode _jump;
    public KeyCode _jump2;
    public KeyCode _shoot;
    public KeyCode _attack;

    #endregion
    #region floats
    [Header("DamageValues")]
    [Tooltip("here you can edit the damages values inside the game")]
    //when the player hit the enemy , he will push the enemy for some values, here is the values for each attack
    public float _damageDistanceAttack1;//distance when player run
    public float _damageDistanceAttack2;//distance when player stop
    public float _damageDistanceAttack3;//distance when player throw a rock


    #endregion
    #region ints
    //enemies damage


    //player damage
    public int _playerRockDamage;
    public int _playerPunch1Damage;
    public int _playerPunch2Damage;
    #endregion
}

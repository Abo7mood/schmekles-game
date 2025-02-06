using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()=>        instance = this;
   [Tooltip("Here is the Main Source to put the background sound")]
    [SerializeField] AudioSource MainSource;
  
    [Header("Audios")]
    [Tooltip("Here is the SoundList to put all your sound effects")]
    [SerializeField] AudioClip[] _audios;
    
enum Audio { mainSound,playerAttack,playerAttackRange, jumpSound, playerDamage, rock, dead,coins,healthPick,
        enemyAttackMeele, enemyAttackRange , enemyDamage,Chest }
   
    public void SoundPlayer(int audioNum)
    {
        GameObject _audiosGameObject = new GameObject();
        _audiosGameObject.AddComponent<AudioSource>();
           GameObject soundObject = Instantiate(_audiosGameObject, transform.position,
   Quaternion.identity, null);
        soundObject.GetComponent<AudioSource>().clip = _audios[audioNum];
        soundObject.GetComponent<AudioSource>().Play();
        Destroy(soundObject, 5f);
    }
  
}

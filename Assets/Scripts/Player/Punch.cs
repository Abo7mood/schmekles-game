using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Punch : MonoBehaviour
{
    /// <summary>
    /// 
    /// punch 2 is better and bigger than punch 1 , it's the punch when the player doesn't move at all
    /// </summary>
    [Header("Constructer")]
    [SerializeField] BoxCollider2D _boxCollider;//the box collider for the pinch
    [SerializeField] GameObject _poof;//the particle
    PlayerController _player;//player script reference
   [SerializeField] EnemyController _enemy;//enemy script reference
    [SerializeField] EnemyCard _enemyCard;//enemy scripable object 
    //const names
    const string PLAYER = "Player", ENEMY = "Enemy", PUNCH1 = "Punch1", PUNCH2 = "Punch2",
        PUNCH1ENEMY = "EnemyPunch1";// names


    //ui

    const string DAMAGEBACK = "DamageBack", DAMAGE = "Damage";

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
    }
    private void Start()
    {
        _boxCollider.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER))//check if the object is player and the player isn't damaged and he isn't an enemy
        {
            if (gameObject.name == PUNCH1ENEMY)
            {
                Debug.Log(PUNCH1ENEMY + " we hit it with the third punch");             
                    other.gameObject.GetComponent<PlayerController>().
                                        DamageOverride(_enemyCard.enemyDamageMeele);//call the damage function
                var particle = Instantiate(_poof, transform.position, Quaternion.identity, null);
                Destroy(particle, 15);
                SoundManager.instance.SoundPlayer(4);//enable the sound

            }
            CinemachineShake.instance.Shaker(5, .2f);
        }
            

        if (other.CompareTag(ENEMY) && !
          other.gameObject.GetComponent<EnemyController>()._isDamage)//check if the enemy isn't damaged 
        {
            SoundManager.instance.SoundPlayer(11);//enable the sound

            var particle = Instantiate(_poof, transform.position, Quaternion.identity, null);
            Destroy(particle, 15);
            if (gameObject.name == PUNCH1)
            {
                Debug.Log(PUNCH1 + " we hit it with the first punch");
                if (_player._sprite.flipX)
                {
                    other.gameObject.GetComponent<EnemyController>().
                   DamageOverride(GameManager.instance._playerPunch1Damage, -GameManager.instance._damageDistanceAttack1);//call the damage function
                    CinemachineShake.instance.Shaker(.5f, .2f);

                }

                else
                {
                    CinemachineShake.instance.Shaker(1, .2f);

                    other.gameObject.GetComponent<EnemyController>().
                                   DamageOverride(GameManager.instance._playerPunch1Damage, GameManager.instance._damageDistanceAttack1);//call the damage function
                }
                var DamageParticle = Instantiate(other.GetComponent<EnemyController>()._damageAmount, other.transform.localPosition, Quaternion.identity, null);// create the damage particle

                DamageParticle.transform.Find(DAMAGE).GetComponent<TextMeshProUGUI>().text = GameManager.instance._playerPunch1Damage.ToString();//set the value for the text
                DamageParticle.transform.Find(DAMAGEBACK).GetComponent<TextMeshProUGUI>().text = GameManager.instance._playerPunch1Damage.ToString();//set the value for the text

                Destroy(DamageParticle, 30);// destroy the damage particle
            }
            if (gameObject.name == PUNCH2)
            {
                Debug.Log(PUNCH2 + " we hit it with the second punch");
                if (_player._sprite.flipX)
                {
                    other.gameObject.GetComponent<EnemyController>().
                                        DamageOverride(GameManager.instance._playerPunch2Damage, -GameManager.instance._damageDistanceAttack2);//call the damage function
                }

                else
                {
                    other.gameObject.GetComponent<EnemyController>().
                                     DamageOverride(GameManager.instance._playerPunch2Damage, GameManager.instance._damageDistanceAttack2);//call the damage function
                }

                var DamageParticle = Instantiate(other.GetComponent<EnemyController>()._damageAmount,other.transform.localPosition, Quaternion.identity, null);// create the damage particle
                DamageParticle.transform.Find(DAMAGE).GetComponent<TextMeshProUGUI>().text = GameManager.instance._playerPunch2Damage.ToString();//set the value for the text
                DamageParticle.transform.Find(DAMAGEBACK).GetComponent<TextMeshProUGUI>().text = GameManager.instance._playerPunch2Damage.ToString();//set the value for the text

                Destroy(DamageParticle, 30);// destroy the damage particle

            }
           

        }




    }
    IEnumerator Cooldown(float seconds)
    {
        _boxCollider.enabled = true;
        yield return new WaitForSeconds(seconds);
        _boxCollider.enabled = false;
    }
    public void EnableTheCollider() 
    {
        if (this != null)
        StartCoroutine(Cooldown(0.05f));
    }
  
}

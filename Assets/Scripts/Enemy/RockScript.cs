using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RockScript : MonoBehaviour
{
    [Header("Constructer")]
    [SerializeField] Card bulletCard;// reference for bullet card scriptable object
    [SerializeField] GameObject _hitEffect;
    PlayerController _player;
    [SerializeField] EnemyCard _enemyCard;//enemy scripable object 
    [Header("Varbiles")]
    [SerializeField] float _speed;// the speed of this bullet
    [HideInInspector] public bool isright;//check if player facing right
    const string PLAYER = "Player",ENEMY="Enemy";// names

     int PLAYERDAMAGE = 1;// damage numbers

    bool _isEnemy;
    //ui
    const string DAMAGEBACK= "DamageBack", DAMAGE="Damage";
    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        
    }
    void Start()
    {
        gameObject.name = bulletCard.name;// set the name of the bullet by the name of the scribtable object card bullet
        gameObject.GetComponent<SpriteRenderer>().sprite = bulletCard.bulletSprite;//set the sprite of the bullet by the sprite of the scribtable object card bullet
        _speed = bulletCard.bulletSpeed;//set the speed of the bullet by the speed of the scribtable object card bullet
        _isEnemy = bulletCard.isEnemyTarget;//set the type of this script
        _hitEffect = bulletCard.particle;
        if (_enemyCard == null)
            return;
        PLAYERDAMAGE = _enemyCard.enemyDamageMeele;

    }

   
    void Update()
    {
        MoveTheRock(_speed); //call the function and speed parameter
    }
    private void MoveTheRock(float speed)// the function and speed parameter
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);//move the object from right to left by the speed
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER)&&!_player._isDamage&&!_isEnemy)//check if the object is player and the player isn't damaged and he isn't an enemy
        {
            CinemachineShake.instance.Shaker(10, .2f);

            _player.DamageOverride(PLAYERDAMAGE);//call the damage function
            var particle = Instantiate(_hitEffect, transform.position, Quaternion.identity, null);
            Destroy(particle, 15);
            SoundManager.instance.SoundPlayer(5);//enable the sound

            Destroy(gameObject);

        }
        else if (other.CompareTag(ENEMY) && _isEnemy && !other.gameObject.GetComponent<EnemyController>()._isDamage)//check if the object is enemy and the enemy isn't damaged and he is an enemy
        {
            CinemachineShake.instance.Shaker(1.5f, .2f);

            if (isright)//check if the rock facing left 
            {
                Debug.Log("right");
                other.gameObject.GetComponent<EnemyController>().DamageOverride
               (GameManager.instance._playerRockDamage, -GameManager.instance._damageDistanceAttack3);//call the damage function, and change the damage distance to negative
            }
            else //check if the rock facing right 
            {
                Debug.Log("left");
                other.gameObject.GetComponent<EnemyController>().DamageOverride
              (GameManager.instance._playerRockDamage, GameManager.instance._damageDistanceAttack3);//call the damage function, and change the damage distance to positive
            }
            var particle = Instantiate(_hitEffect, transform.position, Quaternion.identity, null);
            Destroy(particle, 15);

            var DamageParticle = Instantiate(other.GetComponent<EnemyController>()._damageAmount, other.transform.localPosition, Quaternion.identity, null);// create the damage particle
            DamageParticle.transform.Find(DAMAGE).GetComponent<TextMeshProUGUI>().text = GameManager.instance._playerRockDamage.ToString();//set the value for the text
            DamageParticle.transform.Find(DAMAGEBACK).GetComponent<TextMeshProUGUI>().text = GameManager.instance._playerRockDamage.ToString();//set the value for the text
            SoundManager.instance.SoundPlayer(5);//enable the sound

            Destroy(DamageParticle, 30);// destroy the damage particle

            Destroy(gameObject);

           
        }
        
            
        
    }
}

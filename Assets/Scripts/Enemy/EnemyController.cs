using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
    #region Constructer
    [Header("Constructers")]
    [SerializeField] Transform _groundDetection;//this is the ground detection so the enemy doesn't fall
    Animator _anim;//animator 
    SpriteRenderer _sprite;//enemy image
    Rigidbody2D _rb;//the physicsdad
    PlayerController _player;//the physicsdad
    [SerializeField] GameObject _p;//the coin particle sparks
    public GameObject _damageAmount;//damage amount on text
    [SerializeField] AnimationsEvent _animEvent;
    [SerializeField] GameObject _rock; // rock game object
    [SerializeField] GameObject _rockPos;//rock Transform and Direction
    [SerializeField] Card _bulletCard;// reference for bullet card scriptable object
    [SerializeField] EnemyCard _enemyCard;// reference for enemy card scriptable object
    [SerializeField] HealthBarFade _fade;//this script for enemy health
    #endregion

    #region floats
    [Header("EnemyMovement")]
   public float _speed;//enemy speed
   [HideInInspector] public float _realSpeed;//enemy static speed
    [SerializeField] float _distance;//the distance between the enemy and the floor
    [SerializeField] float _distanceRight;//the distance between the enemy and the floor
    [SerializeField] float _attackRange;//the distance between the enemy and the floor
    [SerializeField] float _damageDistance;//the distance between the enemy and the floor
    [HideInInspector] public float _direction=1;//the enemy speed animation
    [Header("EnemyAttack")]
    [SerializeField] float _attackCooldown;//cooldown for the attack
    [SerializeField] float _rockCooldown;//rock cooldown
    //DAMAGES
   
    #endregion

    #region ints
  int PLAYERDAMAGE;
    [SerializeField] int _scoreAdd;
    #endregion

    #region booleans
   [HideInInspector] public bool _movingRight = true;//check if the enemy moving right
    private bool _canMove = true;//check if the enemy can move
    
    [HideInInspector] public bool _isAttack;
    [HideInInspector] public bool _isDamage;
    [HideInInspector] public bool _isDie;
    #endregion

    #region strings
    //const strings for animations
    const string ATTACK = "Attack", SPEED = "Speed", HURT = "Hurt", DEATH = "Death";
    const string PLAYERMASK = "Player", ENEMYMASK = "Enemy",PLAYER="Player";

    //Ui strings
    const string CANVAS = "Canvas", COINSAMOUNT = "CoinsAmount";
    #endregion
    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();//reference Animator component
        _rb = GetComponent<Rigidbody2D>();//reference Rigidbody2D component
        _sprite = GetComponentInChildren<SpriteRenderer>();//reference SpriteRenderer component
        _player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();

    }
    private void Start()
    {
        _realSpeed = _speed;
        PLAYERDAMAGE = _enemyCard.enemyDamageBody;//set the damage for the enemy when enemy touch the player
           _rockCooldown = _bulletCard.bulletDestroyTime;//set the _rockCooldown to bullet card bulletDestroyTime
        #region card
        gameObject.name = _enemyCard.name;//set the enemy name
        _speed = _enemyCard.enemySpeed;
        _distance = _enemyCard.enemyDistance;
        _attackRange = _enemyCard.enemyAttackRange;
        _attackCooldown = _enemyCard.enemyAttackCooldown;
        _scoreAdd = _enemyCard.enemyCoinsAmount;
        #endregion


    }
    private void Update()
    {
        PartolAI();//call PartolAI
        AnimatorFunc();//call AnimatorFunc
    }
    #region Movements
  private void PartolAI()//here is the AI things
    {
        int layer_mask = LayerMask.GetMask(PLAYERMASK);
        if(_canMove)
        transform.Translate(Vector2.left * _speed * Time.deltaTime);//move our enemy from left to right
        RaycastHit2D groundInfo = Physics2D.Raycast(_groundDetection.position, Vector2.down, _distance);//ray cast from grounddetection object to the ground
        RaycastHit2D groundInfo1 = Physics2D.Raycast(_groundDetection.position, Vector2.left, _distanceRight);//ray cast from grounddetection object to the ground
        RaycastHit2D playerInfo = Physics2D.Raycast(transform.position, Vector2.right,_attackRange, layer_mask);//ray cast from enemy to the player
        if (!groundInfo.collider || groundInfo1.collider)//check if the raycast is hiting the ground or not
        {
            if (_movingRight)//check if the enemy is walking right 
            {
                transform.eulerAngles = new Vector3(0, -180, 0);//turn the enemy to the left
                _movingRight = false;//change the moving right to false
                _attackRange = System.Math.Abs(_attackRange);//change the distance to positive
               
            }
            else
            {
                
                transform.eulerAngles = new Vector3(0, 0, 0);//turn the enemy to the right
                _movingRight = true;//change the moving right to true
                _attackRange = System.Math.Abs(_attackRange) * (-1);//change the distance to negitive
            }
        }
        if (playerInfo&&!_player._isdie)
        {
            Attack();
        }

        if (_movingRight)
            _distanceRight = .1f;
        else
            _distanceRight = -.1f;
      
    }

    private void AnimatorFunc()//Animator Dad
    {
      
        _anim.SetFloat(SPEED, _direction);//make transition between idle/walk/run
    }
    public void Attack()//attack function
    {
        if (!_isAttack)//check if the enemy is not attacking
        {
            StartCoroutine(AttackCooldown(_attackCooldown));//call the enumerator
            _anim.SetTrigger(ATTACK);//play the animation
            _direction = 0;//change the direction
            _speed = 0;//freeze the enemy speed
            SoundManager.instance.SoundPlayer(9);//enable the sound


        }

    }
    public void RealAttack()
    {
        SoundManager.instance.SoundPlayer(10);//enable the sound

        var Rock = Instantiate(_rock, _rockPos.transform.position, transform.rotation, null); // create the rock
        Destroy(Rock, _rockCooldown);// destroy the rock
    }
    #endregion
  
    public void DamageOverride(int Damage,float Distance)//damage function
    {
       
        _fade.Damage(Damage);//call the damage
        StartCoroutine(DamageCooldown(.1f));      //damage cooldown
            _rb.AddForce(Vector2.right * Distance,ForceMode2D.Impulse);//add force to the enemy
     
    }
    public void EnemyDie()//here is how the player die
    {
       
        _anim.SetTrigger(DEATH);//enable the die animation
        _canMove = false;//stop the enemy from moving
        _isDie = true;
        var CoinObject = Instantiate(_p, transform.position, Quaternion.identity, null); // create the coin particle
        Destroy(CoinObject, 30); // destroy the coin particle


       


        //change the particle text to score number
        CoinObject.transform.Find(CANVAS).transform.Find(COINSAMOUNT).GetComponent<TextMeshProUGUI>().text = _scoreAdd.ToString();
        _player._currentScore += _scoreAdd;
        _player._coins.text = _player._currentScore.ToString();
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.right, Color.red);
        
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PLAYER)&&!_player._isDamage&&!_isDie)//check if player damaged or die
        {
            SoundManager.instance.SoundPlayer(4);//enable the sound
            _player.DamageOverride(PLAYERDAMAGE);//damage the player by body
            CinemachineShake.instance.Shaker(8, .3f);

        }
        if (collision.gameObject.CompareTag(PLAYER) && _isDie)
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<BoxCollider2D>());// disable collision between enemy and player
    }
   public  IEnumerator AttackCooldown(float time)//attack cooldown
    {
        _isAttack = true; //attack=true  
          yield return new WaitForSeconds(time);//wait for some time
        _isAttack = false;//attack=false

    }
    public IEnumerator DamageCooldown(float time)//attack cooldown
    {
        _isDamage = true; //attack=true  
        _canMove = false;
        yield return new WaitForSeconds(time);//wait for some time
        _isDamage = false;//attack=false
        if (!_isDie)
            _canMove = true;
    }
   

}

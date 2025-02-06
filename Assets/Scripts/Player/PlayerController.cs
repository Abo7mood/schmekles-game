using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerController : MonoBehaviour
{

    public static PlayerController _player; // singleton
    #region Constructers 
   [SerializeField] Rigidbody2D _rb; //the physicsdad
    [HideInInspector] public SpriteRenderer _sprite;//player image
    Animator _anim;// player animation
    GroundCheck _ground; // ground checker
    Timer _timer; // the timer script
    [Header("Particles")]
    [SerializeField] GameObject _dustParticle;//dustparticle
    [SerializeField] GameObject _dustParticleJump;//dustparticleJump
    [SerializeField] GameObject _dustParticleDrop;//dustparticleDrop
    [SerializeField] GameObject _heartParticle;//heartparticleDrop
    [SerializeField] HealthBarFade _playerFade;//this script for player health
    GameObject _dust; // real dust object that we will destroy it after some time
    GameObject _dustJump; // real dustJump object that we will destroy it after some time
    GameObject _dustFall; // real dustFall object that we will destroy it after some time
    Transform _canvasTransform; // this is the player canvas , to make the canvas follow the player
    [Header("GameObjects")]
    [SerializeField] GameObject _rock;//rock 
    [SerializeField] GameObject _rockPos;//rock 
    [SerializeField] GameObject[] _attacks;//attack objects, I made a reference for them just to rotate the attack collider when player rotate 180d 

    [Header("UI")]
    public TextMeshProUGUI _coins;// the coins text
    [SerializeField] Image _heart;// the heart image above the player
    [SerializeField] Image _rockImage;//teh rock image on the screen
    [SerializeField] GameObject _pausePanel;//pause panel
    [SerializeField] GameObject _gamePanel;//game panel
    [SerializeField] GameObject _leaderboardPanel;//;leaderboard panel
    [SerializeField] Button _informationButton;//this is the information button
    #endregion
    #region floats
    [Header("Movement")] //header on the inspector
    [SerializeField] float _speed; //playerspeed
    [SerializeField] float _sprint;//playersprint
    [SerializeField] float _jumpforce;//playerjump
    [SerializeField] float _fallMulitplier;//physics better
    [SerializeField] float _lowJumpMulitplier;//physics better
    [SerializeField] float _dustCooldown;//dustcooldown
    [Header("Transform")] //header on the inspector 
    [SerializeField] Transform _playerPos;//physics better
    [SerializeField] float _canvasTransformX;//physics better
    [SerializeField] float _canvasTransformY;//physics better
    [Header("Damage")] //header on the inspector 
    [SerializeField] float _damageTime;//physics better
    [SerializeField] float _attack1cooldown;
    [SerializeField] float _attack2cooldown;
    [SerializeField] int _healthAmount;


    float _movX;//horizontal input 
    float _currentHealth = 1;
    #endregion
    #region ints
    //player damage 
    const int PLAYERDAMAGE1 = 1;
    [Header("UI")] //header on the inspector 
    [SerializeField] int _scoreAdd;//the score
    [HideInInspector] public int _currentScore = 0;//the score
    #endregion
    #region booleans
    bool _isSprint;//check if the player sprint or not
    [HideInInspector] public bool _isDamage; //check if the player in damage state or not
    public bool _isMove = false;//check if the player moving or not 
    public bool _isCursor = false;//check if the player moving or not 

    [HideInInspector] public bool _isPause = false;//check if the game in pause or not

    bool _isDust;//check if the dust exist or not
    bool _canAttack = true;//check if the player attacking the first attack 
    bool _canAttack2 = true;//check if the player attacking the second attack 
    [HideInInspector] public bool _isdie = false;
    [HideInInspector] public bool _hasKey;

    #endregion
    #region strings
    //movement strings
    const string HORZ = "Horizontal", SPRINT = "Sprint", JUMPINPUT = "Jump",
        FIRE1 = "Fire1", FIRE2 = "Fire2";
    //movement animations strings
    const string SPEED = "Speed", IDLE = "Idle", JUMP = "IsJumping", FALL = "IsFalling",
        HURT = "Hurt", DIE = "Death", ATTACK1 = "Attack1",
        ATTACK2 = "Attack2", ATTACK3 = "Attack3";
    //collision enemies
    const string ENEMY = "Enemy";
    //randpm strings
    const string CANVAS = "Canvas", COINSAMOUNT = "CoinsAmount", HEALTHAMOUNT = "HealthAmount";
    #endregion
    #region PLayer

    #endregion
    private void Awake()
    {

        _anim = GetComponentInChildren<Animator>();//reference Animator component

        _sprite = GetComponentInChildren<SpriteRenderer>();//reference SpriteRenderer component
        _ground = GetComponentInChildren<GroundCheck>();//reference GroundCheck script
        _canvasTransform = GameObject.Find("PlayerCanvas").transform;// reference _canvasTransform
        _timer = FindObjectOfType<Timer>().gameObject.GetComponent<Timer>();// reference timer
    }
    private void Start()
    {
        _isMove = false;//disable the move
        _coins.text = _currentScore.ToString();//set the score text to score value
        Physics2D.IgnoreLayerCollision(9, 13, false);
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }
    private void FixedUpdate()// fixed update make physics better
    {
        if (_isMove)//check if the player can move or not
        {
            //Movement();//call Movement
            //jump();//call jump
            //Attack();// call Attack
        }

    }
    private void Update()
    {

        if (_isMove)//check if the player can move or not
        {
            Transformer();
            Attack();// call Attack
            //Dust(transform.position.y - 1f); //call Dust
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseFunc();

            }

        }
        AnimatorFunc();   //call AnimatorFunc

        if (_informationButton.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
            _informationButton.onClick.Invoke();
    }


    #region Movement
    private void Movement()
    {
        _movX = Input.GetAxisRaw(HORZ);//get input then move
        _isSprint = Input.GetButton(SPRINT) && _movX != 0 && _ground.isGrounded;//check if the player is holding sprint button + the player is moving

        float _realspeed = _isSprint ? _sprint : _speed;//check if the sprint is true, then the real speed will be like the sprint, otherwise it will keep as the same speed

        _rb.velocity = new Vector2(_movX * _realspeed, _rb.velocity.y); //move with rigibody

        if (_ground.isGrounded == true) //check if the player on ground or not and check if the player press Space or not 
            if (Input.GetKey(GameManager.instance._jump) || Input.GetKey(GameManager.instance._jump2))
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpforce);//jump
                SoundManager.instance.SoundPlayer(3);//enable the sound

            }




    }
    private void Attack()//Attack Void
    {
        if (Input.GetKeyDown(GameManager.instance._attack) && _canAttack && _ground.isGrounded)//if I press left click and if I can attack and if the player on the ground, then we can attack
        {
            StartCoroutine(Attack1(_attack1cooldown));//cooldown to the player attack     
            SoundManager.instance.SoundPlayer(1);//enable the sound

            if (_movX != 0)//if the player didn't move
            {
                _anim.SetTrigger(ATTACK1);//play the attack 1 animation
                StartCoroutine(AttackCooldown(.2f, ATTACK1));//call the Ienumerator

            }
            else
            {
                StartCoroutine(Attack1(_attack1cooldown));//cooldown to the player attack     

                _anim.SetTrigger(ATTACK2);//play the attack 2 animation

            }

        }
        else if (Input.GetKeyDown(GameManager.instance._shoot) && _canAttack2 /*&& _ground.isGrounded*/)//if I press left click and if I can attack and if the player on the ground,the player doesn't moving, then we can attack
        {
            StartCoroutine(Attack2(_attack2cooldown));//cooldown to the player attack
            _anim.SetTrigger(ATTACK3);//play the attack 3 animation
            StartCoroutine(AttackCooldown(.2f, ATTACK3));//call the Ienumerator
            SoundManager.instance.SoundPlayer(2);
        }
    }
    void jump()
    {
        //some caluclations for cool physics
        if (_rb.velocity.y < 0)//check if the Y smaller than 0
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMulitplier - 1) * Time.deltaTime;//make smooth jump

        else if (_rb.velocity.y > 0)//check if the Y greater than 0 and we didn't press the jump button        
            if (!Input.GetKey(GameManager.instance._jump) && !Input.GetKey(GameManager.instance._jump2))
                _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMulitplier - 1) * Time.deltaTime;//make smooth jump


    }
    private void AnimatorFunc()
    {

        if (Input.GetAxis(HORZ) > 0 && _isMove)// if the player hold left the sprite will flip
        {
            _sprite.flipX = false;//the player will face right
            _rockPos.transform.rotation = new Quaternion(0, 0, 0, 0);//the rockdirection will face right
            _attacks[0].transform.localPosition = new Vector2(0.7f, 0);//the attackdirection will face right
            _attacks[1].transform.localPosition = new Vector2(0.7f, 0);//the attackdirection will face right
        }



        else if (Input.GetAxis(HORZ) < 0 & _isMove)//if the player hold right the sprite will flip
        {
            _rockPos.transform.rotation = new Quaternion(0, 180, 0, 0);//the rockdirection will face left
            _sprite.flipX = true;//the player will face left
            _attacks[1].transform.localPosition = new Vector2(-1f, 0);//the attackdirection will face left
            _attacks[0].transform.localPosition = new Vector2(-1f, 0);//the attackdirection will face left

        }



        _anim.SetFloat(SPEED, Mathf.Abs(Mathf.Clamp(_movX, -.5f, 0.5f)) + (_isSprint ? 0.50f : 0));//make transition between idle/walk/run

        //jumping animations checking
        Debug.Log("RB=" + _rb);

        if (_rb.velocity.y <= 0 && _rb.velocity.y > -0.5f) //if the Y smaller or equal 0 and it's bigger than -0.5f , then the player will be idle
            {
                _anim.SetBool(JUMP, false);//set the jump animation to false
                _anim.SetBool(FALL, false);//set the fall animation to false

            }

            if (_rb.velocity.y > 0.01f) //if the Y Greater than  0.1 , then the player will jump
                _anim.SetBool(JUMP, true);//set the jump animation to true

            if (_rb.velocity.y < -.5f)//if the Y smaller than -1.8, then the player will fall
            {
                _anim.SetBool(JUMP, false);//set the jump animation to false
                _anim.SetBool(FALL, true);//set the fall animation to true
            }

            if (_rb.velocity.y >= 0)//if the Y smaller  or equal 0, then the player fall will canceled
                _anim.SetBool(FALL, false);//set the fall animation to false
        

        if (!_canAttack2)//check if the player can't attack the second attack
        {
            _rockImage.fillAmount -= Time.deltaTime * _attack2cooldown / 25f; //then increase  the fill amount, also divide the cooldown to make it the image time like cooldown time
        }

    }
    private void Transformer()
    {
        if (_canvasTransform != null)//check if the _canvasTransform exist or not 
            _canvasTransform.localPosition = new Vector3(transform.position.x + -_canvasTransformX, transform.position.y + _canvasTransformY, 55); // change the canvas transform to follow the player
    }
    public void PlayerDie()//here is how the player die
    {

        _currentHealth -= 1;
        Destroy(_heart);
        var dieparticle = Instantiate(_heartParticle, transform.position, Quaternion.identity, null);
        Destroy(dieparticle, 3f);
        Physics2D.IgnoreLayerCollision(9, 10, true);// stop the collision between the player and enemy
        Physics2D.IgnoreLayerCollision(9, 13, true);// stop the collision between the player and rock
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;//freeze the player
        _rb.velocity = new Vector2(0, _rb.velocity.y);//freeze the player
        _anim.SetTrigger(DIE);//enable the die animation
        _isMove = false;//set the move to false so the player can't move
        _isdie = true;
        SoundManager.instance.SoundPlayer(6);//enable the sound



    }
    public void PlayerLive()//make the player die?? idk 
    {
        if (_currentHealth >= 0 && _timer._currentTime != 0)
        {

            Physics2D.IgnoreLayerCollision(9, 10, false);// stop the collision between the player and enemy
            Physics2D.IgnoreLayerCollision(9, 13, false);// stop the collision between the player and rock
            _playerFade.Health((int)_playerFade.MaxHealth);
            transform.position = _playerPos.position;
            _anim.enabled = false;
            _anim.enabled = true;
            _isMove = true;
            _rb.constraints = RigidbodyConstraints2D.None;//freeze the player
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;//freeze the player
            _isdie = false;
            _anim.ResetTrigger(HURT);
        }
        else
        {
            CurorChanger();

            _gamePanel.SetActive(false);
            _anim.enabled = false;

            Debug.Log(_currentScore + "_currentScore");
            //PlayFabManager.instance.SendLeaderboard(_currentScore);
            //PlayFabManager.instance.Invoke("GetLeaderboardAroundPlayer", 1);

        }


    }
    #endregion
    #region override
    public void DamageOverride(int Damage) //call the damage
    {
        if (!_isdie)//check if the player die or not
        {
            StartCoroutine(this.Damage(_damageTime));//call the damage coroutine
            _playerFade.Damage(Damage);//set the damage
        }


    }
    public void PauseFunc()
    {

        _gamePanel.SetActive(false);
        _pausePanel.SetActive(true);
        _isPause = !_isPause;
        MoveChanger();
        CurorChanger();
    }
    public void MoveChanger() => _isMove = !_isMove;//set the move to true



    public void CurorChanger()
    {
        _isCursor = !_isCursor;
        if (_isCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;//make the cursor at the mid of the screen
            Cursor.visible = false;//hide the cursor
        }
        else
        {

            Cursor.lockState = CursorLockMode.None;//make the cursor at the mid of the screen
            Cursor.visible = true;//hide the cursor
        }
    }


    public void RockThrow()//respawn the rock
    {
        var Rock = Instantiate(_rock, _rockPos.transform.position, _rockPos.transform.rotation, null);//respawn the rock
        if (_sprite.flipX)
            Rock.GetComponent<RockScript>().isright = true;
        else
            Rock.GetComponent<RockScript>().isright = false;

        Destroy(Rock, 30f);//destroy the rock

    }
    #endregion
    #region Particles
    private void Dust(float dustposition)
    {
        if (_movX != 0 && _ground.isGrounded && !_isDust)//check if the player is moving + if the player on the ground + the dust isn't exist
        {
            StartCoroutine(nameof(Dusting));
            _dust = Instantiate(_dustParticle, new Vector2(transform.position.x, dustposition), Quaternion.identity, null); //create the dust
            _dust.GetComponent<ParticleSystem>().Play();//play the particle


            if (_movX > 0)//if the player is moving to the left
                _dust.transform.rotation = new Quaternion(0, -180f, 0, 0); // the particle will go right
            else //if the player is moving to the right
                _dust.transform.rotation = new Quaternion(0, 0, 0, 0); // the particle will go left

            Destroy(_dust, 1);// destroy the dust 
            if (_isSprint)//check if the player sprint or not
            {
                ParticleSystem.EmissionModule em = _dust.GetComponent<ParticleSystem>().emission; //reference the emissoin
                int rand = Random.Range(8, 13);//random values
                em.SetBursts(
            new ParticleSystem.Burst[] {
                  new ParticleSystem.Burst (0, rand),

            });
            }
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsTag(FALL))//check if the player falling
        {
            if (_ground.isGrounded && _dustFall == null)//check if the player on the ground + check if the dustfall is null or not 
            {
                _dustFall = Instantiate(_dustParticleDrop, new Vector2(transform.position.x, dustposition), Quaternion.identity, null); //create the falldust
                _dustFall.GetComponent<ParticleSystem>().Play();//play the particle
                Destroy(_dustFall, .6f);// destroy the falldust 
            }

        }
        if (_ground.isGrounded && Input.GetKey(GameManager.instance._jump) && _dustJump == null && _rb.velocity.y < 0.1f)
        {
            _dustJump = Instantiate(_dustParticleJump, new Vector2(transform.position.x, dustposition + .5f), Quaternion.identity, null); //create the jumpdust
            _dustJump.GetComponent<ParticleSystem>().Play();//play the particle
            Destroy(_dustJump, .6f);// destroy the jumpdust 
        }

    }
    #endregion
    #region Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Coin>() != null)
        {
            //sound
            var CoinObject = Instantiate(collision.GetComponent<Coin>()._p, transform.position, Quaternion.identity, null);
            Destroy(CoinObject, 30);
            Destroy(collision.gameObject);
            CoinObject.transform.Find(CANVAS).transform.Find(COINSAMOUNT).GetComponent<TextMeshProUGUI>().text = _scoreAdd.ToString();
            _currentScore += _scoreAdd;
            _coins.text = _currentScore.ToString();
            SoundManager.instance.SoundPlayer(7);//enable the sound

        }
        else if (collision.gameObject.GetComponent<HealthPack>() != null)
        {
            //sound
            var HealthObject = Instantiate(collision.GetComponent<HealthPack>()._p, transform.position, Quaternion.identity, null);
            HealthObject.transform.Find(CANVAS).transform.Find(HEALTHAMOUNT).GetComponent<TextMeshProUGUI>().text = _scoreAdd.ToString();
            Destroy(HealthObject, 30);
            Destroy(collision.gameObject);
            _playerFade.Health(_healthAmount);
            SoundManager.instance.SoundPlayer(8);//enable the sound

        }
        else if (collision.gameObject.GetComponent<Key>() != null && !_hasKey)
        {
            var CoinObject = Instantiate(collision.GetComponent<Key>()._p, transform.position, Quaternion.identity, null);
            Destroy(CoinObject, 30);
            CoinObject.transform.Find(CANVAS).transform.Find(COINSAMOUNT).GetComponent<TextMeshProUGUI>().text = _scoreAdd.ToString();

            Destroy(collision.gameObject);
            SoundManager.instance.SoundPlayer(7);//enable the sound
            _hasKey = true;
        }
        else if (collision.gameObject.GetComponent<Chest>() != null && _hasKey)
        {
            //sound
            _hasKey = false;
            collision.GetComponent<Animator>().enabled = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {


    }
    #endregion
    #region Ienumerator
    IEnumerator Damage(float time) // damage function to wait until the player recover
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        _isDamage = true;//the player in damage state right now
        _anim.SetTrigger(HURT);
        yield return new WaitForSeconds(time);//wait for some seconds
        _isDamage = false;//the player outside damage state right now
        if (!_isdie)
            Physics2D.IgnoreLayerCollision(9, 10, false);

    }
    IEnumerator Dusting() // dust cooldown
    {
        _isDust = true;//the player in dust state right now
        yield return new WaitForSeconds(_dustCooldown);//wait for some seconds
        _isDust = false;//the player outside dust state right now
    }
    IEnumerator AttackCooldown(float time2, string attackname)//cooldown for attack animation
    {
        yield return new WaitForSeconds(time2);
        _anim.ResetTrigger(attackname);

    }
    IEnumerator Attack1(float time)//cooldown for attack 1
    {
        _canAttack = false;
        yield return new WaitForSeconds(time);
        _canAttack = true;
    }
    IEnumerator Attack2(float time)//cooldown for attack 2
    {
        _canAttack2 = false;
        _rockImage.fillAmount = 1;
        yield return new WaitForSeconds(time);
        _canAttack2 = true;
    }

    #endregion


}

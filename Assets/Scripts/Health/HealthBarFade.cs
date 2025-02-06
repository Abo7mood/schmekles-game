//healthscript
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarFade : MonoBehaviour
{
    #region Constructer

    [SerializeField] PlayerController _player;
    [SerializeField] EnemyController _enemy;
    [SerializeField] EnemyCard _enemyCard;// reference for enemy card scriptable object
    private HealthSystem healthSystem;
    private Image barImage;
    private Image damagedBarImage;
    private Image _healthback;
    private Color damagedColor;
    #endregion
    #region float
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = .6f;
    public float MaxHealth = 100f;
    private float damagedHealthFadeTimer;
    #endregion
    #region int

    #endregion
    #region boolean

    #endregion
    #region string
    const string FADE = "Fade", BACK = "BackGround",HEALTHBACK= "HealthBackGround",PLAYER="Player",ENEMY="Enemy";
    #endregion

   
 
   

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        barImage = transform.Find(FADE).GetComponent<Image>();
        damagedBarImage = transform.Find(BACK).GetComponent<Image>();
        _healthback = transform.Find(HEALTHBACK).GetComponent<Image>();
        _player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        damagedColor = damagedBarImage.color;
        damagedColor.a = 0f;
        damagedBarImage.color = damagedColor;
        
    }

    private void Start()
    {
        healthSystem.SetUp(MaxHealth);
        SetHealth(healthSystem.GetHealthNormalized());
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        if (_enemyCard == null)
            return;


        MaxHealth = _enemyCard.enemyMaxHealth;
        //if(_enemyCard.name==)

    }
  
    private void Update()
    {
        //if (Input.GetKey(KeyCode.X))
        //    Damage(1);
        if (damagedColor.a > 0)
        {
            damagedHealthFadeTimer -= Time.deltaTime;
            if (damagedHealthFadeTimer < 0)
            {
                float fadeAmount = 5f;
                damagedColor.a -= fadeAmount * Time.deltaTime;
                damagedBarImage.color = damagedColor;
            }
        }
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        SetHealth(healthSystem.GetHealthNormalized());
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        if (damagedColor.a <= 0)
        {
            // Damaged bar image is invisible
            damagedBarImage.fillAmount = barImage.fillAmount;
        }
        damagedColor.a = 1;
        damagedBarImage.color = damagedColor;
        damagedHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;

        SetHealth(healthSystem.GetHealthNormalized());
    }

    private void SetHealth(float healthNormalized)
    {            

        barImage.fillAmount = healthNormalized;
       
        if (this.healthSystem.healthAmount <= 0 && gameObject.CompareTag(PLAYER) && _player._isdie == false)
        {
            _player.PlayerDie();
            _healthback.enabled = false;
        }
          

        else if (this.healthSystem.healthAmount <= 0 && !_enemy._isDie && gameObject.CompareTag(ENEMY))
            _enemy.EnemyDie();
        else
            return;
            
    }
    public void Damage(int SetDamage)
    {
        healthSystem.Damage(SetDamage);
    }
    public void Health(int SetHealth)
    {
        healthSystem.Heal(SetHealth);
    }
}

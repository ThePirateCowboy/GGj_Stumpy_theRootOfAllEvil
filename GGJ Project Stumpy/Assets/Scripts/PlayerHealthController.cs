using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public Animator theAnim;
    public GameObject YourDeadUI;
    public static PlayerHealthController instance;
    public int currentHealth, maxHealth;
    public float invincibleLength;
    private float invincibleCounter;
    public SpriteRenderer theSR;
    [SerializeField] private TextMeshProUGUI healthText;
    public Image Heart1, Heart2, Heart3, Heart4, Heart5;
    public Sprite Fullhear, Halfheart, EmptyHeart;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        
        RefreshHealthDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;

            if (invincibleCounter <= 0)
            {
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f);
            }
        }
    }

    public void DealDamage()
    {
        if (invincibleCounter <= 0)
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                RespawnController.instance.Respawn();
            }
            else
            {
                invincibleCounter = invincibleLength;
                //theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, .5f);
                theAnim.SetTrigger("IsHurt");
                PlayerController.instance.KnockBack();
            }
            RefreshHealthDisplay();
        }
    }

    public void HealPlayer()
    {
        currentHealth++;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        RefreshHealthDisplay();
    }

   
    public void RefreshHealthDisplay()
    {
        //healthText.text = currentHealth + "/" + maxHealth;

        UpdateImages();
        if (currentHealth <=0)
        {
            //healthText.text = "You're DEAD";
            theAnim.SetTrigger("IsDead");
            YourDeadUI.SetActive(true);
        }
    }
    public void FillHealth()
    {
        currentHealth = maxHealth;
        RefreshHealthDisplay();
    }
  
    void UpdateImages()
    {
        if(currentHealth == 10)
        {
            Heart1.sprite = Fullhear;
            Heart2.sprite = Fullhear;
            Heart3.sprite = Fullhear;
            Heart4.sprite = Fullhear;
            Heart5.sprite = Fullhear;
        }
        else if(currentHealth == 9)
        {
            Heart1.sprite = Fullhear;
            Heart2.sprite = Fullhear;
            Heart3.sprite = Fullhear;
            Heart4.sprite = Fullhear;
            Heart5.sprite = Halfheart;
        }
        else if(currentHealth == 8)
        {
            Heart1.sprite = Fullhear;
            Heart2.sprite = Fullhear;
            Heart3.sprite = Fullhear;
            Heart4.sprite = Fullhear;
            Heart5.sprite = EmptyHeart;
        }
        else if (currentHealth == 7)
        {
            Heart1.sprite = Fullhear;
            Heart2.sprite = Fullhear;
            Heart3.sprite = Fullhear;
            Heart4.sprite = Halfheart;
            Heart5.sprite = EmptyHeart;
        }
        else if (currentHealth == 6)
        {
            Heart1.sprite = Fullhear;
            Heart2.sprite = Fullhear;
            Heart3.sprite = Fullhear;
            Heart4.sprite = EmptyHeart;
            Heart5.sprite = EmptyHeart;
        }
        else if (currentHealth == 5)
        {
            Heart1.sprite = Fullhear;
            Heart2.sprite = Fullhear;
            Heart3.sprite = Halfheart;
            Heart4.sprite = EmptyHeart;
            Heart5.sprite = EmptyHeart;
        }
        else if (currentHealth == 4)
        {
            Heart1.sprite = Fullhear;
            Heart2.sprite = Fullhear;
            Heart3.sprite = EmptyHeart;
            Heart4.sprite = EmptyHeart;
            Heart5.sprite = EmptyHeart;
        }
        else if (currentHealth == 3)
        {
            Heart1.sprite = Fullhear;
            Heart2.sprite = Halfheart;
            Heart3.sprite = EmptyHeart;
            Heart4.sprite = EmptyHeart;
            Heart5.sprite = EmptyHeart;
        }
        else if (currentHealth == 2)
        {
            Heart1.sprite = Fullhear;
            Heart2.sprite = EmptyHeart;
            Heart3.sprite = EmptyHeart;
            Heart4.sprite = EmptyHeart;
            Heart5.sprite = EmptyHeart;
        }
        else if (currentHealth == 1)
        {
            Heart1.sprite = Halfheart;
            Heart2.sprite = EmptyHeart;
            Heart3.sprite = EmptyHeart;
            Heart4.sprite = EmptyHeart;
            Heart5.sprite = EmptyHeart;
        }
        else if (currentHealth == 2)
        {
            Heart1.sprite = EmptyHeart;
            Heart2.sprite = EmptyHeart;
            Heart3.sprite = EmptyHeart;
            Heart4.sprite = EmptyHeart;
            Heart5.sprite = EmptyHeart;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : Collidable
{
    //dmg struct
    public int[] damagePoint = {1,2,3,4,5,6};
    public float[] pushForce = {2.0f,2.3f,2.6f,2.9f,3.2f,3.5f};
    
    //upgrade
    public int weaponLevel = 0;
    public SpriteRenderer spriteRenderer;

    //swing
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastswing;
    
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Time.time - lastswing > cooldown)
            {
                lastswing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player")
                return;
            
            // Create a new dmg object, then we'll send it to the fighter we've hit;
            Damage dmg = new Damage()
            {
                dmgAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };
            
            coll.SendMessage("ReceiveDamage", dmg);
        }
        
    }

    private void Swing()
    {
        anim.SetTrigger("Swing");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}

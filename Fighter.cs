using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    //public fields
    public int hitPoint = 10;
    public int maxHitpoint = 10;
    public float pushRecoverySpeed = 0.2f;
    
    //immunity
    protected float immuneTime = 1.0f;
    protected float lastImmune;
    
    //push
    protected Vector3 pushDirection;

    //all fighters can receive dmg/die
    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitPoint -= dmg.dmgAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            
            GameManager.instance.ShowText(dmg.dmgAmount.ToString(), 25,Color.red, transform.position, Vector3.zero, 0.5f);

            if (hitPoint <= 0)
            {
                hitPoint = 0;
                Death();
            }
                
        }
    }

    protected virtual void Death()
    {
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator enemyAnimator;
    Rigidbody2D enemyRigidbody;
    CapsuleCollider2D enemyCapsuleCollider;
    [SerializeField] int EnemyHealt=100;
    [SerializeField] int CurrentEnemyHealth=0;
    bool EnemyIsAlive=true;

    
    void Start()
    {
        enemyAnimator=GetComponent<Animator>();
        enemyRigidbody=GetComponent<Rigidbody2D>();
        enemyCapsuleCollider=GetComponent<CapsuleCollider2D>();
        CurrentEnemyHealth=EnemyHealt;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damage){

        if(EnemyIsAlive==true){
            CurrentEnemyHealth=CurrentEnemyHealth-damage;

        if(CurrentEnemyHealth>0){
            enemyAnimator.SetTrigger("Hurt");
           // enemyAnimator.SetBool("Dead",true);
        }
        else if(CurrentEnemyHealth <=0){
            enemyAnimator.SetBool("Dead",true);
            EnemyIsAlive=false;

            
            
        }

        }
        
    }

}

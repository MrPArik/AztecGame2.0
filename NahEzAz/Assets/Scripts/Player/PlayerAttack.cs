using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    Animator myAnimator;
    [SerializeField] Transform attackPoint;
    public float attackRange=0.5f;
    public LayerMask enemyLayers;  
    
    PlayerMovement playerMovement;


    #region BasicAttack

    int attackDamageBasic=20;
    [SerializeField]  float AttackRateBasic=4f; //hányszor üthetünk egy másodperc alatt
    float basicAttackVariation=1;
    float nextAttackTimeBasic=0f; 

    #endregion

    #region JumpBasicAttack

    int attackDamageBasicJump=30;
    [SerializeField]  float AttackRateBasicJump=4f; //hányszor üthetünk egy másodperc alatt
    float nextAttackTimeBasicJump=0f; 


    #endregion

    #region HeavyAttack

     int attackDamageHeavy=60;
    [SerializeField]  float AttackRateHeavy=1f; //hányszor üthetünk egy másodperc alatt
    float nextAttackTimeHeavy=0f; 

    #endregion

    #region JumpHeavyAttack
         int attackDamageHeavyAir=100;
    [SerializeField]  float AttackRateHeavyAir=0.5f; //hányszor üthetünk egy másodperc alatt
    float nextAttackTimeHeavyAir=0f; 

    #endregion



    void Start()
    {
        playerMovement=GetComponent<PlayerMovement>();
        myAnimator=GetComponentInChildren<Animator>();
    }


    void OnFire(InputValue value)
    {  

        #region HeavyAirAttack
            if(Time.time >=nextAttackTimeHeavyAir && Input.GetKey("s")  && !playerMovement.PlayerIsGrounded && !playerMovement.PlayerIsLedgeClimbing && !playerMovement.PlayerIsClimbing && !playerMovement.PlayerIsDashing && !playerMovement.PlayerIsSliding){ //ha nagyobb a jelenlegi idő mint a //nextAttackTime üthetünk
            playerMovement.PlayerIsAttackingAir=true;
            myAnimator.SetBool("PlayerAttackHeavyAir",true);
            playerMovement.HeavyAirAttack();
            
            }
        #endregion

        #region BasicAirAttack

            if(Time.time >=nextAttackTimeBasicJump&& !Input.GetKey("s")  && !playerMovement.PlayerIsGrounded && !playerMovement.PlayerIsLedgeClimbing && !playerMovement.PlayerIsClimbing && !playerMovement.PlayerIsDashing && !playerMovement.PlayerIsSliding){ //ha nagyobb a jelenlegi idő mint a //nextAttackTime üthetünk
            playerMovement.PlayerIsAttackingAir=true;
            myAnimator.SetTrigger("PlayerAirAttack");
            nextAttackTimeBasicJump=Time.time+1f/AttackRateBasicJump; 
            }

        #endregion
            
            
        #region BasicAttack
        
        if(Time.time >=nextAttackTimeBasic && basicAttackVariation==1 && playerMovement.PlayerIsGrounded && !playerMovement.PlayerIsLedgeClimbing && !playerMovement.PlayerIsClimbing && !playerMovement.PlayerIsDashing && !playerMovement.PlayerIsSliding && !playerMovement.PlayerIsAttackingAir){ //ha nagyobb a jelenlegi idő mint a //nextAttackTime üthetünk
            playerMovement.PlayerIsAttackingGround=true;
            myAnimator.SetTrigger("PlayerAttack"+basicAttackVariation);
            basicAttackVariation=2;
            nextAttackTimeBasic=Time.time+1f/AttackRateBasic; 

            

     
        nextAttackTimeBasic=Time.time+1f/AttackRateBasic; 

        }else if(Time.time >=nextAttackTimeBasic && basicAttackVariation==2 && playerMovement.PlayerIsGrounded && !playerMovement.PlayerIsLedgeClimbing && !playerMovement.PlayerIsClimbing && !playerMovement.PlayerIsDashing && !playerMovement.PlayerIsSliding && !playerMovement.PlayerIsAttackingAir){
            playerMovement.PlayerIsAttackingGround=true;
            myAnimator.SetTrigger("PlayerAttack"+basicAttackVariation);
            basicAttackVariation=1;
            nextAttackTimeBasic=Time.time+1f/AttackRateBasic; 
            
        }
        
        #endregion
       
    }

    #region HeavyAttack

    void OnGHAttack(InputValue value){

        if(Time.time >=nextAttackTimeHeavy  && playerMovement.PlayerIsGrounded && !playerMovement.PlayerIsLedgeClimbing && !playerMovement.PlayerIsClimbing && !playerMovement.PlayerIsDashing && !playerMovement.PlayerIsSliding && !playerMovement.PlayerIsAttackingAir){
                playerMovement.PlayerIsAttackingGround=true;
            myAnimator.SetTrigger("PlayerAttackHeavy");
            nextAttackTimeHeavy=Time.time+1f/AttackRateHeavy; 
        }
        
    }

    #endregion


     #region BasicAttackDamage


    public void BasicAttack(){

        Collider2D[] hitEnemis= Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
 
        foreach(Collider2D enemy in hitEnemis)
        {
           enemy.GetComponent<Enemy>().Damage(attackDamageBasic);
        }

        

    }

    #endregion

    #region BasicAttackDamageAir

    public void BasicAttackAir(){

        Collider2D[] hitEnemis= Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
 
        foreach(Collider2D enemy in hitEnemis)
        {
           enemy.GetComponent<Enemy>().Damage(attackDamageBasicJump);
        }

    }

    #endregion

    #region HeavyAttackDamage

    public void HeavyAttack(){

        Collider2D[] hitEnemis= Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
 
        foreach(Collider2D enemy in hitEnemis)
        {
           enemy.GetComponent<Enemy>().Damage(attackDamageHeavy);
        }

    }

    #endregion

    #region HeavyAttackDamageAir

    public void HeavyAttackAir(){

        Collider2D[] hitEnemis= Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
 
        foreach(Collider2D enemy in hitEnemis)
        {
           enemy.GetComponent<Enemy>().Damage(attackDamageHeavyAir);
        }

    }

    #endregion

    public void VegeBasicAttackGround(){
        playerMovement.PlayerIsAttackingGround=false;
    }
    public void VegeBasicAttackAir(){
        playerMovement.PlayerIsAttackingAir=false;
    }

    private void OnDrawGizmosSelected()
     {
        if(attackPoint==null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }//ez az attak point körul kirajzolja a kört hogy lássuk mekkora
        
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    Vector2 moveInput;  
    [SerializeField] BoxCollider2D myBodyCollider;
    float originalGravity;

    #region GroundWallCheck

    public bool PlayerIsGrounded=false;
    [SerializeField] Transform PlayerGroundCheckPlace;
    [SerializeField] float PlayerGrouncdCheckRadius=0.25f;
    [SerializeField] LayerMask groundLayer;

    

    //[SerializeField] WallSensors FalJobbFent;
    //[SerializeField] WallSensors FalJobbLent;
    //[SerializeField] WallSensors FalBalFent;
    //[SerializeField] WallSensors FalBalLent;

    #endregion

    #region Futas

    public float PlayerRunSpeed=5f;
    public bool PlayerIsRunning=false;

    #endregion

    #region Ugras

    public float PlayerJumpSpeed=9f;
    public float PlayerDoubleJumpSpeed=7f;
    
    public bool PlayerIsJumping=false;
    [SerializeField] bool DoubleJumpActive=false;

    #endregion

    #region Dodge

    [SerializeField] float PlayerDodgeSpeed=12f;
    [SerializeField]bool canDash=true;  
    public bool PlayerIsDashing=false;	
    [SerializeField]float dashingTime=0.5f; 
    [SerializeField]float dashingColdown=1f; 

    #endregion

    #region Maszas

    public float PlayerClimbSpeed=5f;
    public bool PlayerIsClimbing=false;

    #endregion

    #region FalonCsuszas

    public bool PlayerIsSliding;
    float SlidingGravity=0.5f;
    



    //kell egy két változó a LedgeGrab ből is 


    #endregion

    #region LedgeGrab/CLimb

    public bool PlayerIsLedgeClimbing=false;
    bool PlayerIsGrabbingLedge=false;
    bool CheckSurroundingsEngedelyezve=true;
    
      [SerializeField]  bool isTouchingLedge;
      
      [SerializeField]  bool isTouchingWallLedgeAlatt;

       [SerializeField] bool canClimbLedge=false;
       [SerializeField] bool ledgeDetected=false;
        

        
         Transform FalJobbFentTransform;
         Transform FalJobbKozepTransform;

        Vector2 ledgePosBot;
        Vector2 ledgePos1;
        Vector2 ledgePos2; 

        [SerializeField] float wallCheckDistance=0.13f;
        
        [SerializeField] float ledgeClimbXOffset2=0.65f;
        [SerializeField] float ledgeClimbYOffset2=1.3f;
    
   
    

    #endregion

    #region GroundAttack

    public bool PlayerIsAttackingGround=false;

    #endregion

    #region AirAttack

    public bool PlayerIsAttackingAir=false;

    #endregion

    #region EgerrelNezesEsForgad

    

    #endregion
    
    void Start()
    {
        FalJobbFentTransform=transform.Find("WallDetectJobbFent");
        FalJobbKozepTransform=transform.Find("WallDetectKozepFent");
        myRigidbody=GetComponent<Rigidbody2D>();
        myAnimator=GetComponentInChildren<Animator>();
       
       originalGravity=myRigidbody.gravityScale;
       
       //FalJobbFent = transform.Find("WallDetectJobbFent").GetComponent<WallSensors>();
      // FalJobbLent = transform.Find("WallDetectJobbLent").GetComponent<WallSensors>();
    // FalBalFent = transform.Find("WallDetectBalFent").GetComponent<WallSensors>();
       //FalBalLent = transform.Find("WallDetectBalLent").GetComponent<WallSensors>();
    }

    
    void Update()
    {

       

        GroundCheck();
        CheckSurroundings();
        LedgeDetect();
        
        if(PlayerIsLedgeClimbing==true){
            return;
        }

       // if(myAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHeavyAir")){
       //    return;
       // }
        
        if(myAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHeavyAir") && PlayerIsGrounded){
            myAnimator.SetBool("PlayerAttackHeavyAir",false);
            myAnimator.SetTrigger("PlayerAttackHeavyAirFoldetErt");

          
        }
        if(myAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHeavyAirFoldetErt")){
            return;
        }
        

        if(PlayerIsDashing==true){
            myAnimator.SetBool("IsRunning",false);
            myAnimator.SetBool("IsJumping",false);
            return;
        }

        if(PlayerIsAttackingGround==true){
            myRigidbody.velocity=new Vector2(0f,0f);
            myAnimator.SetBool("IsRunning",false);
            myAnimator.SetBool("IsJumping",false);
            return;
        }

        if(PlayerIsAttackingAir==true){
            
            myAnimator.SetBool("IsRunning",false);
            myAnimator.SetBool("IsJumping",false);
            return;
        }

         WallSlide();
         
         if(!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHeavyAir") && !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack1") && !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack2")){
            Run();
            FlipPlayer();
         }
        
        
        
        Maszas();
        
        myAnimator.SetFloat("yVelocity",myRigidbody.velocity.y);
    }

    void OnMove(InputValue value)
    {
        moveInput=value.Get<Vector2>(); 
    }
    
    #region GroundCheck/Wallcheck

    void GroundCheck(){
        
            Collider2D[] colliders=Physics2D.OverlapCircleAll(PlayerGroundCheckPlace.position,PlayerGrouncdCheckRadius,groundLayer);
                if(colliders.Length>0 || PlayerIsClimbing==true || ledgeDetected ){
                    PlayerIsGrounded=true;
                     
                   // DoubleJumpActive=false; //dupla ugrás miatt
                    
                }
                else{
                    PlayerIsGrounded=false;
                    myAnimator.SetBool("IsRunning",false);
                }

                if(PlayerIsGrounded==false && !PlayerIsClimbing && !PlayerIsSliding && !PlayerIsAttackingGround && !PlayerIsAttackingAir){
                    myAnimator.SetBool("IsDodging",false);
                   
                    
                    PlayerIsJumping=true;
                    if(myRigidbody.velocity.y>=0f){
                        myAnimator.SetBool("IsJumping",true);
                    }else{
                        myAnimator.SetBool("IsJumping",false);
                        myAnimator.SetBool("IsFalling",true);
                    }
                }else{
                    myAnimator.SetBool("IsJumping",false);
                    myAnimator.SetBool("IsFalling",false);
                    
                    PlayerIsJumping=false;
                }
        }

        void CheckSurroundings(){
            if(CheckSurroundingsEngedelyezve)
            {

                if(transform.localScale.x>=0){
                    isTouchingLedge=Physics2D.Raycast(FalJobbFentTransform.position,transform.right,wallCheckDistance,groundLayer);
                    
                    isTouchingWallLedgeAlatt=Physics2D.Raycast(FalJobbKozepTransform.position,transform.right,wallCheckDistance,groundLayer);
                }else if(transform.localScale.x<0){
                    isTouchingLedge=Physics2D.Raycast(FalJobbFentTransform.position,-transform.right,wallCheckDistance,groundLayer);
                   
                    isTouchingWallLedgeAlatt=Physics2D.Raycast(FalJobbKozepTransform.position,-transform.right,wallCheckDistance,groundLayer);
                }

            }
                
        }

       
        

    private void OnDrawGizmosSelected()
        {
            
            Gizmos.DrawWireSphere(PlayerGroundCheckPlace.position,PlayerGrouncdCheckRadius);

           // Gizmos.DrawLine(FalJobbFentTransform.position, new Vector3(FalJobbFentTransform.position.x + wallCheckDistance,FalJobbFentTransform.position.y,FalJobbFentTransform.position.z));
        }
    

    #endregion

    
    #region FutasMetodus

    void Run(){

        Vector2 playerVelocity=new Vector2(moveInput.x*PlayerRunSpeed,myRigidbody.velocity.y); 

        if(Mathf.Abs(moveInput.x*PlayerRunSpeed)>0){
           // PlayerIsRunning=true;
            myRigidbody.velocity=playerVelocity; 
        }else{
            //PlayerIsRunning=false;
            myRigidbody.velocity=new Vector2(0f,myRigidbody.velocity.y);
        }
        
        if( PlayerIsGrounded==true && PlayerIsClimbing==false && !PlayerIsGrabbingLedge && !PlayerIsLedgeClimbing && !PlayerIsJumping){
            PlayerIsRunning=true;
            RunningAnimation();
        }else{
            myAnimator.SetBool("IsRunning",false);
            PlayerIsRunning=false;
        }
        

    }

    void RunningAnimation(){
        if(Mathf.Abs(myRigidbody.velocity.x)>Mathf.Epsilon )  
        {
            
            myAnimator.SetBool("IsRunning",true); 
        }
        else 
        {
            myAnimator.SetBool("IsRunning",false);
        }
    }

    #endregion

    #region FlipPlayerMetodus

        void FlipPlayer(){
            {
                bool playerHasHorizantalSpeed= Mathf.Abs(myRigidbody.velocity.x)>Mathf.Epsilon;

                if(playerHasHorizantalSpeed)
                {
                    transform.localScale=new Vector2(Mathf.Sign(myRigidbody.velocity.x),1f);

                }

                
            }
        }

    #endregion

    #region UgrasMetodus

    void OnJump(InputValue value){
        if(value.isPressed==true && PlayerIsGrounded==true && PlayerIsDashing==false && !PlayerIsLedgeClimbing){
            myRigidbody.velocity=new Vector2(myRigidbody.velocity.x,PlayerJumpSpeed);
            DoubleJumpActive=true;
        }else if(value.isPressed==true && !PlayerIsGrounded && PlayerIsDashing==false && !PlayerIsLedgeClimbing && DoubleJumpActive){
             myRigidbody.velocity=new Vector2(myRigidbody.velocity.x,PlayerDoubleJumpSpeed);
            DoubleJumpActive=false;

        }
    }
    

    #endregion

    #region DodgeMetodus
    
    void OnDodge(InputValue value){
                if(PlayerIsGrounded==true && !PlayerIsClimbing )
                {  
                if(value.isPressed && canDash) 
                    {
                        StartCoroutine(Dodge()); 
                        PlayerIsAttackingGround=false;
                    }      
                }

            }

            private IEnumerator Dodge()
            {
                canDash = false;
                PlayerIsDashing = true;
                myAnimator.SetBool("IsDodging",true);
                myRigidbody.velocity=new Vector2(myRigidbody.transform.localScale.x*PlayerDodgeSpeed,myRigidbody.velocity.y);
                if(!PlayerIsGrounded){
                    myRigidbody.velocity=new Vector2(0f,myRigidbody.velocity.y);
                    myAnimator.SetBool("IsDodging",false);
                    PlayerIsDashing=false;
                }
                yield return new WaitForSeconds(dashingTime);
                myRigidbody.velocity=new Vector2(0f,myRigidbody.velocity.y);
                myAnimator.SetBool("IsDodging",false);
                PlayerIsDashing = false;
                yield return new WaitForSeconds(dashingColdown);
                canDash = true;

            }


    #endregion

    #region MaszasMetodus

    void Maszas(){
        

        if( myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
           
            PlayerIsClimbing=true;
            myRigidbody.gravityScale=0f;
            myAnimator.SetBool("IsJumping",false);
                
            Vector2 climbVelocity= new Vector2 (myRigidbody.velocity.x,moveInput.y*PlayerClimbSpeed);
            myRigidbody.velocity=climbVelocity;
               
                    
                
	

        }else if(!PlayerIsLedgeClimbing && !PlayerIsSliding){
            myRigidbody.gravityScale=originalGravity;
                PlayerIsClimbing=false;
        }
        
        
    }


    #endregion

    #region FalonCsuszasMetodus

    void WallSlide(){

        if( isTouchingWallLedgeAlatt && isTouchingLedge && PlayerIsGrabbingLedge==false && !PlayerIsClimbing && !PlayerIsGrounded && myRigidbody.velocity.y<=0){
            PlayerIsSliding=true;
            myAnimator.SetBool("IsSliding",true);
            myRigidbody.gravityScale=SlidingGravity;
            PlayerIsAttackingGround=false;
            PlayerIsAttackingGround=false;
        }else{
            PlayerIsSliding=false;
            myAnimator.SetBool("IsSliding",false);
            myRigidbody.gravityScale=originalGravity;
        }
    }

    #endregion

    #region LedgeCLimb/Grab method

    void LedgeDetect(){


         if( isTouchingWallLedgeAlatt && !isTouchingLedge && ledgeDetected==false && !PlayerIsClimbing && !PlayerIsGrounded){
            ledgeDetected=true;
            
            PlayerIsLedgeClimbing=true;
            PlayerIsGrabbingLedge=true;
            PlayerIsAttackingGround=false;
            PlayerIsAttackingGround=false;

            myAnimator.SetBool("IsSliding",false);
            myAnimator.SetBool("IsLedgeGrabbing",true);
            myRigidbody.velocity=new Vector2(0f,0f);
            myRigidbody.gravityScale=0f;   
         }

    }

     void OnClimbUp(InputValue value){
        if(value.isPressed==true && PlayerIsGrabbingLedge==true){
            
            CheckLedgeClimb();
            PlayerIsGrabbingLedge=false;
        }
    }

    void OnClimbDown(InputValue value){
        if(value.isPressed==true && PlayerIsGrabbingLedge==true){
            myAnimator.SetBool("IsLedgeGrabbing",false);
           StartCoroutine(UjraErzekehletLedghez());
           isTouchingWallLedgeAlatt=false;

            ledgeDetected=false;
            PlayerIsLedgeClimbing=false;
            PlayerIsGrabbingLedge=false;
        }
    }

    IEnumerator UjraErzekehletLedghez(){
        CheckSurroundingsEngedelyezve=false;
       yield return new WaitForSeconds(0.1f);
       CheckSurroundingsEngedelyezve=true;

    }



    void CheckLedgeClimb(){
                if(ledgeDetected && !canClimbLedge){
                    canClimbLedge=true;
                   myAnimator.SetBool("IsLedgeGrabbing",false);

                    if(transform.localScale.x>=0){
                       ledgePos2=new Vector2(transform.position.x+ledgeClimbXOffset2,transform.position.y+ledgeClimbYOffset2);
                    }else{
                      ledgePos2=new Vector2(transform.position.x-ledgeClimbXOffset2,transform.position.y+ledgeClimbYOffset2);
                    }
                      
                   myAnimator.SetBool("IsLedgeClimbing",true);
                    
                } 
            }

        public void FinnishLedgeCLimb(){
                canClimbLedge=false;
                transform.position=ledgePos2;
               myAnimator.SetBool("IsLedgeClimbing",false);
                ledgeDetected=false;
                
            }

        public void LedgeUtanLehetIranyitani(){
                PlayerIsLedgeClimbing=false;
            }
    
   
    

    #endregion

    #region HeavyAirAttack

    public void HeavyAirAttack(){
        myRigidbody.velocity=new Vector2(0f,-9f);
    }

    #endregion
}

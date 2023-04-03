using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerHealth : MonoBehaviour
{
    public int MaxPlayerHealth=100;
    public int PLayerCurrentHealth=0;

    public HealtBarScript healtBar;
    void Start()
    {
        PLayerCurrentHealth=MaxPlayerHealth;
        healtBar.SetMaxHealth(MaxPlayerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)){
            PlayerTakeDamage(20);
            
        }

       
    }

    public void PlayerTakeDamage(int health){
        PLayerCurrentHealth-=health;
        healtBar.SetHealth(PLayerCurrentHealth);
    }
}

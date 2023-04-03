using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSensors : MonoBehaviour
{
     private int FalakSzama = 0;
    
    private void OnEnable()
    {
        FalakSzama = 0;
    }

   public bool State()
    {
        //if (m_DisableTimer > 0)
          //  return false;
        return FalakSzama > 0;
    }

    


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Talaj"){
            FalakSzama++;
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
         if(other.tag=="Talaj"){
        FalakSzama--;
         }
    }
}

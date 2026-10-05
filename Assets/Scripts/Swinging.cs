using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Swinging : MonoBehaviour
{
    private bool canSwing = false;
    void Update()
    {
        //Keyboard.current.kKey.value
        if (Keyboard.current.kKey.value > 0)
        {
            var g = GetComponent<ThirdPersonController>().Grounded;
           if (canSwing == false)
            {
                Debug.Log("You cannot swing right now.");
                return;
            }
            else
            {
                Debug.Log("You can swing!");
            }
        }  
    }

    void OnTriggerEnter(Collider collision)
    {
        print(1234);
         if (collision.gameObject.tag == "NoSwing")
            {
                  canSwing = false;  
            }
    }

     void OnTriggerExit(Collider collision)
    {
        print(4321);
        if (collision.gameObject.CompareTag("NoSwing"))
        {
            canSwing = true;
        }
    }
}

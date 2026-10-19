using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Swinging : MonoBehaviour
{
    private bool canSwing = false;

    [SerializeField] private GameObject swinganchorPrefab;
    [SerializeField] private float anchorForwardDistance = 0f;
    [SerializeField] private float anchorHeight = 0f;
    private GameObject currentAnchor;
    void Update()
    {
        //Keyboard.current.kKey.value
        if (Keyboard.current.kKey.wasPressedThisFrame)
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

                //the anchor position is transform.forward, which checks the characters viewing direction. Then adds predifined numbers to it. 
                Vector3 anchorPosition = transform.position + transform.forward * anchorForwardDistance + Vector3.up * anchorHeight;

                //spawns anchor.
                currentAnchor = Instantiate(swinganchorPrefab, anchorPosition, Quaternion.identity);
            }
        }  
    }

    void OnTriggerEnter(Collider collision)
    {
         if (collision.gameObject.tag == "NoSwing")
            {
                  canSwing = false;

                    if (currentAnchor != null)
                    {
                      Destroy(currentAnchor);
                     currentAnchor = null;
                    }
        }
    }

     void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("NoSwing"))
        {
            canSwing = true;
        }
    }
}

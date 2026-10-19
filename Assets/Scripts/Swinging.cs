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
    private LineRenderer Web;

    //For swinging.
    private ThirdPersonController controller;
    private CharacterController characterController;

    [SerializeField] private float swingGravity = 20f;
    [SerializeField] private float initialSwingSpeed = 5f;

    private Vector3 swingVelocity;
    private float ropeLength;

    private bool firstSwing = true;

    private void Start()
    {
        Web = GetComponent<LineRenderer>();
        controller = GetComponent<ThirdPersonController>();
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        //Keyboard.current.kKey.value
        if (Mouse.current.rightButton.wasPressedThisFrame)
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

                if (currentAnchor != null)
                {
                    Destroy(currentAnchor);
                }

                //spawns anchor.
                currentAnchor = Instantiate(swinganchorPrefab, anchorPosition, Quaternion.identity);

                if (firstSwing == true) 
                { 
                    characterController.Move(Vector3.up * 3.7f); 
                    firstSwing = false;
                }

                //Swinging!
                ropeLength = Vector3.Distance(transform.position + Vector3.up * 1.6f, currentAnchor.transform.position);

                Vector3 directionFromAnchor =
                    (transform.position - currentAnchor.transform.position).normalized;

                Vector3 swingDirection =
                    Vector3.ProjectOnPlane(transform.forward, directionFromAnchor).normalized;

                swingVelocity = swingDirection * initialSwingSpeed;

                controller.IsSwinging = true;
            }

        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            EndSwing();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "NoSwing")
        {
            canSwing = false;
            EndSwing();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("NoSwing"))
        {
            canSwing = true;
        }
    }

    void EndSwing()
    {
        if (currentAnchor != null)
        {
            Destroy(currentAnchor);
            currentAnchor = null;
        }
        firstSwing = true;
        controller.IsSwinging = false;
    }

    void LateUpdate()
    {
        SwingMovement();
        if (currentAnchor == null)
        {
            Web.enabled = false;
            return;
        }

        Web.enabled = true;

        Web.SetPosition(0, transform.position + Vector3.up * 1.6f);
        Web.SetPosition(1, currentAnchor.transform.position);
    }
    void SwingMovement()
    {
        if (currentAnchor == null)
        {
            return;
        }

        if (controller.IsSwinging == false)
        {
            return;
        }

        float deltaTime = Time.deltaTime;

        Vector3 anchorPosition = currentAnchor.transform.position;

        Vector3 directionFromAnchor =
            (transform.position - anchorPosition).normalized;

        // Apply gravity to the swinging velocity.
        swingVelocity += Vector3.down * swingGravity * deltaTime;

        // Remove velocity that points directly toward or away from the anchor.
        swingVelocity = Vector3.ProjectOnPlane(
            swingVelocity,
            directionFromAnchor
        );

        // Calculate the movement for this frame.
        Vector3 movement = swingVelocity * deltaTime;

        // Move the player.
        characterController.Move(movement);

        // Correct the position so the rope stays the same length.
        Vector3 newDirection =
            (transform.position - anchorPosition).normalized;

        Vector3 correctedPosition =
            anchorPosition + newDirection * ropeLength;

        characterController.Move(
            correctedPosition - transform.position
        );
    }
}



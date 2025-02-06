using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    [SerializeField] private LayerMask platformLayerMask; //ground layer to check the ground

    public bool isGrounded;//ground boolean to check if the player on ground or not
    
    private void OnTriggerStay2D(Collider2D collider) => isGrounded = collider != null && (((1 << collider.gameObject.layer) & platformLayerMask) != 0);//when ground checker hit the ground


    private void OnTriggerExit2D(Collider2D collision) => isGrounded = false;//when ground checker exit the ground


}

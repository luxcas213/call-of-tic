using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedCheck : MonoBehaviour
{
    PlayerController playerController;

    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == playerController.gameObject)
        {
            return;
        }
        if (other.gameObject.tag == "grounded")
        {
            playerController.SetGroundedState(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            return;
        }
        if (other.gameObject.tag == "grounded")
        {
            playerController.SetGroundedState(false);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            return;
        }
        if (other.gameObject.tag == "grounded")
        {
            playerController.SetGroundedState(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject == playerController.gameObject)
        {
            return;
        }
        if (collision.gameObject.tag == "grounded")
        {
            playerController.SetGroundedState(true);
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
        {
            return;
        }
        if (collision.gameObject.tag == "grounded")
        {
            playerController.SetGroundedState(true);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
        {
            return;
        }
        if (collision.gameObject.tag == "grounded")
        {
            playerController.SetGroundedState(false);
        }
    }
}

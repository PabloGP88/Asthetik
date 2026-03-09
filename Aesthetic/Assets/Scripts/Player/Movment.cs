using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movment : MonoBehaviour
{  
    public bool inputActive {get; private set;}
    [SerializeField] private float movementSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private GameLoop gameLoop;
    [SerializeField] private Pause pause;
    private const float NORMAL_SIZE = 0.8f;
    
    // Add these for velocity tracking
    private Vector3 lastPosition;
    private Vector3 currentVelocity;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (pause.GetPause())
        {
            UpdateVelocity();
            return;
        }
        
        ClickMovement();
        UpdateVelocity();
    }
    
    private void UpdateVelocity()
    {
        // Calculate velocity based on position change
        currentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }
    
    // Add this method for the camera to access velocity
    public Vector3 GetVelocity()
    {
        return currentVelocity;
    }
    
    private void ClickMovement()
    {
        if (Input.GetMouseButton(0) && !gameLoop.haveWon)
        {
            inputActive = true;

            animator.Play("PlayerAnim",0,0f);
            animator.enabled = false;

            transform.localScale = new Vector3(
                Mathf.Lerp(transform.localScale.x,NORMAL_SIZE,5 * Time.deltaTime),
                Mathf.Lerp(transform.localScale.y,NORMAL_SIZE,5 * Time.deltaTime),
                Mathf.Lerp(transform.localScale.z,NORMAL_SIZE,5 * Time.deltaTime)
            );

            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 mouseWolrdPos = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x,mouseWolrdPos.x,movementSpeed * Time.deltaTime),
                Mathf.Lerp(transform.position.y,mouseWolrdPos.y,movementSpeed * Time.deltaTime),
                transform.position.z
            );

        } else if (Input.touchCount > 0 && !gameLoop.haveWon)
        {
            inputActive = true;

            animator.Play("PlayerAnim",0,0f);
            animator.enabled = false;

            transform.localScale = new Vector3(
                Mathf.Lerp(transform.localScale.x,NORMAL_SIZE,5 * Time.deltaTime),
                Mathf.Lerp(transform.localScale.y,NORMAL_SIZE,5 * Time.deltaTime),
                Mathf.Lerp(transform.localScale.z,NORMAL_SIZE,5 * Time.deltaTime)
            );

            Touch touch = Input.GetTouch(0);
            Vector3 touchScreenPosition = touch.position;
            Vector3 touchWolrdPos = Camera.main.ScreenToWorldPoint(touchScreenPosition);

            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x,touchWolrdPos.x,movementSpeed * Time.deltaTime),
                Mathf.Lerp(transform.position.y,touchWolrdPos.y,movementSpeed * Time.deltaTime),
                transform.position.z
            );
        } else
        {
            inputActive = false;
            animator.enabled = true;
        }
    }
}
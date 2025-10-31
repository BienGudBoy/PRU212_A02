using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    Rigidbody2D rb2d; 
    SurfaceEffector2D surfaceEffector2D;
    [SerializeField] float torqueAmount = 3f;
    [SerializeField] float boostSpeed = 30f;
    [SerializeField] float baseSpeed = 20f;
    [SerializeField] float boostAdditive = 10f;
    
    [Header("Tricks")]
    [SerializeField] float trickRotationThreshold = 180f;
    [SerializeField] float airTimeThreshold = 0.5f;
    private float currentRotation = 0f;
    private float airTime = 0f;
    private bool isGrounded = true;
    private bool trickPerformed = false;
    
    [Header("Power-ups")]
    [SerializeField] float invincibilityDuration = 3f;
    [SerializeField] float speedBoostDuration = 5f;
    [SerializeField] float speedBoostMultiplier = 1.5f;
    private bool isInvincible = false;
    private float powerUpEndTime = 0f;
    private bool hasSpeedBoost = false;
    
    [Header("Visual Effects")]
    [SerializeField] ParticleSystem speedBoostEffect;
    [SerializeField] GameObject invincibilityShield;
    
    bool canMove = true;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
        if (GameManager.Instance != null)
        {
            baseSpeed = GameManager.Instance.nextBaseSpeed;
        }
        if (invincibilityShield != null)
            invincibilityShield.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.isGameActive) return;
        if(canMove)
        {
            RotatePlayer();
            RespondToBoost();
            CheckTricks();
            UpdatePowerUps();
        }
    }
    
    void UpdatePowerUps()
    {
        // Check if power-ups have expired
        if ((isInvincible || hasSpeedBoost) && Time.time >= powerUpEndTime)
        {
            if (isInvincible)
                DeactivateInvincibility();
            if (hasSpeedBoost)
                DeactivateSpeedBoost();
        }
    }
    
    void CheckTricks()
    {
        // Track air time
        if (!isGrounded)
        {
            airTime += Time.deltaTime;
            
            // Track rotation for tricks
            float rotationDelta = Mathf.DeltaAngle(transform.eulerAngles.z, currentRotation);
            currentRotation = transform.eulerAngles.z;
            
            // Check for trick completion (full rotation)
            if (Mathf.Abs(rotationDelta) > trickRotationThreshold && !trickPerformed && airTime > airTimeThreshold)
            {
                PerformTrick();
            }
        }
        else
        {
            // Reset when grounded
            if (airTime > 0)
            {
                airTime = 0f;
                trickPerformed = false;
            }
        }
    }
    
    void PerformTrick()
    {
        trickPerformed = true;
        string trickName = "Spin";
        
        // Determine trick type based on air time and rotation
        if (airTime > 2f)
            trickName = "Big Air Spin";
        else if (airTime > 1f)
            trickName = "Air Spin";
        
        if (GameManager.Instance != null)
            GameManager.Instance.AddTrickScore(trickName);
    }

    public void DisableControls()
    {
        canMove = false;
        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;
            rb2d.angularVelocity = 0f;
        }
        if (surfaceEffector2D != null)
            surfaceEffector2D.speed = 0f;
    }
    
    public void EnableControls()
    {
        canMove = true;
    }
    
    public float GetCurrentSpeed()
    {
        if (surfaceEffector2D != null)
            return surfaceEffector2D.speed;
        return 0f;
    }
    
    public bool IsInvincible()
    {
        return isInvincible;
    }

    void RespondToBoost()
    {
        float targetSpeed = baseSpeed;
        
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            targetSpeed = baseSpeed + boostAdditive;
        }
        
        // Apply speed boost power-up
        if (hasSpeedBoost)
        {
            targetSpeed *= speedBoostMultiplier;
        }
        
        surfaceEffector2D.speed = targetSpeed;
    }

    void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb2d.AddTorque(torqueAmount);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb2d.AddTorque(-torqueAmount);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    
    public void ActivateSpeedBoost()
    {
        hasSpeedBoost = true;
        powerUpEndTime = Mathf.Max(powerUpEndTime, Time.time + speedBoostDuration);
        
        if (speedBoostEffect != null)
            speedBoostEffect.Play();
    }
    
    public void ActivateInvincibility()
    {
        isInvincible = true;
        powerUpEndTime = Mathf.Max(powerUpEndTime, Time.time + invincibilityDuration);
        
        if (invincibilityShield != null)
            invincibilityShield.SetActive(true);
    }
    
    void DeactivateSpeedBoost()
    {
        hasSpeedBoost = false;
        
        if (speedBoostEffect != null)
            speedBoostEffect.Stop();
    }
    
    void DeactivateInvincibility()
    {
        isInvincible = false;
        
        if (invincibilityShield != null)
            invincibilityShield.SetActive(false);
    }


public void IncreaseBaseSpeed(float increment)
{
    baseSpeed += increment;
}
}
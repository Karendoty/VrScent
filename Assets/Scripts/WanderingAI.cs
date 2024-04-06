using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingAI : MonoBehaviour
{
   public AnimationClip idleAnimation;
        public AnimationClip walkingAnimation;

        public float moveSpeed = 3f;
        public float rotSpeed = 100f;
        public float turnDuration = 1.5f; // Adjust turn duration as needed
        public float collisionCooldown = 2f; // Cooldown period after collision

        private Animator animator;
        private bool isWalking = false;
        private bool isCooldown = false;

        void Start()
        {
            animator = GetComponent<Animator>();

            if (animator != null)
            {
                // Disable auto play of animations since we'll control it manually
                animator.applyRootMotion = false;
                StartCoroutine(Wander());
            }
        }

        void Update()
        {
            if (isWalking)
            {
                // Move the AI forward
                transform.position += transform.forward * moveSpeed * Time.deltaTime;

                // Check for collision with obstacles if not on cooldown
                if (!isCooldown)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
                    {
                        // If obstacle detected, stop walking, play idle animation, and start turning
                        isWalking = false;
                        SetWalkingAnimation(false);
                        StartCoroutine(Turn());
                        StartCoroutine(Cooldown());
                    }
                }
            }
        }

        void SetWalkingAnimation(bool walking)
        {
            isWalking = walking;

            if (animator != null && walkingAnimation != null && idleAnimation != null)
            {
                if (walking)
                {
                    // Play walking animation with the correct speed
                    animator.Play(walkingAnimation.name);
                }
                else
                {
                    // Play idle animation with the correct speed
                    animator.Play(idleAnimation.name);
                }
            }
        }

        IEnumerator Wander()
        {
            while (true)
            {
                // Start walking
                SetWalkingAnimation(true);

                // Walk for 10 seconds
                yield return new WaitForSeconds(10f);

                // Stop walking
                SetWalkingAnimation(false);

                // Wait for 5 seconds before starting next wandering cycle
                yield return new WaitForSeconds(5f);
            }
        }

        IEnumerator Turn()
        {
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 90f, 0); // Target rotation for turning

            float elapsedTime = 0f;
            while (elapsedTime < turnDuration)
            {
                // Interpolate rotation smoothly
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / turnDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure exact target rotation
            transform.rotation = targetRotation;

            // Resume walking
            SetWalkingAnimation(true);
        }

        IEnumerator Cooldown()
        {
            // Set cooldown flag to prevent immediate collision detection
            isCooldown = true;
            yield return new WaitForSeconds(collisionCooldown);
            isCooldown = false;
        }
    }
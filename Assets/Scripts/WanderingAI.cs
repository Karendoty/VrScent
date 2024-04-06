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
        public float turnDuration = 3f;

        private Animator animator;
        private bool isWalking = false;
        private bool isRotating = false;
        private bool isWandering = false;

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
            if (isRotating)
            {
                // Rotate the AI
                transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
            }
            else if (isWalking)
            {
                // Move the AI forward
                transform.position += transform.forward * moveSpeed * Time.deltaTime;

                // Check for collision with obstacles
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
                {
                    // If obstacle detected, start turning
                    isWalking = false;
                    StartCoroutine(Turn());
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
                isWalking = true;
                SetWalkingAnimation(true);

                // Walk for 10 seconds
                yield return new WaitForSeconds(10f);

                // Stop walking
                isWalking = false;
                SetWalkingAnimation(false);

                // Wait for 5 seconds before starting next wandering cycle
                yield return new WaitForSeconds(5f);
            }
        }

        IEnumerator Turn()
        {
            // Rotate left for turnDuration seconds
            isRotating = true;
            yield return new WaitForSeconds(turnDuration);
            isRotating = false;

            // Rotate right for turnDuration seconds
            transform.rotation = Quaternion.identity; // Reset rotation before rotating right
            isRotating = true;
            yield return new WaitForSeconds(turnDuration);
            isRotating = false;

            // Continue walking after turning
            isWalking = true;
        }
    }
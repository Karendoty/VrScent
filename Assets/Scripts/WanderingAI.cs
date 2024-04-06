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
                    // If obstacle detected, rotate to avoid it
                    isWalking = false;
                    StartCoroutine(RotateToAvoidObstacle());
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
                    animator.CrossFade(walkingAnimation.name, 0.1f); // Adjust the crossfade time as needed
                }
                else
                {
                    animator.CrossFade(idleAnimation.name, 0.1f); // Adjust the crossfade time as needed
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

                // Rotate left for 3 seconds
                isRotating = true;
                yield return new WaitForSeconds(3f);
                isRotating = false;

                // Rotate right for 3 seconds
                transform.rotation = Quaternion.identity; // Reset rotation before rotating right
                isRotating = true;
                yield return new WaitForSeconds(3f);
                isRotating = false;

                // Wait for 5 seconds before next wandering cycle
                yield return new WaitForSeconds(5f);
            }
        }

        IEnumerator RotateToAvoidObstacle()
        {
            // Rotate left or right randomly to avoid obstacle
            isRotating = true;
            float rotateTime = Random.Range(1f, 2f); // Randomize rotation time
            float rotateDirection = Random.Range(0, 2) == 0 ? -1f : 1f; // Randomize rotation direction
            yield return new WaitForSeconds(rotateTime);
            isRotating = false;
            yield return new WaitForSeconds(0.5f); // Wait for a short time before resuming walking
            isWalking = true;
        }
    }

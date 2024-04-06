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
        private bool isWandering = false;
        private bool isRotatingLeft = false;
        private bool isRotatingRight = false;

        void Start()
        {
            animator = GetComponent<Animator>();

            if (animator != null)
            {
                // Disable auto play of animations since we'll control it manually
                animator.applyRootMotion = false;
                StartCoroutine(ShuffleClips());
                StartCoroutine(Wander());
            }
        }

        void Update()
        {
            if (isRotatingRight)
                transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
            if (isRotatingLeft)
                transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
            if (isWalking)
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        void SetWalkingAnimation(bool walking)
        {
            isWalking = walking;

            if (animator != null && walkingAnimation != null && idleAnimation != null)
            {
                if (walking)
                {
                    Debug.Log("Playing walking animation");
                    animator.CrossFade(walkingAnimation.name, 0.1f); // Adjust the crossfade time as needed
                }
                else
                {
                    Debug.Log("Playing idle animation");
                    animator.CrossFade(idleAnimation.name, 0.1f); // Adjust the crossfade time as needed
                }
            }
        }

        void StopWandering()
        {
            isWandering = false;
            isWalking = false;
            isRotatingLeft = false;
            isRotatingRight = false;
        }

        IEnumerator ShuffleClips()
        {
            while (true)
            {
                yield return new WaitForSeconds(15.0f + Random.value * 5.0f);
                SetWalkingAnimation(false); // Ensure idle animation is played when random clip is selected
                PlayAnyClip();
            }
        }

        void PlayAnyClip()
        {
            if (animator != null && idleAnimation != null && walkingAnimation != null)
            {
                // Randomly choose between idle and walking animation
                var randomAnimation = Random.Range(0, 2);
                if (randomAnimation == 0)
                {
                    animator.CrossFade(idleAnimation.name, 0.1f); // Adjust the crossfade time as needed
                }
                else
                {
                    animator.CrossFade(walkingAnimation.name, 0.1f); // Adjust the crossfade time as needed
                }
            }
        }

        IEnumerator Wander()
        {
            while (true)
            {
                if (!isWandering)
                {
                    int rotTime = Random.Range(1, 3);
                    int rotateWait = Random.Range(1, 4);
                    int rotateLorR = Random.Range(1, 3);
                    int walkWait = Random.Range(1, 5);
                    int walkTime = Random.Range(1, 6);

                    isWandering = true;

                    yield return new WaitForSeconds(walkWait);
                    isWalking = true;
                    yield return new WaitForSeconds(walkTime);
                    isWalking = false;
                    yield return new WaitForSeconds(rotateWait);

                    if (rotateLorR == 1)
                    {
                        isRotatingRight = true;
                        yield return new WaitForSeconds(rotTime);
                        isRotatingRight = false;
                    }
                    if (rotateLorR == 2)
                    {
                        isRotatingLeft = true;
                        yield return new WaitForSeconds(rotTime);
                        isRotatingLeft = false;
                    }

                    isWandering = false;
                }
                yield return null;
            }
        }
    }
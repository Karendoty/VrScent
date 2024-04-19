using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingAI : MonoBehaviour
{
  public AnimationClip idleAnimation;
        public AnimationClip walkingAnimation;

        public float collisionCooldown = 2f; // Cooldown period after collision

        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private bool isWalking = false;
        private bool isCooldown = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            if (animator != null && navMeshAgent != null)
            {
                // Disable auto play of animations since we'll control it manually
                animator.applyRootMotion = false;
                StartCoroutine(Wander());
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

                // Set NavMeshAgent destination to a random point within the NavMesh bounds
                Vector3 randomDestination = RandomNavMeshPoint();
                navMeshAgent.SetDestination(randomDestination);

                // Wait until the AI reaches the destination
                yield return new WaitUntil(() => !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f);

                // Stop walking
                SetWalkingAnimation(false);

                // Wait for a while before starting the next wandering cycle
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }

        Vector3 RandomNavMeshPoint()
        {
            // Generate a random point within the NavMesh bounds
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
            return hit.position;
        }
    }

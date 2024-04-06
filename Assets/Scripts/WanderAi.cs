using System.Collections;
using UnityEngine;

namespace CityPeople
{
    public class WanderAi : MonoBehaviour
    {
        public float moveSpeed = 3f;
        public float rotSpeed = 100f;

        private bool isWandering = false;
        private bool isRotatingLeft = false;
        private bool isRotatingRight = false;
        private bool isWalking = false;

        private Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
            StartCoroutine(Wander());
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

        public void StopWandering()
        {
            isWandering = false;
            isWalking = false;
            isRotatingLeft = false;
            isRotatingRight = false;
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
}

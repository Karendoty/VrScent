using UnityEngine;

namespace UI
{
    public class PointToTarget : MonoBehaviour
    {
        [SerializeField] private RoundSystem roundSystem;
        private Transform target;

        private void Start()
        {
            target = roundSystem.objectToFind.transform;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.LookAt(target);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}

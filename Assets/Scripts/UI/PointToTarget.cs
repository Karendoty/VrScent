using UnityEngine;

namespace UI
{
    public class PointToTarget : MonoBehaviour
    {
        [SerializeField] private Transform target;
    
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

using UnityEngine;

namespace UI
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField]
        private Transform _camera;

        private void LateUpdate()
        {
            if (_camera != null)
            {
                transform.LookAt(transform.position + _camera.forward);
            }
        }
    }
}
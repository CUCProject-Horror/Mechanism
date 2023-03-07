using UnityEngine;

namespace Game
{
    public partial class CameraInteractor : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Ray ray = new Ray(
                transform.position,
                new Vector2(Screen.width / 2, Screen.height / 2)
            );
            Gizmos.color = Color.white;
            Gizmos.DrawRay(ray);
            Gizmos.color = Color.red;
            foreach (var target in lastFocused) {
                MeshFilter meshFilter = target.GetComponent<MeshFilter>();
                if(meshFilter)
                    Gizmos.DrawMesh(meshFilter.sharedMesh, target.transform.position);
            }
        }
    }
}
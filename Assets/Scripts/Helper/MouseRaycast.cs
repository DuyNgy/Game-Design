using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Helper
{
    /// <summary>
    /// This class is used to raycast from the mouse position to detect all sorts of stuff.
    /// It can be expanded to include functions needed
    /// </summary>
    public class MouseRaycast : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        /// <summary>
        /// Returns the game object that is hit by the raycast from the mouse position.
        /// If the raycast hits multiple objects, it will return the first one that is not tagged as "Ground".
        /// </summary>
        public GameObject GetGameObject()
        {
            if (EventSystem.current.IsPointerOverGameObject()) 
            {
                return null;
            }
            
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && !hit.collider.CompareTag("Ground"))
                {
                    return hit.collider.gameObject;
                }
            }

            if (hits.Length > 0)
            {
                return hits[0].collider.gameObject;
            }
            return null;
        }

        /// <summary>
        /// Returns the mouse position in world coordinates.
        /// </summary>
        public Vector2 GetMousePosition()
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            return mousePosition;
        }
    }
}
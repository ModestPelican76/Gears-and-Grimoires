using UnityEngine;

public class HideParentOnTouch : MonoBehaviour
{
    // This function is called when this object's collider touches another collider.
    private void OnCollisionEnter2D(Collision2D collision)
    {   
        bool setActive = true;
        // Check if the object we hit is the "Player".
        if (collision.gameObject.CompareTag("Player") && setActive == true)
        {
            // Deactivate our parent GameObject.
            // transform.parent.gameObject refers to the parent of this object.
            transform.parent.gameObject.SetActive(false);
        }
    }
}
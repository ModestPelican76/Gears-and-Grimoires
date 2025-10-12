using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Public variable to set the movement speed from the Unity Inspector.
    public float moveSpeed = 5f;

    // Private reference to the Rigidbody2D component.
    private Rigidbody2D rb;

    // Private Vector2 to store the player's movement input.
    private Vector2 movement;

    // Start is called before the first frame update.
    void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject.
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame. It's best for handling inputs.
    void Update()
    {
        // Get input from the horizontal axis (A/D keys or Left/Right arrows).
        movement.x = Input.GetAxisRaw("Horizontal");

        // Get input from the vertical axis (W/S keys or Up/Down arrows).
        movement.y = Input.GetAxisRaw("Vertical");
    }

    // FixedUpdate is called on a fixed timer. It's best for physics calculations.
    void FixedUpdate()
    {
        // Apply movement to the Rigidbody.
        // We use .normalized to ensure movement speed is consistent in all directions.
        // We multiply by Time.fixedDeltaTime to make the movement frame-rate independent.
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
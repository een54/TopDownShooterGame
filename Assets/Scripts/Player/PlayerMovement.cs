using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;

	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100f;

	void Awake() {
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() { 
		// Want to instantly snap to speed instead of gradually getting there
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		Move (h, v);
		Turning ();
		Animating (h, v);
	}

	void Move(float h, float v) { 
		movement.Set (h, 0f, v);

		// Ensure we're moving the correct distance
		movement = movement.normalized * speed * Time.deltaTime;

		// Apply movement to player
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turning() { 
		// Point underneath where the mouse is placed
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) { 
			Vector3 playerToMouse = floorHit.point - transform.position;

			// Prevent player leaning back
			playerToMouse.y = 0f;

			// Cant store rotations in vectors so use quaternion
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

			// Apply new rotation to player
			playerRigidbody.MoveRotation (newRotation);
		}
	}

	void Animating(float h, float v) { 

		// Check if we are moving horizontally or vertically
		bool walking = h != 0f || v != 0f;

		anim.SetBool ("IsWalking", walking);
	}
}

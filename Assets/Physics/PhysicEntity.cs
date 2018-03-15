using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PhysicEntity : MonoBehaviour {

	public float acceleration = 100f;
	public float moveSpeed = 8f;
	public float jumpForce = 4f; 
	public float gravity = 12f;
	public float height = 1.5f;
	public bool ignoreGravity = false;

	public Vector3 currentVelocity {
		get {
			return m_Velocity;
		}
	}

	public Vector3 planeVelocity {
		get {
			return new Vector3(m_Velocity.x, 0f, m_Velocity.z);
		}
	}

	public Vector3 desiredVelocity {
		get {
			return m_DesiredVelocity;
		}
	}

	public bool isGrounded {
		get {
			return m_Controller.isGrounded;
		}
	}

	protected float m_VerticalSpeed;
	protected float m_ForwardSpeed;
	protected float m_SidewaysSpeed;

	protected Vector3 m_ForwardDirection;
	protected Vector3 m_SidewaysDirection;

	protected Vector3 m_Velocity;
	protected Vector3 m_DesiredVelocity;

	protected Vector3 m_ExtraVelocity;

	protected CharacterController m_Controller;

	protected virtual void Awake () {
		m_Controller = GetComponent<CharacterController>();
		m_Controller.height = height;
		m_VerticalSpeed = 0f;
	}

	protected virtual float ComputeSpeed() {
		float currentSpeed = moveSpeed;
		if(m_ForwardSpeed != 0 && m_SidewaysSpeed != 0) currentSpeed *= .7f;

		return currentSpeed;
	}

	protected virtual CollisionFlags Move(float speed) {

		if(acceleration > 0f) {
			float horizontalSpeed = m_Velocity.y;
			Vector3 nVelocity = Vector3.MoveTowards(m_Velocity, m_DesiredVelocity, acceleration * Time.deltaTime);
			if(!ignoreGravity) nVelocity.y = m_Velocity.y;
			
			m_Velocity = nVelocity;
		} else {
			m_Velocity = new Vector3(m_DesiredVelocity.x, m_Velocity.y, m_DesiredVelocity.z);
		}

		CollisionFlags coll = m_Controller.Move((m_Velocity + m_ExtraVelocity) * Time.deltaTime);		
		if(coll == CollisionFlags.Above && m_Velocity.y > 0f) m_Velocity.y = 0f;
		// if(m_Velocity.magnitude > 20f) EditorApplication.isPaused = true;

		m_ForwardDirection = transform.forward;
		m_SidewaysDirection = transform.right;

		if(ignoreGravity || m_Controller.isGrounded) {
			m_DesiredVelocity = (m_ForwardDirection * m_ForwardSpeed + m_SidewaysDirection * m_SidewaysSpeed) * speed;

			if(!ignoreGravity) m_Velocity.y = -.1f;
			else m_DesiredVelocity += m_VerticalSpeed * transform.up;
		} else {
			m_Velocity.y -= gravity * Time.deltaTime;
		}

		return coll;
	}

	protected virtual void Jump() {
		m_Velocity.y = jumpForce;
	}

	public virtual void AddVelocity(Vector3 velocity) {
		m_Velocity += velocity;
	}

	public virtual void AddExtraVelocity(Vector3 velocity) {
		m_ExtraVelocity += velocity;
	}

	public virtual void RemoveExtraVelocity(Vector3 velocity) {
		m_ExtraVelocity -= velocity;
	}

    void OnControllerColliderHit(ControllerColliderHit hit) {
		// if((m_Controller.collisionFlags & CollisionFlags.Sides) != 0) {
		// 	Vector3 dir = Vector3.Normalize( hit.point - transform.position );
		// 	dir.y = 0f;
        // 	m_Velocity -= dir * m_Velocity.magnitude;
		// }
    }

}

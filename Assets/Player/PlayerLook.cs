using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class PlayerLook : MonoBehaviour {

	private PlayerMovement m_PlayerMovement;
	private Camera m_FPSCamera;
	private Rigidbody m_Rigidbody;

	public float sensitivity = 3F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;
	float rotationX = 0F;
	float rotationY = 0F;
	Quaternion originalRotation;

	private PlayerGun m_PlayerGun;
	private Vector3 m_GunOriginalPosition;
	
	private Transform m_Head;
	
	private bool m_HeadBobbingDown;

	[Header("Head bobbing")]
	[SerializeField]
	private float m_HeadBobbingMin;
	[SerializeField]
	private float m_HeadBobbingDuration;
	private float m_HeadBobbingStart;

	private float m_HeadBobbingVertical;
	private float m_HeadBobbingHorizontal;
	private bool m_IsMoving;

	void Awake () {
        // Application.targetFrameRate = 300;

		m_FPSCamera = GameObject.Find("FPSCamera").GetComponent<Camera>();
		m_PlayerMovement = GetComponent<PlayerMovement>();

		m_Rigidbody = GetComponent<Rigidbody>();
		m_Head = transform.Find("Head");
		// Make the rigid body not change rotation
		if (m_Rigidbody) {
			m_Rigidbody.freezeRotation = true;
		}
		
		m_HeadBobbingDown = true;
		
		// m_PlayerGun = GetComponentInChildren<PlayerGun>();
		// m_GunOriginalPosition = m_PlayerGun.transform.localPosition;

		m_IsMoving = false;

		originalRotation = Quaternion.identity;
		// Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.P)) {
			if(Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
			else Cursor.lockState = CursorLockMode.Locked;
		}
		// Read the mouse input axis
		rotationX += Input.GetAxis("Mouse X") * sensitivity;
		rotationY += Input.GetAxis("Mouse Y") * sensitivity;

		rotationX = ClampAngle (rotationX, minimumX, maximumX);
		rotationY = ClampAngle (rotationY, minimumY, maximumY);

		Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);

		transform.localRotation = originalRotation * xQuaternion;
		m_Head.localRotation = originalRotation * yQuaternion;
		this.HeadBobbing();
	}

	void LateUpdate() {
		// m_FPSCamera.transform.position = m_Head.position;
		Vector3 offset = m_HeadBobbingVertical * m_Head.up + m_HeadBobbingHorizontal * m_Head.right;
		offset *= Time.deltaTime;
		
		m_FPSCamera.transform.position = m_Head.position + offset;
		m_FPSCamera.transform.rotation = m_Head.rotation;
	}

	void HeadBobbing() {
		float moveSpeed = m_PlayerMovement.planeVelocity.magnitude;
		if(!m_PlayerMovement.isGrounded || moveSpeed <= 0f) {
			m_HeadBobbingVertical = 0f;
			m_HeadBobbingHorizontal = 0f;
			m_HeadBobbingStart = 0f;
			m_IsMoving = false;
			m_HeadBobbingDown = true;
			return;
		}

		if(!m_IsMoving) m_HeadBobbingStart = Time.time;
		m_IsMoving = true;

		float duration = m_HeadBobbingDuration / moveSpeed;
		float t = (Time.time - m_HeadBobbingStart) / duration;

		float bobMin = m_HeadBobbingMin * moveSpeed;

		if(m_HeadBobbingDown) {
			m_HeadBobbingVertical = Mathf.Lerp(-bobMin, bobMin, t);
			// m_HeadBobbingHorizontal = Mathf.Lerp(-bobMin, bobMin, t);
		} else {
			m_HeadBobbingVertical = Mathf.Lerp(bobMin, -bobMin, t);
			// m_HeadBobbingHorizontal = Mathf.Lerp(bobMin, -bobMin, t);
		}
		if(t >= 1f) {
			m_HeadBobbingDown = !m_HeadBobbingDown;
			m_HeadBobbingStart = Time.time;
		}
	}

	float ClampAngle (float angle, float min, float max) {
		if (angle < -360F) angle += 360F;
		if (angle > 360F) angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}

}
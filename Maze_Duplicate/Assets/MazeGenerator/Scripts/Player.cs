using UnityEngine;
using System.Collections;

//<summary>
//Ball movement controlls and simple third-person-style camera
//</summary>
public class Player : MonoBehaviour {

	public GameObject ViewCamera = null;
	public AudioClip JumpSound = null;
	public AudioClip HitSound = null;
	public AudioClip CoinSound = null;

	private Rigidbody mRigidBody = null;
	private AudioSource mAudioSource = null;
	private bool mFloorTouched = false;

	private bool gamePlayActive = true;
	public int speed = 5;
	public int rotationSpeed = 1000;

	private GameObject theCollectible;
	private bool mCoinTouched = false;
	public GameObject currentPanel;
	public GameObject gamePanel;

	private const float DISTANCE_THRESHOLD = 1.0f;
	private Vector3 _endPoint;
	private Vector3 direction;

	void Start () {
		mRigidBody = GetComponent<Rigidbody>();
		mAudioSource = GetComponent<AudioSource>();

		// Set initial camera angle and starting point
		direction = (Vector3.up * 2 + Vector3.back) * 4;
		_endPoint = transform.position + direction;
		ViewCamera.transform.position = transform.position + direction;
		ViewCamera.transform.LookAt(transform.position);
	}

	private void Update()
	{
		if (!gamePlayActive)
		{
			return;
		}

		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		// Handle touch input
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			Vector2 touchPosition = touch.position;
			Vector3 movementDirection = new Vector3(touchPosition.x - Screen.width / 2f, 0, touchPosition.y - Screen.height / 2f);
			mRigidBody.velocity = movementDirection.normalized * speed;

			if (movementDirection != Vector3.zero)
			{
				Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
				mRigidBody.MoveRotation(toRotation);
			}
		}
		// No touch input detected, use keyboard input instead
		else
		{
			Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput) * speed;
			mRigidBody.velocity = movementDirection;

			if (movementDirection != Vector3.zero)
			{
				Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
				mRigidBody.MoveRotation(toRotation);
			}
		}
	}

    void LateUpdate () {
		//if (mRigidBody != null) {
		//	if (Input.GetButton ("Horizontal")) {
		//		mRigidBody.AddTorque(Vector3.back * Input.GetAxis("Horizontal")*10);
		//	}
		//	if (Input.GetButton ("Vertical")) {
		//		mRigidBody.AddTorque(Vector3.right * Input.GetAxis("Vertical")*10);
		//	}
		//	if (Input.GetButtonDown("Jump")) {
		//		if(mAudioSource != null && JumpSound != null){
		//			mAudioSource.PlayOneShot(JumpSound);
		//		}
		//		mRigidBody.AddForce(Vector3.up*200);
		//	}
		//}
		if (ViewCamera != null) {
			Vector3 direction = (Vector3.up*2 + Vector3.back)*4;
			//RaycastHit hit;
			//Debug.DrawLine(transform.position,transform.position+direction,Color.red);
			//if(Physics.Linecast(transform.position,transform.position+direction,out hit)){
			//	ViewCamera.transform.position = hit.point;
			//}else{
			//	ViewCamera.transform.position = transform.position+direction;
			//}

			//ViewCamera.transform.position = transform.position + direction;
			//ViewCamera.transform.LookAt(transform.position);

			// Smooth camera moving
			// Check if the distance between the player and the camera is big enough to start moving
			if (Vector3.Distance(transform.position, ViewCamera.transform.position) > DISTANCE_THRESHOLD)
			{
				// Start moving towards the player
				ViewCamera.transform.position = Vector3.Slerp(ViewCamera.transform.position, _endPoint, Time.deltaTime);
			}
			// Check if the camera has finally reached its end point. If it hasn't, keep on moving.
			if (ViewCamera.transform.position != _endPoint)
			{
				ViewCamera.transform.position = Vector3.Slerp(ViewCamera.transform.position, _endPoint, Time.deltaTime);
			}
			// Update the last frame's player position
			_endPoint = transform.position + direction;
			

		}
	}

	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.tag.Equals ("Floor")) {
			mFloorTouched = true;
			if (mAudioSource != null && HitSound != null && coll.relativeVelocity.y > .5f) {
				mAudioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		} else {
			if (mAudioSource != null && HitSound != null && coll.relativeVelocity.magnitude > 2f) {
				mAudioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		}
	}

	void OnCollisionExit(Collision coll){
		if (coll.gameObject.tag.Equals ("Floor")) {
			mFloorTouched = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals ("Coin")) {
			if (!mCoinTouched)
            {
				mCoinTouched = true;
				theCollectible = other.gameObject;
				mRigidBody.velocity = Vector3.zero;
				//Destroy(theCollectible);
				if (mAudioSource != null && CoinSound != null)
				{
					mAudioSource.PlayOneShot(CoinSound);
				}
				// Instantiate game panel
				gamePlayActive = false;
				currentPanel = Instantiate(gamePanel);
			}
		}
	}

	public void EndPanel(bool correct)
    {
		if (correct)
		{
			Destroy(currentPanel);
			Destroy(theCollectible);
			mCoinTouched = false;
			gamePlayActive = true;
		}
		if (!correct)
		{
			Destroy(currentPanel);
			mCoinTouched = false;
			gamePlayActive = true;
		}
	}
}

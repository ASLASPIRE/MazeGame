using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

public class Player : MonoBehaviour {

	[Header("External Components")]
	public GameObject ViewCamera = null;
	public AudioClip JumpSound = null;
	public AudioClip HitSound = null;
	public AudioClip CoinSound = null;
    public Joystick joystick;
    public GameObject MCGamePanel;
	public Timer Timer;
	public TextMeshProUGUI TimerText;
	public Animator TimerDecreaseAnimator;
	public MazeSpawner MazeSpawner;

	public ParticleSystem PlasmaExplosion = null;
	public ParticleSystem EnergyExplosion = null;

    [Header("Player Settings")]
    public int Speed = 5;
    public int TurningSpeed = 1000;
	public int JumpAmount = 5;

    // Components
    private Rigidbody rigidBody = null;
	private AudioSource audioSource = null;
	private Animator animator = null;
	private int isRunningHash = 0;
	[SerializeField] private GameObject shield = null;

    // Parameters
    private const float SLERP_DISTANCE_THRESHOLD = 1.0f;

    [SerializeField] private bool isFloorTouched = false;
    private int numFloorsTouched = 0;
    private bool isGameplayActive = true;
    private bool isCoinTouched = false;
    private Vector3 cameraEndPoint; // Direction from (0, 0, 0)
    private Vector3 cameraDirection; // Direction from player

    // References
    private GameObject theCollectible;
	public GameObject currentPanel;
	

	private void Start()
	{
		// Get components
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
		isRunningHash = Animator.StringToHash("isRunning");
		shield.SetActive(false);

		// Set initial camera angle and starting point
		cameraDirection = (Vector3.up * 4 + Vector3.back) * 4;
		cameraEndPoint = transform.position + cameraDirection;
		ViewCamera.transform.position = transform.position + cameraDirection;
		ViewCamera.transform.LookAt(transform.position);

		// Others
		numFloorsTouched = 0;
	}

    private void Update()
    {
        // Handle isFloorTouched
		if (numFloorsTouched > 0)
		{
			isFloorTouched = true;
		} else
		{
			isFloorTouched = false;
		}
    }

    private void FixedUpdate()
	{
		if (!isGameplayActive)
		{
			return;
		}

		HandlePlayerMovement();
		HandleCameraMovement();

		//Vector3 p = transform.position;
		//if (transform.position.y > 3)
		//{
		//	p.y = 3;
		//	transform.position = p;
		//}
    }

	private void HandlePlayerMovement()
	{
        // Handle player movement with WASD
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Handle touch input
        if (Input.touchCount > 0)
        {
            horizontalInput = joystick.Horizontal;
            verticalInput = joystick.Vertical;
        }

        // If no touch input, just use keyboard's GetAxis() values
        Vector3 movementDirection = new Vector3(horizontalInput * Speed, rigidBody.velocity.y, verticalInput * Speed);
        rigidBody.velocity = movementDirection;

		// Handle animation
		movementDirection.y = 0;
		if (movementDirection == Vector3.zero)
		{
			animator.SetBool(isRunningHash, false);
		} else
		{
			animator.SetBool(isRunningHash, true);
		}

        // Handle player turning
        Vector3 newRotation = new Vector3(movementDirection.x, 0, movementDirection.z);
        if (newRotation != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(newRotation, Vector3.up);
            rigidBody.MoveRotation(toRotation);
        }

        // Handle player jumping with the Space bar
        if (Input.GetKeyDown(KeyCode.C) && isFloorTouched)
        {
			onJumpButtonPress();
        }
    }

	private void HandleCameraMovement()
	{
        if (ViewCamera == null)
        {
            return;
        }

        // Smooth camera moving
        // Check if the distance between the player and the camera is big enough to start moving
        if (Vector3.Distance(transform.position, ViewCamera.transform.position) > SLERP_DISTANCE_THRESHOLD)
        {
            // Start moving towards the player
            ViewCamera.transform.position = Vector3.Lerp(ViewCamera.transform.position, cameraEndPoint, 5 * Time.deltaTime);
        }
        cameraEndPoint = transform.position + cameraDirection;
    }

	private void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag.Equals ("Floor")) {
			numFloorsTouched++;
			if (audioSource != null && HitSound != null && coll.relativeVelocity.y > .5f) {
				audioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		} else {
			if (audioSource != null && HitSound != null && coll.relativeVelocity.magnitude > 2f) {
				audioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		}

		if (coll.gameObject.tag.Equals("Spike")) {
			if (shield.activeInHierarchy) return;
			Debug.Log("touched spike");
			Instantiate(EnergyExplosion, transform.position, Quaternion.Euler(0, 0, 0));
			StartCoroutine(RemoveTime(10.0f));
			//Timer.timeRemaining -= 10.0f;
			//TimerDecreaseAnimator.SetTrigger("PlayAnimation");
		}

		if (coll.gameObject.tag.Equals("PatrolEnemy")) {
			if (shield.activeInHierarchy) return;
			Debug.Log("touched enemy");
			RandomTeleportPlayer();
			StartCoroutine(RemoveTime(10.0f));
		}
	}

	private void RandomTeleportPlayer()
	{
		int randomRow = Random.Range(0, MazeSpawner.Rows);
		int randomColumn = Random.Range(0, MazeSpawner.Columns);
		float x = randomColumn * (MazeSpawner.CellWidth + (MazeSpawner.AddGaps ? .2f : 0));
		float z = randomRow * (MazeSpawner.CellHeight + (MazeSpawner.AddGaps ? .2f : 0));
		Vector3 teleportPos = new Vector3(x, 4, z);
		transform.position = teleportPos;
		Physics.SyncTransforms();

		//Timer.timeRemaining -= 10.0f;
		//TimerDecreaseAnimator.SetTrigger("PlayAnimation");
	}

	private void OnCollisionExit(Collision coll)
	{
		if (coll.gameObject.tag.Equals ("Floor")) {
			numFloorsTouched--;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals ("Coin")) {
			if (!isCoinTouched)
            {
				isCoinTouched = true;
				theCollectible = other.gameObject;
				rigidBody.velocity = Vector3.zero;
				//Destroy(theCollectible);
				if (audioSource != null && CoinSound != null)
				{
					audioSource.PlayOneShot(CoinSound);
				}
				// Instantiate game panel
				isGameplayActive = false;
				currentPanel = Instantiate(MCGamePanel);
			}
		}
		if (other.gameObject.tag.Equals("Powerup_Shield")) {
			Destroy(other.gameObject);
			StartCoroutine(StartShieldPowerup(10.0f));
		}
		if (other.gameObject.tag.Equals("Powerup_Clock")) {
			Destroy(other.gameObject);
			StartCoroutine(AddTime(10.0f));
		}
	}

	private IEnumerator StartShieldPowerup(float durationSeconds)
	{
		shield.SetActive(true);
		PowerupTimer powerupTimer = shield.GetComponentInChildren<PowerupTimer>();
		powerupTimer.RestartTimer(durationSeconds);
		yield return new WaitForSeconds(durationSeconds);
		shield.SetActive(false);
	}

	private IEnumerator AddTime(float durationSeconds)
	{
		Timer.timeRemaining += durationSeconds;
		Color origColor = TimerText.color;
		float origSize = TimerText.fontSize;
		TimerText.color = new Color32(45, 220, 45, 255);
		TimerText.fontSize += 10.0f;
		yield return new WaitForSecondsRealtime(0.5f);
		TimerText.color = origColor;
		TimerText.fontSize = origSize;
	}

	private IEnumerator RemoveTime(float durationSeconds)
	{
		Timer.timeRemaining -= durationSeconds;
		Color origColor = TimerText.color;
		float origSize = TimerText.fontSize;
		TimerText.color = new Color32(245, 25, 25, 255);
		TimerText.fontSize += 10.0f;
		yield return new WaitForSecondsRealtime(0.5f);
		TimerText.color = origColor;
		TimerText.fontSize = origSize;
	}

	public void EndPanel(bool correct)
    {
		if (correct)
		{
			Destroy(currentPanel);
			Destroy(theCollectible);
			isCoinTouched = false;
			isGameplayActive = true;
		}
		if (!correct)
		{
			Destroy(currentPanel);
			isCoinTouched = false;
			isGameplayActive = true;
		}
	}

	public void onJumpButtonPress()
	{
		if (!isFloorTouched)
		{
			return;
		}
		isFloorTouched = false;
        Debug.Log("Space pressed");
        rigidBody.AddForce(Vector3.up * JumpAmount, ForceMode.Impulse);
		Debug.Log($"Force added: {Vector3.up * JumpAmount}");
	}
}

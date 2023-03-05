// -------------------------------------------------------------------------------
// ContactDoor V1.0 - ©2017 Quantum Soup Studios Ltd.
// Code and hacky maths by Chris Payne
//
// Script designed to easily add basic door behaviour
// -------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class ContactDoor : MonoBehaviour
{
	public enum axis
	{
		X,
		Y,
		Z
	};

	public enum doorType
	{
		Hinged,
		Sliding
	};

	[Tooltip("X axis allows cat flaps, drawbridges, etc.")]
	public axis hingeAxis = axis.Y;

	[Tooltip("Set to sliding for scifi games. Everyone knows there are no hinges in space.")]
	public doorType type = doorType.Hinged;

	[Tooltip("Place hinge on the opposite side, or invert slide direction")]
	public bool flipHinge = false;

	[Space(10)]

	[Tooltip("Pair another ContactDoor object with this one and they will open/close together. Pair the other door with this one too.")]
	public ContactDoor pairedWith = null;

	[Range(0.0f,6.0f)]
	[Tooltip("How far must triggering object move away before door closes? Note that thrown objects could land nearby and hold the door open")]
	public float rangeToClose = 3.0f;

	[Range(0.1f,6.0f)]
	[Tooltip("How fast does door open? Note that slow doors will temporarily block the player's movement, so faster is recommended.")]
	public float doorSpeed = 1.0f;

	[Range(0.0f,180.0f)]
	[Tooltip("Maximum angle door will open to. Useful if door opens against wall.")]
	public float maxOpenAngle = 100.0f;

	[Range(0.0f,1.0f)]
	[Tooltip("Multiplier of door width, so you can have sliding doors leave some door visible when open. 0.5 = opens halfway.")]
	public float maxSlideDist = 1.0f;

	[Tooltip("Anim curve for door movement, allowing ease-in/out.")]
	public AnimationCurve curveOpen = AnimationCurve.EaseInOut(0f,0f,1f,1f);

	[Tooltip("Locked doors don't open. Use Lock() function to control from script.")]
	public bool locked;

	[Space(10)]
	[Tooltip("Door SFX will be sent to this mixer group.")]
	public AudioMixerGroup outputGroup;
	[Tooltip("A random SFX from this list will be played when door opens.")]
	public AudioClip[] sfxOpen;
	[Tooltip("A random SFX from this list will be played when door begins closing.")]
	public AudioClip[] sfxClose;
	[Tooltip("A random SFX from this list will be played when door shuts.")]
	public AudioClip[] sfxSlam;

	// Internal vars
	private Vector3 hingeOffset = Vector3.zero;
	private Vector3 slideOffset = Vector3.zero;
	private AudioSource 	audioSource;
	private Vector3	baseEulers;		// Original localEulerAngles
	private Vector3 basePosition;	// Original localPosition
	private float	angle = 0.0f;
	private float 	targetOpen = 0.0f;
	private float	openness = 0.0f;
	private Transform thingThatTriggeredOpen;

	void Start ()
	{
		baseEulers		= transform.localEulerAngles;	// Store door's default position + orientation (assumed to be the closed state)
		basePosition		= transform.position;

		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
		{
			audioSource = (AudioSource)gameObject.AddComponent<AudioSource>();
		}
		audioSource.outputAudioMixerGroup = outputGroup;
		thingThatTriggeredOpen = null;

		// Attempt to calculate hinge position automatically, in case door origin is in the center
		Collider coll = gameObject.GetComponent<Collider>();
		if (coll)
		{
			// Calc offset used for sliding doors
			slideOffset = coll.bounds.extents * 2f;		// Default to full size of collider, then zero the axes we don't care about.
			switch (hingeAxis)
			{
			case axis.X:	slideOffset.y = 0.0f;
							slideOffset.z = 0.0f;
							break; 
			case axis.Y:	slideOffset.x = 0.0f;
							slideOffset.z = 0.0f;
							break; 
			case axis.Z:	slideOffset.x = 0.0f;
							slideOffset.y = 0.0f;
							break; 
			};

			// Calc offset used for hinged doors
			Vector3 centreOffset = coll.bounds.center - gameObject.transform.position;
			if (centreOffset.sqrMagnitude < 0.1f)
			{
				hingeOffset = coll.bounds.extents;
				switch (hingeAxis)
				{
				case axis.X:	hingeOffset.x = 0.0f;
								if (hingeOffset.y>hingeOffset.z)
									hingeOffset.z = 0.0f;
								else
									hingeOffset.y = 0.0f;
								break; 
				case axis.Y:	hingeOffset.y = 0.0f;
								if (hingeOffset.x>hingeOffset.z)
									hingeOffset.z = 0.0f;
								else
									hingeOffset.x = 0.0f;
								break; 
				case axis.Z:	hingeOffset.z = 0.0f;
								if (hingeOffset.y>hingeOffset.x)
									hingeOffset.x = 0.0f;
								else
									hingeOffset.y = 0.0f;
								break; 
				}
			}
		}
	}

	void FixedUpdate ()
	{
		// Abstract value 0-1 representing how open the door is.
		// This is mapped using the AnimCurve and door type to a specific behaviour.
		float old_openness = openness;
		openness = Mathf.MoveTowards( openness, targetOpen, Time.deltaTime * doorSpeed );

		if (openness == 0f && old_openness != 0f)	// Hit 0 this frame?
		{
			if (sfxSlam.Length == 0 || sfxSlam[0] == null)
			{
				Debug.Log("No door slam SFX found!");
			}
			else if (audioSource == null)
			{
				Debug.LogWarning("No audioSource on this door!");
			}
			else
			{
				int rnd = (int)Random.Range(0.0f, (float)sfxSlam.Length - 0.1f);
				//Debug.Log("Playing random door SFX: " + sfxClose[rnd].name);		// Commented out to declutter log, but left here in case you need it
				audioSource.PlayOneShot(sfxSlam[rnd]);
			}
		}

		switch (type)
		{
		case doorType.Hinged:
			{
				angle = curveOpen.Evaluate( Mathf.Abs(openness) ) * maxOpenAngle * ((openness>0.0f)?1.0f:-1.0f);

				if (openness != old_openness)
				{
					Vector3 euler = baseEulers;
					switch (hingeAxis)
					{
						// Sometimes the model might have an unusual rotation, or we want an unusual door
						case axis.X: euler.x += angle; break;
						case axis.Y: euler.y += angle; break;
						case axis.Z: euler.z += angle; break;
					}
					transform.localEulerAngles = euler;
					Vector3 v = transform.localRotation * -hingeOffset * (flipHinge?-1f:1f);
					transform.position = basePosition + hingeOffset * (flipHinge?-1f:1f) + v;
				}
			}
			break;
		case doorType.Sliding:
			{
				if (openness != old_openness)
				{
					transform.position = basePosition + slideOffset * curveOpen.Evaluate( Mathf.Abs(openness) ) * (flipHinge?-1f:1f) * maxSlideDist;
				}
			}
			break;
		}

		if ( Mathf.Abs(openness) >= 1.0f )
		{
			if (thingThatTriggeredOpen != null)
			{
				Vector3 doorPos = basePosition;
				if (pairedWith)
				{	// If paired, then measure distance from midpoint of the two doors, so they react together
					doorPos += pairedWith.basePosition;
					doorPos *= 0.5f;	
				}
				Vector3 d = thingThatTriggeredOpen.position - doorPos;
				if (d.sqrMagnitude > rangeToClose*rangeToClose)
				{
					Close();
				}
			}
			else
			{
				Close();
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Collided with door: " + collision.gameObject.name);

		if (locked)
			return;

		bool clockwise = true;

		if (type==doorType.Hinged)		// Determine which way to swing open (irrelevant if sliding)
		{
			ContactPoint contact = collision.contacts[0];
			Vector3 towardHinge = transform.position + hingeOffset * (flipHinge?-1f:1f) - contact.point;
			Vector3 cross = Vector3.Cross( towardHinge, contact.normal );
			float dir = 0.0f;
			switch (hingeAxis)
			{
			case axis.X:		dir = Vector3.Dot(cross,transform.right); break;
			case axis.Y:		dir = Vector3.Dot(cross,Vector3.up); break;
			case axis.Z:		dir = Vector3.Dot(cross,transform.forward); break;
			}

			if (dir > 0.0f)
				clockwise = false;
			else
				clockwise = true;
		}
		else if (type == doorType.Sliding)
		{
			if (targetOpen != 0f)	// Sliding doors should not move until they have started to close naturally
				return;
		}

		thingThatTriggeredOpen = collision.gameObject.transform;		// Remember the object that bumped the door open, so we can wait for it to get clear before closing

		Open( clockwise );

		if (pairedWith)
		{
			pairedWith.thingThatTriggeredOpen = collision.gameObject.transform;
			pairedWith.Open( !clockwise );	// Paired doors are assumed to open in opposite directions
		}
	}

	// Exposed so you can manually control door from script
	public void Open( bool clockwise )
	{
		// Trigger opening SFX if the door was previously closed
		if (targetOpen == 0.0f)
		{
			if (sfxOpen.Length == 0 || sfxOpen[0] == null)
			{
				Debug.Log("No door SFX found!");
			}
			else if (audioSource == null)
			{
				Debug.LogWarning("No audioSource on this door!");
			}
			else
			{
				int rnd = (int)Random.Range(0.0f, (float)sfxOpen.Length - 0.1f);
				//Debug.Log("Playing random door SFX: " + sfxOpen[rnd].name);		// Commented out to declutter log, but left here in case you need it
				audioSource.PlayOneShot(sfxOpen[rnd]);
			}
		}

		// Now open door
		if (clockwise)
			targetOpen = 1.0f;
		else
			targetOpen = -1.0f;
	}

	// Exposed so you can manually control door from script
	public void Close()
	{
		Debug.Log("Door close, targetOpen = " + targetOpen);		// Commented out to declutter log, but left here in case you need it

		// Trigger closing SFX if the door was previously open
		if (targetOpen != 0f)
		{
			if (sfxClose.Length == 0 || sfxClose[0] == null)
			{
				Debug.Log("No door SFX found!");
			}
			else if (audioSource == null)
			{
				Debug.LogWarning("No audioSource on this door!");
			}
			else
			{
				int rnd = (int)Random.Range(0.0f, (float)sfxClose.Length - 0.1f);
				Debug.Log("Playing random door close SFX: " + sfxClose[rnd].name);		// Commented out to declutter log, but left here in case you need it
				audioSource.PlayOneShot(sfxClose[rnd]);
			}
		}

		targetOpen = 0.0f;
		thingThatTriggeredOpen = null;
	}

	public bool IsLocked()
	{
		return locked;
	}

	public void Lock( bool locked_state )
	{
		locked = locked_state;
	}
}
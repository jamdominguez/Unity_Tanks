using UnityEngine;

public class TankMovement : MonoBehaviour {
    public int m_PlayerNumber = 1;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;


    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_OriginalPitch;


    private void Awake() {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable() {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable() {
        m_Rigidbody.isKinematic = true;
    }


    private void Start() {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }


    private void Update() {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
        EngineAudio();
    }


    private void EngineAudio() {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f) { // No se pulsa tecla
            PlayMovementSound(m_EngineIdling);  // no sonaba parado
        } else { // Se pulsa tecla
            PlayMovementSound(m_EngineDriving); // no sonaba conduciendo
        }
    }

    private void PlayMovementSound(AudioClip clip) {
        if (m_MovementAudio.clip != clip) { // solo entra si no era ese clip
            m_MovementAudio.clip = clip;
            m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
            m_MovementAudio.Play();
        }
    }


    private void FixedUpdate() {
        // Move and turn the tank.
        Move();
        Turn();
    }


    private void Move() { // Se mueve en la Z
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime; // valor concreto por cada fotograma, sin el sería por un segundo
        //m_Rigidbody.AddForce(movement);
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

    }


    private void Turn() { // Rota en la Y
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime; // valor concreto por cada fotograma, sin el sería por un segundo
        Quaternion rotation = Quaternion.Euler(0, turn, 0);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * rotation);
    }
}
using UnityEngine;
using CnControls;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerMovement : MonoBehaviour
{
    //Parametros iniciales del jugador
    public float velocidadJugador;
    public float velocidadRotacion;
    // private Transform camara;
    private Vector3 movementVector;
    public AudioSource audioSource;
    public AudioClip runClip;
    public GameObject graphicPlayer;
    private Vector3 graphicPlayerRotation;
    private Vector3 playerRotation;
    private Animation animationHandler;
    private Rigidbody rbody;
    public Camera playerCamera;
    public float turnSmoothing = 3.0f;
    private Vector3 lastDirection;
    private float horizontalAxis;                              
    private float verticalAxis;                                
    private bool isRunning = false;
    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        graphicPlayerRotation = Vector3.zero; 
        playerRotation = Vector3.zero;
        animationHandler = graphicPlayer.GetComponent<Animation>();
        animationHandler.playAutomatically = true;
        animationHandler["Walk"].speed = 2.0f;
       // playerCamera.transform.localRotation = new Quaternion(0,0,0,0);
    }

    public void unsopported()
    {
        Debug.Log("Unsupported");
    }
    public void FowardMovement()
    {
        this.transform.Translate((Vector3.forward * (verticalAxis)) * velocidadJugador * Time.deltaTime);
    }
    public void RigthMovement()
    {
        this.transform.Translate((Vector3.right * (horizontalAxis)) * velocidadJugador * Time.deltaTime);
    }
    public void changeCameraRunning() {
        Camera.main.fieldOfView = 100;
    }

    public void cambiarVelocidadMira(float nuevaVelocidad)
    {
        velocidadRotacion = nuevaVelocidad;
    }
    private void graphicPlayerRotationHandler() {
        graphicPlayer.transform.localEulerAngles = graphicPlayerRotation;
    }
    private void playerRotationHandler() {
        Debug.Log(playerRotation);
        this.transform.localRotation = playerCamera.transform.localRotation;
    }
    void Update() {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        graphicPlayerRotation = new Vector3(0, Mathf.Atan2(horizontalAxis, verticalAxis) * Mathf.Rad2Deg, 0);
        playerRotation = new Vector3(0, (Input.GetAxis("Axis 4")) * velocidadRotacion * Time.deltaTime, 0);
      if(IsMoving())
           {
            FowardMovement();
            RigthMovement();
            animationHandler.Stop("Idle_1");
            animationHandler.CrossFadeQueued("Walk");
            graphicPlayerRotationHandler();
            
        }
           else {
            animationHandler.Stop("Walk");
            animationHandler.PlayQueued("Idle_1");
        }
      //  playerRotationHandler();

    }

    void LocalFixedUpdate() {
      //  Rotating(horizontalAxis, verticalAxis);
    }

    Vector3 Rotating(float horizontal, float vertical)
    {
        // Get camera forward direction, without vertical component.
        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);

        // Player is moving on ground, Y component of camera facing is not relevant.
        forward.y = 0.0f;
        forward = forward.normalized;

        // Calculate target direction based on camera forward and direction key.
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 targetDirection;
        float finalTurnSmoothing;
        targetDirection = forward * vertical + right * horizontal;
        finalTurnSmoothing = turnSmoothing;

        // Lerp current direction to calculated target direction.
        if ((IsMoving() && targetDirection != Vector3.zero))
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            Quaternion newRotation = Quaternion.Slerp(rbody.rotation, targetRotation, finalTurnSmoothing * Time.deltaTime);
            rbody.MoveRotation(newRotation);
            SetLastDirection(targetDirection);
        }
        // If idle, Ignore current camera facing and consider last moving direction.
        if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
        {
            Repositioning();
        }

        return targetDirection;
    }
    // Set the last player direction of facing.
    public void SetLastDirection(Vector3 direction)
    {
        lastDirection = direction;
    }

    // Put the player on a standing up position based on last direction faced.
    public void Repositioning()
    {
        if (lastDirection != Vector3.zero)
        {
            lastDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
            Quaternion newRotation = Quaternion.Slerp(rbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
            rbody.MoveRotation(newRotation);
        }
    }
    // Check if the player is moving.
    public bool IsMoving()
    {
        return Mathf.Abs(horizontalAxis) > 0.1 || Mathf.Abs(verticalAxis) > 0.1;
    }
}

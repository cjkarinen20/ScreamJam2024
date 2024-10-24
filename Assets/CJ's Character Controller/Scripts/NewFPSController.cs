using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class NewFPSController : MonoBehaviour
{   
    [SerializeField] private GameObject gameOverScreen;
    public enum PlayerMovementState {IDLE, WALKING, RUNNING}
    public PlayerMovementState playerMovementState {private set; get;}
    [SerializeField] private Slider staminaSlider, healthSlider;
    [SerializeField] private InventorySystem inventory;
    [SerializeField] private AudioSource voiceSounds;
    [SerializeField] private AudioClip[] hurt, jump, outOfStam, toolUse;

    private enum TypeOfAudio {HURT, JUMP, OUTOFSTAM, TOOLUSE}
    
    public bool canMove = true;
    private bool isSprinting => sprintEnabled && Input.GetKey(sprintKey) && Input.GetKey(KeyCode.W) && !isCrouching;
    private bool shouldJump => Input.GetKeyDown(jumptKey) && characterController.isGrounded && !isCrouching;
    private bool isHoldingCrouch => Input.GetKey(crouchKey);
    [Header("Functional Options")]
    [SerializeField] public bool mouseLookEnabled = true;
    [SerializeField] private bool sprintEnabled = true;
    [SerializeField] private bool jumpEnabled = true;
    [SerializeField] private bool crouchEnabled = true;
    [SerializeField] private bool headBobEnabled = true;
    [SerializeField] private bool slopeSlidingEnabled = true;
    [SerializeField] private bool zoomEnabled = true;
    [SerializeField] private bool interactionEnabled = true;
    [SerializeField] private bool footstepsEnabled = true;
    [SerializeField] private bool staminaEnabled = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumptKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;
    

    private float recoilAmount = 0;
    [Header("Movement Parameters")]
    [SerializeField] private float acceleraction = 9;
    [SerializeField] public float walkSpeed = 4.5f;
    [SerializeField] private float sprintSpeed = 6.2f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float slopeSpeed = 8f;
    [SerializeField] private float rotSpeed = 50f;


    [Header("Look Parameters")]
    [SerializeField] private Animator cameraAnim;
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 80.0f;

    [Header("Health Parameters")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float regenCooldown = 10;
    [SerializeField] private float healthIncreaseIncrement = 1;
    [SerializeField] private float healthTimeIncrement = 0.1f;
    public float currentHealth;
    private Coroutine regeneratingHealth;
    public static Action<float> OnTakeDamage;
    public static Action<float> OnDamage;
    public static Action<float> OnHeal;
    public Image bloodOverlay;

    [Header("Stamina Parameters")]
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaUseMultiplier = 5;
    [SerializeField] private AudioSource staminaAudioSource;
    [SerializeField] private AudioClip staminaOutSound;
    [SerializeField] private float timeBeforeStaminaRegenStarts = 5;
    [SerializeField] private float staminaIncreaseIncrement = 2;
    [SerializeField] private float staminaTimeIncrement = 0.1f;
    public float currentStamina;
    private Coroutine regeneratingStamina;
    public static Action<float> OnStaminaChange;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3 (0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool ongoingCrouchAnimation;

    [Header("Zoom Parameters")]
    [SerializeField] private float timeToZoom = 0.3f;
    [SerializeField] private float zoomFOV = 40f;
    private float defaultFOV;
    private Coroutine zoomRoutine;


    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.7f;
    [SerializeField] private float sprintStepSpeed = 0.3f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.3f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] stoneSounds = default;
    [SerializeField] private AudioClip[] stoneScuffSounds = default;
    [SerializeField] private AudioClip[] woodSounds = default;
    [SerializeField] private AudioClip[] woodScuffSounds = default;
    [SerializeField] private AudioClip[] dirtSounds = default;
    [SerializeField] private AudioClip[] dirtScuffSounds = default;
    [SerializeField] private AudioClip[] tileSounds = default;
    [SerializeField] private AudioClip[] tileScuffSounds = default;
    [SerializeField] private AudioClip[] waterSounds = default;
    [SerializeField] private AudioClip[] waterScuffSounds = default;



    private float footstepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : isSprinting ? baseStepSpeed = sprintStepMultiplier : baseStepSpeed;

    private float currentSpeed = 0, currentBobSpeed = 0;
    private bool isCurrentlyMoving = false, isGrounded = false;


    // SLIDING PARAMETERS
    private Vector3 hitPointNormal;

    private bool isSliding
    {
        get
        {
            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }
    [Header("Interaction Parameters")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;

    public Camera playerCamera {get; private set;}
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    public static NewFPSController instance;

    private Color bloodOverlayDefaultColor;

    private void OnEnable()
    {
        canMove = true;

        gameOverScreen.SetActive(false);
        OnTakeDamage += ApplyDamage;
        canMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnDisable()
    {
        OnTakeDamage -= ApplyDamage;
    }

    void Awake()
    {
        origWalkSpeed = walkSpeed;
        origSprintSpeed = sprintSpeed;
        origCrouchSpeed = crouchSpeed;
        instance = this;
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponentInChildren<CharacterController>();
        defaultFOV = playerCamera.fieldOfView;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        bloodOverlayDefaultColor = bloodOverlay.color;
    }

    void Update()
    {   
        if(recoilAmount > 0){
            recoilAmount = Mathf.Lerp(recoilAmount, 0, Time.deltaTime * 3);
        }else{
            recoilAmount = 0;
        }

        HandleGrounded();

        //Specifies when to update movement
        if (canMove) 
        {
            if(walkSpeed < origWalkSpeed) walkSpeed += .1f * Time.deltaTime;
            if(sprintSpeed < origSprintSpeed) sprintSpeed += .1f * Time.deltaTime;
            if(crouchSpeed < origCrouchSpeed) crouchSpeed += .1f * Time.deltaTime;

            HandleMovementInput();
            
            if(crouchEnabled){
                HandleCrouch();
            }
            if(mouseLookEnabled)
                HandleMouseLook();
            if (jumpEnabled)
                HandleJump();
            if (zoomEnabled)
                HandleZoom();
            if (footstepsEnabled)
                HandleFootsteps();
            if (interactionEnabled)
            {
                HandleInteractionCheck();
            }
            if (staminaEnabled)
                HandleStamina();

            DamageOverlay();

            //Debug.Log("Health: " + currentHealth);
            //Debug.Log("Stamina: " + currentStamina);

            ApplyFinalMovement();
        }

        staminaSlider.value = currentStamina;
        healthSlider.value = currentHealth;
    }
    private float origWalkSpeed, origSprintSpeed, origCrouchSpeed;
    public void SlowPlayer(){
        walkSpeed /= 3;
        sprintSpeed /= 3;
        crouchSpeed /= 3;
    }
    public void LookAt(GameObject target){
        StartCoroutine(LookAtTarget(target.transform.position));
    }

    IEnumerator LookAtTarget(Vector3 target){
        canMove = false;

        float time = 2;
        float elapsed = 0;

        while(elapsed < time){
            elapsed += Time.deltaTime;

            Vector3 relativePos = target - playerCamera.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos.normalized, Vector3.up);
            Quaternion playerRot = rotation;
            Quaternion cameraRot = rotation;
            playerRot.x = 0;
            playerRot.z = 0;

            cameraRot.z = 0;
            cameraRot.y = 0;

            transform.rotation = Quaternion.Lerp(transform.rotation, playerRot, Time.deltaTime * 5);
            playerCamera.transform.localRotation = Quaternion.Lerp(playerCamera.transform.localRotation, cameraRot, Time.deltaTime * 5);
            

            yield return null;
        }
    }
    private void HandleMovementInput()
    {
        if(isCrouching){
            cameraAnim.speed = .5f;
        }else{
            cameraAnim.speed = 1;
        }
        Vector2 desiredInput = new Vector2(currentSpeed * Input.GetAxis("Vertical"), currentSpeed * Input.GetAxis("Horizontal"));

        if(desiredInput.magnitude <= 0 && isCurrentlyMoving && characterController.isGrounded){
            isCurrentlyMoving = false;
            PlayScuffSound();
        }else if(desiredInput.magnitude > 0 && !isCurrentlyMoving){
            isCurrentlyMoving = true;
        }

        if(desiredInput.magnitude > 0){
            if(isSprinting){
                cameraAnim.ResetTrigger("Walk");
                cameraAnim.ResetTrigger("Idle");
                cameraAnim.SetTrigger("Run");
                playerMovementState = PlayerMovementState.RUNNING;
            }else{
                cameraAnim.ResetTrigger("Idle");
                cameraAnim.ResetTrigger("Run");
                cameraAnim.SetTrigger("Walk");
                playerMovementState = PlayerMovementState.WALKING;
            }
        }else{
            cameraAnim.ResetTrigger("Walk");
            cameraAnim.ResetTrigger("Run");
            cameraAnim.SetTrigger("Idle");
            playerMovementState = PlayerMovementState.IDLE;
        }

        if(characterController.isGrounded){
            currentSpeed = Mathf.Lerp(currentSpeed, isCrouching? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed, Time.deltaTime * acceleraction); 
        }
        
        currentInput = Vector2.Lerp(currentInput, desiredInput, Time.deltaTime * desiredInput.magnitude > 0 ? 3 : 8);

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }
    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX - recoilAmount, 0, 0); //Apply recoil to mouse look

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }
    private void HandleJump()
    {
        if (shouldJump && sprintEnabled){
            PlayVoiceSound(TypeOfAudio.JUMP);
            moveDirection.y = jumpForce;
            cameraAnim.SetTrigger("Jump");
        }
    }
    private void HandleGrounded () {
        if(!isGrounded && characterController.isGrounded){
            cameraAnim.SetTrigger("Land");
            PlayScuffSound();
        }

        isGrounded = characterController.isGrounded;
    }
    private void HandleCrouch()
    {
        if (characterController.isGrounded && !ongoingCrouchAnimation){
            if(!isCrouching && isHoldingCrouch || isCrouching && !isHoldingCrouch){
                StartCoroutine(CrouchStand());
            }
        }
    }
    public void AddRecoil(GunData gunData)
    {
        recoilAmount += gunData.recoilAmount;
    }

    private void HandleStamina()
    {
        if (isSprinting && currentInput != Vector2.zero)
        {
            if (regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }
            currentStamina -= staminaUseMultiplier * Time.deltaTime;

            if (currentStamina < 0)
                currentStamina = 0;

            OnStaminaChange?.Invoke(currentStamina);

            if (currentStamina <= 0)
            {
                PlayVoiceSound(TypeOfAudio.OUTOFSTAM);
                sprintEnabled = false;

            }

        }
        if (!isSprinting && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
        }
    }

    private void HandleZoom()
    {
        if (Input.GetKeyDown(zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomRoutine = StartCoroutine(ToggleZoom(true));

        }
        if (Input.GetKeyUp(zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomRoutine = StartCoroutine(ToggleZoom(false));

        }
    }
    private void ApplyFinalMovement()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;
        else
            moveDirection.y = -gravity * Time.deltaTime * 2;

        if (slopeSlidingEnabled && isSliding)
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.z) * slopeSpeed;


            characterController.Move(moveDirection * Time.deltaTime);
    }
    private void HandleFootsteps()
    {
        if (!characterController.isGrounded) return;
        if (currentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                PlayFootStepSound();
            if (isSprinting)
                footstepTimer = sprintStepSpeed;
            else
                footstepTimer = baseStepSpeed;
            }
    }
    private void PlayFootStepSound(){
        if(!canMove) return;
        footstepAudioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        if (Physics.Raycast(characterController.transform.position, Vector3.down, out RaycastHit hit, 3))
        {
            switch (hit.collider.tag)
            {
                case "Wood":
                    footstepAudioSource.PlayOneShot(woodSounds[UnityEngine.Random.Range(0, woodSounds.Length)]);
                    break;
                case "Stone":
                    footstepAudioSource.PlayOneShot(stoneSounds[UnityEngine.Random.Range(0, stoneSounds.Length)]);
                    break;
                case "Dirt":
                    footstepAudioSource.PlayOneShot(dirtSounds[UnityEngine.Random.Range(0, dirtSounds.Length)]);
                    break;
                case "Tile":
                    footstepAudioSource.PlayOneShot(tileSounds[UnityEngine.Random.Range(0, tileSounds.Length)]);
                    break;
                case "Water":
                    footstepAudioSource.PlayOneShot(waterSounds[UnityEngine.Random.Range(0, waterSounds.Length)]);
                    break;
                default:
                if (isSprinting)
                    footstepTimer = sprintStepSpeed;
                else
                    footstepTimer = baseStepSpeed;
                break;
            }
        }
    }
    private void PlayScuffSound(){
        if(!canMove) return;
        footstepAudioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        if (Physics.Raycast(characterController.transform.position, Vector3.down, out RaycastHit hit, 3))
        {
            switch (hit.collider.tag)
            {
                case "Wood":
                    footstepAudioSource.PlayOneShot(woodScuffSounds[UnityEngine.Random.Range(0, woodScuffSounds.Length)]);
                    break;
                case "Stone":
                    footstepAudioSource.PlayOneShot(stoneScuffSounds[UnityEngine.Random.Range(0, stoneScuffSounds.Length)]);
                    break;
                case "Dirt":
                    footstepAudioSource.PlayOneShot(dirtScuffSounds[UnityEngine.Random.Range(0, dirtScuffSounds.Length)]);
                    break;
                case "Tile":
                    footstepAudioSource.PlayOneShot(tileScuffSounds[UnityEngine.Random.Range(0, tileScuffSounds.Length)]);
                    break;
                case "Water":
                    footstepAudioSource.PlayOneShot(waterScuffSounds[UnityEngine.Random.Range(0, waterScuffSounds.Length)]);
                    break;
                default:
                
                break;
            }
        }
    }
    private void DamageOverlay()
    {
        float transparency = 1f - (currentHealth / 100f);
        Color imageColor = bloodOverlayDefaultColor;
        imageColor.a = transparency;
        bloodOverlay.color = imageColor;
    }
    public void ApplyDamage(float damage)
    {
        PlayVoiceSound(TypeOfAudio.HURT);
        currentHealth -= damage;
        OnDamage?.Invoke(currentHealth);

        if (currentHealth <= 0)
            KillPlayer();
        //else if (regeneratingHealth != null)
            //StopCoroutine(regeneratingHealth);

        //regeneratingHealth = StartCoroutine(RegenerateHealth());
    }
    public void KillPlayer()
    {
        canMove = false;
        currentHealth = 0;

        if (regeneratingHealth != null)
            //StopCoroutine(regeneratingHealth);

        Debug.Log("DEAD");

        gameOverScreen.SetActive(true);

        //Add further implementation later
        //Make a "fade to black" animation play here and restart the scene
    }

    //IEnumerator Respawn(){
    //    gameObject.SetActive(false);
    //    currentHealth = 100;
    //    currentStamina = 100;
    //    transform.position = respawnPosition.transform.position;
    //    monsterObject.SetActive(false);
//
    //    yield return null;
    //    monsterObject.SetActive(true);
    //    gameObject.SetActive(true);
    //}
    private void HandleInteractionCheck()
    {
        if(Input.GetKeyDown(interactKey)){
            if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
            {
                if(hit.collider.TryGetComponent(out Interactable interactable)){
                    if(interactable != null){
                        interactable.OnInteract();
                    }
                }
                if(hit.collider.TryGetComponent(out iBreakable breakable)){
                    if(breakable != null){
                        if(breakable.transform.TryGetComponent<Breakable>(out Breakable childBreakable) && !childBreakable.hasBeenBroken && childBreakable.requiredTool == iBreakable.RequiredTool.Crowbar){
                            if(childBreakable.requiredTool == iBreakable.RequiredTool.Crowbar && !inventory.hasCrowbar){
                                return;
                            } 

                            inventory.UseCrowbar();
                            PlayVoiceSound(TypeOfAudio.TOOLUSE);
                        }
                        if(breakable.transform.TryGetComponent<Breakable>(out childBreakable) && !childBreakable.hasBeenBroken && childBreakable.requiredTool == iBreakable.RequiredTool.Boltcutters){
                            if(childBreakable.requiredTool == iBreakable.RequiredTool.Boltcutters && !inventory.hasBoltCutters){
                                return;
                            }

                            inventory.UseBoltCutter();
                            PlayVoiceSound(TypeOfAudio.TOOLUSE);
                        }

                        breakable.OnInteract();
                    }
                }
            }
        }
    }
    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.SphereCast(playerCamera.transform.position, characterController.radius + .15f, Vector3.up, out RaycastHit hit, 1f))
            yield break;

        ongoingCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        isCrouching = !isCrouching;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        yield return new WaitForSeconds(.1f);// ? Wait an additional delay to prevent some clipping bugs

        ongoingCrouchAnimation = false;
    }
    private IEnumerator ToggleZoom(bool enterZoom)
    {
        float targetFOV = enterZoom ? zoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView;
        float timeElapsed = 0;

        while (timeElapsed < timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = targetFOV;
        zoomRoutine = null;
    }
    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(regenCooldown);
        WaitForSeconds timeToWait = new WaitForSeconds(healthTimeIncrement);

        while(currentHealth < maxHealth)
        {
            currentHealth += healthIncreaseIncrement;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            OnHeal?.Invoke(currentHealth);
            yield return timeToWait;
        }

        regeneratingHealth = null;
    }
    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(timeBeforeStaminaRegenStarts);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaTimeIncrement);

        while(currentStamina < maxStamina)
        {
            if (currentStamina > 0)
                sprintEnabled = true;
            currentStamina += staminaIncreaseIncrement;

            if (currentStamina > maxStamina)
                currentStamina = maxStamina;

            yield return timeToWait;
        }
        regeneratingStamina = null;
    }

    private void PlayVoiceSound (TypeOfAudio typeOfAudio) {
        if(!canMove) return;
        switch (typeOfAudio) {
            case TypeOfAudio.HURT:
                voiceSounds.clip = hurt[UnityEngine.Random.Range(0, hurt.Length)];
                voiceSounds.Play();
                break;
            case TypeOfAudio.JUMP:
                voiceSounds.clip = jump[UnityEngine.Random.Range(0, jump.Length)];
                voiceSounds.Play();
                break;
            case TypeOfAudio.OUTOFSTAM:
                voiceSounds.clip = outOfStam[UnityEngine.Random.Range(0, outOfStam.Length)];
                voiceSounds.Play();
                break;
            case TypeOfAudio.TOOLUSE:
                voiceSounds.clip = toolUse[UnityEngine.Random.Range(0, toolUse.Length)];
                voiceSounds.Play();
                break;
            default :
                
                break;
        }
    }
}

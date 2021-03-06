﻿using UnityEngine;

public class Ball : MonoBehaviour {

    [HideInInspector]
    public float minSpeed;
    [HideInInspector]
    public float magnetedTime;
    [HideInInspector]
    public bool canScore = true;
    [HideInInspector]
    public bool canBePulsed = false;
    [HideInInspector]
    public bool isFixedY = false;
    [HideInInspector]
    public bool canBeMagneted = true;

    [HideInInspector]
    public Rigidbody Rb;
    private TrailRenderer trail;
    [SerializeField]
    private Color neutralColor;

    [HideInInspector]
    public AI_Ball AI;

    [SerializeField]
    protected ParticleSystem BallBlastEffect;
    [SerializeField]
    private ParticleSystem ballColorBlast;

    private bool justPulsed;
    private int lastHitPlayer;
    private int secondLastHitPlayer;
    Color lastHitPlayerColor;
    
    private PlayFMODEvent _collisionSound;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        AI = GetComponent<AI_Ball>();
        neutralColor = new Color(0.5f,0.5f,0.5f,1f);

        _collisionSound = GetComponent<PlayFMODEvent>();
    }

    private void OnEnable()
    {
        canScore = true;
        canBePulsed = false;
        isFixedY = false;

        justPulsed = false;
        lastHitPlayer = -1;
        secondLastHitPlayer = -1;
    }

    private void OnDisable()
    {
        Rb.velocity = Vector3.zero;
        trail.material.SetColor("_TintColor", neutralColor);
    }

    protected void FixedUpdate()
    {
        magnetedTime -= Time.fixedDeltaTime;

        if (Rb.velocity.magnitude < minSpeed && magnetedTime < 0)
            Rb.velocity = Rb.velocity.normalized * minSpeed;

        RaycastHit hit;
        bool isOverStage = Physics.SphereCast(transform.position, transform.localScale.x, Vector3.down, out hit, 2f, LayerMask.NameToLayer("Floor"), QueryTriggerInteraction.Ignore);

        if (isFixedY && isOverStage)
            Rb.velocity = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);
        else if (transform.position.y < -5)
            gameObject.SetActive(false);

        //Makes balls turn towards their velocity direction. Prevents balls from rolling
        //transform.rotation = Quaternion.LookRotation(Rb.velocity, Vector3.up);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        OnCollisionStay(collision);

        _collisionSound.Play(playAnyway:false);
    }

    protected void OnCollisionStay(Collision collision)
    {
        Player player = collision.collider.GetComponentInParent<Player>();
        if (player != null)
            SetLastHitPlayer(player, false);

        Ball ball = collision.collider.GetComponent<Ball>();
        if (ball != null && justPulsed)
        {
            ball.SetLastHitPlayer(lastHitPlayer, lastHitPlayerColor);
            ball.SpawnColorEffect(collision);
        }

        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Floor"))
            justPulsed = false;

        if (canBePulsed) return;
        
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            isFixedY = true;
            canBePulsed = true;
        }
    }

    private void SpawnColorEffect(Collision collision)
    {
        ParticleSystem colorBlast = ballColorBlast;
        var BallBlastParts = colorBlast.GetComponentsInChildren<ParticleSystem>();
        foreach (var part in BallBlastParts)
        {
            var partStartColor = part.main;
            partStartColor.startColor = lastHitPlayerColor;
        }
        Destroy(Instantiate(colorBlast, collision.collider.transform.position, collision.collider.transform.rotation), 2f);
    }

    protected void OnCollisionExit(Collision collision){
        OnCollisionStay(collision);
    }

    public void SpawnPulseBlastOff(Color playerColor, Vector3 direction)
    {
        ParticleSystem ballBlast = BallBlastEffect;
        var BallBlastParts = ballBlast.GetComponentsInChildren<ParticleSystem>();
        lastHitPlayerColor = playerColor;
        foreach (var part in BallBlastParts)
        {
            var partStartColor = part.main;
            partStartColor.startColor = lastHitPlayerColor;
        }
        
        Instantiate(ballBlast, transform.position, Quaternion.LookRotation(direction));
    }

    public void SetLastHitPlayer(Player player, bool fromPulse)
    {
        int playerNumber = (int)player.currentPlayer;
        if (lastHitPlayer == playerNumber)
            return;

        secondLastHitPlayer = lastHitPlayer;
        lastHitPlayer = playerNumber;
        lastHitPlayerColor = player.GetColor();
        trail.material.SetColor("_TintColor", lastHitPlayerColor);

        justPulsed = fromPulse;
    }

    public void SetLastHitPlayer(int playerNumber, Color playerColor)
    {
        if (lastHitPlayer == playerNumber)
            return;

        secondLastHitPlayer = lastHitPlayer;
        lastHitPlayer = playerNumber;
        lastHitPlayerColor = playerColor;
        trail.material.SetColor("_TintColor", lastHitPlayerColor);
    }

    public int[] GetLastPlayerHits()
    {
        return new int[2] { lastHitPlayer, secondLastHitPlayer };
    }
}

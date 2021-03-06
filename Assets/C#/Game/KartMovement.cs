﻿using UnityEngine;

public class KartMovement : MonoBehaviour
{
    private float speed;
    private float maxStunTime;
    [HideInInspector]
    public float stunTimer;

    public Curve movementCurve;
    [SerializeField]
    private float _curveTime = 0.5f;
    public float CurveTime { get { return _curveTime; } }

    [SerializeField]
    private ParticleSystem stunParticles;
    [SerializeField]
    private ParticleSystem moveParticles;
    private Animator kartAnimator;
    private PlayerAnimator playerAnimator;

    private void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        speed = gameManager.playerSpeed;
        maxStunTime = gameManager.maxStunTime;
        stunTimer = maxStunTime;

        kartAnimator = GetComponentInChildren<Animator>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void Update()
    {
        stunTimer += Time.deltaTime;
    }

    public void Move(float _input)
    {
        if (stunTimer < maxStunTime)
        {
            _input *= 0.25f;
            stunParticles.Play();
        }
        else
        {
            if (stunParticles.isPlaying) stunParticles.Stop();            
        }
        if (moveParticles != null) {
            if (Mathf.Abs(_input)>0.2f) {
                if (!moveParticles.isPlaying) moveParticles.Play();
            }
        }

        _curveTime += _input * speed * Time.unscaledDeltaTime;
        if (_curveTime < 0)
            _curveTime = 0;
        if (_curveTime > 1)
            _curveTime = 1;

        transform.position = MathHelp.GetCurvePosition(movementCurve.start.position, movementCurve.middle.position, movementCurve.end.position, _curveTime);
        transform.rotation = MathHelp.GetCurveRotation(movementCurve.start.rotation, movementCurve.middle.rotation, movementCurve.end.rotation, _curveTime);

        if (kartAnimator != null)
        {
            kartAnimator.SetFloat("Input", _input);
            var bonus = (_curveTime < 0.55 && _curveTime > 0.45);
            kartAnimator.SetBool("Bonus", bonus);

            
        }
        if(playerAnimator != null)
        {
            playerAnimator.AnimateMovement(_input);
        }
    }
}

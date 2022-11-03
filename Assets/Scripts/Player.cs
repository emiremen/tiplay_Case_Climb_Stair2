using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Playables;
using Newtonsoft.Json.Linq;
using System;

public class Player : MonoBehaviour
{
    private GameData gameData;
    private Animator animator;
    [SerializeField] private ParticleSystem sweatParticle;
    [SerializeField] private float sweatingAmount = 0;

    private SkinnedMeshRenderer playerMeshRenderer;

    private Tween makeRedTween;
    [SerializeField] private bool isTouchedScreen;

    [SerializeField] private float waitForStep = 1f;

    private void OnEnable()
    {
        sweatingAmount = 0;
        EventManager.getPlayer += getPlayer;
        EventManager.getPlayerAnimator += getPlayerAnimator;
        EventManager.characterJump += CharacterJump;
        EventManager.setPlayerVisibility += SetPlayerVisibility;
        EventManager.setIsTouchedScreen += checkIsTouchedScreen;
    }

    private void OnDisable()
    {
        EventManager.getPlayer -= getPlayer;
        EventManager.getPlayerAnimator -= getPlayerAnimator;
        EventManager.characterJump -= CharacterJump;
        EventManager.setPlayerVisibility -= SetPlayerVisibility;
        EventManager.setIsTouchedScreen -= checkIsTouchedScreen;
    }

    private Animator getPlayerAnimator()
    {
        return animator;
    }

    void Start()
    {
        gameData = EventManager.getGameData?.Invoke();
        playerMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        ChangeCharacterMaterialTiling(1f, 0);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        waitForStep -= Time.deltaTime;
        if (EventManager.getGameState.Invoke() == GameState.start)
        {
            CharacterSweatingAndBreathing();
            if (isTouchedScreen && waitForStep <= 0)
            {
                waitForStep = 1f;
                animator.SetBool("isRunning", true);
                animator.SetBool("isIdle", false);
                CharacterJump();
            }
            else
            {
                if (waitForStep > 0 && !animator.GetBool("isRunning"))
                {
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isIdle", true);
                }
            }
        }
    }

    public void StopRunningAnimationEvent()
    {
        if (waitForStep > 0) return;
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", true);
    }

    private void checkIsTouchedScreen(bool value)
    {
        isTouchedScreen = value;
    }

    private void CharacterSweatingAndBreathing()
    {
        if (sweatingAmount >= 0)
        {
            sweatingAmount -= Time.deltaTime;
        }
        if (sweatingAmount > 8 + (gameData.staminaLevel * 2))
        {
            sweatParticle.Play();
            if (sweatingAmount > 10 + (gameData.staminaLevel * 2))
            {
                ChangeCharacterMaterialTiling(1.15f);
            }
            else
            {
                ChangeCharacterMaterialTiling(1f);
            }
        }
        else
        {
            animator.SetBool("isBreathing", false);
        }
    }

    private GameObject getPlayer()
    {
        return gameObject;
    }

    private void CharacterJump()
    {
        GameObject spawnedStair = EventManager.spawnStair?.Invoke();

        transform.DOComplete();
        transform.DOJump(spawnedStair.transform.GetChild(0).position + new Vector3(0, 0.035f, 0), .05f, 1, .5f).OnUpdate(() => transform.LookAt(new Vector3(spawnedStair.transform.GetChild(0).position.x, transform.position.y, spawnedStair.transform.GetChild(0).position.z))).OnComplete(() =>
        {
            if (EventManager.getGameState.Invoke() == GameState.start)
            {
                spawnedStair = EventManager.spawnStair?.Invoke();
                EventManager.decreaseScore?.Invoke();
                EventManager.gainMoney?.Invoke();
                if (sweatingAmount <= 15 + (gameData.staminaLevel * 2))
                {
                    sweatingAmount += 2;
                }
                else
                {
                    animator.SetBool("isRunning", false);
                    EventManager.showGameOverPanel?.Invoke();
                    SetPlayerVisibility(false);
                    sweatParticle.Stop();
                    sweatingAmount = 0;
                    ChangeCharacterMaterialTiling(1f);
                }
                transform.DOJump(spawnedStair.transform.GetChild(0).position + new Vector3(0, 0.035f, 0), .05f, 1, .5f).OnUpdate(() => transform.LookAt(new Vector3(spawnedStair.transform.GetChild(0).position.x, transform.position.y, spawnedStair.transform.GetChild(0).position.z))).OnComplete(() =>
                {
                    if (EventManager.getGameState.Invoke() == GameState.start)
                    {
                        EventManager.decreaseScore?.Invoke();
                        EventManager.gainMoney?.Invoke();
                    }
                });
            }
        }
        );
    }

    private void SetPlayerVisibility(bool visibility)
    {
        playerMeshRenderer.enabled = visibility;
        ChangeCharacterMaterialTiling(1f, 0);
    }

    private void ChangeCharacterMaterialTiling(float value)
    {
        makeRedTween.Kill();
        makeRedTween = playerMeshRenderer.material.DOOffset(new Vector2(0, value), 1f);
    }
    private void ChangeCharacterMaterialTiling(float value, float duration)
    {
        makeRedTween.Kill();
        makeRedTween = playerMeshRenderer.material.DOOffset(new Vector2(0, value), duration);
    }
}

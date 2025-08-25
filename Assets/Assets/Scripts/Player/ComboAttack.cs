using UnityEngine;
using System.Collections;

public class ComboAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;

    private int comboStep = 0;
    private bool nextComboQueued = false;
    private bool isAttacking = false;

    [Header("Hitbox GameObjects cho từng đòn")]
    public GameObject hitboxAA1;
    public GameObject hitboxAA2;
    public GameObject hitboxAA3;

    // Delay thời gian trước khi bật hitbox (theo animation)
    private readonly float[] hitboxDelays = { 0f, 0.25f, 0.17f, 0.12f };
    // index 1 = AA1, 2 = AA2, 3 = AA3

    private Coroutine hitboxCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        if (animator == null)
        {
            Debug.LogError("Animator not found!");
            enabled = false;
        }

        DisableAllHitboxes();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }

        HandleDirectionInput();
    }

    private void OnClick()
    {
        if (!isAttacking && comboStep == 0)
        {
            if (playerController != null && playerController.isGrounded)
            {
                comboStep = 1;
                animator.SetTrigger("AA1");
                Debug.Log("Start combo: AA1");
            }
        }
        else if (isAttacking)
        {
            nextComboQueued = true;
            Debug.Log("Queued next attack");
        }
    }

    private void HandleDirectionInput()
    {
        if (playerController == null) return;

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (!isAttacking && comboStep > 0)
        {
            if (moveInput > 0 && !playerController.isFacingRight)
            {
                playerController.Flip(moveInput);
            }
            else if (moveInput < 0 && playerController.isFacingRight)
            {
                playerController.Flip(moveInput);
            }
        }
    }

    public void OnAttackStart()
    {
        isAttacking = true;

        if (playerController != null && playerController.rb != null)
        {
            playerController.rb.linearVelocity = new Vector2(0f, playerController.rb.linearVelocity.y);
        }

        // Bật hitbox sau delay
        if (hitboxCoroutine != null) StopCoroutine(hitboxCoroutine);
        hitboxCoroutine = StartCoroutine(EnableHitboxWithDelay(comboStep));

        Debug.Log($"Attack started: Step {comboStep}");
    }

    public void OnAttackEndOrChain()
    {
        isAttacking = false;
        DisableAllHitboxes();

        if (hitboxCoroutine != null) StopCoroutine(hitboxCoroutine);

        if (nextComboQueued)
        {
            nextComboQueued = false;
            comboStep++;

            switch (comboStep)
            {
                case 2:
                    animator.SetTrigger("AA2");
                    Debug.Log("Chain to AA2");
                    break;
                case 3:
                    animator.SetTrigger("AA3");
                    Debug.Log("Chain to AA3");
                    break;
                default:
                    comboStep = 1;
                    animator.SetTrigger("AA1");
                    Debug.Log("Loop back to AA1");
                    break;
            }
        }
        else
        {
            ResetCombo();
        }
    }

    public void OnFinalAttackEnd()
    {
        ResetCombo();
    }

    public void ResetCombo()
    {
        comboStep = 0;
        nextComboQueued = false;
        isAttacking = false;
        DisableAllHitboxes();

        if (hitboxCoroutine != null) StopCoroutine(hitboxCoroutine);

        Debug.Log("Combo reset.");
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void CancelAttack()
    {
        comboStep = 0;
        nextComboQueued = false;
        isAttacking = false;

        animator.ResetTrigger("AA1");
        animator.ResetTrigger("AA2");
        animator.ResetTrigger("AA3");

        DisableAllHitboxes();

        if (hitboxCoroutine != null) StopCoroutine(hitboxCoroutine);

        Debug.Log("Attack canceled");
    }

    // Coroutine bật hitbox sau delay
    private IEnumerator EnableHitboxWithDelay(int step)
    {
        float delay = (step >= 1 && step <= 3) ? hitboxDelays[step] : 0f;
        yield return new WaitForSeconds(delay);
        EnableHitbox(step);
    }

    private void EnableHitbox(int step)
    {
        DisableAllHitboxes();

        switch (step)
        {
            case 1:
                if (hitboxAA1 != null) hitboxAA1.SetActive(true);
                break;
            case 2:
                if (hitboxAA2 != null) hitboxAA2.SetActive(true);
                break;
            case 3:
                if (hitboxAA3 != null) hitboxAA3.SetActive(true);
                break;
        }
    }

    private void DisableAllHitboxes()
    {
        if (hitboxAA1 != null) hitboxAA1.SetActive(false);
        if (hitboxAA2 != null) hitboxAA2.SetActive(false);
        if (hitboxAA3 != null) hitboxAA3.SetActive(false);
    }
}

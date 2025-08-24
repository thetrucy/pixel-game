using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    private Animator animator;
    private int comboStep = 0;
    private float lastClickTime;
    private float maxComboDelay = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on " + gameObject.name + "! Please assign an Animator component.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Xử lý input tấn công độc lập cho cả mặt đất và không trung
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }

        // Reset combo nếu quá thời gian
        if (Time.time - lastClickTime > maxComboDelay)
        {
            comboStep = 0;
        }
    }

    public void OnClick()
    {
        lastClickTime = Time.time;
        comboStep++;

        if (animator != null)
        {
            PlayerController pc = GetComponent<PlayerController>();
            if (pc != null)
            {
                if (!pc.isGrounded) Debug.Log("Attacking in air, comboStep: " + comboStep);
                else Debug.Log("Attacking on ground, comboStep: " + comboStep);
            }

            if (comboStep == 1)
            {
                animator.SetTrigger("AA1");
                Debug.Log("Triggering AA1 at " + Time.time);
            }
            else if (comboStep == 2)
            {
                animator.SetTrigger("AA2");
                Debug.Log("Triggering AA2 at " + Time.time);
            }
            else if (comboStep == 3)
            {
                animator.SetTrigger("AA3");
                Debug.Log("Triggering AA3 at " + Time.time);
                // Không reset combo ngay, để Animator hoàn thành
            }
        }
        else
        {
            Debug.LogError("Animator is null in ComboAttack.OnClick");
        }
    }

    public void ResetCombo()
    {
        comboStep = 0;
        Debug.Log("Combo reset at " + Time.time);
    }
}
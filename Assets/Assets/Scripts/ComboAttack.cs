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
    }

    void Update()
    {
        // Bấm chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }

        // Reset combo nếu chờ quá lâu
        if (Time.time - lastClickTime > maxComboDelay)
        {
            comboStep = 0;
        }
    }

    void OnClick()
    {
        lastClickTime = Time.time;
        comboStep++;

        if (comboStep == 1)
        {
            animator.SetTrigger("AA1");
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("AA2");
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("AA3");
            comboStep = 0; // Reset lại sau combo cuối
        }
    }

    // Có thể gọi từ Animation Event để reset combo chính xác theo thời gian clip
    public void ResetCombo()
    {
        comboStep = 0;
    }
}

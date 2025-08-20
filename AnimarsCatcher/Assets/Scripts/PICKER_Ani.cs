using UnityEngine;

public class PICKER_Ani : MonoBehaviour
{
    public Transform LeftHandIKTrans;
    public Transform RightHandIKTrans;

    private Animator m_Animator;
    private bool m_CanMove = false;  // 是否可以移动
    private Vector3 m_TargetPos;    // 要移动到的位置
    private float m_AniSpeed = 5.0f; // 移动速度
    private static readonly int AniSpeedHash = Animator.StringToHash("AniSpeed");   // 动画参数哈希值
                                                                                    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void SetMoveTargetPos(Vector3 targetPos)
    {
        m_TargetPos = targetPos;
        m_CanMove = true; // 设置可以移动
    }

    private void Update()
    {
        if (m_CanMove)
        {
            m_Animator.SetFloat(AniSpeedHash, m_AniSpeed);
            float step = m_AniSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, m_TargetPos, step);

            var dir = (m_TargetPos - transform.position).normalized;    // 计算方向向量, 使得头朝向目标点
            transform.forward = dir;

            if (Vector3.Distance(transform.position, m_TargetPos) < 1f)
            {
                m_CanMove = false; // 到达目标位置后停止移动
                m_Animator.SetFloat(AniSpeedHash, 0f); // 停止动画
            }
        }
    }
}

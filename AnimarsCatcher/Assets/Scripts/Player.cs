using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // 新增


namespace AnimarsCatcher
{
    public class Player : MonoBehaviour
    {
        [Header("List of Anis")]
        [Tooltip("拾取者Ani")] public Animator PICKER_Ani;
        [Tooltip("战斗者Ani")] public Animator BLASTER_Ani;
        [Tooltip("Ani的移动速度")]  public float MoveSpeed = 20.0f;

        private List<PICKER_Ani> m_PickerAniList = new List<PICKER_Ani>();
        private List<BLASTER_Ani> m_BlasterAniList = new List<BLASTER_Ani>();
        private float m_AniSpeed = 2.0f;

        // 右键控制圈地范围
        [Header("Control Range")]
        [Tooltip("圈地范围的最小值")] public float ControlRadiusMin = 0f;
        [Tooltip("圈地范围的最大值")] public float ControlRadiusMax = 5f;

        private float m_CurrentRadius = 0f;     // 当前圈地范围
        private Vector3 m_TargetPos;            // 鼠标点击的世界坐标
        private bool m_RightMouseButton;        // 鼠标右键是否按下

        // Component references 组件引用
        private Rigidbody m_Rigidbody;

        // MainCamera reference 主摄像机引用
        private Camera m_MainCamera;

        private void Awake()
        {
            // 获取刚体组件
            m_Rigidbody = GetComponent<Rigidbody>();
            if (m_Rigidbody == null)
            {
                Debug.LogError("Rigidbody component not found on the player object.");
            }
            // 获取主摄像机
            m_MainCamera = Camera.main;
            if (m_MainCamera == null)
            {
                Debug.LogError("Main camera not found in the scene.");
            }
            // 初始化输入系统
            controls = new InputSystem_Actions();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            RobotMove();

            // 按下鼠标右键时清理之前的Ani列表(只在按下的那一下执行)
            if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
            {
                ClearAniLists(); // 清理之前的Ani列表
            }
                
            // 鼠标右键框选Ani
            if (Mouse.current != null && Mouse.current.rightButton.isPressed)
            {
              
                m_RightMouseButton = true;
                m_TargetPos = GetMouseWorldPosition();
                GetControlAnis();
            }
            else
            {
                 m_RightMouseButton = false;
            }

            // 鼠标左键移动Ani们
            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                var targetPos = GetMouseWorldPosition();
                foreach (var pickerAni in m_PickerAniList)
                {
                    if (pickerAni != null)
                    {
                        pickerAni.SetMoveTargetPos(targetPos);
                    }
                }

                foreach (var blasterAni in m_BlasterAniList)
                {
                    if (blasterAni != null)
                    {
                        blasterAni.SetMoveTargetPos(targetPos);
                    }
                }
            }

            m_CurrentRadius = Mathf.Lerp(m_CurrentRadius, m_RightMouseButton ? ControlRadiusMax : ControlRadiusMin,
                Time.deltaTime * 10);
        }

        /// <summary>
        /// 机器人移动方法
        /// 使用Input System获取输入
        /// </summary>
        private void RobotMove()
        {
            //float h = Input.GetAxis("Horizontal");
            float h = controls.Player.Move.ReadValue<Vector2>().x;          //使用inputsystem
            float v = controls.Player.Move.ReadValue<Vector2>().y;
            //float v = Input.GetAxis("Vertical");
            float y = m_MainCamera.transform.rotation.eulerAngles.y;

            // 目标移动方向
            Vector3 targetDirection = new Vector3(h, 0, v);
            targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;

            if (targetDirection != Vector3.zero)
            {
                transform.forward = Vector3.Lerp(transform.forward, targetDirection, Time.deltaTime * 10f);
            }
            var speed = targetDirection * MoveSpeed;
            m_Rigidbody.linearVelocity = speed; //设置刚体线性速度


        }

        /// <summary>
        /// 清理Ani列表
        /// </summary>
        private void ClearAniLists()
        {
            m_PickerAniList.Clear();
            m_BlasterAniList.Clear();
        }

        /// <summary>
        /// 获取控制圈内的Ani
        /// </summary>
        private void GetControlAnis()
        {
            Collider[] hitColliders = new Collider[50];
            int numColliders = Physics.OverlapSphereNonAlloc(m_TargetPos,m_CurrentRadius,hitColliders);
            foreach (var collider in hitColliders)
            {
                if (collider != null)
                {
                    if(collider.CompareTag("PICKER_Ani"))
                    {
                        m_PickerAniList.Add(collider.GetComponent<PICKER_Ani>());
                    }
                    else if (collider.CompareTag("BLASTER_Ani"))
                    {
                        m_BlasterAniList.Add(collider.GetComponent<BLASTER_Ani>());
                    }
                }
            }
        }
        private Vector3 GetMouseWorldPosition()
        {
            // 获取鼠标在屏幕上的位置
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            // 将鼠标位置转换为世界坐标
            Ray ray = m_MainCamera.ScreenPointToRay(mousePosition);
            
            if (Physics.Raycast(ray, out var hit, 200f))
            {
                // 绘制射线
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 0.1f);
           
                return hit.point;
            }
            return Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            // 绘制当前圈地范围
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_TargetPos, m_CurrentRadius);
        
        }

        /// <summary>
        /// 用输入系统测试动画器
        /// </summary>
        private void AnimationSystemTest()
        {
            // 使用新Input System检测鼠标左键按下
            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                PICKER_Ani.SetBool("LeftMouseDown", true);
                BLASTER_Ani.SetBool("LeftMouseDown", true);
            }

            if (Mouse.current != null && Mouse.current.rightButton.isPressed)
            {
                m_AniSpeed = Mathf.Clamp(m_AniSpeed - Time.deltaTime * 5, 2, 5);
                PICKER_Ani.SetFloat("AniSpeed", m_AniSpeed);
                BLASTER_Ani.SetFloat("AniSpeed", m_AniSpeed);
            }

            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                m_AniSpeed = Mathf.Clamp(m_AniSpeed + Time.deltaTime * 5, 2, 5);
                PICKER_Ani.SetFloat("AniSpeed", m_AniSpeed);
                BLASTER_Ani.SetFloat("AniSpeed", m_AniSpeed);
            }

            if (Keyboard.current != null && Keyboard.current.jKey.isPressed)
            {
                BLASTER_Ani.SetTrigger("Shoot");
            }
        }

        private InputSystem_Actions controls;


        void OnEnable()
        {
            controls.Enable();
        }

        void OnDisable()
        {
            controls.Disable();
        }
    }
}
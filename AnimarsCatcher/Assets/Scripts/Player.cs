using UnityEngine;
using UnityEngine.InputSystem; // 新增


namespace AnimarsCatcher
{
    public class Player : MonoBehaviour
    {
        public Animator PICKER_Ani;
        public Animator BLASTER_Ani;
        
        public float MoveSpeed = 20.0f;

        private float m_AniSpeed = 2.0f;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //float h = Input.GetAxis("Horizontal");
            float h = controls.Player.Move.ReadValue<Vector2>().x;          //使用inputsystem
            float v = controls.Player.Move.ReadValue<Vector2>().y;
            //float v = Input.GetAxis("Vertical");
            float y = Camera.main.transform.rotation.eulerAngles.y;

            // 目标移动方向
            Vector3 targetDirection = new Vector3(h, 0, v);
            targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;

            if (targetDirection != Vector3.zero)
            {
                transform.forward = Vector3.Lerp(transform.forward, targetDirection, Time.deltaTime * 10f);
            }
            var speed = targetDirection * MoveSpeed;
            transform.GetComponent<Rigidbody>().linearVelocity = speed; //设置刚体线性速度

            // 使用新Input System检测鼠标左键按下
            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                PICKER_Ani.SetBool("LeftMouseDown", true);
                BLASTER_Ani.SetBool("LeftMouseDown", true);
            }

            if (Mouse.current != null && Mouse.current.rightButton.isPressed)
            {
                m_AniSpeed = Mathf.Clamp(m_AniSpeed - Time.deltaTime * 5,2,5);
                PICKER_Ani.SetFloat("AniSpeed", m_AniSpeed);
                BLASTER_Ani.SetFloat("AniSpeed", m_AniSpeed);
            }

            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                m_AniSpeed = Mathf.Clamp(m_AniSpeed + Time.deltaTime * 5,2,5);
                PICKER_Ani.SetFloat("AniSpeed", m_AniSpeed);
                BLASTER_Ani.SetFloat("AniSpeed", m_AniSpeed);
            }

            if (Keyboard.current != null && Keyboard.current.jKey.isPressed)
            {
                BLASTER_Ani.SetTrigger("Shoot");
            }
        }

        private InputSystem_Actions controls;

        void Awake()
        {
            controls = new InputSystem_Actions();
        }

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
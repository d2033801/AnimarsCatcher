using UnityEngine;
using UnityEngine.InputSystem; // 新增


namespace AnimarsCatcher
{
    public class Player : MonoBehaviour
    {
        public Animator PICKER_Ani;
        public Animator BLASTER_Ani;
        

        private float m_AniSpeed = 2.0f;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
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
    }
}
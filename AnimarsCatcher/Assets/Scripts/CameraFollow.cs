using UnityEngine;

namespace AnimarsCatcher
{
    // CameraFollow ������ʵ�������������ҵĹ���
    public class CameraFollow : MonoBehaviour
    {
        private Vector3 m_Offset; // ����������֮���ƫ����

        private Transform m_Player; // ��Ҷ���� Transform

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_Player = GameObject.FindGameObjectWithTag("Player").transform; // ���Ҵ��� "Player" ��ǩ����Ϸ����
            m_Offset = transform.position - m_Player.position; // ��������������֮��ĳ�ʼƫ����
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = m_Player.position + m_Offset; // ���������λ�ã�ʹ��ʼ�ո������
        }
    }
}
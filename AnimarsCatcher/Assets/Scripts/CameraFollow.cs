using UnityEngine;

namespace AnimarsCatcher
{
    // CameraFollow 类用于实现摄像机跟随玩家的功能
    public class CameraFollow : MonoBehaviour
    {
        private Vector3 m_Offset; // 摄像机与玩家之间的偏移量

        private Transform m_Player; // 玩家对象的 Transform

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_Player = GameObject.FindGameObjectWithTag("Player").transform; // 查找带有 "Player" 标签的游戏对象
            m_Offset = transform.position - m_Player.position; // 计算摄像机与玩家之间的初始偏移量
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = m_Player.position + m_Offset; // 更新摄像机位置，使其始终跟随玩家
        }
    }
}
using UnityEngine;

public class TitleMovement : MonoBehaviour
{
    public GameObject m_Title;
    private float m_Time = 2f;

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        if(m_Time > 0)
        {
            m_Title.transform.Rotate(0, 0, 0.1f);
            m_Time -= 1f * Time.fixedDeltaTime;
        }
        else
        {
            m_Title.transform.Rotate(0, 0, -0.1f);
            m_Time -= 1f * Time.fixedDeltaTime;
            if(m_Time < -2f)
                m_Time = 2f;
        }
    }
}

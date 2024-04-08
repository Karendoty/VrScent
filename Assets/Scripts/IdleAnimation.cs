using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimation : MonoBehaviour
{
    private Animator m_Animator;
    public AnimationClip m_Animation;


    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Animator.Play(m_Animation.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

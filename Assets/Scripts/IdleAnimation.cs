using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 -- Attach this to NPCs for certain idle animations --

Simple script that finds animator and plays specific animation clip that you refference.
 */

public class IdleAnimation : MonoBehaviour
{
    private Animator m_Animator;
    public AnimationClip m_Animation;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Animator.Play(m_Animation.name);
    }
}

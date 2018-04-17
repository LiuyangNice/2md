using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine <T> {
    public T m_Target;
    private State<T> m_CourrentState;
    private State<T> m_NextState;
    public StateMachine(T target)
    {
        m_Target = target;
        CourrentState = null;        
    }
    public void SetCourrentState(State<T> courrentState)
    {
        m_CourrentState = courrentState;
        m_CourrentState.target = m_Target;
        m_CourrentState.EnterState(m_Target);
    }
    public void ChangeState(State<T> newState)
    {
        m_CourrentState.ExitState(m_Target);        
        m_CourrentState = newState;
        m_CourrentState.target = m_Target;
        m_CourrentState.EnterState(m_Target);
    }
    public void ChangeStateNext()
    {
        m_CourrentState.ExitState(m_Target);
        m_CourrentState = NextState;
        m_CourrentState.target = m_Target;
        m_CourrentState.EnterState(m_Target);

    }
    public void UpdateFsm()
    {
        m_CourrentState.UpdateState(m_Target);
        
    }
    public State<T> CourrentState
    {
        get
        {
            return m_CourrentState;
        }

        set
        {
            m_CourrentState = value;
        }
    }

    public State<T> NextState
    {
        get
        {
            return m_NextState;
        }

        set
        {
            m_NextState = value;
        }
    }
}
public class State<T>
{

    public T target;

    public virtual void EnterState(T target)
    {

    }
    public virtual void UpdateState(T target)
    {

    }
    public virtual void ExitState(T target)
    {

    }
}

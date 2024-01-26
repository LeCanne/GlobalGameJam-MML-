using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimsScript : MonoBehaviour
{
    [SerializeField] private BlaBlaScript script;

    public void EndAnim()
    {
        script.EndExpression();
    }
}

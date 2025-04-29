using System;
using System.Collections;
using UnityEngine;

public class CustomCoroutine : MonoBehaviour
{

    private void Start()
    {
        IEnumerator a = Test();
        //a.MoveNext()
        //WaitForSeconds
    }

    private IEnumerator Test()
    {
        yield return 1;
    }
}

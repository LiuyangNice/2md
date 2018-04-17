using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.IO;
using Vectrosity;
using System.Collections.Generic;
public class Test : MonoBehaviour
{
    public List<Vector3> list;
    private void Start()
    {
        VectorLine line = new VectorLine("aaa",list , 2, LineType.Continuous, Joins.Fill);
        line.Draw3DAuto();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSetUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.SetUpCanvas();
    }
}

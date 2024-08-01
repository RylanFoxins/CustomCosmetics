﻿using System.Collections.Generic;
using UnityEngine;

public class HatDescriptor : MonoBehaviour
{
    public string Name = "";
    public string Author = "";
    public string Description = "";

    public List<CosmeticBehaviour> behaviours = new List<CosmeticBehaviour>();
}

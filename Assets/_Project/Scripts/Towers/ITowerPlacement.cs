using System;
using UnityEngine;

namespace _Project.Scripts.Towers
{
    public interface ITowerPlacement
    {
        event Action<Vector3> OnPlaceClicked;
    }
}
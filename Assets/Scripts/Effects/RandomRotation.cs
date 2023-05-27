using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomRotation : MonoBehaviour
{
    private void Enable()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }
}

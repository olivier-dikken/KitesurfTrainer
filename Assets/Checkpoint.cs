using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public Path parentPath;

    public void SetDisplacement()
    {
        transform.position.Set(0, 0, 0);
    }
}

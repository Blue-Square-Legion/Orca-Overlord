using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{
    void OnGetGrabbed(float grabForce, Transform playerPos);
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeviceRotation
{

    public static bool gyroInitialized = false;

    public static bool HasGyroScope
    {
        get
        {
            return SystemInfo.supportsGyroscope;
        }
    }

    public static Quaternion Get()
    {
        if (!gyroInitialized)
        {
            InitGyro();
        }

        return HasGyroScope ? ReadGyroscopeRotation() : Quaternion.identity;

    }

    private static void InitGyro()
    {
        if (HasGyroScope)
        {
            Input.gyro.enabled = true;
            Input.gyro.updateInterval = 0.0167f; // set interval to maximum update speed
        }
        gyroInitialized = true;
    }

    private static Quaternion ReadGyroscopeRotation()
    {
        return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
    }
}

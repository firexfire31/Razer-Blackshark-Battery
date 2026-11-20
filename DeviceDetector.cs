
using System;
using System.Management;

namespace RazerBattery;

public static class DeviceDetector
{
    private const string VendorId = "VID_1532";
    private const string ProductId = "PID_0A41";

    public static bool IsHeadsetConnected()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity");

            foreach (ManagementObject device in searcher.Get())
            {
                string? id = device["DeviceID"]?.ToString();

                if (id != null &&
                    id.Contains(VendorId, StringComparison.OrdinalIgnoreCase) &&
                    id.Contains(ProductId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }

        return false;
    }
}

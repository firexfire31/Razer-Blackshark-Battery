
using System;
using System.Management;

namespace RazerBattery;

public static class DeviceDetector
{
    public static bool IsRazerDeviceConnected()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity");

            foreach (ManagementObject device in searcher.Get())
            {
                string id = device["DeviceID"]?.ToString() ?? "";
                string name = device["Name"]?.ToString() ?? "";

                if (id.Contains("VID_1532&PID_0A41",
                    StringComparison.OrdinalIgnoreCase)
                    || name.Contains("BlackShark V2 Pro Xbox",
                    StringComparison.OrdinalIgnoreCase))
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

    public static string GetRazerHidDevices()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity");

            var result = "";

            foreach (ManagementObject device in searcher.Get())
            {
                string id = device["DeviceID"]?.ToString() ?? "";

                if (id.Contains("VID_1532",
                    StringComparison.OrdinalIgnoreCase))
                {
                    result += "Name: " + device["Name"] + Environment.NewLine;
                    result += "ID: " + id + Environment.NewLine;
                    result += "Status: " + device["Status"] + Environment.NewLine;
                    result += Environment.NewLine;
                }
            }

            return result.Length > 0 ? result : "Razer cihazı bulunamadı.";
        }
        catch (Exception ex)
        {
            return "HID ERROR: " + ex.Message;
        }
    }
}

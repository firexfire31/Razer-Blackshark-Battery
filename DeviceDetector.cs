
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

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

                if (id.Contains("VID_1532&PID_0A41",
                    StringComparison.OrdinalIgnoreCase))
                    return true;
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
        var report = new StringBuilder();

        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity");

            foreach (ManagementObject device in searcher.Get())
            {
                string id = device["DeviceID"]?.ToString() ?? "";

                if (id.Contains("VID_1532",
                    StringComparison.OrdinalIgnoreCase))
                {
                    report.AppendLine("NAME: " + device["Name"]);
                    report.AppendLine("CLASS: " + device["PNPClass"]);
                    report.AppendLine("STATUS: " + device["Status"]);
                    report.AppendLine("DEVICE ID: " + id);
                    report.AppendLine("------------------------------");
                }
            }
        }
        catch (Exception ex)
        {
            return "HID ERROR: " + ex.Message;
        }

        return report.Length > 0
            ? report.ToString()
            : "Razer cihazı bulunamadı.";
    }
}

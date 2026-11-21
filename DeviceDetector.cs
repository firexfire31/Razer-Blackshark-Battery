
using System;
using System.Linq;
using System.Management;
using System.Text;

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
                string id = device["DeviceID"]?.ToString() ?? "";

                if (id.Contains(VendorId,
                        StringComparison.OrdinalIgnoreCase) &&
                    id.Contains(ProductId,
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
        var result = new StringBuilder();

        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity WHERE PNPClass='HIDClass'");

            foreach (ManagementObject device in searcher.Get())
            {
                string id = device["DeviceID"]?.ToString() ?? "";

                if (!id.Contains(VendorId,
                    StringComparison.OrdinalIgnoreCase))
                    continue;

                result.AppendLine("Name: " + device["Name"]);
                result.AppendLine("Status: " + device["Status"]);
                result.AppendLine("ID: " + id);
                result.AppendLine();
            }

            if (result.Length == 0)
                result.AppendLine("No Razer HID devices found.");
        }
        catch (Exception ex)
        {
            result.AppendLine("HID ERROR: " + ex.Message);
        }

        return result.ToString();
    }
}

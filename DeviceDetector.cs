
using System;
using System.Management;
using System.Text;
using HidSharp;

namespace RazerBattery;

public static class DeviceDetector
{
    // USB bağlantısını Windows üzerinden kontrol et.
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

    // HID arayüzlerini ayrıca listele.
    public static string GetRazerHidDevices()
    {
        var report = new StringBuilder();

        try
        {
            var devices = DeviceList.Local.GetHidDevices(0x1532);

            foreach (var device in devices)
            {
                report.AppendLine("VENDOR: " +
                    device.VendorID.ToString("X4"));

                report.AppendLine("PRODUCT: " +
                    device.ProductID.ToString("X4"));

                report.AppendLine("PATH: " +
                    device.DevicePath);

                report.AppendLine("INPUT LENGTH: " +
                    device.MaxInputReportLength);

                report.AppendLine("OUTPUT LENGTH: " +
                    device.MaxOutputReportLength);

                report.AppendLine("FEATURE LENGTH: " +
                    device.MaxFeatureReportLength);

                report.AppendLine("-----------------------------");
            }

            if (report.Length == 0)
                return "No Razer HID devices found.";
        }
        catch (Exception ex)
        {
            return "HID ERROR: " + ex.Message;
        }

        return report.ToString();
    }
}

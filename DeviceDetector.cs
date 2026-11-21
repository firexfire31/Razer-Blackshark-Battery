
using System;
using System.Management;
using System.Text;
using HidSharp;

namespace RazerBattery;

public static class DeviceDetector
{
    public static bool IsRazerDeviceConnected()
    {
        try
        {
            foreach (var device in
                DeviceList.Local.GetHidDevices(0x1532, 0x0A41))
            {
                return true;
            }
        }
        catch
        {
        }

        return false;
    }

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

                report.AppendLine("--------------------------------");
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

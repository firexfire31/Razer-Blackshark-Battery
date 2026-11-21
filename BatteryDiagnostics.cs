
using System;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;

namespace RazerBattery;

public static class BatteryDiagnostics
{
    public static string GetReport()
    {
        var report = new StringBuilder();

        report.AppendLine("RAZER BATTERY DIAGNOSTICS");
        report.AppendLine("========================");

        // USB DEVICE DETECTION
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity");

            var devices = searcher.Get()
                .Cast<ManagementObject>()
                .Where(d =>
                    (d["DeviceID"]?.ToString() ?? "")
                    .Contains("VID_1532&PID_0A41",
                        StringComparison.OrdinalIgnoreCase));

            foreach (var device in devices)
            {
                report.AppendLine("DEVICE: " + device["Name"]);
                report.AppendLine("STATUS: " + device["Status"]);
            }
        }
        catch (Exception ex)
        {
            report.AppendLine("USB ERROR: " + ex.Message);
        }

        // GAMEINPUT SERVICE
        try
        {
            using var service = new ServiceController("GameInputSvc");
            report.AppendLine("GAMEINPUT: " + service.Status);
        }
        catch (Exception ex)
        {
            report.AppendLine("GAMEINPUT ERROR: " + ex.Message);
        }

        // RAZER HID DEVICES
        report.AppendLine();
        report.AppendLine("=== RAZER HID DEVICES ===");

        try
        {
            var hidDevices = DeviceDetector.GetRazerHidDevices();

            if (hidDevices.Count == 0)
            {
                report.AppendLine("No Razer HID interfaces found.");
            }
            else
            {
                foreach (var device in hidDevices)
                {
                    report.AppendLine("HID PATH: " + device);
                }
            }
        }
        catch (Exception ex)
        {
            report.AppendLine("HID ERROR: " + ex.Message);
        }

        // ALL RAZER USB INTERFACES
        report.AppendLine();
        report.AppendLine("=== ALL RAZER USB INTERFACES ===");

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
                    report.AppendLine("Name: " + device["Name"]);
                    report.AppendLine("Class: " + device["PNPClass"]);
                    report.AppendLine("Status: " + device["Status"]);
                    report.AppendLine("ID: " + id);
                    report.AppendLine();
                }
            }
        }
        catch (Exception ex)
        {
            report.AppendLine("USB ERROR: " + ex.Message);
        }

        // BATTERY STATUS
        report.AppendLine();
        report.AppendLine("=== BATTERY STATUS ===");
        report.AppendLine("DEVICE: Razer BlackShark V2 Pro Xbox 2.4");
        report.AppendLine("BATTERY: Not yet detected");
        report.AppendLine("No estimated percentage is displayed.");

        return report.ToString();
    }
}

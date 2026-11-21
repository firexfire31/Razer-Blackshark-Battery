
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

        report.AppendLine("=== USB DEVICE DETECTION ===");

        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity");

            var devices = searcher.Get()
                .Cast<ManagementObject>()
                .Where(d =>
                {
                    string id = d["DeviceID"]?.ToString() ?? "";

                    return id.Contains("VID_1532&PID_0A41",
                               StringComparison.OrdinalIgnoreCase) ||
                           id.Contains("VID_1532&PID_0A49",
                               StringComparison.OrdinalIgnoreCase);
                });

            foreach (var device in devices)
            {
                report.AppendLine("DEVICE: " + device["Name"]);
                report.AppendLine("STATUS: " + device["Status"]);
                report.AppendLine("ID: " + device["DeviceID"]);
                report.AppendLine();
            }
        }
        catch (Exception ex)
        {
            report.AppendLine("USB ERROR: " + ex.Message);
        }

        report.AppendLine("=== GAMEINPUT SERVICE ===");

        try
        {
            using var service = new ServiceController("GameInputSvc");
            report.AppendLine("GAMEINPUT: " + service.Status);
        }
        catch (Exception ex)
        {
            report.AppendLine("GAMEINPUT ERROR: " + ex.Message);
        }

        report.AppendLine();
        report.AppendLine("=== RAZER HID DEVICES ===");

        try
        {
            report.AppendLine(DeviceDetector.GetRazerHidDevices());
        }
        catch (Exception ex)
        {
            report.AppendLine("HID ERROR: " + ex.Message);
        }

        report.AppendLine();
        report.AppendLine("=== BATTERY STATUS ===");
        report.AppendLine("DEVICE: Razer BlackShark V2 Pro Xbox");
        report.AppendLine("SUPPORTED USB IDS: 1532:0A41 / 1532:0A49");
        report.AppendLine("BATTERY: Not yet queried");
        report.AppendLine("REAL BATTERY READING: Pending HID protocol test");
        report.AppendLine("NO ESTIMATED PERCENTAGE IS DISPLAYED.");

        return report.ToString();
    }
}

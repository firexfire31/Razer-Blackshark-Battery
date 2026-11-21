
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RazerBattery;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainWindow());
    }
}

public class MainWindow : Form
{
    private readonly Label batteryLabel;
    private readonly Label statusLabel;
    private readonly Label connectionLabel;
    private readonly System.Windows.Forms.Timer refreshTimer;

    public MainWindow()
    {
        Text = "RAZER BLACKSHARK CONTROL";
        Size = new Size(520, 450);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(12, 14, 12);
        ForeColor = Color.White;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var title = new Label
        {
            Text = "RAZER",
            Font = new Font("Segoe UI", 25, FontStyle.Bold),
            ForeColor = Color.FromArgb(68, 255, 68),
            AutoSize = true,
            Location = new Point(35, 25)
        };

        var subtitle = new Label
        {
            Text = "BLACKSHARK V2 PRO XBOX",
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            ForeColor = Color.LightGray,
            AutoSize = true,
            Location = new Point(38, 80)
        };

        batteryLabel = new Label
        {
            Text = "--%",
            Font = new Font("Segoe UI", 58, FontStyle.Bold),
            ForeColor = Color.FromArgb(68, 255, 68),
            AutoSize = true,
            Location = new Point(175, 125)
        };

        statusLabel = new Label
        {
            Text = "PİL BİLGİSİ BEKLENİYOR",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.Gray,
            AutoSize = true,
            Location = new Point(145, 235)
        };

        connectionLabel = new Label
        {
            Text = "USB CİHAZI KONTROL EDİLİYOR...",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.Gray,
            AutoSize = true,
            Location = new Point(125, 270)
        };

        var diagnosticsButton = new Button
        {
            Text = "SİSTEM TANILAMASI",
            Location = new Point(145, 315),
            Size = new Size(220, 42),
            BackColor = Color.FromArgb(25, 45, 25),
            ForeColor = Color.FromArgb(68, 255, 68),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };

        diagnosticsButton.FlatAppearance.BorderColor =
            Color.FromArgb(68, 255, 68);

        diagnosticsButton.Click += (_, _) =>
        {
            string report = BatteryDiagnostics.GetReport();

            MessageBox.Show(
                this,
                report,
                "RAZER SYSTEM DIAGNOSTICS",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        };

        var deviceLabel = new Label
        {
            Text = "USB DEVICE: 1532:0A41",
            Font = new Font("Consolas", 10),
            ForeColor = Color.LightGray,
            AutoSize = true,
            Location = new Point(35, 380)
        };

        Controls.Add(title);
        Controls.Add(subtitle);
        Controls.Add(batteryLabel);
        Controls.Add(statusLabel);
        Controls.Add(connectionLabel);
        Controls.Add(diagnosticsButton);
        Controls.Add(deviceLabel);

        refreshTimer = new System.Windows.Forms.Timer
        {
            Interval = 3000
        };

        refreshTimer.Tick += (_, _) => RefreshDevice();
        refreshTimer.Start();

        RefreshDevice();
    }

    private void RefreshDevice()
    {
        bool connected = DeviceDetector.IsRazerDeviceConnected();

        connectionLabel.Text = connected
            ? "● KULAKLIK ALICISI BAĞLI"
            : "● KULAKLIK ALICISI BULUNAMADI";

        connectionLabel.ForeColor = connected
            ? Color.FromArgb(68, 255, 68)
            : Color.OrangeRed;
    }
}

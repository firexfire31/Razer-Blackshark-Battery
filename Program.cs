
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

    public MainWindow()
    {
        Text = "RAZER BLACKSHARK CONTROL";
        Size = new Size(520, 380);
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
            Text = "PIL BİLGİSİ BEKLENİYOR",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.Gray,
            AutoSize = true,
            Location = new Point(145, 235)
        };

        var deviceLabel = new Label
        {
            Text = "USB DEVICE: 1532:0A41",
            Font = new Font("Consolas", 10),
            ForeColor = Color.LightGray,
            AutoSize = true,
            Location = new Point(35, 295)
        };

        Controls.Add(title);
        Controls.Add(subtitle);
        Controls.Add(batteryLabel);
        Controls.Add(statusLabel);
        Controls.Add(deviceLabel);
    }
}

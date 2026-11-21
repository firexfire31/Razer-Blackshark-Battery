
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RazerBattery;

public static class DeviceDetector
{
    private const ushort VendorId = 0x1532;
    private const ushort ProductId = 0x0A41;

    private static readonly Guid HidGuid =
        new("4D1E55B2-F16F-11CF-88CB-001111000030");

    [StructLayout(LayoutKind.Sequential)]
    private struct SP_DEVICE_INTERFACE_DATA
    {
        public int cbSize;
        public Guid InterfaceClassGuid;
        public int Flags;
        public UIntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SP_DEVINFO_DATA
    {
        public int cbSize;
        public Guid ClassGuid;
        public uint DevInst;
        public UIntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        public int cbSize;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string DevicePath;
    }

    [DllImport("hid.dll")]
    private static extern void HidD_GetHidGuid(out Guid guid);

    [DllImport("setupapi.dll", SetLastError = true)]
    private static extern IntPtr SetupDiGetClassDevs(
        ref Guid ClassGuid,
        IntPtr Enumerator,
        IntPtr hwndParent,
        uint Flags);

    [DllImport("setupapi.dll", SetLastError = true)]
    private static extern bool SetupDiEnumDeviceInterfaces(
        IntPtr DeviceInfoSet,
        IntPtr DeviceInfoData,
        ref Guid InterfaceClassGuid,
        uint MemberIndex,
        ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool SetupDiGetDeviceInterfaceDetail(
        IntPtr DeviceInfoSet,
        ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
        IntPtr DeviceInterfaceDetailData,
        uint DeviceInterfaceDetailDataSize,
        out uint RequiredSize,
        IntPtr DeviceInfoData);

    [DllImport("setupapi.dll", SetLastError = true)]
    private static extern bool SetupDiDestroyDeviceInfoList(
        IntPtr DeviceInfoSet);

    public static List<string> GetRazerHidDevices()
    {
        var results = new List<string>();

        HidD_GetHidGuid(out Guid guid);

        IntPtr handle = SetupDiGetClassDevs(
            ref guid,
            IntPtr.Zero,
            IntPtr.Zero,
            0x12);

        if (handle == new IntPtr(-1))
        {
            results.Add("HID enumeration failed.");
            return results;
        }

        try
        {
            for (uint i = 0; ; i++)
            {
                var data = new SP_DEVICE_INTERFACE_DATA
                {
                    cbSize = Marshal.SizeOf<SP_DEVICE_INTERFACE_DATA>()
                };

                if (!SetupDiEnumDeviceInterfaces(
                    handle, IntPtr.Zero, ref guid, i, ref data))
                    break;

                SetupDiGetDeviceInterfaceDetail(
                    handle, ref data, IntPtr.Zero, 0,
                    out uint required, IntPtr.Zero);

                if (required == 0)
                    continue;

                IntPtr buffer = Marshal.AllocHGlobal((int)required);

                try
                {
                    Marshal.WriteInt32(
                        buffer,
                        IntPtr.Size == 8 ? 8 : 6);

                    if (!SetupDiGetDeviceInterfaceDetail(
                        handle, ref data, buffer, required,
                        out _, IntPtr.Zero))
                        continue;

                    string path = Marshal.PtrToStringAuto(
                        IntPtr.Add(buffer, 4)) ?? "";

                    if (path.Contains(
                        $"vid_{VendorId:x4}&pid_{ProductId:x4}",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(path);
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
        }
        finally
        {
            SetupDiDestroyDeviceInfoList(handle);
        }

        return results;
    }
}

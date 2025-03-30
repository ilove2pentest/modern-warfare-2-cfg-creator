using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace BrightnessSlider
{
    public partial class MainWindow : Window
    {
        // Windows API declarations for memory manipulation
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        // Constants
        private const uint PROCESS_ALL_ACCESS = 0x1F0FFF; // Full access rights
        private const string GAME_PROCESS_NAME = "iw4m"; // MW2 single-player process
        private const IntPtr BRIGHTNESS_ADDRESS = (IntPtr)0x06408450; // seta r_brightness
        private const IntPtr SECOND_VALUE_ADDRESS = (IntPtr)0x0085B884; // Sun X
        private const IntPtr THIRD_VALUE_ADDRESS = (IntPtr)0x0085B888; // Sun Y
        private const IntPtr FOURTH_VALUE_ADDRESS = (IntPtr)0x0085B88C; // Sun Z
        private const IntPtr FITH_VALUE_ADDRESS = (IntPtr)0x0085B878; // Sun Color Red
        private const IntPtr SIXTH_VALUE_ADDRESS = (IntPtr)0x0085B87C; // Sun Color Green
        private const IntPtr SEVENTH_VALUE_ADDRESS = (IntPtr)0x0085B880; // Sun Color Blue
        private const IntPtr EIGHTH_VALUE_ADDRESS = (IntPtr)0x064083B0; // seta r_FilmTweakContrast 
        private const IntPtr NINTH_VALUE_ADDRESS = (IntPtr)0x06408540; // seta r_filmTweakDesaturation 
        private const IntPtr TENTH_VALUE_ADDRESS = (IntPtr)0x06408630; // seta r_filmTweakDarkTint RED

        public MainWindow()
        {
            InitializeComponent();
        }

        // Handler for the brightness slider (existing)
        private void BrightnessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WriteFloatToMemory(BRIGHTNESS_ADDRESS, (float)e.NewValue);
        }

        // Handler for the second slider (existing)
        private void SecondValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WriteFloatToMemory(SECOND_VALUE_ADDRESS, (float)e.NewValue);
        }

        // Handler for the new third slider
        private void ThirdValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WriteFloatToMemory(THIRD_VALUE_ADDRESS, (float)e.NewValue);
        }

        // Handler for the new fourth slider
        private void FourthValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WriteFloatToMemory(FOURTH_VALUE_ADDRESS, (float)e.NewValue);
        }

        // Handler for the new fith slider
        private void FithValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WriteFloatToMemory(FITH_VALUE_ADDRESS, (float)e.NewValue);
        }

        // Handler for the new Sixth slider
        private void SixthValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WriteFloatToMemory(SIXTH_VALUE_ADDRESS, (float)e.NewValue);
        }

        // Handler for the new Seventh slider
        private void SeventhValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WriteFloatToMemory(SEVENTH_VALUE_ADDRESS, (float)e.NewValue);
        }

        // Handler for the new Eighth slider
        private void EighthValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WriteFloatToMemory(EIGHTH_VALUE_ADDRESS, (float)e.NewValue);
        }

        // Handler for the new Ninth slider
        private void NinthValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WriteFloatToMemory(NINTH_VALUE_ADDRESS, (float)e.NewValue);
        }

        // Handler for the new Tenth slider
        private void TenthValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WriteFloatToMemory(TENTH_VALUE_ADDRESS, (float)e.NewValue);
        }

        // Helper method to write a float value to memory
        private void WriteFloatToMemory(IntPtr address, float value)
        {
            // Find the game process
            Process[] processes = Process.GetProcessesByName(GAME_PROCESS_NAME);
            if (processes.Length == 0)
            {
                MessageBox.Show("Modern Warfare 2 is not running.");
                return;
            }

            Process gameProcess = processes[0];

            // Open the process with full access
            IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, gameProcess.Id);
            if (hProcess == IntPtr.Zero)
            {
                MessageBox.Show("Failed to open game process. Run as administrator.");
                return;
            }

            // Convert the float value to a byte array
            byte[] buffer = BitConverter.GetBytes(value);
            int bytesWritten;

            // Write the value to the specified memory address
            bool success = WriteProcessMemory(hProcess, address, buffer, (uint)buffer.Length, out bytesWritten);
            if (!success || bytesWritten != buffer.Length)
            {
                MessageBox.Show("Failed to write to memory.");
            }

            // Clean up by closing the process handle
            CloseHandle(hProcess);
        }
    }
}
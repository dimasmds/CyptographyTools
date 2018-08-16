using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.VisualBasic.Devices;
using System.Drawing;
using System.Windows.Forms;

namespace CryptograhpyTools
{
    class DeviceProperties
    {
        static ComputerInfo CI = new ComputerInfo();
        public static string getOSName()
        {
            //var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
            //            select x.GetPropertyValue("Caption")).FirstOrDefault();
            //return name != null ? name.ToString() : "Unknown";

            return CI.OSFullName;
        }
        
        public static string getProcName()
        {
            ManagementObjectSearcher mosProcessor = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            string Procname = null;
            foreach (ManagementObject moProcessor in mosProcessor.Get())
            {
                if (moProcessor["name"] != null)
                {
                    Procname = moProcessor["name"].ToString();

                }

            }
            return Procname;
        }

        public static string getTotalRAM()
        {
            return CI.TotalPhysicalMemory.ToString();
        }

        public static string getAvailableRAM()
        {
            return CI.AvailablePhysicalMemory.ToString();
        }

        public static string getGraphicCardName()
        {
            ManagementObjectSearcher searcher
            = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");

            string graphicsCard = string.Empty;
            foreach (ManagementObject mo in searcher.Get())
            {
                foreach (PropertyData property in mo.Properties)
                {
                    if (property.Name == "Description")
                    {
                        graphicsCard = property.Value.ToString();
                    }
                }
            }

            return graphicsCard;
        }

        public static string getCurrentResolution()
        {
            Rectangle resolution = Screen.PrimaryScreen.Bounds;

            return resolution.Width.ToString() + "x" + resolution.Height.ToString();
        }
    }
}

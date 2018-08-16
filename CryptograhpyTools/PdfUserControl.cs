using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptograhpyTools
{
    public partial class PdfUserControl : UserControl
    {
        public PdfUserControl(string FileName)
        {
            InitializeComponent();
            this.axAcroPDF1.LoadFile(FileName);
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using Keras;

namespace Microsoft.ML.TensorFlow_Test
{
    public partial class Form1 : Form
    {
        //DirectoryInfo mediaDIR = new DirectoryInfo(Program.mediaPath);
        private string mediaPath = System.Windows.Forms.Application.StartupPath.ToString + Program.mediaPath;
        private string prototxtPath = System.Windows.Forms.Application.StartupPath + Program.prototxtPath;
        private string caffemodelPath = System.Windows.Forms.Application.StartupPath + Program.caffemodelPath;
        private string maskdetectorPath = System.Windows.Forms.Application.StartupPath + Program.maskdetectorPath;


        public Form1()
        {
            //var net = CvDnn.ReadNetFromCaffe(Program.prototxt, Program.caffemodel);
            //var model = Keras.Models.Model.LoadModel(Program.Maskmodel);
            InitializeComponent();
            Console.WriteLine(Program.mediaPath);
            Console.WriteLine(Program.prototxtPath);
            Console.WriteLine(Program.caffemodelPath);
            Console.WriteLine(Program.maskdetectorPath);
            Console.WriteLine(System.Windows.Forms.Application.StartupPath);
        }

        private void pictureBoxIpl1_Click(object sender, EventArgs e)
        {

        }
    }
}

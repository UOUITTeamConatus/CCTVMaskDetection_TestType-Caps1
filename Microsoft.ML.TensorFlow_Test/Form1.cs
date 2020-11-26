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
using Numpy;
using Keras;
using Python;
using Python.Runtime;

namespace Microsoft.ML.TensorFlow_Test
{
    public partial class Form1 : Form
    {
        DirectoryInfo mediaDIR = new DirectoryInfo(Program.mediaPath);
        private DirectoryInfo di;

        private OpenCvSharp.Dnn.Net net = CvDnn.ReadNetFromCaffe(Program.prototxtPath, Program.caffemodelPath);
        private Keras.Models.BaseModel model = Keras.Models.Model.LoadModel(Program.maskdetectorPath); 

        VideoCapture video;
        Mat frame = new Mat();
        Bitmap BitmapImage;

        public Form1()
        {
            var net = CvDnn.ReadNetFromCaffe(Program.prototxtPath, Program.caffemodelPath);
            //CvDnn.ReadNetFromCaffe 메서드 테스트
            var model = Keras.Models.Model.LoadModel(Program.maskdetectorPath);
            //di = new DirectoryInfo(Program.mediaPath);
            //Keras.Models.Model.LoadModel 메서드 테스트
            InitializeComponent();
            Console.WriteLine(net.GetType());
            Console.WriteLine(model.GetType());
            //아래의 코드는 경로 테스트 코드
            /*
            Console.WriteLine(mediaPath);
            Console.WriteLine(prototxtPath);
            Console.WriteLine(caffemodelPath);
            Console.WriteLine(maskdetectorPath);
            Console.WriteLine(System.Windows.Forms.Application.StartupPath);*/
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                video = new VideoCapture(0);
                video.FrameWidth = 640;
                video.FrameHeight = 480;
            }
            catch
            {
                timer1.Enabled = false;
                Console.WriteLine("Error");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            video.Read(frame);
            BitmapImage = BitmapConverter.ToBitmap(frame);
            //여기다가 얼굴 인식 및 마스크 인식 처리를 해 줘야 함
        
            pictureBoxIpl1.Image = BitmapImage;
            using (Py.GIL())
            {
                PythonEngine.RunSimpleString(@"print('Hello')");
            }
           
             
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            frame.Dispose();
        }
    }
}

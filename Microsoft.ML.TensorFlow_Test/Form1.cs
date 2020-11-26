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
using Python;
using Python.Runtime;

namespace Microsoft.ML.TensorFlow_Test
{
    public partial class Form1 : Form
    {
        //DirectoryInfo mediaDIR = new DirectoryInfo(Program.mediaPath);
        private DirectoryInfo di;

        private OpenCvSharp.Dnn.Net net = CvDnn.ReadNetFromCaffe(Program.prototxtPath, Program.caffemodelPath);
        private Keras.Models.BaseModel model = Keras.Models.Model.LoadModel(Program.maskdetectorPath); 

        VideoCapture video;
        Mat frame = new Mat();
        Bitmap BitmapImage;

        public Form1()
        {
            //var net = CvDnn.ReadNetFromCaffe(Program.prototxtPath, Program.caffemodelPath);
            //CvDnn.ReadNetFromCaffe 메서드 테스트
            //var model = Keras.Models.Model.LoadModel(Program.maskdetectorPath);
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
            var blob = CvDnn.BlobFromImage(frame, 1, new OpenCvSharp.Size(300, 300), new Scalar(104, 177, 123));
            net.SetInput(blob, "data");

            //var out = 

            var dets = net.Forward();

            var w = frame.Width;
            var h = frame.Height;

            var matrix = dets.Reshape(1, dets.Size(2));

            for (int i = 0; i < dets.Size(2); i++) //  prob.Size(2)=200
            {
                var confidence = matrix.At<float>(i, 2);
                // var label = $"{confidence * 100:0.00}%";
                if (confidence < 0.5)
                    continue;
                // 바운딩 박스를 구함 (얼굴영역을 의미)
                var x1 = (int)(w * matrix.At<float>(i, 3)); // 특정 array element를 반환
                var y1 = (int)(h * matrix.At<float>(i, 4));
                var x2 = (int)(w * matrix.At<float>(i, 5));
                var y2 = (int)(h * matrix.At<float>(i, 6));
                /*
                try
                {
                    //얼굴 부분만 잘라내서 출력
                    Mat dst = img.SubMat(new Rect(x1, y1, x2 - y1, x2 - y1));
                    Cv2.ImShow("dst", dst);
                }
                catch (OpenCvSharp.OpenCVException ex)
                {
                    Console.WriteLine(" ");
                }*/

                //결과 출력
                Cv2.Rectangle(frame, new Rect(x1, y1, x2 - y1, x2 - y1), new Scalar(0, 255, 0), 2);
                var textSize = Cv2.GetTextSize("face", HersheyFonts.HersheyTriplex, 0.5, 1, out var baseline);
                Cv2.PutText(frame, "face", new OpenCvSharp.Point(x1, y1), HersheyFonts.HersheyTriplex, 0.5, Scalar.Black);
                //BitmapImage = Cv2.
            }

            pictureBoxIpl1.Image = BitmapImage;
           
             
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            frame.Dispose();
        }
    }
}

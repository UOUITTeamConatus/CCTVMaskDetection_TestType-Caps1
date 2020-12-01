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
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using Keras;
using Keras.PreProcessing.Image;
using Python;
using Python.Runtime;
using Numpy;

namespace Microsoft.ML.TensorFlow_Test
{
    public partial class Form1 : Form
    {
        DirectoryInfo mediaDIR = new DirectoryInfo(Program.mediaPath);
        private Net facenet = CvDnn.ReadNetFromCaffe(Program.prototxtPath, Program.caffemodelPath);
        private Keras.Models.BaseModel model = Keras.Models.Model.LoadModel(Program.maskdetectorPath);
        private Keras.Applications.MobileNet.MobileNetV2 mobilenetv2 = new Keras.Applications.MobileNet.MobileNetV2();

        VideoCapture video;
        Mat frame = new Mat();
        Bitmap BitmapImage;

        public Form1()
        {
            //var facenet = CvDnn.ReadNetFromCaffe(Program.prototxtPath, Program.caffemodelPath);
            //CvDnn.ReadNetFromCaffe 메서드 테스트
            //var model = Keras.Models.Model.LoadModel(Program.maskdetectorPath);
            //di = new DirectoryInfo(Program.mediaPath);
            //Keras.Models.Model.LoadModel 메서드 테스트
            InitializeComponent();
            //Console.WriteLine(net.GetType());
            //Console.WriteLine(model.GetType());
            //아래의 코드는 경로 테스트 코드

            Console.WriteLine(Program.mediaPath);
            Console.WriteLine(Program.prototxtPath);
            Console.WriteLine(Program.caffemodelPath);
            Console.WriteLine(Program.maskdetectorPath);
            Console.WriteLine(System.Windows.Forms.Application.StartupPath);
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

            dynamic cam = video.Read(frame);
            Mat result = DetectMask(frame);
            BitmapImage = BitmapConverter.ToBitmap(result);
            pictureBoxIpl1.Image = BitmapImage;
            //retrived = test[0];

            //Console.WriteLine(retrived);
            //Console.WriteLine(image);
            //Mat result;
            //result = Cv2.ImRead(BitmapImage);
            /*
            using (Py.GIL())
            {
                //dynamic import = Py.Import("MaskDetect");
               // dynamic maskDetection = import.MaskDetection(image, facenet, model);
                //maskDetection.execute();
            }*/
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            frame.Dispose();
        }

        private Mat DetectmaskPy(Mat frame)
        {
            Mat result = frame;
            return result;
        }
        private Mat DetectMask(Mat frame)
        {
            Mat result = frame;
            Mat blob = CvDnn.BlobFromImage(result, 1, new OpenCvSharp.Size(300, 300), new OpenCvSharp.Scalar(104, 177, 123));
            facenet.SetInput(blob, "data");
            Mat dets = facenet.Forward();
            int w = result.Width;
            int h = result.Height;
            Mat p = dets.Reshape(1, dets.Size(2));

            for (int i = 0; i < dets.Size(2); i++) //  prob.Size(2)=얼굴이라고 판단된 BOX의 개수 
            {
                float confidence = p.At<float>(i, 2);

                if (confidence < 0.5)
                    continue;
                // 바운딩 박스를 구함 (얼굴영역을 의미)
                int x1 = (int)(w * p.At<float>(i, 3)); // 특정 array element를 반환
                int y1 = (int)(h * p.At<float>(i, 4));
                int x2 = (int)(w * p.At<float>(i, 5));
                int y2 = (int)(h * p.At<float>(i, 6));

                try
                {
                    //원본 이미지에서 얼굴 부분만 추출
                    Mat face = result.SubMat(new Rect(x1, y1, x2 - y1, x2 - y1));

                    //이미지 전처리
                    Mat face_input = new Mat(face.Size(), MatType.CV_8UC3);
                    Cv2.Resize(face, face_input, new OpenCvSharp.Size(224, 224));   //Cv2.Resize*(원본 이미지, 결과 이미지, 절대 크기, 상대 크기(X), 상대 크기(Y), 보간법)
                    Cv2.CvtColor(face_input, face_input, ColorConversionCodes.BGR2RGB); //컬러변환       
                    Cv2.ImWrite("face0.bmp", face_input);
                    string img_path = "face0.bmp";
                    dynamic img1 = ImageUtil.LoadImg(img_path);
                    dynamic x = ImageUtil.ImageToArray(img1);

                    // Numsharp<->Numpy 변환불가
                    //var mat2 = new SharpCV.Mat(face_input.CvPtr);  //OpenCVsharp의 Mat Matrix를 SharpCV Mat으로 가져옴.
                    //var x = mat2.GetData();   //Mat으로부터 NDarray 가져옴

                    x = mobilenetv2.PreprocessInput(x);
                    x = np.expand_dims(x, axis: 0);

                    //마스크 판별
                    NDarray preds = model.Predict(x);

                    //차원 중 사이즈가 1인 것을 찾아 제거 
                    preds = np.squeeze(preds);
                    //예측값 가져오기
                    //var a = preds.ToString();
                    //Scalar b = preds.asscalar<Scalar>();
                    //var b = preds.;
                    var maxIndex = preds.argmax();
                    var max = maxIndex.ToString();
                    //byte[] b = preds.tobytes();
                    //var result = b.ToArray<byte>();
                    Scalar color;
                    string label;
                    //Console.WriteLine(preds);
                    Console.WriteLine(preds);
                    Console.WriteLine(max);
                    //Console.WriteLine(d);
                    /*
                     * 1/1 [==============================] - 0s 0s/step
                       [0.02961567 0.97038436]
                       이게 위의 출력 양식
                       string 처리해서 출력해본 결과 preds는 이렇게 출력된다. 
                       [0.02961567 0.97038436]
                     */
                    //Console.WriteLine("Mask : "+mask + " NoMask : " + nomask);
                    if (max.Equals("0"))
                    {
                        Console.WriteLine("Mask");
                        color = new Scalar(0, 255, 0);
                        label = "Mask";
                    }
                    else
                    {
                        Console.WriteLine("No Mask");
                        color = new Scalar(0, 0, 255);
                        label = "No Mask";
                    }
                    Cv2.Rectangle(result, new Rect(x1, y1, x2 - y1, x2 - y1), color, 2);
                    OpenCvSharp.Size textSize = Cv2.GetTextSize("face", HersheyFonts.HersheyTriplex, 0.5, 1, out var baseline); //추후 string 부분 수정 예정.
                    Cv2.PutText(result, label, new OpenCvSharp.Point(x1, y1), HersheyFonts.HersheyTriplex, 0.5, color);
                    return result;
                }
                catch (OpenCVException ex)
                {
                    Console.WriteLine(" ");
                    return result;
                }
            }
            return result;
        }
    }
}

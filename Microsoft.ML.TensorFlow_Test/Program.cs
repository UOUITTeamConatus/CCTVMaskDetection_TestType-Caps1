using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Python;
using Python.Runtime;

namespace Microsoft.ML.TensorFlow_Test
{
    static class Program
    {
        //public const string startPath = ;
        public static string currentPath = System.Environment.CurrentDirectory;
        public const string mediaPath = "models\\media";
        public const string prototxtPath = "models\\deploy.prototxt";
        public const string caffemodelPath = "models\\res10_300x300_ssd_iter_140000.caffemodel";
        public const string maskdetectorPath = "models\\mask_detector.model";
        /*
        public static string mediaPath = Path.GetFullPath(path1);
        public static string prototxt = Path.GetFullPath(path2);
        public static string caffemodel = Path.GetFullPath(path3);
        public static string Maskmodel = Path.GetFullPath(path4);*/
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        //What the??
        static void Main()
        {
            var PYTHON_HOME = Environment.ExpandEnvironmentVariables(@"C:\Users\swj12\anaconda3\");
            // 환경 변수 설정
            AddEnvPath(PYTHON_HOME, Path.Combine(PYTHON_HOME, @"Library\bin"));
            // Python 홈 설정
            PythonEngine.PythonHome = PYTHON_HOME;

            // 모듈 패키지 패스 설정
            PythonEngine.PythonPath = string.Join(Path.PathSeparator.ToString(), new string[] { PythonEngine.PythonPath, Path.Combine(PYTHON_HOME, @"Lib\site-packages") });
            // Python 엔진 초기화
            PythonEngine.Initialize();
            //currentPath = GetCurrentDirectory();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Console.WriteLine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName);
        }
        public static void AddEnvPath(params string[] paths)
        {
            // PC에 설정되어 있는 환경 변수를 가져온다.
            var envPaths = Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator).ToList();
            // 중복 환경 변수가 없으면 list에 넣는다.
            envPaths.InsertRange(0, paths.Where(x => x.Length > 0 && !envPaths.Contains(x)).ToArray());
            // 환경 변수를 다시 설정한다.
            Environment.SetEnvironmentVariable("PATH", string.Join(Path.PathSeparator.ToString(), envPaths), EnvironmentVariableTarget.Process);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Math;
using Accord.Statistics;
using Accord;
using Accord.MachineLearning;
using Accord.Statistics.Analysis;
using Accord.Statistics.Analysis.ContrastFunctions;
using Accord.Statistics.Models.Regression.Linear;
using Accord.Video.VFW;
using Accord.Video;


//Aforge Referece
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using System.Drawing.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;

namespace HeartLens
{
    /// <summary>
    /// http://accord-framework.net/
    /// </summary>
    public partial class Form1 : Form
    {

        public static Form1 MyApp;


        double FrameRate = 0;
        int timeWindows = 15;

        Bitmap CurrentImage;

        #region Capture Device

        //Create a Instance for Capture Device

        //Create a Instance for VideoCaptureDevice
     //   VideoCaptureDevice videoCaptureDevice;

        //Create a Instance for Haar Object
        HaarObjectDetector haarObjectDetector;

        FilterInfoCollection filterInfoCollection;

        Bitmap grayImage;

        //private void LoadDevice()
        //{
        //    filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        //    foreach (FilterInfo filterInfo in filterInfoCollection)
        //    {
        //        DeviceComboBox.Items.Add(filterInfo.Name);
        //    }
        //    DeviceComboBox.SelectedIndex = 0;
        //}

        #endregion

        internal Emgu.CV.UI.ImageBox pictureBoxObservedImage;

        OpenCVVideo MyOpenCVVideo;
        FaceDetection MyFaceDetection;


        public Form1()
        {
            //Singleton
            Form1.MyApp = this;

            InitializeComponent();
        }




        private void buttonOk_Click(object sender, EventArgs e)
        {
            //Start the video
            if (DeviceComboBox.SelectedIndex != -1)
            {
                MyOpenCVVideo.SetupCapture(DeviceComboBox.SelectedIndex);
            }
        }


        public Bitmap getImage
        {
            get;
            set;
        }
        


        Rectangle LastRectangle = new Rectangle(0,0,20,20);

        internal void ProcessImage(Mat frame)
        {         
           var faces = MyFaceDetection.RunFaceDetection(frame,2);

            if (faces.Count > 0)
                LastRectangle = faces[0];

            //Use the last rectangle for ever if not updated
            Mat image = new Mat(frame, LastRectangle);

            pictureBoxObservedImage.Image = image;
    

            double hr = 0;
            hr = HeartComputation(image.Bitmap);

            //Safe threading process
            this.BeginInvoke((Action)delegate ()
            {
                //code to update UI
                textBoxHeartRate.Text = hr.ToString();
            });

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            InitData();
           
            FrameRate = 15;// (double)timeWindows / (double)(timerFrameUpdate.Interval) * 1000.0;
  
            InitOpenCV();

            MyFaceDetection = new FaceDetection();

            //Debug, automatic device selection and start
            DeviceComboBox.SelectedIndex = 0;
            buttonOk_Click(this, null);

        }

     
        private void InitOpenCV()
        {
            MyOpenCVVideo = new OpenCVVideo();

            var devices = MyOpenCVVideo.InitCameraList();

            DeviceComboBox.Items.Clear();

            foreach (var device in devices)
            {
                DeviceComboBox.Items.Add(device.ToString());
            }

            pictureBoxObservedImage = new Emgu.CV.UI.ImageBox();
            pictureBoxObservedImage.Size = new Size(800, 600);
            pictureBoxObservedImage.Location = new System.Drawing.Point(10, 50);
            pictureBoxObservedImage.BackColor = Color.Gray;
            pictureBoxObservedImage.Dock = DockStyle.Fill;

            this.groupBoxVideo.Controls.Add(pictureBoxObservedImage);
        }

        private void InitData()
        {
            red = Matrix.Vector(timeWindows, 1, 1.0f);

            green = Matrix.Vector(timeWindows, 1, 1.0f);

            blue = Matrix.Vector(timeWindows, 1, 1.0f);
        }
        

        double[] red;
        double[] green;
        double[] blue;

        int FramePointer = 0;

        bool isDataReady = false;


        double GetMean(double[] val)
        {
            double mean = 0;

            foreach (var item in val)
            {
                mean += item;
            }
            mean /= val.Length;

            return mean;
        }

        double[] GetMag(double[] real, double[] img)
        {
            int l = real.Length;
            double[] mag = new double[l];

            for (int i = 0; i < l; i++)
            {
                mag[i] = Math.Sqrt((real[i] * real[i] + img[i] * img[i]));
            }

            return mag;
        }


        private double HeartComputation(Bitmap image)
        {

            //ToDO turn  to 0


            ComputeAndStorAverageRGB(image);

            if (isDataReady)
            {
                //More than timeWindows records

                double[] redN = new double[red.Length];
                double[] greenN = new double[green.Length];
                double[] blueN = new double[blue.Length];

                //substract the average the the color channel

                double meanR = GetMean(red);
                double meanG = GetMean(green);
                double meanB = GetMean(blue);

                int length = red.Length;
                for (int i = 0; i < length; i++)
                {
                    redN[i] = red[i] - meanR;
                    greenN[i] = green[i] - meanG;
                    blueN[i] = blue[i] - meanB;
                }

                var ica = new IndependentComponentAnalysis()
                {
                    Algorithm = IndependentComponentAlgorithm.Parallel,
                    Contrast = new Logcosh()
                };


                //build color matrix
                double[][] colors =
                {
                    redN,
                    greenN,
                    blueN
                };

                MultivariateLinearRegression demix = ica.Learn(colors);
                double[][] result = demix.Transform(colors);

                //FFT of eah rows
                double[] imagR = new double[length];
                double[] imagG = new double[length];
                double[] imagB = new double[length];

                Accord.Math.Transforms.FourierTransform2.FFT(result[0], imagR, FourierTransform.Direction.Forward);
                Accord.Math.Transforms.FourierTransform2.FFT(result[1], imagG, FourierTransform.Direction.Forward);
                Accord.Math.Transforms.FourierTransform2.FFT(result[2], imagB, FourierTransform.Direction.Forward);


                double[] magR = GetMag(result[0], imagR);
                double[] magG = GetMag(result[1], imagG);
                double[] magB = GetMag(result[2], imagB);


                //getting freauency range

                double[] freq = Vector.Interval((double)0, (double)(timeWindows - 1));
                for (int i = 0; i < freq.Length; i++)
                {
                    freq[i] = freq[i] / (double)timeWindows * FrameRate;
                }

                int MaxRedIndex;
                double MaxRedValue;
                GetMax(out MaxRedIndex, out MaxRedValue, freq, magR, 0.45, 2);

                int MaxGreenIndex;
                double MaxGreenValue;
                GetMax(out MaxGreenIndex, out MaxGreenValue, freq, magG, 0.45, 2);

                int MaxBlueIndex;
                double MaxBlueValue;
                GetMax(out MaxBlueIndex, out MaxBlueValue, freq, magB, 0.45, 2);


                double HeartBeatFrequency = -1;
                //Get the max power
                if ((MaxRedValue > MaxGreenValue) && (MaxRedValue > MaxBlueValue))
                {
                    //Red is the Max
                    HeartBeatFrequency = freq[MaxRedIndex];
                }

                if ((MaxGreenValue > MaxRedValue) && (MaxGreenValue > MaxBlueValue))
                {
                    //Green is the Max
                    HeartBeatFrequency = freq[MaxGreenIndex];
                }

                if ((MaxBlueValue > MaxRedValue) && (MaxBlueValue > MaxGreenValue))
                {
                    //Blue is the Max
                    HeartBeatFrequency = freq[MaxBlueIndex];
                }

                return HeartBeatFrequency;
            }
            return -1;
        }

        private void GetMax(out int maxFreqIndex, out double maxColorValue, double[] freq, double[] magColor, double minFreq, int maxFreq)
        {
            int l = freq.Length;
            double max = double.MinValue;
            int maxIndex = -1;

            for (int i = 0; i < l; i++)
            {
                if ((freq[i] > minFreq) && (freq[i] < maxFreq))
                {
                    if (magColor[i] > max)
                    {
                        max = magColor[i];
                        maxIndex = i;
                    }
                }
            }

            maxFreqIndex = maxIndex;
            maxColorValue = max;
        }
        
        


        /// <summary>
        /// Unsafe mode
        /// </summary>
        /// <param name="image"></param>
        private void ComputeAndStorAverageRGB(Bitmap bmp)
        {
            int nbrPixels = bmp.Width * bmp.Height;
            int w = bmp.Width;
            int h = bmp.Height;

            int r = 0;
            int g = 0;
            int b = 0;

            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                  System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                  bmp.PixelFormat);

            int PixelSize = 4;

            unsafe
            {
                for (int y = 0; y < bmd.Height; y++)
                {
                    byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);

                    for (int x = 0; x < bmd.Width; x++)
                    {
                        //row[x * PixelSize] = 0;   //Blue  0-255
                        //row[x * PixelSize + 1] = 255; //Green 0-255
                        //row[x * PixelSize + 2] = 0;   //Red   0-255
                        //row[x * PixelSize + 3] = 50;  //Alpha 0-255

                        r += row[x * PixelSize + 2];
                        g += row[x * PixelSize + 1];
                        b += row[x * PixelSize];
                    }
                }
            }

            bmp.UnlockBits(bmd);


            double arvR = (double)r / (double)nbrPixels;
            double arvG = (double)g / (double)nbrPixels;
            double arvB = (double)b / (double)nbrPixels;

            red[FramePointer] = arvR;
            green[FramePointer] = arvG;
            blue[FramePointer] = arvB;


            FramePointer++;
            if (FramePointer >= timeWindows)
            {
                isDataReady = true;
                FramePointer = 0;

            }
        }

        

        private void ComputeAndStorAverageRGB_Safe(Bitmap image)
        {
            int nbrPixels = image.Width * image.Height;
            int w = image.Width;
            int h = image.Height;

            int r = 0;
            int g = 0;
            int b = 0;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color c = image.GetPixel(x, y);

                    r += c.R;
                    g += c.G;
                    b += c.B;
                }
            }


            double arvR = (double)r / (double)nbrPixels;
            double arvG = (double)g / (double)nbrPixels;
            double arvB = (double)b / (double)nbrPixels;

            red[FramePointer] = arvR;
            green[FramePointer] = arvG;
            blue[FramePointer] = arvB;


            FramePointer++;
            if (FramePointer >= timeWindows)
            {
                isDataReady = true;
                FramePointer = 0;

            }
        }



        DateTime LastFpsComputation = DateTime.Now;

        private void timerFrameUpdate_Tick(object sender, EventArgs e)
        {
            //Update the frame Rate
            TimeSpan timeBetzeenTwoFpsComputation = DateTime.Now - LastFpsComputation;

            LastFpsComputation = DateTime.Now;

            FrameRate = MyOpenCVVideo.FPS / timeBetzeenTwoFpsComputation.TotalSeconds; 


            this.Text = "Nbr Images per seconds : " + FrameRate;

            MyOpenCVVideo.FPS = 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
             MyOpenCVVideo.StopCamera();
            MyFaceDetection.Dispose();
        }

        private void groupBoxVideo_Enter(object sender, EventArgs e)
        {

        }
    }
}

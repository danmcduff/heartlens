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
        int timeWindows = 300;

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
            //Rectangle thinRectangle;
            int size = LastRectangle.Width/2;
            int halfSize = size / 2;
            Rectangle thinRectangle = new Rectangle(
                LastRectangle.X + halfSize,
                LastRectangle.Y,
                LastRectangle.Width - size,
                LastRectangle.Height);

            Mat image = new Mat(frame, thinRectangle);

            pictureBoxObservedImage.Image = image;
    

            double hr = 0;
            hr = HeartComputation(image.Bitmap);

            //Safe threading process
            this.BeginInvoke((Action)delegate ()
            {
                //code to update UI
                textBoxHeartRate.Text = hr.ToString();

                this.chart1.Series[0].Points.Clear();
                this.chart1.Series[1].Points.Clear();
                this.chart1.Series[2].Points.Clear();

                for (int i = 0; i < red_norm.Length; i++)
                {
                    this.chart1.Series[0].Points.Add(red_norm[i]);
                    this.chart1.Series[1].Points.Add(green_norm[i]);
                    this.chart1.Series[2].Points.Add(blue_norm[i]);
                }

                this.chart3.Series[0].Points.Clear();
                this.chart3.Series[1].Points.Clear();
                this.chart3.Series[2].Points.Clear();

                for (int i = 0; i < ica_source_1.Length; i++)
                {
                    this.chart3.Series[0].Points.Add(ica_source_1[i]);
                    this.chart3.Series[1].Points.Add(ica_source_2[i]);
                    this.chart3.Series[2].Points.Add(ica_source_3[i]);
                }

                this.chart2.Series[0].Points.Clear();
                this.chart2.Series[1].Points.Clear();
                this.chart2.Series[2].Points.Clear();

                for (int i = 0; i < red_norm_FFT.Length; i++)
                {
                    this.chart2.Series[0].Points.Add(red_norm_FFT[i]);
                    this.chart2.Series[1].Points.Add(green_norm_FFT[i]);
                    this.chart2.Series[2].Points.Add(blue_norm_FFT[i]);
                }
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
            pictureBoxObservedImage.Size = new Size(1280, 900);
            pictureBoxObservedImage.Location = new System.Drawing.Point(10, 50);
            pictureBoxObservedImage.BackColor = Color.Gray;
            pictureBoxObservedImage.Dock = DockStyle.Fill;

            this.groupBoxVideo.Controls.Add(pictureBoxObservedImage);

            this.chart1.Titles.Add("Pets");


        }

        private void InitData()
        {
            red = Matrix.Vector(timeWindows, 1, 1.0f);

            green = Matrix.Vector(timeWindows, 1, 1.0f);

            blue = Matrix.Vector(timeWindows, 1, 1.0f);

            red_norm = Matrix.Vector(timeWindows, 1, 1.0f);
            green_norm = Matrix.Vector(timeWindows, 1, 1.0f);
            blue_norm = Matrix.Vector(timeWindows, 1, 1.0f);

            ica_source_1 = Matrix.Vector(timeWindows, 1, 1.0f);
            ica_source_2 = Matrix.Vector(timeWindows, 1, 1.0f);
            ica_source_3 = Matrix.Vector(timeWindows, 1, 1.0f);

            red_norm_FFT = Matrix.Vector(timeWindows/2, 1, 1.0f);
            green_norm_FFT = Matrix.Vector(timeWindows/2, 1, 1.0f);
            blue_norm_FFT = Matrix.Vector(timeWindows/2, 1, 1.0f);

            HeartBeatFrequency = -1;
        }
        

        double[] red;
        double[] green;
        double[] blue;

        double[] red_norm;
        double[] green_norm;
        double[] blue_norm;

        double[] ica_source_1;
        double[] ica_source_2;
        double[] ica_source_3;

        double[] red_norm_FFT;
        double[] green_norm_FFT;
        double[] blue_norm_FFT;

        int FramePointer = 0;

        bool isDataReady = false;
        double HeartBeatFrequency;



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



        // THE MAIN CARDIO COMPUTATION:
        private double HeartComputation(Bitmap image)
        {

           // ComputeAndStorAverageRGB(image);
            ComputeAndStorAverageRGB(image);

            if (isDataReady)
            {

                ////////////////////////////////
                // NORMALIZE THE RGB SIGNALS BY SUBTRACTING THE MEAN:
                //build color matrix
                double[][] colors =
                {
                    red,
                    green,
                    blue
                };

                double[][] norm_colorsT;
                double[][] norm_colors;
                double[][] colorsT = colors.Transpose();
                norm_colorsT = Accord.Statistics.Tools.ZScores(colorsT);
                norm_colors = norm_colorsT.Transpose();

                ///////////////////////////////////
                // ADD A HIGH PASS FILTERING STEP:
                /*
                var testFilter = new Accord.Audio.Filters.HighPassFilter(0.1f);
                float RC = 1 / (2 * (float)(3.142) * (float)(0.7));
                float dt = 1 / (float)(30);
                float ALPHA = dt / (dt + RC);
                testFilter.Alpha = ALPHA;

                Accord.Audio.Signal target = Accord.Audio.Signal.FromArray(norm_colorsT[0], sampleRate: 30);
                Accord.Audio.Signal target_out = testFilter.Apply(target);
                target_out.CopyTo(norm_colorsT[0]);

                target = Accord.Audio.Signal.FromArray(norm_colorsT[1], sampleRate: 30);
                target_out = testFilter.Apply(target);
                target_out.CopyTo(norm_colorsT[1]);

                target = Accord.Audio.Signal.FromArray(norm_colorsT[2], sampleRate: 30);
                target_out = testFilter.Apply(target);
                target_out.CopyTo(norm_colorsT[2]);
                */
                //byte[] test = target_out.RawData;

                ///////////////////////////////////

                ///////////////////////////////////
                // PERFORM ICA:
                var ica = new IndependentComponentAnalysis()
                {
                    Algorithm = IndependentComponentAlgorithm.Parallel,
                    Contrast = new Logcosh(),
                    Iterations = 1000,
                    NumberOfInputs = 3,
                    NumberOfOutputs = 3
                };
                ica.Iterations = 1000000;

                MultivariateLinearRegression demix = ica.Learn(norm_colorsT);
                double[][] resultT = demix.Transform(norm_colorsT);
                double[][] result = resultT.Transpose();

                ///////////////////////////////////

                ///////////////////////////////////
                // ADD A BANDPASS FILTERING STEP:


                ///////////////////////////////////


                ///////////////////////////////////
                // CALCULATE THE FFT OF ECH CHANNEL:
                int length = red.Length;
                double[] imagR = new double[length];
                double[] imagG = new double[length];
                double[] imagB = new double[length];

                red_norm = norm_colors[0];
                green_norm = norm_colors[1];
                blue_norm = norm_colors[2];

                ica_source_1 = result[0];
                ica_source_2 = result[1];
                ica_source_3 = result[2];

                Accord.Math.Transforms.FourierTransform2.FFT(result[0], imagR, FourierTransform.Direction.Forward);
                Accord.Math.Transforms.FourierTransform2.FFT(result[1], imagG, FourierTransform.Direction.Forward);
                Accord.Math.Transforms.FourierTransform2.FFT(result[2], imagB, FourierTransform.Direction.Forward);


                double[] magR = GetMag(result[0], imagR);
                double[] magG = GetMag(result[1], imagG);
                double[] magB = GetMag(result[2], imagB);

                for (int i = 0; i < timeWindows / 2; i++)
                {
                    red_norm_FFT[i] = magR[i];
                    green_norm_FFT[i] = magG[i];
                    blue_norm_FFT[i] = magB[i];
                }
                ///////////////////////////////////


                ///////////////////////////////////
                // FIND THE MAX FREQUENCY AND POWER FROM THE FFTs:
                double[] freq = Vector.Interval((double)0, (double)(timeWindows - 1));
                for (int i = 0; i < freq.Length; i++)
                {
                    freq[i] = freq[i] / (double)timeWindows * FrameRate;
                }

                int MaxRedIndex;
                double MaxRedValue;
                GetMax(out MaxRedIndex, out MaxRedValue, freq, magR, 0.45, 3);

                int MaxGreenIndex;
                double MaxGreenValue;
                GetMax(out MaxGreenIndex, out MaxGreenValue, freq, magG, 0.45, 3);

                int MaxBlueIndex;
                double MaxBlueValue;
                GetMax(out MaxBlueIndex, out MaxBlueValue, freq, magB, 0.45, 3);


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
                ///////////////////////////////////
                HeartBeatFrequency = HeartBeatFrequency * 60;
                isDataReady = false;
                return HeartBeatFrequency;
            }
            return HeartBeatFrequency;
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
        private void ComputeAndStorAverageRGB(Bitmap processedBitmap)
        {
            int r = 0;
            int g = 0;
            int b = 0;

            int nbrPixels = processedBitmap.Width * processedBitmap.Height;

            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;

                byte* ptrFirstPixel = (byte*)bitmapData.Scan0;

                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* currentLine = ptrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        r += currentLine[x + 2];  //red
                        g += currentLine[x + 1];  //green
                        b += currentLine[x];   // blue
                    }
                }
                processedBitmap.UnlockBits(bitmapData);
            }

            double arvR = (double)r / (double)nbrPixels;
            double arvG = (double)g / (double)nbrPixels;
            double arvB = (double)b / (double)nbrPixels;

            red[FramePointer] = arvR;
            green[FramePointer] = arvG;
            blue[FramePointer] = arvB;


            //manage the current pointer position
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.CvEnum;

using DirectShowLib;
using Emgu.CV.Util;


namespace HeartLens
{
    internal class OpenCVVideo
    {
        internal int FPS = 0;


        int CameraDevice = 0; //Variable to track camera device selected
        Capture CaptureEnvironementCamera = null;

        internal struct Video_Device
        {
            public string Device_Name;
            public int Device_ID;
            public Guid Identifier;

            public Video_Device(int ID, string Name, Guid Identity = new Guid())
            {
                Device_ID = ID;
                Device_Name = Name;
                Identifier = Identity;
            }

            /// <summary>
            /// Represent the Device as a String
            /// </summary>
            /// <returns>The string representation of this color</returns>
            public override string ToString()
            {
                return String.Format("[{0}] {1}: {2}", Device_ID, Device_Name, Identifier);
            }
        }


        internal void StopCamera()
        {
            CaptureEnvironementCamera.Stop();
        }

        internal Video_Device[] InitCameraList()
        {
            Video_Device[] WebCams; //List containing all the camera available
      
            DsDevice[] _SystemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            WebCams = new Video_Device[_SystemCamereas.Length];
            for (int i = 0; i < _SystemCamereas.Length; i++)
            {
                WebCams[i] = new Video_Device(i, _SystemCamereas[i].Name, _SystemCamereas[i].ClassID); //fill web cam array
                                                                                                       //         comboBoxCamera.Items.Add(WebCams[i].ToString());
            }          

            return WebCams;
        }


        /// <summary>
        /// Sets up the _capture variable with the selected camera index
        /// </summary>
        /// <param name="Camera_Identifier"></param>
        internal void SetupCapture(int Camera_Identifier)
        {
            //update the selected device
            CameraDevice = Camera_Identifier;

            //Dispose of Capture if it was created before
            if (CaptureEnvironementCamera != null) CaptureEnvironementCamera.Dispose();
            try
            {
                //Set up capture device
                CaptureEnvironementCamera = new Capture(CameraDevice);

                //Set the Autoexposure to true: need to be tested
                CaptureEnvironementCamera.SetCaptureProperty(CapProp.AutoExposure, 1);
                // CaptureEnvironementCamera.SetCaptureProperty(CAP_PROP.

                CaptureEnvironementCamera.ImageGrabbed += Capture_ImageGrabbed;

                CaptureEnvironementCamera.Start();
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

      
        /// <summary>
        /// Capture an image from the camera and process it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            //Get the image and turn it into B&W
            Mat frame = new Mat();
            CaptureEnvironementCamera.Retrieve(frame, 0);

            // Mat grayFrame = new Mat();
            //  CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);
       
            Form1.MyApp.ProcessImage(frame);

            FPS++; 
         }


    }
}

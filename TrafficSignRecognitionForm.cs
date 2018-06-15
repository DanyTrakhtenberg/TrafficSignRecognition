//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System.Threading;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Diagnostics;

namespace TrafficSignRecognition
{



    public partial class TrafficSignRecognitionForm : Form
    {
        private StopSignDetector _stopSignDetector;
        Image<Bgr, byte> img;
        CameraCapture cam;
        Image<Bgr, byte> img0;
        Rectangle[] last_rectangle;
        Thread t1=null;
        double tick;
        double[] checks_after_detection;
        LineSegment2D line1, line2;
        Bgr c;

        System.Windows.Forms.Timer timer;
        
        public TrafficSignRecognitionForm()
        
        {
            InitializeComponent();
            
            _stopSignDetector = new StopSignDetector();
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(timer_Tick);
            checks_after_detection=new double[10];
            last_rectangle=new Rectangle[10];
           String txt;
            txt = "stop-sign.jpg";
            img0 =
               new Image<Bgr, byte>(txt);
            
            /*
            Image<Bgr, Byte> noise;
            noise = img0.CopyBlank();

            MCvScalar a, b;
            a = new MCvScalar(90.0,90.0,50.0);
            b = new MCvScalar(10.0,10.0,10.0);
            noise.SetRandNormal(a, b);
            img0 = img0 - noise;
             * */
            ProcessImage();
                

            cam = new CameraCapture();
      
        }


        void timer_Tick(object sender, EventArgs e)
        {
            timer.Interval = 20;
            //imageBox1.Image = img0;
            //imageBox1.Refresh();
            ProcessImage();
        }

        private void ProcessImage()
        {

            if (tick % 5 == 0)
            {
               // Stopwatch watch = Stopwatch.StartNew(); // time the detection process
                Image<Bgr, byte> image;
                image = img0;
       
                List<Image<Gray, Byte>> stopSignList = new List<Image<Gray, byte>>();
                List<Rectangle> stopSignBoxList = new List<Rectangle>();
                List<String> id = new List<String>();
                _stopSignDetector.DetectStopSign(image, stopSignList, stopSignBoxList, id);

           //     watch.Stop(); //stop the timer
                //processTimeLabel.Text = String.Format("Stop Sign Detection time: {0} milli-seconds", watch.Elapsed.TotalMilliseconds);

                panel1.Controls.Clear();
                Point startPoint = new Point(10, 10);

                for (int i = 0; i < stopSignList.Count; i++)
                {
                    Rectangle rect = stopSignBoxList[i];
                    AddLabelAndImage(
                       ref startPoint,
                       String.Format("{0} Sign [{1},{2}]:", id[i], rect.Location.Y + rect.Width / 2, rect.Location.Y + rect.Height / 2),
                       stopSignList[i]);

                        last_rectangle[i] = rect;
                     checks_after_detection[i]=7;        
                }
           }
            for (int i=0; i<last_rectangle.Length; i++)
                if (checks_after_detection[i]-->0)
                    img0=add_x(img0, last_rectangle[i]);  
                
            imageBox1.Image = img0;
        //    imageBox1.Refresh();
        }

        private Image<Bgr,byte> add_x(Image<Bgr,byte> image, Rectangle rect)
        {
                         c=new Bgr(Color.FromArgb(0,0,0));
                         //line1 - up-down
                         Point p1 = new Point(rect.X+rect.Width/2, rect.Top+rect.Height/2+20);
                         Point p2 = new Point(rect.X+rect.Width/2, rect.Top+rect.Height/2-20);
                         line1 = new LineSegment2D(p1, p2); ;
                        
                        //line2 - left-right
                         Point p3 = new Point(rect.X + rect.Width/2-20,              rect.Top+(rect.Height/2));
                         Point p4 = new Point(rect.X + rect.Width/2+20, rect.Top+(rect.Height/2));
                         line2 = new LineSegment2D(p3, p4); ;
                       
                        //   image.Draw(last_rectangle,(new Bgr(0,0,0)),6);
                         image.Draw(line1,c,3);
                         image.Draw(line2,c,3);
            return image;
        }

        private void AddLabelAndImage(ref Point startPoint, String labelText, IImage image)
        {
            Label label = new Label();
            panel1.Controls.Add(label);
            label.Text = labelText;
            label.Width = 100;
            label.Height = 30;
            label.Location = startPoint;
            startPoint.Y += label.Height;

            ImageBox box = new ImageBox();
            panel1.Controls.Add(box);
            box.ClientSize = image.Size;
            box.Image = image;
            box.Location = startPoint;
            startPoint.Y += box.Height + 10;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                
                try
                {
                    img = new Image<Bgr, byte>(openFileDialog1.FileName);
                }
                catch
                {
                    MessageBox.Show("Invalide file format");
                    return;
                }

                ProcessImage();
            }
        }


        private void do_work()
        {
            while (true)
            {
                img0 = cam.get_frame();
                
                Thread.Sleep(30);
                tick = tick + 1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            timer.Enabled = true;
            timer.Start();

            if (t1 == null)
            {
                cam.start();
                t1 = new Thread(this.do_work);
                t1.Start();
                button2.Text = "Pause";
            }
            else
            {
                t1.Abort();
                t1 = null;
                cam.pause();
                button2.Text = "Resume";
            }
    
        }

        
    }




    public class CameraCapture
    {
        private Capture _capture = null;
        Image<Bgr, Byte> frame;
            public CameraCapture()
        {
         //   ts = ins1;
            try
            {
                _capture = new Capture();
                _capture.ImageGrabbed += ProcessFrame;
       
            }
            catch (NullReferenceException )
            {
                //MessageBox.Show(excpt.Message);s
            }
         
        }
            public void start()
            {
                _capture.Start();
            }

            private void ProcessFrame(object sender, EventArgs arg)
            {
            //start the capture
            frame = _capture.RetrieveBgrFrame();
            }

        public void pause()
        {
        _capture.Pause();
        }

        public Image<Bgr, Byte>  get_frame()
        {

            if (frame != null)
                       frame = frame.Resize(600, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);  //600, 400,
            
            return frame;
             
                 
        }


        private void ReleaseData()
        {
            if (_capture != null)
                _capture.Dispose();
        }


    }

}
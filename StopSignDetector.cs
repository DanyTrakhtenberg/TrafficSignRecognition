//----------------------------------------------------------------------------
//  Copyright (C) 2013 by Dany&Noam. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.Util;

namespace TrafficSignRecognition
{
    /// <summary>
    /// has special featurs to recognize colors
    /// </summary>
    public class colors
    {

        public Image<Hsv, Byte> hsvimg;
        Image<Gray, Byte> graybox;
        public int extra;


        public int set(Image<Bgr, byte> image)
        {
            this.hsvimg = image.Convert<Hsv, Byte>();
            return 1;
        }

        public int setbox(Image<Gray, Byte> image)
        {

            this.graybox = image;

            return 1;
        }
        /// <summary>
        /// ///////////////////
        /// </summary>
        /// <returns></returns>
        /// 

        public Image<Gray, Byte> getbox()
        {
            return this.graybox;
        }

        /// <summary>
        /// define what color is the pixel in X an Y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// 0-red|||2-Blue||3-white
        /// 
        public int check(int x, int y)
        {
            byte h, s, v;
            h = hsvimg.Data[y, x, 0];
            s = hsvimg.Data[y, x, 1];
            v = hsvimg.Data[y, x, 2];


            if ((h > 157 & h < 180 & s > 55 & v > 100 && v < 250) ||
                 (h >= 0 & h < 8 & s > 100 & v > 47 & v < 250))
            {
               
                return 0; //red
            }

            if (h > 98 & h < 114 & s > 78 & v > 70 & v < 180)//v<30 for distance
            {
          
                return 2; //blue                
            }

            if
                (h >= 0 & h < 180 &
                (s < 50) &
                v > 150)
            {
              
                return 1; //white
            }
            return -1;


            //else
            // return 3;     
        }
        /// <summary>
        /// statistical recognition by color
        /// </summary>
        /// <returns></returns>
        public int check_redboxStop()
        {
            double red = 0, y;
            for (int i = 0; i < graybox.Width - 1; i = i + 1)
                for (int j = 0; j < graybox.Height - 1; j = j + 1)
                {
                    //     System.Drawing.Color color = System.Drawing.Color.FromArgb(img.Data[j, i, 2], img.Data[j, i, 1], img.Data[j, i, 0]);

                    if (graybox.Data[j, i, 0] == 255)
                        red++;
                }
            y = red / (graybox.Width * graybox.Height);
            if (((red / (graybox.Width * graybox.Height)) > 0.38 && (red / (graybox.Width * graybox.Height)) < 0.6))
                return 1;
            else return 0;
        }
        ////////////////
        /// <summary>
        /// statistical recognition by color
        /// </summary>
        /// <returns></returns>
        public int check_redboxKdima()
        {
            double red = 0, y;
            for (int i = 0; i < graybox.Width - 1; i = i + 1)
                for (int j = 0; j < graybox.Height - 1; j = j + 1)
                {
                    //     System.Drawing.Color color = System.Drawing.Color.FromArgb(img.Data[j, i, 2], img.Data[j, i, 1], img.Data[j, i, 0]);

                    if (graybox.Data[j, i, 0] == 255)
                        red++;
                }
            y = red / (graybox.Width * graybox.Height);
            if (((red / (graybox.Width * graybox.Height)) > 0.38 && (red / (graybox.Width * graybox.Height)) < 0.6))
                return 1;

            else if ((red / (graybox.Width * graybox.Height)) > 0.2 && (red / (graybox.Width * graybox.Height)) < 0.45)
                return 2;
            else return 0;
        }

        /////////////////
        /// <summary>
        /// statistical recognition by color
        /// </summary>
        /// <returns></returns>
        public int check_bluebox()
        {
            double blue = 0, y;
            for (int i = 0; i < graybox.Width - 1; i = i + 1)
                for (int j = 0; j < graybox.Height - 1; j = j + 1)
                {
                    //     System.Drawing.Color color = System.Drawing.Color.FromArgb(img.Data[j, i, 2], img.Data[j, i, 1], img.Data[j, i, 0]);

                    if (graybox.Data[j, i, 0] == 255)
                        blue++;
                }
            y = blue / (graybox.Width * graybox.Height);

            if ((blue / (graybox.Width * graybox.Height)) > 0.45 && (blue / (graybox.Width * graybox.Height)) < 0.65)
                return 1;
            if (extra == 1 & (blue / (graybox.Width * graybox.Height)) > 0.2 && (blue / (graybox.Width * graybox.Height)) < 0.4)
                return 2;

            else return 0;
        }
    }
    /// <summary>
    /// detection by color
    /// </summary>
    public class StopSignDetector : DisposableObject
    {
        private colors col_class;
        Image<Gray, Byte> gray_blue, gray_red, gray_white;
        int boxy, boxx;
        Rectangle box2;
        Rectangle box;
        int avgx = 0, avgy = 0;
        Image<Bgr, byte> tmp3;
        public StopSignDetector()
        {
            col_class = new colors();

        }
        /// <summary>
        /// statistical recognition by color
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        /// 
        public int check_whitebox(Image<Gray, Byte> graybox)//for blue
        {
            double white = 0, y;
            for (int i = 0; i < graybox.Width - 1; i = i + 1)
                for (int j = 0; j < graybox.Height - 1; j = j + 1)
                {
                    //     System.Drawing.Color color = System.Drawing.Color.FromArgb(img.Data[j, i, 2], img.Data[j, i, 1], img.Data[j, i, 0]);

                    if (graybox.Data[j, i, 0] == 255)
                        white++;
                }
            y = white / (graybox.Width * graybox.Height);
            if (((white / (graybox.Width * graybox.Height)) > 0.17 && (white / (graybox.Width * graybox.Height)) < 0.49))
                return 1;

            else if ((white / (graybox.Width * graybox.Height)) > 0.5 && (white / (graybox.Width * graybox.Height)) < 0.7)
                return 2;
            else return 0;
        }

        
        /// <summary>
        /// statistical recognition by color
        /// </summary>
        /// <param name="graybox"></param>
        /// <returns></returns>
        public int check_whiteboxKdima(Image<Gray, Byte> graybox)
        {
            double white = 0, y;
            for (int i = 0; i < graybox.Width - 1; i = i + 1)
                for (int j = 0; j < graybox.Height - 1; j = j + 1)
                {
                    //     System.Drawing.Color color = System.Drawing.Color.FromArgb(img.Data[j, i, 2], img.Data[j, i, 1], img.Data[j, i, 0]);

                    if (graybox.Data[j, i, 0] == 255)
                        white++;
                }
            y = white / (graybox.Width * graybox.Height);
            if (((white / (graybox.Width * graybox.Height)) > 0.35 && (white / (graybox.Width * graybox.Height)) < 0.65))
                return 1;

            else return 0;
        }

        /// <summary>
        /// statistical recognition by color
        /// </summary>
        /// <param name="graybox"></param>
        /// <returns></returns>
        public int check_whiteboxKstop(Image<Gray, Byte> graybox)
        {
            double white = 0, y;
            for (int i = 0; i < graybox.Width - 1; i = i + 1)
                for (int j = 0; j < graybox.Height - 1; j = j + 1)
                {
                    //     System.Drawing.Color color = System.Drawing.Color.FromArgb(img.Data[j, i, 2], img.Data[j, i, 1], img.Data[j, i, 0]);

                    if (graybox.Data[j, i, 0] == 255)
                        white++;
                }
            y = white / (graybox.Width * graybox.Height);
            if (((white / (graybox.Width * graybox.Height)) > 0.25 && (white / (graybox.Width * graybox.Height)) < 0.55))
                return 1;

            else return 0;
        }
        /// <summary>
        /// sets the center of an object by the "mass" of its color
        /// </summary>
        /// <param name="image"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public int setbox_CenterBlue(Image<Gray, Byte> image, int Width, int Height, Image<Bgr, byte> img)
        {

            int h, sumx = 0, countx = 0, county = 0, sumy = 0;

            for (int i = 0; i < image.Width - 1; i = i + 1)
                for (int j = 0; j < image.Height - 1; j = j + 1)
                {

                    h = image.Data[j, i, 0];

                    if (h == 255)
                    {
                        countx++;
                        sumx = sumx + i;
                        county++;
                        sumy = sumy + j;
                    }
                }
            avgx = sumx / countx;
            avgy = sumy / county;



            box2.X = (int)Math.Abs(((box.X + avgx) - box.Width * 0.744));
            box2.Y = (int)Math.Abs(((box.Y + avgy) - box.Height * 0.744));
            box2.Height = (int)Math.Abs((box.Height * 1.53));
            box2.Width = (int)Math.Abs((box.Width * 1.53));

            if (box2.Height + box2.Y < img.Height & box2.X + box2.Width < img.Width)
            {

                col_class.extra = 1;

                tmp3 = img.Copy(box2);
                col_class.set(tmp3);
                boxx = box2.X;
                boxy = box2.Y;
            }
            else return -1;
            col_class.setbox(image);



            return 1;
        }
/// <summary>
/// See if the pixel is blue
/// </summary>
/// <param name="x"></param>
/// <param name="y"></param>
/// <returns></returns>
        public int checkblue(int x, int y)
        {//
            // int xx = x + boxx;
            //int yy = y + boxy;

            if (gray_blue.Data[y + boxy, x + boxx, 0] == 255)
                return 2;
            return -1;
        }

        /// <summary>
        ///  see if the pixel is red
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int checkred(int x, int y)
        {
       

            if (gray_red.Data[y + boxy, x + boxx, 0] == 255)
                return 0;
            return -1;
        }

        /// <summary>
        /// see if the pixel is white
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int checkwhite(int x, int y)
        {



            if (gray_white.Data[y + boxy, x + boxx, 0] == 255)
                return 1;
            return -1;


        }


        /// <summary>
        /// </summary>
        /// <param name="img"></param>
        /// <param name="stopSignList"></param>
        /// <param name="boxList"></param>
        /// <param name="contours"></param>
        private void FindStopSignBlue(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, Contour<Point> contours, List<String> id)
        {
            Image<Bgr, byte> tmp2;

            for (; contours != null; contours = contours.HNext)
            {
                box = contours.BoundingRectangle;
                box2 = contours.BoundingRectangle;
                tmp2 = img.Copy(box);
                tmp3 = img.Copy(box);
                col_class.extra = 0;
                boxx = box.X;
                boxy = box.Y;
                int ww = 0;

                contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);
                if (contours.Area > 40 & ((box.Width <= (box.Height / 5 + box.Height)) && ((box.Width + box.Width / 5) >= box.Height)) &&
                    checkblue((int)Math.Abs(tmp2.Width / 2), (int)Math.Abs(tmp2.Height / 3)) == 2 &&//up
                     checkblue((int)Math.Abs(tmp2.Width / 3.14), (int)Math.Abs(tmp2.Height / 2)) == 2 &&//left
                      checkblue((int)Math.Abs(tmp2.Width / 1.41), (int)Math.Abs(tmp2.Height / 2)) == 2 &&//right
                       checkblue((int)Math.Abs(tmp2.Width / 2), (int)Math.Abs(tmp2.Height / 1.5)) == 2 &&
                       checkblue((int)Math.Abs(tmp2.Width / 2.83), (int)Math.Abs(tmp2.Height / 1.62)) == 2 &&//down left
                       checkblue((int)Math.Abs(tmp2.Width / 1.57), (int)Math.Abs(tmp2.Height / 2.89)) == 2 &&//up right
                        checkblue((int)Math.Abs(tmp2.Width / 2), (int)Math.Abs(tmp2.Height / 2)) == 2)//down
                //  if (contours.Area > 1000 )//2000
                {

                    col_class.set(tmp2);
                    col_class.setbox(gray_blue.Copy(box));//convert the box to hsv

                    //round sign detect

                    //***/_\
                    if (

                       checkwhite((int)Math.Abs(tmp2.Width / 2), (int)Math.Abs(tmp2.Height / 5.9)) == 1 && //up

                       //right
                       (checkwhite((int)Math.Abs(tmp2.Width / 1.3), (int)Math.Abs(tmp2.Height / 1.4)) == 1 ||
                       checkwhite((int)Math.Abs(tmp2.Width / 1.2), (int)Math.Abs(tmp2.Height / 2.32)) == 1)


                       && //left

                      (checkwhite((int)Math.Abs(tmp2.Width / 5.2), (int)Math.Abs(tmp2.Height / 1.5)) == 1 ||
                       checkwhite((int)Math.Abs(tmp2.Width / 3.5), (int)Math.Abs(tmp2.Height / 1.31)) == 1)

                      // && (col_class.check_bluebox() == 1)
                        //&& ((ww = check_whitebox(gray_white.Copy(box2))) == 1 )
                       )
                    {

                        id.Add("round1");
                        boxList.Add(box);
                        stopSignList.Add(col_class.getbox());
                    }



                    else if (//V


                       (checkwhite((int)Math.Abs(tmp2.Width / 2), (int)Math.Abs(tmp2.Height / 1.2)) == 1 ||  //down
                        checkwhite((int)Math.Abs(tmp2.Width / 4.87), (int)Math.Abs(tmp2.Height / 1.16)) == 1 ||
                        checkwhite((int)Math.Abs(tmp2.Width / 2.032), (int)Math.Abs(tmp2.Height / 1.087)) == 1 ||
                         checkwhite((int)Math.Abs(tmp2.Width / 1.31), (int)Math.Abs(tmp2.Height / 1.121)) == 1) &&

                        (checkwhite((int)Math.Abs(tmp2.Width / 15), (int)Math.Abs(tmp2.Height / 2)) == 1 || //left
                        checkwhite((int)Math.Abs(tmp2.Width / 6), (int)Math.Abs(tmp2.Height / 2)) == 1) &&
                        (checkwhite((int)Math.Abs(tmp2.Width / 1.18), (int)Math.Abs(tmp2.Height / 2)) == 1 || //right
                        checkwhite((int)Math.Abs(tmp2.Width / 1.2), (int)Math.Abs(tmp2.Height / 3)) == 1)
                        //&& col_class.check_bluebox() == 1
                        //&& ((check_whitebox(gray_white.Copy(box2))) == 1)
                        )
                    {

                        id.Add("round2");
                        boxList.Add(box);
                        stopSignList.Add(col_class.getbox());
                    }

                     //***/_\ with no ends and no blue on top 
                    else if (



                        //right
                      (checkwhite((int)Math.Abs(tmp2.Width - 3), (int)Math.Abs(tmp2.Height / 2)) == 1 &&
                      checkwhite((int)Math.Abs(tmp2.Width - 5), (int)Math.Abs(tmp2.Height / 2)) == 1)

                       && checkwhite((int)Math.Abs(tmp2.Width - 5), (int)Math.Abs(tmp2.Height / 1.56)) == 1



                      && //left

                     checkwhite((int)Math.Abs(4), (int)Math.Abs(tmp2.Height / 2)) == 1
                    && checkwhite((int)Math.Abs(5), (int)Math.Abs(tmp2.Height / 1.74)) == 1
                  
                      )
                    {

                        id.Add("round no ends");
                        boxList.Add(box);
                        stopSignList.Add(col_class.getbox());
                    }

                    else if (setbox_CenterBlue(gray_blue.Copy(box), img.Width, img.Height, img) == 1 &&
                        col_class.setbox(gray_blue.Copy(box2)) == 1 &&
                        //***/_\****
                       ((checkwhite((int)Math.Abs(tmp3.Width / 2), (int)Math.Abs(tmp3.Height / 5.9)) == 1 && //up
                        (checkwhite((int)Math.Abs(tmp3.Width / 1.3), (int)Math.Abs(tmp3.Height / 1.4)) == 1 ||
                        checkwhite((int)Math.Abs(tmp3.Width / 1.2), (int)Math.Abs(tmp3.Height / 2.32)) == 1 ||
                       checkwhite((int)Math.Abs(tmp3.Width / 1.05), (int)Math.Abs(tmp3.Height / 1.69)) == 1)
                        && //right
                        //left
                       (checkwhite((int)Math.Abs(tmp3.Width / 5.2), (int)Math.Abs(tmp3.Height / 1.5)) == 1 ||
                        checkwhite((int)Math.Abs(tmp3.Width / 3.5), (int)Math.Abs(tmp3.Height / 1.31)) == 1 ||
                         checkwhite((int)Math.Abs(tmp3.Width / 18), (int)Math.Abs(tmp3.Height / 1.625)) == 1)) ||

                        //V
                        ((checkwhite((int)Math.Abs(tmp3.Width / 2.7), (int)Math.Abs(tmp3.Height / 1.11)) == 1 ||  //down
                        checkwhite((int)Math.Abs(tmp2.Width / 1.5), (int)Math.Abs(tmp3.Height / 3.6)) == 1) &&

                        (checkwhite((int)Math.Abs(tmp3.Width / 12.8), (int)Math.Abs(tmp3.Height / 2.59)) == 1 || //left
                        checkwhite((int)Math.Abs(tmp3.Width / 4.57), (int)Math.Abs(tmp3.Height / 5)) == 1) &&
                        (checkwhite((int)Math.Abs(tmp3.Width / 1.18), (int)Math.Abs(tmp3.Height / 2)) == 1 || //right
                        checkwhite((int)Math.Abs(tmp3.Width / 1.1), (int)Math.Abs(tmp3.Height / 3.18)) == 1)))
                        && (col_class.check_bluebox() == 2)
                         && check_whitebox(gray_white.Copy(box2))==2
                                            )
                    {

                        id.Add("round1_extra");
                        boxList.Add(box2);
                        stopSignList.Add(col_class.getbox());
                    }

                    else
                    {

                        Contour<Point> child = contours.VNext;
                        if (child != null)
                            FindStopSignBlue(img, stopSignList, boxList, child, id);
                        continue;

                    }
                }
                else
                {

                    Contour<Point> child = contours.VNext;
                    if (child != null)
                        FindStopSignBlue(img, stopSignList, boxList, child, id);
                    continue;
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        private void FindStopSignRed(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, Contour<Point> contours, List<String> id)
        {
            Image<Bgr, byte> tmp2;

            for (; contours != null; contours = contours.HNext)
            {
                Rectangle box = contours.BoundingRectangle;
                Rectangle box2 = contours.BoundingRectangle;
                tmp2 = img.Copy(box);

                boxx = box.X;
                boxy = box.Y;


                // if (box.Width==36)

                contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);
                if (contours.Area > 200 & ((box.Width <= (box.Height / 5 + box.Height)) && ((box.Width + box.Width / 5) >= box.Height)))//2000
                //  if (contours.Area > 1000 )//2000
                {

                    col_class.set(tmp2);


                    col_class.setbox(gray_red.Copy(box));//convert the box to hsv




                    //yield sign detect
                    if (
                       checkwhite((int)Math.Abs(tmp2.Width / 2), (int)Math.Abs(tmp2.Height / 3)) == 1 &&
                       checkred((int)10, (int)Math.Abs(tmp2.Height / 13)) == 0 && //left

                         //red in up right
                       checkred((int)Math.Abs(tmp2.Width - 10), (int)Math.Abs(tmp2.Height / 13)) == 0 &&//up right
                        checkred((int)Math.Abs(tmp2.Width / 2.5), (int)Math.Abs(tmp2.Height / 13)) == 0 && // //red in up middle
                        checkred((int)Math.Abs(tmp2.Width / 3.44), (int)Math.Abs(tmp2.Height / 1.98)) == 0 &&  //LEFT MIDDLE
                        checkred((int)Math.Abs(tmp2.Width / 1.39), (int)Math.Abs(tmp2.Height / 1.98)) == 0
                     
                           )
                    {


                        id.Add("yield");
                        boxList.Add(box);
                        stopSignList.Add(col_class.getbox());
                    }



                    //detect stop sign
                    else if (
                        checkred((int)Math.Abs(tmp2.Width / 6), (int)Math.Abs(tmp2.Height / 2)) == 0 &&  //middle left
                        checkred((int)Math.Abs(tmp2.Width / 1.12), (int)Math.Abs(tmp2.Height / 2)) == 0 & //middle right
                        (checkwhite((int)Math.Abs(tmp2.Width / 2), (int)Math.Abs(tmp2.Height / 1.2)) == 1 || // middle down
                        checkwhite((int)Math.Abs(tmp2.Width / 2), (int)Math.Abs(tmp2.Height / 1.4)) == 1) &&
                         checkwhite((int)Math.Abs(tmp2.Width / 2), (int)Math.Abs(tmp2.Height / 1.9)) == 1 && // midlle
                        (checkwhite((int)Math.Abs(tmp2.Width / 2), (int)Math.Abs(tmp2.Height / 3.09)) == 1 ||// middle up
                        (checkwhite((int)Math.Abs(tmp2.Width / 2), (int)Math.Abs(tmp2.Height / 3.8)) == 1)) &&
                        col_class.check((int)Math.Abs(tmp2.Width / 5.166), (int)Math.Abs(tmp2.Height / 3.81)) == 0 //left_right
             
                         )
                    {

                        id.Add("stop");
                        boxList.Add(box);
                        stopSignList.Add(col_class.getbox());

                    }


                    else
                    {

                        Contour<Point> child = contours.VNext;
                        if (child != null)
                            FindStopSignRed(img, stopSignList, boxList, child, id);
                        continue;

                    }
                }
                else
                {

                    Contour<Point> child = contours.VNext;
                    if (child != null)
                        FindStopSignRed(img, stopSignList, boxList, child, id);
                    continue;
                }
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// class that deals with the detection by color
        /// </summary>
        /// <param name="img"></param>
        /// <param name="stopSignList"></param>
        /// <param name="boxList"></param>
        public void DetectStopSign(Image<Bgr, Byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, List<String> id)
        {

            Image<Bgr, Byte> img2;
            Image<Hsv, Byte> hsvimg;

          
            hsvimg = img.Convert<Hsv, Byte>();
          
            img2 = img;

            col_class.set(img);

            gray_red = img.Convert<Gray, Byte>().PyrDown().PyrUp();
            gray_red = gray_red.CopyBlank();

            gray_blue = gray_red.CopyBlank();
            gray_white = gray_red.CopyBlank();



            for (int i = 0; i < img.Width - 1; i = i + 1)
                for (int j = 0; j < img.Height - 1; j = j + 1)
                {
             
                    if (col_class.check(i, j) == 0)
                    {

                        gray_red.Data[(int)j, (int)i, 0] = 255;
                    }

                    else if (col_class.check(i, j) == 2)
                    {
                        gray_blue.Data[(int)j, (int)i, 0] = 255;

                    }


                    else if (col_class.check(i, j) == 1)
                    {
                        gray_white.Data[(int)j, (int)i, 0] = 255;

                    }


                }


            using (MemStorage stor = new MemStorage())
            {
                Contour<Point> contours = gray_blue.FindContours(
                   Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                      Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                      stor);
                FindStopSignBlue(img2, stopSignList, boxList, contours, id);

                contours = gray_red.FindContours(
                Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                   Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                   stor);
                FindStopSignRed(img2, stopSignList, boxList, contours, id);
            }


        }

        protected override void DisposeObject()
        {


        }
    }
}





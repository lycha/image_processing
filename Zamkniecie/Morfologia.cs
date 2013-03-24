using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Morfologia
{
    class Morfologia
    {
        byte[,] maska;
        int maska_rozm;

        public Morfologia(int rozmiar, int kat)//konstruktor elementu strukturalnego
        {
            maska = new byte[rozmiar, rozmiar];
            int mid = rozmiar / 2;
            switch (kat)
            {
                case 0:
                    for (int i = 0; i < rozmiar; i++)
                    {
                        for (int j = 0; j < rozmiar; j++)
                        {
                            if (j == mid)
                                maska[i, j] = 1;
                        }
                    }
                    break;
                case 45:
                    for (int i = 0; i < rozmiar; i++)
                    {
                        for (int j = 0; j < rozmiar; j++)
                        {
                            if (j == i)
                                maska[i, j] = 1;
                        }
                    }
                    break;
            }
            maska_rozm = rozmiar;
        }

        public Bitmap Dylatacja(Bitmap SrcImage)//operacja dylatacji
        {
            Bitmap obraz = new Bitmap(SrcImage.Width, SrcImage.Height);

            BitmapData SrcData = SrcImage.LockBits(new Rectangle(0, 0,
                SrcImage.Width, SrcImage.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            BitmapData DestData = obraz.LockBits(new Rectangle(0, 0, obraz.Width,
                obraz.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            
            byte max, wart;
            int promien = maska_rozm / 2;
            int ir, jr;

            unsafe
            {
                for (int kol = promien; kol < DestData.Height - promien; kol++)
                {
                    byte* ptr = (byte*)SrcData.Scan0 + (kol * SrcData.Stride);
                    byte* dstPtr = (byte*)DestData.Scan0 + (kol * SrcData.Stride);

                    for (int wier = promien; wier < DestData.Width - promien; wier++)
                    {
                        max = 0;
                        wart = 0;

                        for (int eleKol = 0; eleKol < maska_rozm; eleKol++)
                        {
                            ir = eleKol - promien;
                            byte* tempPtr = (byte*)SrcData.Scan0 +
                                ((kol + ir) * SrcData.Stride);

                            for (int eleWier = 0; eleWier < maska_rozm; eleWier++)
                            {
                                jr = eleWier - promien;

                                wart = (byte)((tempPtr[wier * 3 + jr] +
                                    tempPtr[wier * 3 + jr + 1] + tempPtr[wier * 3 + jr + 2]) / 3);

                                if (max < wart)
                                {
                                    if (maska[eleKol, eleWier] != 0)
                                        max = wart;
                                }
                            }
                        }

                        dstPtr[0] = dstPtr[1] = dstPtr[2] = max;

                        ptr += 3;
                        dstPtr += 3;
                    }
                }
            }

            SrcImage.UnlockBits(SrcData);
            obraz.UnlockBits(DestData);

            return obraz;
        }

        public Bitmap Erozja(Bitmap SrcImage)//operacja erozji
        {
            Bitmap obraz = new Bitmap(SrcImage.Width, SrcImage.Height);

            BitmapData SrcData = SrcImage.LockBits(new Rectangle(0, 0,
                SrcImage.Width, SrcImage.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            BitmapData DestData = obraz.LockBits(new Rectangle(0, 0, obraz.Width,
                obraz.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte min, wart;
            int promien = maska_rozm / 2;
            int ir, jr;

            unsafe
            {

                for (int kol = promien; kol < DestData.Height - promien; kol++)
                {
                    byte* ptr = (byte*)SrcData.Scan0 + (kol * SrcData.Stride);
                    byte* dstPtr = (byte*)DestData.Scan0 + (kol * SrcData.Stride);

                    for (int wier = promien; wier < DestData.Width - promien; wier++)
                    {
                        min = 255;
                        wart = 0;

                        for (int eleKol = 0; eleKol < maska_rozm; eleKol++)
                        {
                            ir = eleKol - promien;
                            byte* tempPtr = (byte*)SrcData.Scan0 +
                                ((kol + ir) * SrcData.Stride);

                            for (int eleWier = 0; eleWier < maska_rozm; eleWier++)
                            {
                                jr = eleWier - promien;

                                wart = (byte)((tempPtr[wier * 3 + jr] +
                                        tempPtr[wier * 3 + jr] + tempPtr[wier * 3 + jr]) / 3);
                                
                                if (min > wart)
                                {
                                    if (maska[eleKol, eleWier] != 0)
                                        min = wart;
                                }
                            }
                        }

                        dstPtr[0] = dstPtr[1] = dstPtr[2] = min;

                        ptr += 3;
                        dstPtr += 3;
                    }
                }
            }

            SrcImage.UnlockBits(SrcData);
            obraz.UnlockBits(DestData);

            return obraz;
        }

        public Bitmap Gradient(Bitmap SrcImage)//operacja gradientu morfologicznego
        {
            Bitmap ero = new Bitmap(SrcImage.Width, SrcImage.Height);
            Bitmap obraz = new Bitmap(SrcImage.Width, SrcImage.Height);

            ero = Erozja(SrcImage);

            for (int i = 0; i < SrcImage.Width; i++)
            {
                for (int j = 0; j < SrcImage.Height; j++)
                {
                    Color eroKol = ero.GetPixel(i, j);
                    Color srcKol = SrcImage.GetPixel(i, j);

                    int czerw = srcKol.R - eroKol.R;
                    int ziel = srcKol.G - eroKol.G;
                    int nieb = srcKol.B - eroKol.B;
                  
                    if (czerw < 0)
                        czerw = 0;
                    if (ziel < 0)
                        ziel = 0;
                    if (nieb < 0)
                        nieb = 0;

                    Color obrKol = Color.FromArgb(czerw, ziel, nieb);
                    obraz.SetPixel(i, j, obrKol);
                }
            }   
            return obraz;
        }

        private int licz(int cr, int cl, int cu, int cd, int cld, int clu, int cru, int crd, int[,] tablica)//obliczanie nowej wartosci piksela
        {
            return Math.Abs(tablica[0, 0] * clu + tablica[0, 1] * cu + tablica[0, 2] * cru
               + tablica[1, 0] * cl + tablica[1, 2] * cr
                  + tablica[2, 0] * cld + tablica[2, 1] * cd + tablica[2, 2] * crd);
        }
        private int skladanie(int cr, int cl, int cu, int cd, int cld, int clu, int cru, int crd)//skladanie filtracji klasą L2
        {
            int max = 0;
            for (int i = 0; i < maski.Count; i++)
            {
                int nowaW = licz(cr, cl, cu, cd, cld, clu, cru, crd, maski[i]);
                if (nowaW > max)
                    max = nowaW;
            }
            return max;
        }
        private List<int[,]> maski = new List<int[,]> //kolekcja zawierająca maski
        {
            new int[3, 3]{ 
                {1,2,1},
                {0,0,0},
                {-1,-2,-1}},
            new int[3, 3]{ 
                {1,0,-1},
                {2,0,-2},
                {1,0,-1}}
        };
        public Bitmap Filtracja(Bitmap SrcImage)//funkcja inicjująca filtrację maską Sobela
        {
            Bitmap obraz = new Bitmap(SrcImage.Width, SrcImage.Height);
            for (int i = 1; i < SrcImage.Width - 1; i++)
            {
                for (int j = 1; j < SrcImage.Height - 1; j++)
                {
                    Color cr = SrcImage.GetPixel(i + 1, j);
                    Color cl = SrcImage.GetPixel(i - 1, j);
                    Color cu = SrcImage.GetPixel(i, j - 1);
                    Color cd = SrcImage.GetPixel(i, j + 1);
                    Color cld = SrcImage.GetPixel(i - 1, j + 1);
                    Color clu = SrcImage.GetPixel(i - 1, j - 1);
                    Color crd = SrcImage.GetPixel(i + 1, j + 1);
                    Color cru = SrcImage.GetPixel(i + 1, j - 1);
                    int wsp = skladanie(cr.R, cl.R, cu.R, cd.R, cld.R, clu.R, cru.R, crd.R);
                    int wsp2 = skladanie(cr.G, cl.G, cu.G, cd.G, cld.G, clu.G, cru.G, crd.G);
                    int wsp3 = skladanie(cr.B, cl.B, cu.B, cd.B, cld.B, clu.B, cru.B, crd.B);

                    if (wsp > 255)
                        wsp = 255;
                    if (wsp2 > 255)
                        wsp2 = 255;
                    if (wsp3 > 255)
                        wsp3 = 255;

                    Color ou = Color.FromArgb(wsp,wsp2,wsp3);
                    obraz.SetPixel(i, j, ou);
                }
            }
            return obraz;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;

namespace Demo
{
    public struct CElementInfo
    {
        public string sName;
        public string sPath;
        public int nX;
        public int nY;
        public int nZ;
        public int nRow;
        public int nColumn;
        public int nHeight;
        public int nWidth;
    };

    static class COriginalInfo
    {
        public const int nAdventureMapInfo = 0;
        public const int nGameMainMapInfo = 1;
        public const int nDogInfo = 2;
        public static ArrayList mapData = new ArrayList()
       {
           //NAME,PATH,nX,nY,nZ,nROW,nCOLUMN,nWidth,nHeight
           //保证Map在第一位
           new ArrayList() //Adventure
           {
               new ArrayList(){"Map"  ,@"Resources\Background\Map.png"  ,0   ,  0,-1,1,1,2000,600},
               new ArrayList(){"Arrow",@"Resources\Background\Arrow.png",0   ,435,-1,1,1,128 ,96 },
               new ArrayList(){"BOX1" ,@"Resources\Background\Box0.png" ,400 ,450,-1,1,1,60  ,59 },
               new ArrayList(){"BOX2" ,@"Resources\Background\Box0.png" ,800 ,420,-1,1,1,60  ,59 },
               new ArrayList(){"BOX3" ,@"Resources\Background\Box0.png" ,1400,460,-1,1,1,60  ,59 },
               new ArrayList(){"Grass1" ,@"Resources\Background\Sun.png" ,300,100,-1,1,2,128,128 },
               
           },
           new ArrayList() //GameMain
           {
               new ArrayList(){"Map",@"Resources\Background\Home.png",0,10,-1,1,1,800,560},
               new ArrayList(){"Arrow1",@"Resources\Background\Arrow1.png",670,340,-1,1,1,128,96}
           },
           new ArrayList()
           {
               new ArrayList(){"Dog",@"Resources\Animal\Doggy.png",300,450,0,2,6,64,64},
               new ArrayList(){"Dog",@"Resources\Animal\Doggy.png",300,375,0,2,6,64,64}
           }
       };

        public static int GetElementNum(int PageIndex)
        {
            return (mapData[PageIndex] as ArrayList).Count;
        }

        public static CElementInfo FillMapData(int PageIndex,int ElementIndex)
        {
            CElementInfo ei = new CElementInfo();
            ei.sName = (string)((ArrayList)((ArrayList)mapData[PageIndex])[ElementIndex])[0];
            ei.sPath = (string)((ArrayList)((ArrayList)mapData[PageIndex])[ElementIndex])[1];
            ei.nX = (int)((ArrayList)((ArrayList)mapData[PageIndex])[ElementIndex])[2];
            ei.nY = (int)((ArrayList)((ArrayList)mapData[PageIndex])[ElementIndex])[3];
            ei.nZ = (int)((ArrayList)((ArrayList)mapData[PageIndex])[ElementIndex])[4]; 
            ei.nRow = (int)((ArrayList)((ArrayList)mapData[PageIndex])[ElementIndex])[5]; 
            ei.nColumn = (int)((ArrayList)((ArrayList)mapData[PageIndex])[ElementIndex])[6];
            ei.nWidth = (int)((ArrayList)((ArrayList)mapData[PageIndex])[ElementIndex])[7];
            ei.nHeight = (int)((ArrayList)((ArrayList)mapData[PageIndex])[ElementIndex])[8];
            return ei;
        }
    }
}

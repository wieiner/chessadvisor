using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace mychess2
{
        
    public class gmmtrx
    {
        /// <summary>
        /// матрица ходов. ось ’ - ходы компьютерного игрока. ось Y возможные ответные ходы противника
        /// </summary>
        //private turn[,] gm;
        public chess_board p_chb;
        /// <summary>
        /// information string
        /// </summary>
        public string s11;        
        /// <summary>
        /// number of black korol turns
        /// </summary>
        public int HaveKorolTurnsB;
        /// <summary>
        /// number of white korol turns
        /// </summary>
        public int HaveKorolTurnsW;
        /// <summary>
        /// number of enemy(white) shasch actions
        /// </summary>
        public int nShachB;
        /// <summary>
        /// number of enemy(black) shasch actions
        /// </summary>
        public int nShachW;
        /// <summary>
        /// v kakom pokolenii black korol hodil?(-1 ne hodil)
        /// </summary>
        public int[] nKorolTuraUgeHodilB;

        /// <summary>
        /// v kakom pokolenii white korol hodil?(-1 ne hodil)
        /// </summary>
        public int[] nKorolTuraUgeHodilW;
        /// <summary>
        /// est' (kol-vo) hody dlja rokirovki Black Korol?
        /// </summary>
        public int nRokirovkaAvailableB;

        /// <summary>
        /// est'(kol-vo) hody dlja rokirovki White Korol?
        /// </summary>
        public int nRokirovkaAvailableW;
        private const int uhNeHodil = -1;
        private const int uhKorol = 0;
        private const int uhTura0 = 1;
        private const int uhTura7 = 2;
        /// <summary>
        /// pokolenije rasheta hoda (s 0)
        /// </summary>
        private int hodlegacy;


        public turn[] xAxis;
        public turn[] yAxis;


        public gmmtrx()
        {
            p_chb = new chess_board();
            s11 = "";
            HaveKorolTurnsB =0;
            HaveKorolTurnsW =0;            
            nShachB = 0;
            nShachW = 0;
            nKorolTuraUgeHodilB = new int[3];
            nKorolTuraUgeHodilB[uhKorol] = uhNeHodil;
            nKorolTuraUgeHodilB[uhTura0] = uhNeHodil;
            nKorolTuraUgeHodilB[uhTura7] = uhNeHodil;

            nKorolTuraUgeHodilW = new int[3];
            nKorolTuraUgeHodilW[uhKorol] = uhNeHodil;
            nKorolTuraUgeHodilW[uhTura0] = uhNeHodil;
            nKorolTuraUgeHodilW[uhTura7] = uhNeHodil;
            
            nRokirovkaAvailableB = 0;
            nRokirovkaAvailableW = 0;
            hodlegacy = 0;
        }


        public gmmtrx(gmmtrx g)
        {

            p_chb = new chess_board(g.p_chb);
 
            s11 = g.s11;
            HaveKorolTurnsB = g.HaveKorolTurnsB;
            HaveKorolTurnsW = g.HaveKorolTurnsW;
            nShachB = g.nShachB;
            nShachW = g.nShachW;
            nKorolTuraUgeHodilB = new int[3];
            nKorolTuraUgeHodilB[uhKorol] = g.nKorolTuraUgeHodilB[uhKorol];
            nKorolTuraUgeHodilB[uhTura0] = g.nKorolTuraUgeHodilB[uhTura0];
            nKorolTuraUgeHodilB[uhTura7] = g.nKorolTuraUgeHodilB[uhTura7];

            nKorolTuraUgeHodilW = new int[3];
            nKorolTuraUgeHodilW[uhKorol] = g.nKorolTuraUgeHodilW[uhKorol];
            nKorolTuraUgeHodilW[uhTura0] = g.nKorolTuraUgeHodilW[uhTura0];
            nKorolTuraUgeHodilW[uhTura7] = g.nKorolTuraUgeHodilW[uhTura7];

            nRokirovkaAvailableB = g.nRokirovkaAvailableB;
            nRokirovkaAvailableW = g.nRokirovkaAvailableW;
            hodlegacy = g.hodlegacy;

            xAxis = new turn[g.xAxis.Length];
            for (int i = 0; i < xAxis.Length; i++) xAxis[i] = g.xAxis[i];

            yAxis = new turn[g.yAxis.Length];
            for (int i = 0; i < yAxis.Length; i++) yAxis[i] = g.yAxis[i];

        }


        public void CalculateKoroljHodMatrix(int x, int y, ref point[,] mkho)
        {
                                mkho[0, 0].x = x-1;     mkho[0, 0].y = y-1;
                                mkho[1, 0].x = x;       mkho[1, 0].y = y-1;
                                mkho[2, 0].x = x+1;     mkho[2, 0].y = y-1;

                                mkho[0, 1].x = x - 1;   mkho[0, 1].y = y;
                                mkho[1, 1].x = x;       mkho[1, 1].y = y;
                                mkho[2, 1].x = x + 1;   mkho[2, 1].y = y;

                                mkho[0, 2].x = x - 1;   mkho[0, 2].y = y+1;
                                mkho[1, 2].x = x;       mkho[1, 2].y = y+1;
                                mkho[2, 2].x = x + 1;   mkho[2, 2].y = y+1;

        }


        public void CalculateAllOfHods()
        {
            CalculateAllOfHodsDirty();

            bool[] mask = new bool[yAxis.Length];
            int len = 0;
            turn[] x1, x2;
            gmmtrx gam2;
            //играем черными            
            gam2 = new gmmtrx(this);
            for (int i = 0; i < gam2.yAxis.Length; i++)
            {
                gam2.MakeHod(gam2.yAxis[i],false);
                gam2.CalculateAllOfHodsDirty();
                if (gam2.nShachB <= 0) { mask[i] = false; len++; }
                else { mask[i] = true; }

                gam2 = new gmmtrx(this);
            }

            x1 = new turn[len];
            int k1 = 0;
            for (int i = 0; i < yAxis.Length; i++)
                if (!mask[i]) { x1[k1] = yAxis[i]; k1++; }

            if (len == 0) nShachB = 555;




            
            mask = new bool[xAxis.Length];
            len = 0;            
            //играем белыми
            gam2 = new gmmtrx(this);
            for (int i = 0; i < gam2.xAxis.Length; i++)
            {
                gam2.MakeHod(gam2.xAxis[i],false);
                gam2.CalculateAllOfHodsDirty();
                if (gam2.nShachW <= 0) { mask[i] = false; len++; }
                else { mask[i] = true; }

                gam2 = new gmmtrx(this);
            }

            x2 = new turn[len];
            k1 = 0;
            for (int i = 0; i < xAxis.Length; i++)
                if (!mask[i]) { x2[k1] = xAxis[i]; k1++; }
            if (len == 0) nShachW = 555;
            

            //yAxis = new turn[x1.Length];
            //for (int i = 0; i < x1.Length; i++) yAxis[i] = x1[i];
           // xAxis = new turn[x2.Length];
           // for (int i = 0; i < x2.Length; i++) xAxis[i] = x2[i];
            

            xAxis = x2;
            yAxis = x1;
            US111();
        }

        internal void US111()
        {
                        
            s11 = "";
            if (nShachW > 0) s11 += "Ўј’ Ѕ≈Ћџћ !!!";
            s11 += " ” Ѕ≈Ћќ√ќ  ќ–ќЋя ";
            s11 += HaveKorolTurnsW.ToString();
            s11 += " ’ќƒќ¬ ";
            if (nShachB > 0) s11 += "Ўј’ „≈–Ќџћ!!! ";
            s11 += " ” „≈–Ќќ√ќ  ќ–ќЋя ";
            s11 += HaveKorolTurnsB.ToString();
            s11 += " ’ќƒќ¬ ";



            turn[] Axis;
            bool isWhiteTurn = true;

        beginlabel2:
            //s11 = xAxis.Length.ToString();            
            if (isWhiteTurn)
            {
                Axis = xAxis;
                s11 += "===ходы белых===(";
                s11 += Axis.Length.ToString();
                s11 += ")=";
            }
            else
            {
                Axis = yAxis;
                s11 += "==ходы черных==(";
                s11 += Axis.Length.ToString();
                s11 += ")=";
            }

            int k;
            string[] abc = { "a", "b", "c", "d", "e", "f", "g", "h" };
            string[] fig = { "бпешка", "бладь¤", "бконь", "бслон", "бферзь", "бкороль", "пешка", "ладь¤", "конь", "слон", "ферзь", "король", "пусто" };
            string[] act = { "ход", "рубим", "шах", "мат", "пат", "пешка на последней линии", "рубим проходную пешку", "ход пешки на 2 клетки", "рокировка" };

            for (int i = 0; i < Axis.Length; i++)
            {
                k = i + 1;
                s11 += k.ToString();
                s11 += ") ";

                k = (int)(Axis[i].fig_source);
                s11 += fig[k];

                s11 += " ";

                k = (int)(Axis[i].act);
                s11 += act[k];

                if ((Axis[i].act == Actions.aRubka) ||
                    (Axis[i].act == Actions.aRubkaProhodnajaPeshka))
                {
                    s11 += " ";
                    k = (int)(Axis[i].fig_rubka);
                    s11 += fig[k];
                }

                s11 += " ";
                k = Axis[i].from_pos.x;

                s11 += abc[k];
                s11 += "";

                k = 8 - (Axis[i].from_pos.y + 0);
                s11 += k.ToString();

                s11 += " - ";
                k = Axis[i].to_pos.x;
                s11 += abc[k];
                s11 += "";
                k = 8 - (Axis[i].to_pos.y + 0);
                s11 += k.ToString();
                s11 += "   ";

            }

            if (isWhiteTurn) { isWhiteTurn = false; goto beginlabel2; }
        }
        /// <summary>
        /// прокалькулировать список всех возможных ходов на основании состо¤ни¤ chess_board
        /// </summary>        
        /// <param name="isWhiteTurn">ход белых?</param>
        public void CalculateAllOfHodsDirty()
        {
            point[,] wkho = new point[3, 3];
            point[,] bkho = new point[3, 3];

                        
            xAxis = new turn[0];
            yAxis = new turn[0];

            bool isWhiteTurn = true;

            p_chb.clear_protect_fig();

            //beginlabel:
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                {
                //    if (isWhiteTurn)//все ходы белых
                        switch (p_chb.board[y, x])
                        {
                            case figura.fPeshkaW: CalculatePeshkaHod(isWhiteTurn, x, y, ref xAxis); break;
                            case figura.fTuraW: CalculateTuraSlonHod(false, isWhiteTurn, x, y, ref xAxis); break;
                            case figura.fKonjW: CalculatePferdHod(isWhiteTurn, x, y, ref xAxis); break;
                            case figura.fOfizerW: CalculateTuraSlonHod(true, isWhiteTurn, x, y, ref xAxis); break;
                            case figura.fFerzjW: CalculateFerzHod(isWhiteTurn, x, y, ref xAxis); break;
                            case figura.fKoroljW:CalculateKoroljHodMatrix(x,y, ref wkho);  break;
                            //case figura.fKoroljB: CalculateKoroljHodMatrix(x,y,ref bkho); break;
               //         }
               //     else// все ходы черных
               //         switch (p_chb.board[y, x])
               //         {
                            case figura.fPeshkaB: CalculatePeshkaHod(!isWhiteTurn, x, y, ref yAxis); break;                            
                            case figura.fTuraB: CalculateTuraSlonHod(false, !isWhiteTurn, x, y, ref yAxis); break;
                            case figura.fKonjB: CalculatePferdHod(!isWhiteTurn, x, y, ref yAxis); break;
                            case figura.fOfizerB: CalculateTuraSlonHod(true, !isWhiteTurn, x, y, ref yAxis); break;
                            case figura.fFerzjB: CalculateFerzHod(!isWhiteTurn, x, y, ref yAxis); break;
                           // case figura.fKoroljW: CalculateKoroljHodMatrix(x,y,ref wkho);  break;
                            case figura.fKoroljB: CalculateKoroljHodMatrix(x,y,ref bkho); break;
                        }
                       ////p_chb.clear_protect_fig();

                }

            //if (isWhiteTurn) {isWhiteTurn = false; goto beginlabel;}
            //isWhiteTurn = true;
            //calculate hods of black korol


            HaveKorolTurnsB = yAxis.Length;
            CalculateKoroljHod(!(isWhiteTurn), ref bkho, ref wkho, ref xAxis, ref yAxis);
            nShachB = 0;
                for (int i = 0; i < xAxis.Length; i++)                
                    if (xAxis[i].act == Actions.aShach)
                {          
                    nShachB ++;
                }

            nRokirovkaAvailableB = 0;
            if ((nShachB == 0) && (nKorolTuraUgeHodilB[uhKorol] == uhNeHodil) && ((nKorolTuraUgeHodilB[uhTura0] == uhNeHodil)
                || (nKorolTuraUgeHodilB[uhTura7] == uhNeHodil)))
                nRokirovkaAvailableB = CalculateKoroljRokirovka(!(isWhiteTurn), ref yAxis);
            HaveKorolTurnsB = yAxis.Length - HaveKorolTurnsB;



            //calculate hods of white korol
            HaveKorolTurnsW = xAxis.Length;
            CalculateKoroljHod(isWhiteTurn, ref wkho, ref bkho, ref yAxis, ref xAxis);            
            nShachW = 0;
            for (int i = 0; i < yAxis.Length; i++)
                if (yAxis[i].act == Actions.aShach)
                {
                    nShachW++;
                }
            nRokirovkaAvailableW = 0;
            if ((nShachW == 0) && (nKorolTuraUgeHodilW[uhKorol] == uhNeHodil) && ((nKorolTuraUgeHodilW[uhTura0] == uhNeHodil)
                || (nKorolTuraUgeHodilW[uhTura7] == uhNeHodil)))
                nRokirovkaAvailableW = CalculateKoroljRokirovka(isWhiteTurn, ref xAxis);
            HaveKorolTurnsW = xAxis.Length - HaveKorolTurnsW;

            
            
            
        }

        /// <param name="hods">список ходов пешки</param>
        public void CalculatePeshkaHod(bool isWhiteTurn, int x, int y, ref turn[] hods)
        {
            Array.Resize<turn>(ref hods, hods.Length + 28);
            int extralen = 0;
            int up = 1;//направление хода черной пешки
            if (isWhiteTurn) up = -1;//если пешка бела¤

            if (!p_chb.isWhiteBoardOrigin) up = -up;//если начало доски - бела¤ тура

            #region ход пешкой вперед на 1
            if (((y + up) >= 0) && ((y + up) <= 7))
                if (p_chb.board[y + up, x] == figura.fCount)
                {
                    if ((((y + up) == 7) && (up > 0)) ||
                         (((y + up) == 0) && (up < 0))
                        ) hods[hods.Length - 28 + extralen].act = Actions.aPeshkaOn8;
                    else hods[hods.Length - 28 + extralen].act = Actions.aHod;

                    if (isWhiteTurn)
                    {
                        hods[hods.Length - 28 + extralen].fig_source = figura.fPeshkaW;
                        hods[hods.Length - 28 + extralen].fig_rubka = figura.fCount;
                    }
                    else
                    {
                        hods[hods.Length - 28 + extralen].fig_source = figura.fPeshkaB;
                        hods[hods.Length - 28 + extralen].fig_rubka = figura.fCount;
                    }

                    hods[hods.Length - 28 + extralen].from_pos.x = x;
                    hods[hods.Length - 28 + extralen].from_pos.y = y;

                    hods[hods.Length - 28 + extralen].to_pos.x = x;
                    hods[hods.Length - 28 + extralen].to_pos.y = y + up;

                    extralen++;
                }
            #endregion


            #region ход пешкой вперед на 2 вложенное условие
            //сложное условие первого хода пешки на 2 клетки
            if (((up > 0) && (y == 1)) ||
                ((up < 0) && (y == 6)))
                if ((p_chb.board[y + 2 * up, x] == figura.fCount)&&(p_chb.board[y + up, x] == figura.fCount))
                {
                    hods[hods.Length - 28 + extralen].act = Actions.aHodPeshka2;
                    if (isWhiteTurn)
                    {
                        hods[hods.Length - 28 + extralen].fig_source = figura.fPeshkaW;
                        hods[hods.Length - 28 + extralen].fig_rubka = figura.fCount;
                    }
                    else
                    {
                        hods[hods.Length - 28 + extralen].fig_source = figura.fPeshkaB;
                        hods[hods.Length - 28 + extralen].fig_rubka = figura.fCount;
                    }

                    hods[hods.Length - 28 + extralen].from_pos.x = x;
                    hods[hods.Length - 28 + extralen].from_pos.y = y;

                    hods[hods.Length - 28 + extralen].to_pos.x = x;
                    hods[hods.Length - 28 + extralen].to_pos.y = y + 2 * up;
                    extralen++;
                }
            #endregion



            #region рубка пешкой влево и вправо вперед
            int kx = 1;
        metka1:
            if (((y + up) >= 0) && ((y + up) <= 7))
                if (((x + kx) >= 0) && ((x + kx) <= 7))
                {
                    if (((isWhiteTurn) &&
                         (p_chb.board[y + up, x + kx] >= figura.fPeshkaB) &&
                         (p_chb.board[y + up, x + kx] <= figura.fKoroljB)) ||

                        ((!isWhiteTurn) &&
                         (p_chb.board[y + up, x + kx] >= figura.fPeshkaW) &&
                         (p_chb.board[y + up, x + kx] <= figura.fKoroljW)))
                    {

                        if ((p_chb.board[y + up, x + kx] == figura.fKoroljB) ||
                            (p_chb.board[y + up, x + kx] == figura.fKoroljW))
                            hods[hods.Length - 28 + extralen].act = Actions.aShach;
                        else
                            hods[hods.Length - 28 + extralen].act = Actions.aRubka;

                        hods[hods.Length - 28 + extralen].fig_rubka = p_chb.board[y + up, x + kx];
                        if (isWhiteTurn)
                            hods[hods.Length - 28 + extralen].fig_source = figura.fPeshkaW;
                        else hods[hods.Length - 28 + extralen].fig_source = figura.fPeshkaB;

                        hods[hods.Length - 28 + extralen].from_pos.x = x;
                        hods[hods.Length - 28 + extralen].from_pos.y = y;

                        hods[hods.Length - 28 + extralen].to_pos.x = x + kx;
                        hods[hods.Length - 28 + extralen].to_pos.y = y + up;

                        extralen++;
                    }
                    //set protected figure matrix
                    else /*if (p_chb.board[y + up, x + kx] != figura.fCount)*/ p_chb.set_protected_fig(y + up, x + kx, isWhiteTurn, true);
                }

            if (kx > 0) { kx = -1; goto metka1; }
            #endregion



            Array.Resize<turn>(ref hods, hods.Length - 28 + extralen);
        }

        /// <param name="hods">список ходов ладьи</param>
        private void CalculateTuraHod_partial1(bool isWhiteTurn, point from_pos, point to_pos, ref turn[] hods, int id, Actions act, figura fig_rubka, figura fig_source)
        {
            hods[id].act = act;
            hods[id].fig_rubka = fig_rubka;
            hods[id].fig_source = fig_source;
            /*
            if (act == Actions.aHod)
            {
                / *
                if (isWhiteTurn)
                    hods[id].fig = figura.fTuraW;
                else
                    hods[id].fig = figura.fTuraB;
                 * /
                hods[id].fig = fig;
            }
            else
                if (act == Actions.aRubka)
                {
                    hods[id].fig = fig;
                }
            */
            hods[id].from_pos = from_pos;
            hods[id].to_pos = to_pos;
        }

        /// <param name="hods">список ходов ладьи и офицера</param>
        public void CalculateTuraSlonHod(bool isSlon, bool isWhiteTurn, int x, int y, ref turn[] hods)
        {

            Array.Resize<turn>(ref hods, hods.Length + 28);
            int extralen = 0;

            int radius = 0;
            int isX = 1;

            int isX1;
            int isX2;
            
            int CalcId;
            bool[] isCalcAllowedDir = { true, true, true, true };

           // figura f1;//наша текуща¤ фигура, ходы которой мы подсчитываем
           // if (isWhiteTurn) f1 = figura.fTuraW; else f1 = figura.fTuraB;

            while (isCalcAllowedDir[0] || isCalcAllowedDir[1] || isCalcAllowedDir[2] || isCalcAllowedDir[3])
            {
                radius++;

            metka2:
                isX1 = isX;
                isX2 = 1 - isX;
                if (isSlon) {   isX1 = 1 - 2 * isX;
                                isX2 = isX1 - (2 * isX1) * isX;                    
                            };

                CalcId = 2 * isX;
                if (radius > 0) CalcId++;

                if (isCalcAllowedDir[CalcId])
                {
                    isCalcAllowedDir[CalcId] = false;
                    //if (isSlon) isX = 1 - isX;

                    if (((x + radius * isX1) <= 7) && (((x + radius * isX1) >= 0)) &&
                        ((y + radius * isX2) <= 7) && (((y + radius * isX2) >= 0))
                        )
                    {
                        if (p_chb.board[y + radius * isX2, x + radius * isX1] == figura.fCount)
                        {
                            CalculateTuraHod_partial1(isWhiteTurn,
                                new point(x, y),
                                new point(x + radius * isX1, y + radius * isX2),
                                ref hods,
                                hods.Length - 28 + extralen,
                                Actions.aHod,
                                figura.fCount,
                                p_chb.board[y, x]);
                            extralen++;
                            isCalcAllowedDir[CalcId] = true;
                        }

                        if (((isWhiteTurn) &&
                              (p_chb.board[y + radius * isX2, x + radius * isX1] >= figura.fPeshkaB) &&
                              (p_chb.board[y + radius * isX2, x + radius * isX1] <= figura.fKoroljB)) ||

                             ((!isWhiteTurn) &&
                              (p_chb.board[y + radius * isX2, x + radius * isX1] >= figura.fPeshkaW) &&
                              (p_chb.board[y + radius * isX2, x + radius * isX1] <= figura.fKoroljW)))
                        {

                            CalculateTuraHod_partial1(isWhiteTurn,
                                new point(x, y),
                                new point(x + radius * isX1, y + radius * isX2),
                                ref hods,
                                hods.Length - 28 + extralen,
                                Actions.aRubka,
                                p_chb.board[y + radius * isX2, x + radius * isX1],
                                p_chb.board[y, x]);


                            if ((p_chb.board[y + radius * isX2, x + radius * isX1] == figura.fKoroljB) ||
                                (p_chb.board[y + radius * isX2, x + radius * isX1] == figura.fKoroljW))
                                hods[hods.Length - 28 + extralen].act = Actions.aShach;                            

                            extralen++;
                        }
                        //set protected figure matrix
                        else if (p_chb.board[y + radius * isX2, x + radius * isX1] != figura.fCount)
                                 p_chb.set_protected_fig(y + radius * isX2, x + radius * isX1, isWhiteTurn, true);
                    }
                }
                radius = -radius;
                if (radius < 0) goto metka2;

                isX = 1 - isX;
                if (isX == 0) goto metka2;
            }



            Array.Resize<turn>(ref hods, hods.Length - 28 + extralen);

        }


        /// <param name="hods">список ходов ферз¤</param>
        public void CalculateFerzHod(bool isWhiteTurn, int x, int y, ref turn[] hods)
        {
            CalculateTuraSlonHod(true, isWhiteTurn, x, y, ref hods);
            CalculateTuraSlonHod(false, isWhiteTurn, x, y, ref hods);
        }

        /// <param name="hods">список ходов кон¤</param>
        public void CalculatePferdHod(bool isWhiteTurn, int x, int y, ref turn[] hods)
        {
            Array.Resize<turn>(ref hods, hods.Length + 28);
            int extralen = 0;
            point[] ho = new point[8];
            ho[0].x =  2;
            ho[0].y = -1;

            ho[1].x = 2;
            ho[1].y = 1;

            ho[2].x = 1;
            ho[2].y = 2;

            ho[3].x = -1;
            ho[3].y = 2;

            ho[4].x = -2;
            ho[4].y = 1;

            ho[5].x = -2;
            ho[5].y = -1;

            ho[6].x = -1;
            ho[6].y = -2;

            ho[7].x = 1;
            ho[7].y = -2;
            
            #region ход конем в 8 клетках по кругу
            for (int i = 0; i < 8; i++)

                if (((x + ho[i].x) >= 0) && ((x + ho[i].x) <= 7) &&
                       ((y + ho[i].y) >= 0) && ((y + ho[i].y) <= 7))
                {
                    if (p_chb.board[y + ho[i].y, x + ho[i].x] == figura.fCount)
                    {
                        hods[hods.Length - 28 + extralen].act = Actions.aHod;

                        if (isWhiteTurn)
                        {
                            hods[hods.Length - 28 + extralen].fig_source = figura.fKonjW;
                            hods[hods.Length - 28 + extralen].fig_rubka = figura.fCount;
                        }
                        else
                        {
                            hods[hods.Length - 28 + extralen].fig_source = figura.fKonjB;
                            hods[hods.Length - 28 + extralen].fig_rubka = figura.fCount;
                        }


                        hods[hods.Length - 28 + extralen].from_pos.x = x;
                        hods[hods.Length - 28 + extralen].from_pos.y = y;

                        hods[hods.Length - 28 + extralen].to_pos.x = x + ho[i].x;
                        hods[hods.Length - 28 + extralen].to_pos.y = y + ho[i].y;
                        extralen++;
                    }
                    else
                    {
                        if (((isWhiteTurn) &&
                         (p_chb.board[y + ho[i].y, x + ho[i].x] >= figura.fPeshkaB) &&
                         (p_chb.board[y + ho[i].y, x + ho[i].x] <= figura.fKoroljB)) ||

                        ((!isWhiteTurn) &&
                         (p_chb.board[y + ho[i].y, x + ho[i].x] >= figura.fPeshkaW) &&
                         (p_chb.board[y + ho[i].y, x + ho[i].x] <= figura.fKoroljW)))
                        {

                            hods[hods.Length - 28 + extralen].act = Actions.aRubka;
                            if ((p_chb.board[y + ho[i].y, x + ho[i].x] == figura.fKoroljB) ||
                                (p_chb.board[y + ho[i].y, x + ho[i].x] == figura.fKoroljW))
                                hods[hods.Length - 28 + extralen].act = Actions.aShach;


                            hods[hods.Length - 28 + extralen].fig_rubka = p_chb.board[y + ho[i].y, x + ho[i].x];
                            if (isWhiteTurn) hods[hods.Length - 28 + extralen].fig_source = figura.fKonjW;
                            else hods[hods.Length - 28 + extralen].fig_source = figura.fKonjB;


                            hods[hods.Length - 28 + extralen].from_pos.x = x;
                            hods[hods.Length - 28 + extralen].from_pos.y = y;

                            hods[hods.Length - 28 + extralen].to_pos.x = x + ho[i].x;
                            hods[hods.Length - 28 + extralen].to_pos.y = y + ho[i].y;
                            extralen++;
                        }
                        //set protected figure matrix
                        else if (p_chb.board[y + ho[i].y, x + ho[i].x] != figura.fCount)
                            p_chb.set_protected_fig(y + ho[i].y, x + ho[i].x, isWhiteTurn, true); ;
                    }
                }

            #endregion

            Array.Resize<turn>(ref hods, hods.Length - 28 + extralen);
        }

        public int CalculateKoroljRokirovka(
            bool isWhiteTurn,            
            ref turn[] hods)
        {

            
            point KorolPlace = new point (4,0);            
            figura korol = figura.fKoroljB;
            figura tura = figura.fTuraB;
            int puhTura0 = nKorolTuraUgeHodilB[uhTura0];
            int puhTura7 = nKorolTuraUgeHodilB[uhTura7];

            if (isWhiteTurn)
            {
                KorolPlace.y = 7;
                korol = figura.fKoroljW;
                tura = figura.fTuraW;

                puhTura0 = nKorolTuraUgeHodilW[uhTura0];// = uhNeHodil;
                puhTura0 = nKorolTuraUgeHodilW[uhTura7];
            }


            Array.Resize<turn>(ref hods, hods.Length + 28);
            int extralen = 0;
            
            if (p_chb.board[KorolPlace.y, KorolPlace.x] == korol)
            {
                //провер¤ем левую рокировку
                if ((p_chb.board[KorolPlace.y, 0] == tura) &&
                    (puhTura0 == uhNeHodil) &&                
                    (p_chb.board[KorolPlace.y, 3] == figura.fCount) &&
                    (p_chb.board[KorolPlace.y, 2] == figura.fCount) &&
                    (p_chb.board[KorolPlace.y, 1] == figura.fCount)
                    )
                    {                        
                        hods[hods.Length - 28 + extralen].act = Actions.aRokirovka;
                        hods[hods.Length - 28 + extralen].fig_rubka = tura;
                        hods[hods.Length - 28 + extralen].fig_source = korol;


                        hods[hods.Length - 28 + extralen].from_pos.x = KorolPlace.x;
                        hods[hods.Length - 28 + extralen].from_pos.y = KorolPlace.y;

                        hods[hods.Length - 28 + extralen].to_pos.x = 0;
                        hods[hods.Length - 28 + extralen].to_pos.y = KorolPlace.y;
                        extralen++;
                    }

                    //провер¤ем правую рокировку
                    if ((p_chb.board[KorolPlace.y, 7] == tura) &&
                        (puhTura0 == uhNeHodil) &&
                        (p_chb.board[KorolPlace.y, 5] == figura.fCount) &&
                        (p_chb.board[KorolPlace.y, 6] == figura.fCount)                        
                        )
                    {
                        hods[hods.Length - 28 + extralen].act = Actions.aRokirovka;
                        hods[hods.Length - 28 + extralen].fig_rubka = tura;
                        hods[hods.Length - 28 + extralen].fig_source = korol;


                        hods[hods.Length - 28 + extralen].from_pos.x = KorolPlace.x;
                        hods[hods.Length - 28 + extralen].from_pos.y = KorolPlace.y;

                        hods[hods.Length - 28 + extralen].to_pos.x = 7;
                        hods[hods.Length - 28 + extralen].to_pos.y = KorolPlace.y;
                        extralen++;
                    }
                
            }

            Array.Resize<turn>(ref hods, hods.Length - 28 + extralen);
            return extralen;
        }

        public void CalculateKoroljHod(
            bool isWhiteTurn,
            ref point[,] mkho,
            ref point[,] skho,
            ref turn[] enemyturns,
            ref turn[] hods)
        {

            //point[,] mkho;
           // point[,] skho;
            bool [,] isAllowed = new bool[3,3];

            isAllowed[0, 0] = true; isAllowed[1, 0] = true; isAllowed[2, 0] = true;
            isAllowed[0, 1] = true; isAllowed[1, 1] = true; isAllowed[2, 1] = true;
            isAllowed[0, 2] = true; isAllowed[1, 2] = true; isAllowed[2, 2] = true;

            Array.Resize<turn>(ref hods, hods.Length + 28);
            int extralen = 0;

            /*
            if (isWhiteTurn)
            { mkho = wkho; skho = bkho;}
            else
            { mkho = bkho; skho = wkho;}
             */
            
                for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    if ( ((mkho[x, y].x) >= 0) && ((mkho[x, y].x) <= 7) &&
                         ((mkho[x, y].y) >= 0) && ((mkho[x, y].y) <= 7)     )                
                {

                    for (int x1 = 0; x1 < 3; x1++)
                    for (int y1 = 0; y1 < 3; y1++)
                    
                    if ( (mkho[x,y].x==skho[x1,y1].x) && (mkho[x,y].y==skho[x1,y1].y) )
                    {
                        isAllowed[x, y] = false;
                    }
                    

                        if (
                          ((isWhiteTurn) &&
                            (
                         ( (p_chb.board[mkho[x,y].y,mkho[x,y].x] >= figura.fPeshkaW) &&
                           (p_chb.board[mkho[x,y].y,mkho[x,y].x] <= figura.fKoroljW)
                         ) ||
                         p_chb.protected_fig[mkho[x, y].y, mkho[x, y].x, 0]
                         ) 

                         ||

                        ((!isWhiteTurn) &&
                            (
                         ( (p_chb.board[mkho[x,y].y,mkho[x,y].x] >= figura.fPeshkaB) &&
                           (p_chb.board[mkho[x,y].y,mkho[x,y].x] <= figura.fKoroljB)
                           ) ||
                         p_chb.protected_fig[mkho[x, y].y, mkho[x, y].x, 1]
                           )))
                            )
                        {
                            isAllowed[x, y] = false;
                        }

                    for (int i = 0; i < enemyturns.Length; i++)
                    {
                        if ((enemyturns[i].to_pos.x == mkho[x, y].x) && (enemyturns[i].to_pos.y == mkho[x, y].y))
                        {
                            if ((enemyturns[i].fig_source!=figura.fPeshkaB) && 
                                (enemyturns[i].fig_source!=figura.fPeshkaW) )
                            isAllowed[x, y] = false;
                            //else 
                            
                            //если король под шахом
                            if ((x == 1) && (y == 1))
                            {
                                int dx = Math.Sign(enemyturns[i].to_pos.x - enemyturns[i].from_pos.x);
                                int dy = Math.Sign(enemyturns[i].to_pos.y - enemyturns[i].from_pos.y);
                              

                                switch (enemyturns[i].fig_source)
                                {
                                    case figura.fFerzjB:
                                    case figura.fFerzjW:
                                    case figura.fOfizerB:
                                    case figura.fOfizerW:
                                    case figura.fTuraB:
                                    case figura.fTuraW:
                                        isAllowed[1+dx, 1+dy] = false;                                        
                                        break;
                                }
                            }
                        }

                    }
                                            
                }





for (int x = 0; x < 3; x++)
for (int y = 0; y < 3; y++)
    if (((mkho[x, y].x) >= 0) && ((mkho[x, y].x) <= 7) &&
        ((mkho[x, y].y) >= 0) && ((mkho[x, y].y) <= 7))                
        if (isAllowed[x, y])
        {

            if (p_chb.board[mkho[x, y].y, mkho[x, y].x] == figura.fCount)
            {
                hods[hods.Length - 28 + extralen].act = Actions.aHod;
                hods[hods.Length - 28 + extralen].fig_rubka = figura.fCount;

                if (isWhiteTurn)
                    hods[hods.Length - 28 + extralen].fig_source = figura.fKoroljW;
                else
                    hods[hods.Length - 28 + extralen].fig_source = figura.fKoroljB;

                hods[hods.Length - 28 + extralen].from_pos.x = mkho[1, 1].x;
                hods[hods.Length - 28 + extralen].from_pos.y = mkho[1, 1].y;

                hods[hods.Length - 28 + extralen].to_pos.x = mkho[x, y].x;
                hods[hods.Length - 28 + extralen].to_pos.y = mkho[x, y].y;
                extralen++;
            }
            else
                if (((isWhiteTurn) &&
                 (p_chb.board[mkho[x, y].y, mkho[x, y].x] >= figura.fPeshkaB) &&
                 (p_chb.board[mkho[x, y].y, mkho[x, y].x] < figura.fKoroljB)  &&
                 (!p_chb.protected_fig[mkho[x, y].y, mkho[x, y].x, 0])) ||

                ((!isWhiteTurn) &&
                 (p_chb.board[mkho[x, y].y, mkho[x, y].x] >= figura.fPeshkaW) &&
                 (p_chb.board[mkho[x, y].y, mkho[x, y].x] < figura.fKoroljW)) &&
                 (!p_chb.protected_fig[mkho[x, y].y, mkho[x, y].x, 1]))

                {

                    hods[hods.Length - 28 + extralen].act = Actions.aRubka;

                    if (isWhiteTurn)
                        hods[hods.Length - 28 + extralen].fig_source = figura.fKoroljW;
                    else
                        hods[hods.Length - 28 + extralen].fig_source = figura.fKoroljB;

                    hods[hods.Length - 28 + extralen].fig_rubka = p_chb.board[mkho[x, y].y, mkho[x, y].x];

                    hods[hods.Length - 28 + extralen].from_pos.x = mkho[1, 1].x;
                    hods[hods.Length - 28 + extralen].from_pos.y = mkho[1, 1].y;

                    hods[hods.Length - 28 + extralen].to_pos.x = mkho[x, y].x;
                    hods[hods.Length - 28 + extralen].to_pos.y = mkho[x, y].y;
                    extralen++;
                }

        }



            Array.Resize<turn>(ref hods, hods.Length - 28 + extralen);

            /*
            int x, y;
            if (isWhiteTurn)
            {
                x = wkho[1, 1].x;
                y = wkho[1, 1].y;
            }
            else
            {
                x = bkho[1, 1].x;
                y = bkho[1, 1].y;
            }

            Array.Resize<turn>(ref hods, hods.Length + 28);
            int extralen = 0;
            point[] ho = new point[8];
            ho[0].x = 1;
            ho[0].y = 0;

            ho[1].x = 1;
            ho[1].y = -1;

            ho[2].x = 0;
            ho[2].y = -1;

            ho[3].x = -1;
            ho[3].y = -1;

            ho[4].x = -1;
            ho[4].y = 0;

            ho[5].x = -1;
            ho[5].y =  1;

            ho[6].x =  0;
            ho[6].y =  1;

            ho[7].x = 1;
            ho[7].y = 1;

            #region ход королем в 8 клетках по кругу
            for (int i = 0; i < 8; i++)

                if (((x + ho[i].x) >= 0) && ((x + ho[i].x) <= 7) &&
                       ((y + ho[i].y) >= 0) && ((y + ho[i].y) <= 7))
                {
                    if (p_chb.board[y + ho[i].y, x + ho[i].x] == figura.fCount)
                    {
                        hods[hods.Length - 28 + extralen].act = Actions.aHod;
                        hods[hods.Length - 28 + extralen].fig_rubka = figura.fCount;

                        if (isWhiteTurn)
                            hods[hods.Length - 28 + extralen].fig_source = figura.fKoroljW;
                        else
                            hods[hods.Length - 28 + extralen].fig_source = figura.fKoroljB;

                        hods[hods.Length - 28 + extralen].from_pos.x = x;
                        hods[hods.Length - 28 + extralen].from_pos.y = y;

                        hods[hods.Length - 28 + extralen].to_pos.x = x + ho[i].x;
                        hods[hods.Length - 28 + extralen].to_pos.y = y + ho[i].y;
                        extralen++;
                    }
                    else
                        if (((isWhiteTurn) &&
                         (p_chb.board[y + ho[i].y, x + ho[i].x] >= figura.fPeshkaB) &&
                         (p_chb.board[y + ho[i].y, x + ho[i].x] < figura.fKoroljB)) ||

                        ((!isWhiteTurn) &&
                         (p_chb.board[y + ho[i].y, x + ho[i].x] >= figura.fPeshkaW) &&
                         (p_chb.board[y + ho[i].y, x + ho[i].x] < figura.fKoroljW)))
                        {

                            hods[hods.Length - 28 + extralen].act = Actions.aRubka;

                            if (isWhiteTurn)
                                hods[hods.Length - 28 + extralen].fig_source = figura.fKoroljW;
                            else
                                hods[hods.Length - 28 + extralen].fig_source = figura.fKoroljB;

                            hods[hods.Length - 28 + extralen].fig_rubka = p_chb.board[y + ho[i].y, x + ho[i].x];

                            hods[hods.Length - 28 + extralen].from_pos.x = x;
                            hods[hods.Length - 28 + extralen].from_pos.y = y;

                            hods[hods.Length - 28 + extralen].to_pos.x = x + ho[i].x;
                            hods[hods.Length - 28 + extralen].to_pos.y = y + ho[i].y;
                            extralen++;
                        }
                }

            #endregion
            */
        }

        /// <summary>
        /// список ходов сбивающих(шахующих) корол¤
        /// </summary>
        public bool CalculateKoroljIsChecked(bool isWhiteKorolj, ref turn[] hods)
        {
            Array.Resize<turn>(ref hods, hods.Length + 28);
            int extralen = 0;
            bool result = false;

            for (int i=0; i<8; i++)
                for (int j = 0; j < 8; j++)
                    if (((isWhiteKorolj) &&
                         (p_chb.board[j,i] >= figura.fPeshkaB) &&
                         (p_chb.board[j,i] < figura.fKoroljB)) ||

                        ((!isWhiteKorolj) &&
                         (p_chb.board[j,i] >= figura.fPeshkaW) &&
                         (p_chb.board[j, i] < figura.fKoroljW)))
                        {
                        /*
                            hods[hods.Length - 28 + extralen].act = Actions.aRubka;

                            hods[hods.Length - 28 + extralen].fig = p_chb.board[y + ho[i].y, x + ho[i].x];

                            hods[hods.Length - 28 + extralen].from_pos.x = x;
                            hods[hods.Length - 28 + extralen].from_pos.y = y;

                            hods[hods.Length - 28 + extralen].to_pos.x = x + ho[i].x;
                            hods[hods.Length - 28 + extralen].to_pos.y = y + ho[i].y;
                         */
                            extralen++;
                        }
                    


            Array.Resize<turn>(ref hods, hods.Length - 28 + extralen);
            return result;
        }

        /// <summary>
        /// make hod in chess
        /// </summary>
        public bool MakeHod(turn hod, bool backward)
        {            
            switch (hod.act)
            {
                case Actions.aRubkaProhodnajaPeshka:
                    {
                        //not impemented yet
                        return false;

                    }
                case Actions.aRubka:
                case Actions.aHodPeshka2:
                case Actions.aHod:
                    {                        
                        p_chb.board[hod.to_pos.y, hod.to_pos.x] = hod.fig_source;
                        p_chb.board[hod.from_pos.y, hod.from_pos.x] = figura.fCount;
                        return true;
                    }
                case Actions.aRokirovka:
                    {

                        p_chb.board[hod.from_pos.y, hod.from_pos.x] = figura.fCount;
                        p_chb.board[hod.to_pos.y, hod.to_pos.x] = figura.fCount;

                        //esli levaja rokirovka
                        if (hod.to_pos.x < hod.from_pos.x)
                        {
                            p_chb.board[hod.to_pos.y, 3] = hod.fig_rubka;//tura
                            p_chb.board[hod.from_pos.y, 2] = hod.fig_source;//korol
                        }
                        else
                        {
                            p_chb.board[hod.to_pos.y, 5] = hod.fig_rubka;//tura
                            p_chb.board[hod.from_pos.y, 6] = hod.fig_source;//korol
                        }

                        //esli rokirovka belych to..
                        if (hod.fig_source == figura.fKoroljW)
                        {

                            nKorolTuraUgeHodilW[uhKorol] = uhNeHodil + 1;
                            nKorolTuraUgeHodilW[uhTura0] = uhNeHodil + 1;
                            nKorolTuraUgeHodilW[uhTura7] = uhNeHodil + 1;                             
                        }
                        else
                        {
                            nKorolTuraUgeHodilB[uhKorol] = uhNeHodil + 1;
                            nKorolTuraUgeHodilB[uhTura0] = uhNeHodil + 1;
                            nKorolTuraUgeHodilB[uhTura7] = uhNeHodil + 1; 
                        }

                        return true;
                    }
            }
            return false;
        }





        internal void US112(turn xx)
        {


            //s11 = "";
            if (nShachW > 0) s11 += "Ўј’ Ѕ≈Ћџћ !!!";
            s11 += " ” Ѕ≈Ћќ√ќ  ќ–ќЋя ";
            s11 += HaveKorolTurnsW.ToString();
            s11 += " ’ќƒќ¬ ";
            if (nShachB > 0) s11 += "Ўј’ „≈–Ќџћ!!! ";
            s11 += " ” „≈–Ќќ√ќ  ќ–ќЋя ";
            s11 += HaveKorolTurnsB.ToString();
            s11 += " ’ќƒќ¬ ";

            

            string[] abc = { "a", "b", "c", "d", "e", "f", "g", "h" };
            string[] fig = { "бпешка", "бладь¤", "бконь", "бслон", "бферзь", "бкороль", "пешка", "ладь¤", "конь", "слон", "ферзь", "король", "пусто" };
            string[] act = { "ход", "рубим", "шах", "мат", "пат", "пешка на последней линии", "рубим проходную пешку", "ход пешки на 2 клетки", "рокировка" };

            int k;

                k = (int)(xx.fig_source);
                s11 += fig[k];

                s11 += " ";

                k = (int)(xx.act);
                s11 += act[k];

                if ((xx.act == Actions.aRubka) ||
                    (xx.act == Actions.aRubkaProhodnajaPeshka))
                {
                    s11 += " ";
                    k = (int)(xx.fig_rubka);
                    s11 += fig[k];
                }

                s11 += " ";
                k = xx.from_pos.x;

                s11 += abc[k];
                s11 += "";

                k = 8 - (xx.from_pos.y + 0);
                s11 += k.ToString();

                s11 += " - ";
                k = xx.to_pos.x;
                s11 += abc[k];
                s11 += "";
                k = 8 - (xx.to_pos.y + 0);
                s11 += k.ToString();
                s11 += "   ";
        }

        internal void swap(ref double x1, ref double x2)
        {
            double x;
            x=x1;x1=x2;x2=x;
        }

        public turn fake_turn;
        internal turn MakeDecision(bool isWhiteTurn, int recursia)
        {
            turn[] @x = yAxis;
            if (isWhiteTurn) x = xAxis;

            double[] estimation = new double[(int)figura.fCount+1];
            estimation[0] = Math.Sqrt(1 * 1 + 1 * 1);
            for (int i=1;i<6;i++)
            estimation[i] = Math.Sqrt(estimation[i-1] * estimation[i-1] + estimation[i-1] * estimation[i-1]);
            
            swap(ref estimation[(int)figura.fTuraW], ref estimation[(int)figura.fOfizerW]);
            swap(ref estimation[(int)figura.fKonjW], ref estimation[(int)figura.fOfizerW]);

            for (int i=0;i<6;i++) estimation[i+6] = estimation[i];
            estimation[(int)figura.fCount] = 0.0;

            s11 = "";
            s11 += " figura.fPeshkaW = ";
            s11 += estimation[(int)figura.fPeshkaW].ToString();

            s11 += " figura.fKonjW = ";
            s11 += estimation[(int)figura.fKonjW].ToString();

            s11 += " figura.fOfizerW = ";
            s11 += estimation[(int)figura.fOfizerW].ToString();

            s11 += " figura.fTuraW = ";
            s11 += estimation[(int)figura.fTuraW].ToString();

            s11 += " figura.fFerzjW = ";
            s11 += estimation[(int)figura.fFerzjW].ToString();

            s11 += " figura.fKoroljW = ";
            s11 += estimation[(int)figura.fKoroljW].ToString();

            int ind = -1;
            double e = -10000000;

            for (int i = 0; i < x.Length; i++)
            {
                x[i].estim=0.0;
                switch (x[i].act)
            {

                    case Actions.aHodPeshka2:
                    case Actions.aHod:
                    x[i].estim += (estimation[(int)figura.fKoroljW] - estimation[(int)x[i].fig_source])
                        /(estimation[(int)figura.fKoroljW]);
                    break;

                    case Actions.aRubkaProhodnajaPeshka:
                    case Actions.aRubka:
                        x[i].estim += estimation[(int)x[i].fig_rubka];
                        break;

                    case Actions.aRokirovka:
                        x[i].estim += estimation[(int)x[i].fig_source];
                        break;

                    case Actions.aMat:
                    case Actions.aPat:
                    case Actions.aShach:
                        x[i].estim += estimation[(int)x[i].fig_rubka];
                        break;

            }

            //  блок оценки ходов противника
            if (recursia>0)
            {
            gmmtrx gam2 = new gmmtrx(this);
            gam2.MakeHod(x[i],false);
            gam2.CalculateAllOfHods();
            turn sv = gam2.MakeDecision(!isWhiteTurn, recursia - 1);
            x[i].estim -= sv.estim;


            }


            if (x[i].estim > e) { ind = i; e=x[i].estim; }
            }


            if (ind >= 0)
            {
                //s11 += " выбран ход "; US112(x[ind]);                
                return x[ind];
            }
            
            fake_turn.act = Actions.aPeshkaOn8;
            fake_turn.from_pos.x = 0;
            fake_turn.from_pos.y = 0;
            fake_turn.to_pos.x = 0;
            fake_turn.to_pos.y = 0;
            fake_turn.estim = 1000.0;
            return fake_turn;

/*
            bool[] mask = new bool[yAxis.Length];
            int len = 0;
            gmmtrx gam2;
            //играем черными            
           //if (nShachB > 0)
            //{
                gam2 = new gmmtrx(this);
                for (int i = 0; i < gam2.yAxis.Length; i++)
                {                    
                    gam2.MakeHod(gam2.yAxis[i]);
                    gam2.CalculateAllOfHods();
                    if (gam2.nShachB <= 0) { mask[i] = false; len++; }
                    else { mask[i] = true; }

                    gam2 = new gmmtrx(this);
                }
                
            //}
                x = new turn[len];
                int k1=0;
                for (int i = 0; i < yAxis.Length; i++)
                    if (!mask[i]) { x[k1] = yAxis[i]; k1++; }

                if (len == 0) nShachB = 555;

            */

            /*
                gam2 = new gmmtrx(this);
                gmmtrx gam3 = new gmmtrx(this);
                for (int i = 0; i < gam2.xAxis.Length; i++)
                {
                    gam2.MakeHod(gam2.xAxis[i]);
                    gam2.CalculateAllOfHods();
                    if (gam2.nShachW <= 0) { mask[i] = false; len++; }
                    else { mask[i] = true; }

                    gam3 = new gmmtrx(gam2);
                    gam2 = gam3;
                }
            */

            for (int i = 0; i < x.Length; i++)
                if ((x[i].act == Actions.aRubka) ||
                    (x[i].act == Actions.aRubkaProhodnajaPeshka))
                    return x[i];

            for (int i = 0; i < x.Length; i++)
                if (x[i].act == Actions.aRokirovka)
                    return x[i];

            for (int i = 0; i < x.Length; i++)
                if ((x[i].act == Actions.aHod) ||
                    (x[i].act == Actions.aHodPeshka2)
                    )
                    return x[i];

            //если нет ходов возвращаем "стуб"- ход противника
            if (isWhiteTurn) x = yAxis;
            else x = xAxis;
            return x[0];
        }
    }


    public enum figura
    {
        fPeshkaW=0,
        fTuraW,
        fKonjW,
        fOfizerW,
        fFerzjW,
        fKoroljW,
        fPeshkaB,
        fTuraB,
        fKonjB,
        fOfizerB,
        fFerzjB,
        fKoroljB,
        fCount
    }

    public enum Actions
    {
        /// <summary>
        /// действи¤ в игре
        /// </summary>
        aHod,
        aRubka,
        aShach,
        aMat,
        aPat,
        aPeshkaOn8,
        aRubkaProhodnajaPeshka,
        aHodPeshka2,
        aRokirovka,
    }

    public class chess_board
    {
        /// <summary>
        /// keeps chessboard of the game
        /// </summary>
        public figura [,] board  = {               
            {figura.fTuraB,figura.fKonjB,figura.fOfizerB,figura.fFerzjB,figura.fKoroljB,figura.fOfizerB,figura.fKonjB,figura.fTuraB},
            {figura.fPeshkaB,figura.fPeshkaB,figura.fPeshkaB,figura.fPeshkaB,figura.fPeshkaB,figura.fPeshkaB,figura.fPeshkaB,figura.fPeshkaB},
            {figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount},
            {figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount},
            {figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount},
            {figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount,figura.fCount},
            {figura.fPeshkaW,figura.fPeshkaW,figura.fPeshkaW,figura.fPeshkaW,figura.fPeshkaW,figura.fPeshkaW,figura.fPeshkaW,figura.fPeshkaW},
            {figura.fTuraW,figura.fKonjW,figura.fOfizerW,figura.fFerzjW,figura.fKoroljW,figura.fOfizerW,figura.fKonjW,figura.fTuraW}
                        };
        public bool isWhiteBoardOrigin = true;
        public bool[,,] protected_fig = {
{ {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false} },
{ {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false} },
{ {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false} },
{ {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false} },
{ {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false} },
{ {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false} },
{ {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false} },
{ {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false}, {false,false} }
                                        };
        public void clear_protect_fig()
        {
            for (int k = 0; k < 2; k++)
            for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++) protected_fig[i, j, k] = false;
        }

        public void set_protected_fig(int i, int j, bool k, bool value)
        {
            if (k)
            protected_fig[i, j, 1] = value;
            else
            protected_fig[i, j, 0] = value;
        }

        public chess_board()
        {
            //isWhiteBoardOrigin = true;
           // board = new int[8, 8];
           // board[,]  

        // throw new System.NotImplementedException();
        }

        public chess_board(chess_board cb)
        {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        board[i, j] = cb.board[i, j];
                        for (int k = 0; k < 2; k++) protected_fig[i, j, k] = cb.protected_fig[i, j, k];
                        
                    }
                isWhiteBoardOrigin = cb.isWhiteBoardOrigin;
                
        }
    }

    public struct turn
    {
        /// <summary>
        /// тип срубываемой фигуры
        /// </summary>
        public figura fig_rubka;
        /// <summary>
        /// из клетки
        /// </summary>
        public point from_pos;
        /// <summary>
        /// в клетку
        /// </summary>
        public point to_pos;
        /// <summary>
        /// действие
        /// </summary>
        public Actions act;
        /// <summary>
        /// резервна¤ ¤чейка дл¤ информации о фигуре воздействи¤ (срубываема¤ фигура или мен¤ема¤ при пешка на 8)
        /// </summary>
        public figura reserved;
        /// <summary>
        /// тип исходной фигуры
        /// </summary>
        public figura fig_source;
        /// <summary>
        /// этим ходом обь¤вл¤етс¤ шах
        /// </summary>
        public bool isCheck;
        /// <summary>
        /// оценка хода
        /// </summary>
        public double estim;
    }

    public struct point
    {
        public point(int px, int py) { x = px; y = py; }
        public int x;
        public int y;
    }

    public class gmmtrxbuilding
    {
        /// <summary>
        /// пирамида ходов
        /// </summary>
        private gmmtrx pyramid;
    }

    public class htable
    {
        string make_sdid(chess_board x)
        {
            //
            byte[] k = new byte[13];
            string s = "";

            for (int f = 0; f < 13; f++)
            {
                k[f] = 0;
            for (int i = 0; i < 8; i++)     //vertical axis
                for (int j = 0; j < 8; j++) //horizon axis
                {
                    if (x.board[i, j] == (mychess2.figura)(f) ) k[f] += (byte)Math.Pow(2.0, (double)(j));
                }
            s += k[f];
            }
            
           return s;
        }

    }

}

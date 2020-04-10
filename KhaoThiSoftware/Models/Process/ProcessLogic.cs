using System;

namespace KhaoThiSoftware.Models.Process
{
    public class ProcessLogic
    {
        int[] arr; string[] arrReturn;
        public string[] GenerateBeatcode(int istart, int ifinish, int inumber)
        {
            int iPhanTu = ifinish - istart;
            arr = new int[iPhanTu];
            for (int i = 0; i < iPhanTu; i++)
            {
                arr[i] = istart;
                istart++;
            }
            arr = SortRandomArray(SortRandomArray(arr));
            arrReturn = new string[iPhanTu];
            for (int i = 0; i < arr.Length; i++)
            {
                arrReturn[i] = AddZero(arr[i], inumber);
            }
            return arrReturn;
        }
        //xu ly do dai so phach theo yeu cau = inumber
        //iinput: so nguyen ma phach dau vao
        //inumber: do dai ma phach dau ra
        private string AddZero(int iinput, int inumber)
        {
            string sinput = "";
            int countzero = inumber - iinput.ToString().Length;
            for (int i = 0; i < countzero; i++)
            {
                sinput += "0";
            }
            sinput += iinput;
            return sinput;
        }
        //sap xep ngau nhien mang dau vao
        private int[] SortRandomArray(int[] arrinput)
        {
            int itemp;
            int arrlength = arrinput.Length;
            for (int i = 0; i < arrlength; i++)
            {
                Random rd = new Random();
                int iindex = rd.Next(i, arrlength - 1);
                itemp = arrinput[iindex];
                arrinput[iindex] = arrinput[i];
                arrinput[i] = itemp;
            }
            return arrinput;
        }
    }
}
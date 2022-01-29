using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGraphWinForms
{
    class HelpClassel
    {
        public static void MoveVector(ref int[] vector, int points)
        {
            if (points < 0)
                throw new Exception("Задано неверное значение смещения вектора!");
            for (int i = 0; i < points; i++)
            {
                for (int j = vector.Length; j >= 0; j--)
                {
                    try
                    {
                        int tmp = vector[j];
                        vector[j] = -1;
                        vector[j + 1] = tmp;
                    }
                    catch { }
                }
            }
        }
    }

    public delegate void VoidDelegate();
}

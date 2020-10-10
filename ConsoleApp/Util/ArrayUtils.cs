namespace Util
{
    public class ArrayUtils
    {
        public static T[][] ConvertToJagged<T>(T[,] multiArray)
        {
            var fin = new T[multiArray.GetLength(0)][];
            for (var i = 0; i < multiArray.GetLength(0); i++)
            {
                fin[i] = new T[multiArray.GetLength(1)];
                for (var j = 0; j < multiArray.GetLength(1); j++)
                    fin[i][j] = multiArray[i, j];
            }
            return fin;
        }
    }
}
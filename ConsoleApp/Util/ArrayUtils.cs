namespace Util
{
    public class ArrayUtils
    {
        public static T[][] ConvertToJagged<T>(T[,] multiArray)
        {
            var result = new T[multiArray.GetLength(0)][];
            for (var i = 0; i < multiArray.GetLength(0); i++)
            {
                result[i] = new T[multiArray.GetLength(1)];
                for (var j = 0; j < multiArray.GetLength(1); j++)
                    result[i][j] = multiArray[i, j];
            }
            return result;
        }
        
        public static T[,] ConvertTo2D<T>(T[][] source)
        {
            T[,] result = new T[source.Length, source[0].Length];
 
            for (int i = 0; i < source.Length; i++)
            {
                for (int k = 0; k < source[0].Length; k++)
                {
                    result[i, k] = source[i][k];
                }
            }
 
            return result;
        }
    }
}
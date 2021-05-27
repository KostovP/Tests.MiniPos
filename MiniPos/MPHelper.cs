using System;

namespace MiniPos
{
    public enum AlignmentH
    {
        Left,
        Center,
        Right
    }

    public enum DocInputMode
    {
        LineName = 1,
        LineQty = 2,
        LinePrc = 3,
        LineDiscount = 4,
        LineTotal = 5
    }

    public static class MPHelper
    {
        public const char DOC_SEP = '=';
        public const char DOC_SECTION_SEP = '-';

        public const int PREC_TOTAL_DOC_LINE = 5;
        public const int PREC_TOTAL_DOC = 2;
        public const string FMT_TOTAL = "0.00";
        public const string FMT_QTY = "0";
        public const string FMT_PRC = "0.00000";
        public const string FMT_DISCOUNT = FMT_TOTAL;
        public const string FMT_DOC_DATE = "dd.MM.yy";

        public const string DEF_CLIENT_NAME = "JOHN DOE";

        private const int TABLE_WIDTH = 73;
        private const char CELL_SEPARATOR = '|';
        private const string ELLIPSIS = "...";

        public static void ClearCurrentConsoleLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void PrintLine(char sep)
        {
            Console.WriteLine(new string(sep, TABLE_WIDTH));
        }

        public static void PrintRow(AlignmentH al, int offset, params string[] columns)
        {
            if (offset != 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop - (offset + 1));
            }

            int width = (TABLE_WIDTH - columns.Length) / columns.Length;
            string row = CELL_SEPARATOR.ToString();

            foreach (string column in columns)
            {
                row += Align(column, width, al) + CELL_SEPARATOR;
            }

            Console.WriteLine(row);

            if (offset != 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop + offset);
            }
        }

        private static string Align(string text, int width, AlignmentH al)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + ELLIPSIS : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }

            return al switch
            {
                AlignmentH.Left => text.PadRight(width - 1),
                AlignmentH.Right => text.PadLeft(width - 1),
                _ => text.PadRight(width - (width - text.Length) / 2).PadLeft(width),
            };
        }


    }
}

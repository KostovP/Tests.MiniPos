using System;
using System.Text.Json;

// todos
// extract all human labels, errors, etc as resource strings
// transform the decimal inputs with something like,
//     var sep = FormatSettings.DecimalSeparator;
//     decval = input.Replace(',' sep).Replace('.', sep);
// align the visualization of int/dec to the right / string - left
// show the doc totals

namespace MiniPos
{
    class Program
    {
        static int currDocNo = 0;
        static DocInputMode im;
        static readonly MPDoc doc = new();
        static MPLine line;

        static void Main()
        {
            ConsoleKey[] fnkeys = {ConsoleKey.F4, ConsoleKey.Enter};            

            PrintAppInfo();
            
            string errmsg;
            
            while (true)  // main loop
            {
                currDocNo++;

                // doc header
                // #
                MPHelper.PrintLine(MPHelper.DOC_SEP);
                MPHelper.PrintRow(AlignmentH.Left, 0, string.Format("Document #{0}", currDocNo));

                // Date
                Console.Write("> Date (defaults to today): ");
                doc.DT = Console.ReadLine();                 
                MPHelper.ClearCurrentConsoleLine();
                MPHelper.PrintRow(AlignmentH.Left, 0, string.Format("Date: {0}", doc.DT));

                // Client 
                Console.Write("> Client: ");
                doc.Contractor = Console.ReadLine();
                MPHelper.ClearCurrentConsoleLine();
                MPHelper.PrintRow(AlignmentH.Left, 0, string.Format("Client: {0}", doc.Contractor));

                // doc details
                // details header
                MPHelper.PrintLine(MPHelper.DOC_SECTION_SEP);
                MPHelper.PrintRow(AlignmentH.Center, 0, "Name", "Qty", "Prc", "Discount (%)", "Total");
                MPHelper.PrintLine(MPHelper.DOC_SECTION_SEP);

                CreateEmptyDocLine();              
                errmsg = "";

                while (true) // lines loop
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(errmsg))
                        {
                            Console.ForegroundColor = ConsoleColor.Red; 
                        }

                        switch (im)
                        {
                            case DocInputMode.LineName:
                                Console.Write(string.Format("> Product name{0}: ", errmsg));
                                line.Name = Console.ReadLine();
                                break;
                            case DocInputMode.LineQty:
                                Console.Write(string.Format("> Quantity (default 1){0}: ", errmsg));
                                line.Qty = Console.ReadLine();
                                break;
                            case DocInputMode.LinePrc:
                                Console.Write(string.Format("> Unit price (default gift){0}: ", errmsg));
                                line.Prc = Console.ReadLine();
                                break;
                            case DocInputMode.LineDiscount:
                                Console.Write(string.Format("> Discount % (default none){0}: ", errmsg));
                                line.Discount = Console.ReadLine();                                
                                break;
                            default:
                                break;
                        }

                      Console.ResetColor();
                      errmsg = "";                      
                      im++;

                    }
                    catch (Exception e)
                    {                        
                        errmsg = " " + e.Message;  // pad with a whitespace                          
                        continue;
                    }
                    finally
                    {
                        MPHelper.ClearCurrentConsoleLine();
                    }

                    // todo: optimize. don't print the whole row on each iteration, but only the updated cell
                    MPHelper.PrintRow(
                        AlignmentH.Center,
                        1,
                        line.Name, 
                        line.Qty, 
                        line.Prc, 
                        line.Discount,
                        line.TotalTarget.ToString(MPHelper.FMT_TOTAL)
                    );

                    if (im == DocInputMode.LineTotal)
                    {                        
                        Console.Write("> Press ENTER for a new line or F4 for save");

                        ConsoleKeyInfo kc; // we want to process only the poi fnkeys
                        do
                        {
                            kc = Console.ReadKey(true);
                        } while (!Array.Exists(fnkeys, key => key == kc.Key));


                        if (kc.Key == ConsoleKey.Enter)
                        {
                            MPHelper.ClearCurrentConsoleLine();
                            CreateEmptyDocLine();
                            continue; // will add a new line
                        }

                        if (kc.Key == ConsoleKey.F4)
                        {
                            MPHelper.ClearCurrentConsoleLine();
                            MPHelper.PrintLine(MPHelper.DOC_SECTION_SEP);

                            string jsonString = JsonSerializer.Serialize(doc);
                            doc.Lines.Clear();
                            Console.WriteLine(jsonString);
                            MPHelper.PrintLine(MPHelper.DOC_SEP);

                            break; // breaks the lines loop
                        }
                    }
                }
            }
        }

        private static void PrintAppInfo()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Hello (world)!");
            Console.WriteLine("How to use this application ? ");
            Console.WriteLine("    [RETURN] - populate document properties and move to the next ones");            
            Console.WriteLine("    [F4] - save the current document");
            Console.WriteLine("Notes: ");
            Console.WriteLine("    - when populating decimal types, you have to respect your system decimal separator; ");
            Console.WriteLine("    - when populating date type, you have to respect your system date format (eg 01.12.2020); ");
            Console.ResetColor();
        }

        private static void CreateEmptyDocLine()
        {
            line = doc.LineAdd();
            im = DocInputMode.LineName;
            MPHelper.PrintRow(AlignmentH.Center, 0, "", "", "", "", "");
            MPHelper.PrintLine(MPHelper.DOC_SECTION_SEP);
        }
    }
}

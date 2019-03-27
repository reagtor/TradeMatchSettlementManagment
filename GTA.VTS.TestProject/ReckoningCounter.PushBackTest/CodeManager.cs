#region Using Namespace

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using GTA.VTS.Common.CommonUtility;

#endregion

namespace ReckoningCounter.PushBackTest
{
    public class CodeManager
    {
        public static List<Commodity> CommodityList = new List<Commodity>();
        private static XDocument doc;
        private static string file = "config\\code.xml";

        private static List<Keys> keyList;

        //<Item>
        //  <Stockno Type="String">704096</Stockno>
        //  <Name Type="String">0096配债</Name>
        //  <PY Type="String">0096PZ</PY>
        //  <MarketType Type="String">SH</MarketType>
        //</Item>
        public static bool Load()
        {
            try
            {
                doc = XDocument.Load(file);

                var q = from p in doc.Root.Elements("Item")
                        select p;

                foreach (var e in q)
                {
                    string code = e.Element("Stockno").Value;
                    string name = e.Element("Name").Value;
                    string py = e.Element("PY").Value;
                    string market = e.Element("MarketType").Value;

                    Commodity commodity = new Commodity();
                    commodity.Code = code;
                    commodity.Name = name;
                    commodity.PY = py.ToUpper();
                    commodity.Market = market;

                    if (commodity.Market.Trim().ToUpper() == "QH")
                        continue;

                    CommodityList.Add(commodity);
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            CodeComparer comparer = new CodeComparer();
            CommodityList.Sort(comparer);

            InitializeKeys();

            return CommodityList.Count > 0;
        }

        private static void InitializeKeys()
        {
            keyList = new List<Keys>();
            keyList.Add(Keys.A);
            keyList.Add(Keys.B);
            keyList.Add(Keys.C);
            keyList.Add(Keys.D);
            keyList.Add(Keys.E);
            keyList.Add(Keys.F);
            keyList.Add(Keys.G);
            keyList.Add(Keys.H);
            keyList.Add(Keys.I);
            keyList.Add(Keys.J);
            keyList.Add(Keys.K);
            keyList.Add(Keys.L);
            keyList.Add(Keys.M);
            keyList.Add(Keys.N);
            keyList.Add(Keys.O);
            keyList.Add(Keys.P);
            keyList.Add(Keys.Q);
            keyList.Add(Keys.R);
            keyList.Add(Keys.S);
            keyList.Add(Keys.T);
            keyList.Add(Keys.U);
            keyList.Add(Keys.V);
            keyList.Add(Keys.W);
            keyList.Add(Keys.X);
            keyList.Add(Keys.Y);
            keyList.Add(Keys.Z);

            keyList.Add(Keys.D0);
            keyList.Add(Keys.D1);
            keyList.Add(Keys.D2);
            keyList.Add(Keys.D3);
            keyList.Add(Keys.D4);
            keyList.Add(Keys.D5);
            keyList.Add(Keys.D6);
            keyList.Add(Keys.D7);
            keyList.Add(Keys.D8);
            keyList.Add(Keys.D9);

            keyList.Add(Keys.NumPad0);
            keyList.Add(Keys.NumPad1);
            keyList.Add(Keys.NumPad2);
            keyList.Add(Keys.NumPad3);
            keyList.Add(Keys.NumPad4);
            keyList.Add(Keys.NumPad5);
            keyList.Add(Keys.NumPad6);
            keyList.Add(Keys.NumPad7);
            keyList.Add(Keys.NumPad8);
            keyList.Add(Keys.NumPad9);

            //keyList.Add(Keys.Back);
        }

        public static void FillList(ListBox listBox)
        {
            foreach (Commodity commodity in CommodityList)
            {
                listBox.Items.Add(commodity.ToString());
            }
        }

        public static void FillList(ListBox listBox, string txt)
        {
            if (txt.Trim().Length == 0)
            {
                FillList(listBox);
                return;
            }

            txt = txt.ToUpper();

            var q = from c in CommodityList
                    where c.Code.StartsWith(txt) || c.PY.StartsWith(txt)
                    select c;

            listBox.Items.Clear();
            listBox.SuspendLayout();
            foreach (Commodity commodity in q)
            {
                listBox.Items.Add(commodity.ToString());
            }
            listBox.ResumeLayout();
        }

        public static bool Isvalidkey(Keys keycode)
        {
            if (keyList.Contains(keycode))
                return true;

            return false;
        }
    }

    public class Commodity
    {
        public string Code;
        public string Market;
        public string Name;
        public string PY;

        public override string ToString()
        {
            return Code + "    " + Name;
        }
    }

    public class CodeComparer : IComparer<Commodity>
    {
        public int Compare(Commodity xc, Commodity yc)
        {
            string x = xc.Code;
            string y = yc.Code;

            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    int retval = x.Length.CompareTo(y.Length);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        //
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // sort them with ordinary string comparison.
                        //
                        return x.CompareTo(y);
                    }
                }
            }
        }
    }

}
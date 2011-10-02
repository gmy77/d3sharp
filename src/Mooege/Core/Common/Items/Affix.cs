using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common;


namespace Mooege.Core.Common.Items
{
    public class Affix
    {

        public static readonly Logger Logger = LogManager.CreateLogger();

        public int AffixGbid { get; set; }
  
        public Affix(int gbid)
        {
            AffixGbid = gbid;         
        }

        public String ToString()
        {
            return String.Format("{0}",AffixGbid);
        }

        public static Affix Parse(String affixString)
        {
            try
            {              
                int gbid = int.Parse(affixString);    
                Affix affix = new Affix(gbid);
                return affix;
            }catch(Exception e)
            {
                throw new Exception( "Affix could't parsed of String: "+ affixString,e);
            }
        }

    }
}

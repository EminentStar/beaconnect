using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvvm
{
    public class GlobalVariables
    {
        private static GlobalVariables instance;
        private string firemanName;
        private string firemanNumber;

        public static GlobalVariables Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalVariables();
                }
                return instance;
            }
        }

        private GlobalVariables()
        {

        }
        

        public string FiremanName { get; set; }
        public string FiremanNumber { get; set; }
        
    }
}
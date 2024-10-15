using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_POE_PART_2.Classes
{
    public class Validations
    {
        //**********************************************************NAKA****************************************************************//
        // A method to validate the location
        public bool ValidateLocation(string location)
        {
            if ((string.IsNullOrEmpty(location)) || (string.IsNullOrWhiteSpace(location)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //**********************************************************NAKA****************************************************************//
        //A method to validate the category
        public bool ValidateCategory(string category)
        {
            if ((string.IsNullOrEmpty(category)) || (string.IsNullOrWhiteSpace(category)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //**********************************************************NAKA****************************************************************//
        //A method to validate the description
        public bool ValidateDescription(string description)
        {
            if ((string.IsNullOrEmpty(description)) || (string.IsNullOrWhiteSpace(description)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

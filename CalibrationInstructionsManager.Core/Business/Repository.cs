using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalibrationInstructionsManager.Core.Models;

namespace CalibrationInstructionsManager.Core.Business
{
    public class Repository : IRepository
    {
        #region Properties

        private int[] id = new int[] { 0001, 0002, 0003, 0004, 0005 };

        private string[] fullName = new string[]
        {
            "[0,88222;1,30893] +- abs. value; [0,1 % 0,05] max. value; [0,05 % 0,02] rel. value (to nom. range);",
            "[1,88222;2,30893] +- abs. value; [0,2 % 0,05] max. value; [0,06 % 0,02] rel. value (to nom. range);",
            "[2,88222;3,30893] +- abs. value; [0,3 % 0,05] max. value; [0,07 % 0,02] rel. value (to nom. range);",
            "[3,88222;4,30893] +- abs. value; [0,4 % 0,05] max. value; [0,08 % 0,02] rel. value (to nom. range);",
            "[4,88222;5,30893] +- abs. value; [0,5 % 0,05] max. value; [0,09 % 0,02] rel. value (to nom. range);"
        };

        private string[] commentary = new string[]
        {
            "Measures abs. value",
            "Measures max. value",
            "Measures rel. value",
            "Measures abs. and max. value",
            "Measures rel. and abs. value"
        };

        private string[] properties = new string[]
        {
            "abs. value properties",
            "max. value properties",
            "rel. value properties",
            "noise properties",
            "p-p noise properties"
        };

        private string[] propertiesDefaultValue = new string[] { "0,1", "0,2", "0,3", "0,4", "0,5" };

        private string[] propertiesDefaultType = new string[]
        {
            "abs. value",
            "max. value",
            "rel. value",
            "abs. rms noise",
            "abs. p-p noise"
        };
        #endregion // Properties

        #region Methods
        
        public List<MeasurementPoint> GetMeasurementPoints(int total)
        {
            List<MeasurementPoint> output = new List<MeasurementPoint>();
        
            for (int i = 0; i < total; i++)
            {
                output.Add(GetMeasurementPoint(i));
            }
        
            return output;
        }
        
        public MeasurementPoint GetMeasurementPoint(int id)
        {
            MeasurementPoint outputMeasurementPoint = new MeasurementPoint();
            outputMeasurementPoint.Id = id;
            outputMeasurementPoint.FullName = fullName[id];
            outputMeasurementPoint.Commentary = commentary[id];
            outputMeasurementPoint.Properties = properties[id];
            outputMeasurementPoint.PropertiesDefaultValue = propertiesDefaultValue[id];
            outputMeasurementPoint.PropertiesDefaultType = propertiesDefaultType[id];
        
            return outputMeasurementPoint;
        }
        
        public List<string> GetPropertiesList()
        {
            return properties.ToList();
        }
        
        #endregion // Methods
    }
}

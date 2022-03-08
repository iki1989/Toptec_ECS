using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.LabelPrinter
{
    public enum LabelErrorEnum
    {
        AdsorptionError = 1,    //흡착 에러
        Seobo = 2,              //서보 에러
        Paper = 3,              //용지 에러
        Ribbon = 4,             //리본 에러
        SeoboOrigin = 5,        //서보원점에러
        PrinterError = 6,       //프린터 에러

        Unknown = 99,
    }
}

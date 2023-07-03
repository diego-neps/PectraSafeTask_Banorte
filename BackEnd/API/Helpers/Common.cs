using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PectraForms.WebApplication.BackEnd.API.Helpers
{
    public class Common
    {
        public static string MakeValidIdentifierName(string name)
        { //Compliant with item 2.4.2 of the C# specification
            string ret = string.Empty;
            if (name != null)
            {
                Regex regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]");
                ret = regex.Replace(name, "");
                //The identifier must start with a character 
                if (!char.IsLetter(ret, 0))
                    ret = string.Concat("", ret);
                //Tests whether the identifier conflicts with any reserved or language keywords, and if so, 
                //returns an equivalent name with language-specific escape code formatting
                ret = Microsoft.CSharp.CSharpCodeProvider.CreateProvider("C#").CreateEscapedIdentifier(ret);
            }
            return ret;
        }
    }

    /// <summary>
    /// Constantes 
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Devuelve la cultura por defecto.
        /// </summary>
        public static readonly System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo("es-ES");
    }
}
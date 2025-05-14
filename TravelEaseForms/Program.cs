using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelEaseForms.Forms;

namespace TravelEaseForms { 
    public static class UserSession
    {
        public static string CurrentUserID { get; set; }
        public static string CurrentUserEmail { get; set; }
        public static string CurrentUserName { get; set; }
        
    }

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new SP_Login());
        }
    }
}

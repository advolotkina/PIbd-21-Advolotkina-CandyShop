﻿using System;
using System.Windows.Forms;

namespace CandyShopView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ClientAPI.Connect();
            Console.WriteLine("afsafa");
            MailClient.CheckMail();
            Console.WriteLine("AAAAAAAAA");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PasswordKeeper
{
    public partial class Form2 : Form
    {
        public string f2_site, f2_userLogin, f2_userPass, f2_userName, f2_userPhone;
        static string cyrillic = "йцукенгшщзхъэждлорпавыфячсмитьбю";


        public Form2()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            textBox3.UseSystemPasswordChar = true;
        }

        public void SetTextBox()
        {
            textBox1.Text = f2_site;
            textBox2.Text = f2_userLogin;
            textBox3.Text = f2_userPass;
            textBox4.Text = f2_userName;
            maskedTextBox1.Text = f2_userPhone;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool site_res = Uri.IsWellFormedUriString(textBox1.Text, UriKind.Absolute);
            if (site_res == false)
            {
                MessageBox.Show("Поле сайта не является URL!", "Внимание");
                return;
            }
            f2_site = textBox1.Text;
            f2_userLogin = textBox2.Text;
            f2_userPass = textBox3.Text;
            f2_userPhone = maskedTextBox1.Text;
            if (f2_site == "" || f2_userLogin == "" || f2_userPass == "")
            {
                MessageBox.Show("Вы не ввели сайт, логин и пароль для добавления пароля!", "Внимание");
                return;
            }

            if ((f2_userPhone.Length < 10) && (f2_userPhone.Length > 0))
            {
                MessageBox.Show("Вы не ввели полный номер телефона!", "Внимание");
                return;
            }

            if(textBox4.Text.Length >= 0 &&  !checkIfFullName(textBox4.Text, out f2_userName))
            {
                MessageBox.Show("Вы не правильно ввели ФИО!", "Внимание");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool checkIfFullName(string FullName, out string FullNameNew)
        {
            if (FullName == "")
            {
                FullNameNew = "";
                return true;
            }
            else
            {
                var words = Regex.Split(FullName, @"\s{1,}");
                FullNameNew = "";
                int wordCount = 0;
                for (int i = 0; i < words.Length; ++i)
                {
                    if (words.Length < 0) continue;
                    string word = words[i].ToLower();
                    int? firstNotOf = word.Select((x, j) => new { Val = x, Idx = (int?)j })
                        .Where(x => cyrillic.IndexOf(x.Val) == -1)
                        .Select(x => x.Idx)
                        .FirstOrDefault();
                    ++wordCount;
                    if (firstNotOf != null || wordCount > 3 || word == "")
                    {
                        FullNameNew = "";
                        return false;
                    }
                    StringBuilder wordSB = new StringBuilder(word);
                    wordSB[0] = char.ToUpper(wordSB[0]);
                    word = wordSB.ToString();
                    FullNameNew = FullNameNew + " " + word;
                }
                FullNameNew = FullNameNew.Trim();
                return true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox3.UseSystemPasswordChar = false;
            }
            else
            {
                textBox3.UseSystemPasswordChar = true;
            }
        }
    }
}

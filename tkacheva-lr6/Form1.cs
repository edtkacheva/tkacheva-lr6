﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace tkacheva_lr6
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ClassStudent
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool baseclass;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string lastname;

        [MarshalAs(UnmanagedType.I4)]
        public int age;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string group_name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string email;
    }

    public partial class Form1 : Form
    {
        [DllImport(@"C:\Users\katuha\Documents\files\study\5sem\tkacheva-lr6\Dll\x64\Debug\Dll.dll", CharSet = CharSet.Ansi)]
        static extern void GetClassStudent(ref ClassStudent s, int i);

        [DllImport(@"C:\Users\katuha\Documents\files\study\5sem\tkacheva-lr6\Dll\x64\Debug\Dll.dll", CharSet = CharSet.Ansi)]
        static extern void SetClassStudent(ref ClassStudent s, int i);

        [DllImport(@"C:\Users\katuha\Documents\files\study\5sem\tkacheva-lr6\Dll\x64\Debug\Dll.dll", CharSet = CharSet.Ansi)]
        static extern int GetGroupSize();

        [DllImport(@"C:\Users\katuha\Documents\files\study\5sem\tkacheva-lr6\Dll\x64\Debug\Dll.dll", CharSet = CharSet.Ansi)]
        static extern void Erase(int i);

        [DllImport(@"C:\Users\katuha\Documents\files\study\5sem\tkacheva-lr6\Dll\x64\Debug\Dll.dll", CharSet = CharSet.Ansi)]
        static extern void AddStudent();

        [DllImport(@"C:\Users\katuha\Documents\files\study\5sem\tkacheva-lr6\Dll\x64\Debug\Dll.dll", CharSet = CharSet.Ansi)]
        static extern void AddHeadman();

        [DllImport(@"C:\Users\katuha\Documents\files\study\5sem\tkacheva-lr6\Dll\x64\Debug\Dll.dll", CharSet = CharSet.Ansi)]
        static extern void SaveData(StringBuilder filename);

        [DllImport(@"C:\Users\katuha\Documents\files\study\5sem\tkacheva-lr6\Dll\x64\Debug\Dll.dll", CharSet = CharSet.Ansi)]
        static extern void LoadData(StringBuilder filename);

        ClassStudent student;
        int currentIndex;

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateListBox()
        {
            group.Items.Clear();
            int group_size = GetGroupSize();

            for (int i = 0; i < group_size; i++)
            {
                GetClassStudent(ref student, i);
                group.Items.Add(student.lastname);
            }
        }

        private void ClearTable()
        {
            name.Text = String.Empty;
            lastname.Text = String.Empty;
            age.Text = String.Empty;
            group_name.Text = String.Empty;
            email.Text = String.Empty;
        }

        private void group_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentIndex = group.SelectedIndex;
            if (currentIndex == -1)
            {
                ClearTable();
                return;
            }
            GetClassStudent(ref student, currentIndex);

            name.Text = student.name;
            lastname.Text = student.lastname;
            age.Text = Convert.ToString(student.age);

            if (student.baseclass)
            {
                group_name.Text = "xxx";
                group_name.Enabled = false;
                email.Text = "xxxx";
                email.Enabled = false;
            }
            else
            {
                group_name.Enabled = true;
                group_name.Text = student.group_name;
                email.Enabled = true;
                email.Text = student.email;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddStudent();
            UpdateListBox();
            group.SelectedIndex = group.Items.Count - 1;
            currentIndex = group.SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddHeadman();
            UpdateListBox();
            group.SelectedIndex = group.Items.Count - 1;
            currentIndex = group.SelectedIndex;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            currentIndex = group.SelectedIndex;
            int tmp = currentIndex;
            if (currentIndex == -1)
                return;
            Erase(currentIndex);
            group.Items.RemoveAt(currentIndex);
            currentIndex = tmp;
            if (group.Items.Count == 0)
            {
                currentIndex = -1;
                return;
            }
            if (currentIndex == group.Items.Count)
            {
                group.SelectedIndex = group.Items.Count - 1;
                currentIndex = group.SelectedIndex;
            }
            else
            {
                group.SelectedIndex = currentIndex;
                currentIndex = group.SelectedIndex;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\Users\katuha\Documents\files\study\5sem\oop_files";
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder fileName = new StringBuilder(openFileDialog.FileName);

                    LoadData(fileName);
                    UpdateListBox();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = @"C:\Users\katuha\Documents\files\study\5sem\oop_files";
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder fileName = new StringBuilder(saveFileDialog.FileName);
                    SaveData(fileName);
                }
            }
        }

        private void name_TextChanged(object sender, EventArgs e)
        {
            student.name = name.Text;
            UpdateListBox();
        }

        private void lastname_TextChanged(object sender, EventArgs e)
        {
            student.lastname = lastname.Text;
            UpdateListBox();
        }

        private void age_TextChanged(object sender, EventArgs e)
        {
            try
            {
                student.age = (int)Convert.ToUInt32(age.Text);
            }
            catch
            {
                student.age = 0;
            }
            UpdateListBox();
        }

        private void group_name_TextChanged(object sender, EventArgs e)
        {
            student.group_name = group_name.Text;
            UpdateListBox();
        }

        private void email_TextChanged(object sender, EventArgs e)
        {
            student.email = email.Text;
            UpdateListBox();
        }
    }
}

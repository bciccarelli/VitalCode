// Decompiled with JetBrains decompiler
// Type: mainForm
// Assembly: Vital, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 844C30AE-F97D-44F8-8E93-9FC88F351A41
// Assembly location: C:\Users\Alumi\Programming\OldWebsite\public\Vital\vital.exe

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public class mainForm : Form
{
    private static Timer updateTimer = new Timer();
    public string textPrefs = "{\\rtf1\\ansi\\deff0{\\colortbl;\\red255\\green255\\blue255;\\red0\\green0\\blue255;\\red0\\green255\\blue255;\\red0\\green255\\blue0;\\red255\\green0\\blue255;\\red255\\green0\\blue0;\\red255\\green255\\blue0;}{\\fonttbl{\\f0\\fscript Helvetica;\\fs24}}\\cf1";
    private Button runButton;
    private Button openButton;
    private Button saveButton;
    private RichTextBox mainCodeEditor;
    private RichTextBox fileField;

    public mainForm()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.setupRunButton();
        this.setupSaveButton();
        this.setupOpenButton();
        this.setupEditBox();
        this.setupFileField();
        this.PerformLayout();
    }

    private void updateSyntax()
    {
        this.syntax(this.mainCodeEditor.Text);
    }

    private string openFile(string filePath)
    {
        if (File.Exists(filePath))
        return File.ReadAllText(filePath);
        return "File does not exist";
    }

    private void setupOpenButton()
    {
        this.openButton = new Button();
        this.openButton.Size = new Size(100, 40);
        this.openButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.openButton.Location = new Point(0, 100);
        this.openButton.Text = "Open";
        this.Controls.Add((Control) this.openButton);
        this.openButton.Click += new EventHandler(this.openButton_Click);
    }

    private void setupSaveButton()
    {
        this.saveButton = new Button();
        this.saveButton.Size = new Size(100, 40);
        this.saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.saveButton.Location = new Point(0, 50);
        this.saveButton.Text = "Save";
        this.Controls.Add((Control) this.saveButton);
        this.saveButton.Click += new EventHandler(this.saveButton_Click);
    }

    private void setupRunButton()
    {
        this.runButton = new Button();
        this.runButton.Size = new Size(100, 40);
        this.runButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.runButton.Location = new Point(0, 0);
        this.runButton.Text = "Run";
        this.Controls.Add((Control) this.runButton);
        this.runButton.Click += new EventHandler(this.runButton_Click);
    }

    private void setupFileField()
    {
        this.fileField = new RichTextBox();
        this.SuspendLayout();
        this.fileField.Size = new Size(1000, 24);
        this.fileField.Padding = new System.Windows.Forms.Padding(100,10,0,0);
        this.fileField.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.fileField.AcceptsTab = false;
        this.fileField.Dock = DockStyle.Top;
        this.fileField.Multiline = false;
        this.Controls.Add((Control) this.fileField);
        this.fileField.Rtf = this.textPrefs + "new.py";
        this.fileField.BackColor = Color.FromArgb(0, 0, 100);
        this.ResumeLayout(false);
    }

    private void setupEditBox()
    {

        this.mainCodeEditor = new RichTextBox();
        this.SuspendLayout();
        this.mainCodeEditor.Size = new Size(1000, 800);
        this.mainCodeEditor.Padding = new System.Windows.Forms.Padding(5);
        this.mainCodeEditor.AcceptsTab = true;
        this.mainCodeEditor.Dock = DockStyle.Fill;
        this.mainCodeEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.mainCodeEditor.Multiline = true;
        this.mainCodeEditor.ScrollBars = RichTextBoxScrollBars.Vertical;
        this.Controls.Add((Control) this.mainCodeEditor);
        this.mainCodeEditor.Rtf = this.textPrefs;
        this.mainCodeEditor.BackColor = Color.FromArgb(0, 0, 0);
        this.mainCodeEditor.KeyPress += new KeyPressEventHandler(this.textBoxInput);
        this.ResumeLayout(false);
    }

    private void textBoxInput(object sender, KeyPressEventArgs e)
    {
        this.updateSyntax();
    }

    private void debugAlert(string toAlert)
    {
        int num = (int) MessageBox.Show(toAlert);
    }

    private void syntax(string text)
    {
        int selectionStart = this.mainCodeEditor.SelectionStart;
        int selectionLength = this.mainCodeEditor.SelectionLength;
        int i;
        string ending = "~";
        if ((i = this.fileField.Text.IndexOf("."))>0)
        {
            ending = this.fileField.Text.Substring(i).ToLower();
        }
        string str = ending == ".py" ? Regex.Replace(text, "\n", "\\line\\cf1").Replace(".", "\\cf2.").Replace("def", "\\cf3def \\cf4").Replace("print", "\\cf3print\\cf4").Replace("==", "\\cf3 == \\cf4").Replace("=", "\\cf3=") : Regex.Replace(text, "\n", "\\line\\cf1");
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(this.textPrefs);
        stringBuilder.Append(str);
        stringBuilder.Append("\\line}");
        this.mainCodeEditor.Rtf = stringBuilder.ToString();
        this.mainCodeEditor.SelectionStart = selectionStart;
        this.mainCodeEditor.SelectionLength = selectionLength;
    }

    private void save()
    {
        File.WriteAllText(this.fileField.Text, this.mainCodeEditor.Text);
        
    }

    private void runButton_Click(object sender, EventArgs e)
    {
        this.build();
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
        this.save();
    }

    private void openButton_Click(object sender, EventArgs e)
    {
        this.mainCodeEditor.Text = this.openFile(this.fileField.Text);
        updateSyntax();
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run((Form) new mainForm());
    }

    public void build()
    {
        string lower = this.fileField.Text.Substring(this.fileField.Text.IndexOf(".")).ToLower();
        if (string.IsNullOrEmpty(this.mainCodeEditor.Text))
        this.openFile(this.fileField.Text);
        else
        this.save();
        if (!(lower == ".py"))
        return;
        ProcessStartInfo processStartInfo = new ProcessStartInfo("CMD.exe", "/K py -i " + this.fileField.Text);
        new Process() { StartInfo = processStartInfo }.Start();
        }
}

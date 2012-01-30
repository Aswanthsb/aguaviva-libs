using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace midiplayer
{
   public partial class MDIParent1 : Form
   {


      private int childFormNumber = 0;

      Speaker sp;

      public MDIParent1()
      {
         InitializeComponent();        
      }

      private void ShowNewForm(object sender, EventArgs e)
      {
         Form childForm = new Form();
         childForm.MdiParent = this;
         childForm.Text = "Window " + childFormNumber++;
         childForm.Show();
      }

      private void OpenFile(object sender, EventArgs e)
      {
         OpenFileDialog openFileDialog = new OpenFileDialog();
         openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
         openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
         if (openFileDialog.ShowDialog(this) == DialogResult.OK)
         {
            string FileName = openFileDialog.FileName;
            /*
            // Open the file, creating it if necessary.
            FileStream stream = File.Open(FileName, FileMode.Open);

            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer1 = new XmlSerializer(typeof(List<SoundGenerator>));
            m_boxes = (List<SoundGenerator>)serializer1.Deserialize(stream);
            XmlSerializer serializer2 = new XmlSerializer(typeof(List<Link>));
            m_links = (List<Link>)serializer2.Deserialize(stream);

            // Close the file.
            stream.Close();
             */
         }
      }

      private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         SaveFileDialog saveFileDialog = new SaveFileDialog();
         saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
         saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
         if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
         {
            string FileName = saveFileDialog.FileName;
            /*
            // Open the file, creating it if necessary.
            FileStream stream = File.Open(FileName, FileMode.Create);

            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer1 = new XmlSerializer(typeof(List<SoundGenerator>));
            serializer1.Serialize(stream, m_boxes);
            XmlSerializer serializer2 = new XmlSerializer(typeof(List<Link>));
            serializer2.Serialize(stream, m_links);

            // Close the file.
            stream.Close();
             */
         }
      }

      private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      private void CutToolStripMenuItem_Click(object sender, EventArgs e)
      {
      }

      private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
      {
      }

      private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
      {
      }

      private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
      {
         toolStrip.Visible = toolBarToolStripMenuItem.Checked;
      }

      private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
      {
         statusStrip.Visible = statusBarToolStripMenuItem.Checked;
      }

      private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
      {
         LayoutMdi(MdiLayout.Cascade);
      }

      private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
      {
         LayoutMdi(MdiLayout.TileVertical);
      }

      private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
      {
         LayoutMdi(MdiLayout.TileHorizontal);
      }

      private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         LayoutMdi(MdiLayout.ArrangeIcons);
      }

      private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
      {
         foreach (Form childForm in MdiChildren)
         {
            childForm.Close();
         }
      }


      private void speakerToolStripMenuItem_Click(object sender, EventArgs e)
      {

      }

      private void CreatePatternEditor()
      {
         //simpleOscillatorToolStripMenuItem_Click(null,null);
         //simpleOscillatorToolStripMenuItem_Click(null, null);

         PatternEditor childForm = new PatternEditor();
         childForm.MdiParent = this;
         childForm.Text = "Pattern Editor";
         childForm.Show();
      }
      

      private void CreateSpeaker()
      {
         /*
         sp = new Speaker("Speaker", 44100);

         Box childForm = new Box();
         //childForm.MdiParent = this;
         childForm.Text = "Speaker";

         childForm.Show();

         sp.Init(childForm);
         childForm.SetMachine(sp);

         sp.Play();
         */
         //m_boxes.Add(sp);
      }

      private void timer1_Tick(object sender, EventArgs e)
      {
         if (sp != null)
         {
            while (sp.FreeInBuffer() >= 4410 * 8)
            {
               sp.RenderSound();
            }
         }
      }

      private void MDIParent1_Load(object sender, EventArgs e)
      {
         CreateSpeaker();
         //CreatePatternEditor();

         Form2 childForm = new Form2();
         childForm.MdiParent = this;
         childForm.Text = "Pattern Editor";
         childForm.Show();

         ScopeForm sc = new ScopeForm();
         sc.MdiParent = this;
         sc.Text = "Pattern Editor";
         sc.Show();
      

      }

      private void oscilloToolStripMenuItem_Click(object sender, EventArgs e)
      {

      }

      private List<Scope> m_Scopes = new List<Scope>();

      public void AddScope(Scope s)
      {
         m_Scopes.Add(s);
      }

      public List<Scope> GetScopes()
      {
         return m_Scopes;
      }


   }
}

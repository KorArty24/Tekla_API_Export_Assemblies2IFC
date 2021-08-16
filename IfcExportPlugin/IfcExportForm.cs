using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using TeklaOpenAPIExtension;

namespace IfcExportPlugin
{
	public partial class IfcExportForm : Form
	{
		private readonly Model _model = new Model();


		public IfcExportForm()
		{
			InitializeComponent();
		}

		private void buttonExport_Click(object sender, EventArgs e)
		{
			var selection = new Picker()
				.PickObjects(Picker.PickObjectsEnum.PICK_N_OBJECTS, "Pick Objects")
				.ToAList<ModelObject>();
			//ArrayList elements = new ArrayList();
			var selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
			var selectedAssemblies = selector.GetSelectedObjects().ToList<Tekla.Structures.Model.Assembly>();

			//var modelPath = _model.GetInfo().ModelPath;
			string modelPath = textBox1.Text;
			//var modelName = _model.GetInfo().ModelName.Split('.')[0];
			foreach (var element in selectedAssemblies)
			{
				var assemblyName = element.Name;
				var assemblyPrefix = element.AssemblyNumber.Prefix.ToString();
				var name = assemblyPrefix + "." + assemblyName;
				ExportIFC($"{modelPath}\\IFC\\OUT_{name}");
			}
			
		}

		private static void ExportIFC(string outputFileName)
		{
			var componentInput = new ComponentInput();
			componentInput.AddOneInputPosition(new Point(0, 0, 0));
			var comp = new Component(componentInput)
			{
				Name = "ExportIFC",
				Number = BaseComponent.PLUGIN_OBJECT_NUMBER
			};

			// Parameters
			comp.SetAttribute("OutputFile", outputFileName);
			comp.SetAttribute("Format", 0);
			comp.SetAttribute("ExportType", 1);
			//comp.SetAttribute("AdditionalPSets", "");
			comp.SetAttribute("CreateAll", 0);  // 0 to export only selected objects

			// Advanced
			comp.SetAttribute("Assemblies", 1);
			comp.SetAttribute("Bolts", 0);
			comp.SetAttribute("Welds", 0);
			comp.SetAttribute("SurfaceTreatments", 1);
			comp.SetAttribute("BaseQuantities", 0);
			comp.SetAttribute("GridExport", 0);
			comp.SetAttribute("ReinforcingBars", 1);
			comp.SetAttribute("PourObjects", 0);
			comp.SetAttribute("LayersNameAsPart", 1);
			comp.SetAttribute("PLprofileToPlate", 0);
			comp.SetAttribute("ExcludeSnglPrtAsmb", 0);
			comp.SetAttribute("LocsFromOrganizer", 0);
			comp.Insert();
		}

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}
		private void button1_Click(object sender, EventArgs e)
        {
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.RootFolder = Environment.SpecialFolder.Desktop;
			fbd.Description = "{|Select a folder|}";
			fbd.ShowNewFolderButton = false;
			if (fbd.ShowDialog() == DialogResult.OK)
            {
				textBox1.Text = fbd.SelectedPath;
			}
        }

       
    }

    public static class ExtentionMethods
	{
		public static List<T> ToAList<T>(this IEnumerator enumerator)
		{
			var list = new List<T>();
			while (enumerator.MoveNext())
			{
				var current = (T)enumerator.Current;
				if (current != null)
					list.Add(current);
			}
			return list;
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using Robot;
using Serial;
using Router;
using OpenTK;
using Geometry;
using Router.Paths;
using Commands;
using System.Reflection;

namespace GUI
{   
    public partial class PathCAM : Form, IOpenGLDrawable
    {
        private RouterGUI router;
        private string dragDropFilename = null;
        private Regex acceptedFileRegex = new Regex(@".*(\.dae|\.obj|\.stl|\.nc|\.gcode)", RegexOptions.IgnoreCase);
        private Settings settings;

        public PathCAM()
        {
            InitializeComponent();

            router = new RouterGUI();
            drawing3D.AddObject(router);
            robotControl.AssignRouter(router);
            
            drawing3D.AddObject(robotControl);
            drawing3D.AddObject(this);
            drawing3D.DragDrop += this.Drawing3D_DragDrop;
            drawing3D.DragOver += this.Drawing3D_DragOver;
            drawing3D.DragEnter += this.Drawing3D_DragEnter;
            drawing3D.DragLeave += this.Drawing3D_DragLeave;
            
            settings = new Settings(robotControl.GetRobot(), router);
            propertyGrid.SelectedObject = settings;
        }
 
        void Drawing3D_DragLeave(object sender, EventArgs e)
        {
            //dragDropFilename = null;
            // TODO: cancel the background worker loading the mesh, completely remove the mesh from everywhere.
        }

        Vector3 dragEnterLocation = Vector3.Zero;
        void Drawing3D_DragOver(object sender, DragEventArgs e)
        {
            if (dragDropMesh != null)
            {
                var plane = new Plane(Vector3.UnitZ, new Vector3(0, 0, 0));
                var ray = drawing3D.GetPointerRay(drawing3D.PointToClient(MousePosition));
                dragDropMesh.Offset = ray.Start + ray.Direction * plane.Distance(ray);
            }
        }

        void Drawing3D_DragDrop(object sender, DragEventArgs e)
        {
            dragDropMesh = null;
        }

        TriangleMeshGUI dragDropMesh = null;
        void Drawing3D_DragEnter(object sender, DragEventArgs e)
        {
            this.Activate();
            if (dragDropMesh != null)
            {
                e.Effect = DragDropEffects.Copy;
                return;
            }

            e.Effect = DragDropEffects.None;
            if (openFileButton.Enabled && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                if (a != null && a.Length == 1)
                {
                    string filename = a.GetValue(0).ToString();
                    if (acceptedFileRegex.IsMatch(filename))
                    {
                        dragDropFilename = filename;
                        dragDropMesh = AddFile(dragDropFilename, loadObjectScale);
                        var plane = new Plane(Vector3.UnitZ, new Vector3(0, 0, 0));
                        var ray = drawing3D.GetPointerRay(new Point(e.X, e.Y));
                        dragEnterLocation = ray.Start + ray.Direction * plane.Distance(ray);
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        private class LoadMeshData
        {
            public float scale;
            public string filename;
            public TriangleMeshGUI mesh;
        }

        private List<TriangleMeshGUI> inProgressMeshes = new List<TriangleMeshGUI>();
        internal TriangleMeshGUI AddFile(string filename, float scale)
        {
            if (filename.EndsWith(".nc", StringComparison.OrdinalIgnoreCase) || filename.EndsWith(".gcode", StringComparison.OrdinalIgnoreCase))
            {
                var commands = GCodeLoader.Load(filename);
                foreach (ICommand command in commands)
                {
                    router.AddCommand(command);
                }
                return null;
            }
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_LoadMesh;
            worker.RunWorkerCompleted += worker_LoadMeshCompleted;
            var mesh = new TriangleMeshGUI();
            inProgressMeshes.Add(mesh);
            worker.RunWorkerAsync(new LoadMeshData() { filename = filename, scale = loadObjectScale, mesh = mesh });
            return mesh;
        }

        void worker_LoadMesh(object sender, DoWorkEventArgs e)
        {
            var data = e.Argument as LoadMeshData;
            string filename = data.filename;
            float scale = data.scale;
            var triangleMesh = data.mesh;
            if (filename.EndsWith(".dae", StringComparison.OrdinalIgnoreCase))
            {
                DAE_Loader.Load(filename, triangleMesh, scale);
            }
            else if (filename.EndsWith(".obj", StringComparison.OrdinalIgnoreCase))
            {
                OBJ_Loader.Load(filename, triangleMesh, scale);
            }
            else if (filename.EndsWith(".stl", StringComparison.OrdinalIgnoreCase))
            {
                STL_Loader.Load(filename, triangleMesh, scale);
            }
            if (triangleMesh != null && triangleMesh.Triangles.Count() > 0)
            {
                triangleMesh.GenerateTabPaths(router.ToolDiameterScale / 2.0f);
                triangleMesh.RefreshDisplayLists(); // The triangles will be static after this point - make sure they're correctly displayed.
            }
            e.Result = triangleMesh;
        }

        // For debugging, generate paths for the first loaded
        // trianglemesh every frame (allows drawing code inside the generation)
        //TriangleMeshGUI m = null;
        void IOpenGLDrawable.Draw()
        {
            try
            {
                foreach (var mesh in inProgressMeshes)
                {
                    mesh.Draw();
                    //if (m == null)
                    //{
                    //    m = mesh;
                    //}
                }
                //if (m != null)
                //{
                //    PathPlanner.PlanPaths(m, m.Tabs.ConvertAll(tab => tab as Tabs), router);
                //}
            }
            catch (Exception)
            {
            }
        }

        void worker_LoadMeshCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var mesh = e.Result as TriangleMeshGUI;
            if (mesh != null && mesh.Triangles.Count() > 0)
            {
                drawing3D.AddObject(mesh);
                foreach (var tab in mesh.Tabs)
                {
                    drawing3D.AddObject(tab);
                }
                inProgressMeshes.RemoveAll(m => m == mesh);
            }
        }

        private void PermiterRoutsClick(object sender, EventArgs e)
        {
            foreach (Object o in drawing3D.GetObjects())
            {
                if (o is TriangleMeshGUI)
                {
                    var triangles = o as TriangleMeshGUI;
                    var routs = PathPlanner.PlanPaths(triangles, triangles.Tabs.ConvertAll<Tabs>(tab => tab as Tabs), router);
                    foreach (var rout in routs)
                    {
                        router.RoutPath(rout, false, triangles.Offset);
                    }
                }
            }
            router.Complete();
        }

        private void boundaryCheckButton_Click(object sender, EventArgs e)
        {
            float xMin = -router.ToolDiameterScale;
            float xMax = router.ToolDiameterScale;
            float yMin = -router.ToolDiameterScale;
            float yMax = router.ToolDiameterScale;

            foreach (Object o in drawing3D.GetObjects())
            {
                if (o is TriangleMeshGUI)
                {
                    var triangles = o as TriangleMeshGUI;
                    xMin = Math.Min(triangles.MinPoint.X - router.ToolDiameterScale + triangles.Offset.X, xMin);
                    xMax = Math.Max(triangles.MaxPoint.X + router.ToolDiameterScale + triangles.Offset.X, xMax);
                    yMin = Math.Min(triangles.MinPoint.Y - router.ToolDiameterScale + triangles.Offset.Y, yMin);
                    yMax = Math.Max(triangles.MaxPoint.Y + router.ToolDiameterScale + triangles.Offset.Y, yMax);
                }
            }

            LineStrip r = new LineStrip();
            r.Append(new Vector3(xMin, yMin, router.MoveHeight));
            r.Append(new Vector3(xMax, yMin, router.MoveHeight));
            r.Append(new Vector3(xMax, yMax, router.MoveHeight));
            r.Append(new Vector3(xMin, yMax, router.MoveHeight));
            r.Append(new Vector3(xMin, yMin, router.MoveHeight));
            router.RoutPath(r, false, Vector3.Zero);
            router.Complete();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "3D Files |*.dae;*.obj;*.stl;*.nc;*.gcode";
            if (DialogResult.OK == d.ShowDialog())
            {
                AddFile(d.FileName, loadObjectScale);
                foreach (Object o in drawing3D.GetObjects())
                {
                    TriangleMesh mesh = o as TriangleMesh;
                    if (mesh != null)
                    {
                        router.MoveHeight = mesh.MaxPoint.Z + 0.025f;
                        this.propertyGrid.Refresh();
                    }
                }
            }
        }

        private void clearPathsButton_Click(object sender, EventArgs e)
        {
            router.ClearCommands();
        }

        private float loadObjectScale = 2.54f;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            float targetScale = 0.0f;
            float sourceScale = 0.0f;

            var match = new Regex(@"^(?<source>\S+):(?<target>\S+)").Match(comboBox1.Text);

            if (match.Success
                && float.TryParse(match.Groups["source"].Value, out sourceScale)
                && float.TryParse(match.Groups["target"].Value, out targetScale)
                && targetScale != 0 && sourceScale != 0)
            {
                comboBox1.BackColor = SystemColors.Window;
                openFileButton.Enabled = true;
                loadObjectScale = 25.4f;// targetScale / sourceScale;
            }
            else
            {
                comboBox1.BackColor = Color.LightPink;
                openFileButton.Enabled = false;
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathCAM));
            this.button2 = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.saveGcodeButton = new System.Windows.Forms.Button();
            this.clearPathsButton = new System.Windows.Forms.Button();
            this.boundaryCheck = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.openFileButton = new System.Windows.Forms.Button();
            this.showRobotFormCheckbox = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.robotControl = new GUI.RobotControl();
            this.drawing3D = new GUI.Drawing3D();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(12, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(225, 25);
            this.button2.TabIndex = 1;
            this.button2.Text = "生成切割路径";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.PermiterRoutsClick);
            // 
            // propertyGrid
            // 
            this.propertyGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.propertyGrid.Location = new System.Drawing.Point(12, 175);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid.Size = new System.Drawing.Size(225, 189);
            this.propertyGrid.TabIndex = 5;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // saveGcodeButton
            // 
            this.saveGcodeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveGcodeButton.Location = new System.Drawing.Point(12, 109);
            this.saveGcodeButton.Name = "saveGcodeButton";
            this.saveGcodeButton.Size = new System.Drawing.Size(225, 25);
            this.saveGcodeButton.TabIndex = 4;
            this.saveGcodeButton.Text = "保存 GCode";
            this.saveGcodeButton.UseVisualStyleBackColor = true;
            this.saveGcodeButton.Click += new System.EventHandler(this.saveGcodeButton_Click);
            // 
            // clearPathsButton
            // 
            this.clearPathsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearPathsButton.Location = new System.Drawing.Point(12, 76);
            this.clearPathsButton.Name = "clearPathsButton";
            this.clearPathsButton.Size = new System.Drawing.Size(225, 27);
            this.clearPathsButton.TabIndex = 3;
            this.clearPathsButton.Text = "清除路径";
            this.clearPathsButton.UseVisualStyleBackColor = true;
            this.clearPathsButton.Click += new System.EventHandler(this.clearPathsButton_Click);
            // 
            // boundaryCheck
            // 
            this.boundaryCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boundaryCheck.Location = new System.Drawing.Point(12, 42);
            this.boundaryCheck.Name = "boundaryCheck";
            this.boundaryCheck.Size = new System.Drawing.Size(225, 28);
            this.boundaryCheck.TabIndex = 2;
            this.boundaryCheck.Text = "生成边界路径";
            this.boundaryCheck.UseVisualStyleBackColor = true;
            this.boundaryCheck.Click += new System.EventHandler(this.boundaryCheckButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.Window;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1:1 (inches)",
            "25.4:1 (millimeters)",
            ".254:1 (meters)",
            "1:12 (feet)"});
            this.comboBox1.Location = new System.Drawing.Point(103, 146);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(134, 20);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "1:1 (inches)";
            this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // openFileButton
            // 
            this.openFileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openFileButton.Location = new System.Drawing.Point(12, 140);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(85, 29);
            this.openFileButton.TabIndex = 6;
            this.openFileButton.Text = "打开3D文件";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // showRobotFormCheckbox
            // 
            this.showRobotFormCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showRobotFormCheckbox.AutoSize = true;
            this.showRobotFormCheckbox.Checked = true;
            this.showRobotFormCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showRobotFormCheckbox.Location = new System.Drawing.Point(-1, 525);
            this.showRobotFormCheckbox.Name = "showRobotFormCheckbox";
            this.showRobotFormCheckbox.Size = new System.Drawing.Size(15, 14);
            this.showRobotFormCheckbox.TabIndex = 69;
            this.showRobotFormCheckbox.UseVisualStyleBackColor = true;
            this.showRobotFormCheckbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(103, 145);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(134, 21);
            this.pictureBox1.TabIndex = 70;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // robotControl
            // 
            this.robotControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.robotControl.BackColor = System.Drawing.Color.Transparent;
            this.robotControl.Location = new System.Drawing.Point(12, 370);
            this.robotControl.Name = "robotControl";
            this.robotControl.Size = new System.Drawing.Size(603, 135);
            this.robotControl.TabIndex = 8;
            this.robotControl.Load += new System.EventHandler(this.robotControl_Load);
            // 
            // drawing3D
            // 
            this.drawing3D.AllowDrop = true;
            this.drawing3D.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drawing3D.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.drawing3D.BackColor = System.Drawing.Color.Black;
            this.drawing3D.ClearColor = System.Drawing.Color.Empty;
            this.drawing3D.Location = new System.Drawing.Point(472, 11);
            this.drawing3D.MinimumSize = new System.Drawing.Size(10, 9);
            this.drawing3D.Name = "drawing3D";
            this.drawing3D.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.drawing3D.Size = new System.Drawing.Size(300, 183);
            this.drawing3D.TabIndex = 68;
            this.drawing3D.VSync = false;
            // 
            // PathCAM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 538);
            this.Controls.Add(this.showRobotFormCheckbox);
            this.Controls.Add(this.saveGcodeButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.clearPathsButton);
            this.Controls.Add(this.boundaryCheck);
            this.Controls.Add(this.robotControl);
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.drawing3D);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 465);
            this.Name = "PathCAM";
            this.Text = "PathCAM - Toolpath generation software for CNC robots";
            this.Load += new System.EventHandler(this.PathCAM_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void saveGcodeButton_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "GCode Files |*.nc;*.gcode";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = dialog.FileName;
                if (!filename.EndsWith(".nc", StringComparison.OrdinalIgnoreCase) && !filename.EndsWith(".gcode", StringComparison.OrdinalIgnoreCase))
                {
                    filename = filename + ".nc";
                }
                GCodeLoader.ExportGCode(router.GetCommands(), filename);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            robotControl.Visible = showRobotFormCheckbox.Checked;
        }

        private void PathCAM_Load(object sender, EventArgs e)
        {
            // Programatically fill the entire client rectangle with the drawing area.
            // This makes sure the size is independent of window border and makes
            // editing the GUI in the designer much easier.
            this.drawing3D.Location = new Point(0, 0);
            this.drawing3D.Size = this.ClientRectangle.Size;
            robotControl.Location = new Point(0, ClientRectangle.Height - robotControl.Height);
            showRobotFormCheckbox.Location = new Point(2, ClientRectangle.Height - showRobotFormCheckbox.Height - 2);
        }

        private void robotControl_Load(object sender, EventArgs e)
        {

        }
    }
}

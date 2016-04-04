using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Robot;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Serial;
using Router;
using Commands;

namespace GUI
{
    public partial class RobotControl : UserControl, IOpenGLDrawable
    {
        private Router.Router router;
        private Robot.Robot robot;
        private SerialPortWrapper serial;
        private COMPortForm comPortForm = null;
        
        public RobotControl()
        {
            InitializeComponent();
            serial = new SerialPortWrapper();
        }

        public void AssignRouter(Router.Router router)
        {
            this.router = router;
            this.robot = new Robot.Robot(serial);
            this.robot.onRobotStatusChange += new EventHandler(RobotStatusUpdate);
        }

        public Robot.Robot GetRobot()
        {
            return robot;
        }

        void RobotStatusUpdate(object o, EventArgs e)
        {
            if (this.InvokeRequired && !this.Disposing)
            {
                try
                {
                    this.Invoke(new EventHandler(RobotStatusUpdate), new object[] { o, e });
                }
                catch (ObjectDisposedException)
                {
                }
            }
            else
            {
                IRobotCommandWithStatus status = o as IRobotCommandWithStatus;
                if (status != null)
                {
                    this.runButton.Enabled = true;
                    this.steppersEnabledBox.Enabled = true;
                    this.checkBoxMotor.Enabled = true;
                    this.buttonLeftX.Enabled = true;
                    this.buttonLeftY.Enabled = true;
                    this.buttonLeftZ.Enabled = true;
                    this.buttonLeftX10.Enabled = true;
                    this.buttonLeftY10.Enabled = true;
                    this.buttonLeftZ10.Enabled = true;
                    this.buttonRightX.Enabled = true;
                    this.buttonRightY.Enabled = true;
                    this.buttonRightZ.Enabled = true;
                    this.buttonRightX10.Enabled = true;
                    this.buttonRightY10.Enabled = true;
                    this.buttonRightZ10.Enabled = true;
                    this.buttonBackX.Enabled = true;
                    this.buttonBackY.Enabled = true;
                    this.buttonBackZ.Enabled = true;
                    this.buttonHomePosition.Enabled = true;
                    this.steppersEnabledBox.Checked = status.SteppersEnabled;
                    if (status.Pausing)
                    {
                        pause_resume_button.Text = "暂停中...";
                        pause_resume_button.Enabled = false;
                    }
                    else
                    {
                        if (status.Paused)
                        {
                            if (pause_resume_button.Text != "继续")
                            {
                                cancelButton.Enabled = true;
                                pause_resume_button.Text = "继续";
                            }
                        }
                        else
                        {
                            if (pause_resume_button.Text != "暂停")
                            {
                                pause_resume_button.Text = "暂停";
                            }
                        }
                        pause_resume_button.Enabled = true;
                    }
                }
            }
        }

        void IOpenGLDrawable.Draw()
        {
            // Draw the tool location as a cone
            Vector3 position = robot.GetPosition();
            GL.Color3(Color.Silver);
            Polyhedra.DrawCone(position + new Vector3(0, 0, router.ToolDiameter), position, router.ToolDiameter / 2.0f);

            Vector3 physicalPosition = robot.GetPhysicalPosition();
            GL.Color3(Color.Black);
            Polyhedra.DrawCone(physicalPosition + new Vector3(0, 0, router.ToolDiameter), physicalPosition, router.ToolDiameter / 2.0f);


            //// Draw the past positions & velocity graph
            //float lastTime = 0;
            //Vector3 lastPos = new Vector3(0, 0, 0);
            //float lastVel = 0;
            //bool lastIsGood = false;
            //GL.Disable(EnableCap.Lighting);
            //lock (previousPoints)
            //{
            //    Vector3 lastpoint = new Vector3(0, 0, 0);
            //    for (int i = 0; i < previousPoints.Count(); i++)
            //    {
            //        PreviousPoint point = previousPoints[i];
            //        float age_delta = point.createTime - lastTime;
            //        float time = age_delta / 1000.0f; // Age is microseconds, time is seconds
            //        float pos_delta = (point.location - lastPos).Length;
            //        float vel = pos_delta / time; // Inches per second
            //        Vector3 atpoint = new Vector3(point.location.X * 1000, point.location.Y * 1000, point.location.Z * 1000);
            //        if (lastIsGood)
            //        {
            //            GL.LineWidth(1);
            //            GL.Begin(PrimitiveType.Lines);
            //            GL.Color3(Color.LightGray);
            //            for (int j = 0; j < 5; j++)
            //            {
            //                GL.Vertex3(lastpoint + new Vector3(0, 0, j * 10));
            //                GL.Vertex3(lastpoint + new Vector3(0, 0, j * 10 + 10));
            //
            //                GL.Vertex3(lastpoint + new Vector3(0, 0, j * 10));
            //                GL.Vertex3(atpoint + new Vector3(0, 0, j * 10));
            //            }
            //            GL.End();
            //            GL.LineWidth(2);
            //            GL.Begin(PrimitiveType.Lines);
            //            GL.Color3(Color.Orange);
            //            GL.Vertex3(lastpoint + new Vector3(0, 0, lastVel * lastVel * 200));
            //            GL.Vertex3(atpoint + new Vector3(0, 0, vel * vel * 200));
            //            GL.End();
            //        }
            //        lastVel = vel;
            //        lastpoint = atpoint;
            //
            //        lastPos = point.location;
            //        lastTime = point.createTime;
            //        lastIsGood = true;
            //    }
            //}
            //GL.Enable(EnableCap.Lighting);
            //GL.LineWidth(1);

        }

        private void pause_resume_button_Click(object sender, EventArgs e)
        {
            if (pause_resume_button.Text == "暂停")
            {
                robot.SendPauseCommand();
            }
            else if (pause_resume_button.Text == "继续")
            {
                robot.SendResumeCommand();
                cancelButton.Enabled = false;
            }
            pause_resume_button.Enabled = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            robot.CancelPendingCommands();
            Vector3 position = robot.GetPosition();
            robot.AddCommand(new MoveTool(new Vector3(position.X, position.Y, router.MoveHeight), MoveTool.SpeedType.Rapid));
            robot.AddCommand(new MoveTool(new Vector3(0, 0, router.MoveHeight), MoveTool.SpeedType.Rapid));
            robot.SendResumeCommand();
            this.pause_resume_button.Enabled = false;
            this.cancelButton.Enabled = false;
        }

        private void steppersEnabledBox_Click(object sender, EventArgs e)
        {
            if (steppersEnabledBox.Checked)
            {
                robot.DisableMotors();
            }
            else
            {
                robot.EnableMotors();
            }
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            foreach (ICommand command in router.GetCommands())
            {
                robot.AddCommand(command);
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (comPortForm == null || comPortForm.IsDisposed)
            {
                comPortForm = new COMPortForm(serial);
            }

            if (!comPortForm.Visible)
            {
                comPortForm.Show(null);
            }
            else
            {
                comPortForm.Focus();
            }
        }

        private void buttonHomePosition_Click(object sender, EventArgs e)
        {
            robot.Zero();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            robot.z_offset = (float)numericUpDown1.Value;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void steppersEnabledBox_CheckedChanged(object sender, EventArgs e)
        {

        }
        

        private void buttonBackX_Click(object sender, EventArgs e)
        {
            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(0, vec.Y, vec.Z), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonBackY_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X, 0, vec.Z), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonBackZ_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X, vec.Y, 0), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonLeftX_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X - 1, vec.Y, vec.Z), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonLeftX10_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X - 10, vec.Y, vec.Z), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonLeftY_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X, vec.Y-1, vec.Z), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonLeftY10_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X, vec.Y - 10, vec.Z), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonLeftZ_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X, vec.Y, vec.Z-1), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonLeftZ10_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X, vec.Y, vec.Z-10), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonRightZ_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X, vec.Y, vec.Z+1), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonRightZ10_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X, vec.Y, vec.Z+10), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonRightY10_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X, vec.Y+10, vec.Z), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonRightY_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X, vec.Y+1, vec.Z), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonRightX_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X+1, vec.Y, vec.Z), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void buttonRightX10_Click(object sender, EventArgs e)
        {

            Vector3 vec = robot.GetPosition();
            MoveTool move = new MoveTool(new Vector3(vec.X+10, vec.Y, vec.Z), MoveTool.SpeedType.Rapid);
            robot.AddCommand(move);
        }

        private void checkBoxMotor_Click(object sender, EventArgs e)
        {
            if (checkBoxMotor.Checked)
            {
                robot.DisableDCMotor();
            }
            else
            {
                robot.EnableDCMotor();
            }
            checkBoxMotor.Checked = !checkBoxMotor.Checked;
        }

        private void RobotControl_Load(object sender, EventArgs e)
        {

        }


        //private List<PreviousPoint> previousPoints = new List<PreviousPoint>();
        //public class PreviousPoint
        //{
        //    public PreviousPoint(float time, Vector3 location)
        //    {
        //        this.createTime = time;
        //        this.location = location;
        //    }
        //    public float createTime;
        //    public Vector3 location;
        //}


        //void RouterPositionUpdate(object o, EventArgs e)
        //{
        //    StatusCommand status = o as StatusCommand;
        //    if (status != null)
        //    {
        //        Vector3 position = status.CurrentPosition;
        //        float time = status.time;
        //        float distance = (lastPosition - position).Length;
        //
        //        if ((lastPosition - position).Length > 0.0001f)
        //        {
        //            lock (previousPoints)
        //            {
        //                //Console.WriteLine("{0},{1}", time, distance);
        //                while (previousPoints.Count > 1000)
        //                {
        //                    previousPoints.RemoveAt(0);
        //                }
        //                previousPoints.Add(new PreviousPoint(time, position));
        //                lastPosition = position;
        //            }
        //        }
        //    }
        //}

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Robot;
using Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Serial;

namespace GUI
{
    public class RobotGUI : Robot.Robot, IOpenGLDrawable
    {
        private Router.Router router;
        
        public RobotGUI(SerialPortWrapper serial, Router.Router router) : base (serial)
        {
            this.router = router;
            base.onRobotStatusChange += new EventHandler (RouterPositionUpdate);
        }

        void IOpenGLDrawable.Draw()
        {
            // Draw the tool location as a cone
            Vector3 position = GetPosition();
            GL.Color3(Color.Silver);
            Polyhedra.DrawCone(position + new Vector3(0, 0, router.ToolDiameterScale), position, router.ToolDiameterScale / 2.0f);




            // Draw the past positions & velocity graph
            float lastTime = 0;
            Vector3 lastPos = new Vector3(0, 0, 0);
            float lastVel = 0;
            bool lastIsGood = false;
            GL.Disable(EnableCap.Lighting);
            lock (previousPoints)
            {
                Vector3 lastpoint = new Vector3(0, 0, 0);
                for (int i = 0; i < previousPoints.Count(); i++)
                {
                    PreviousPoint point = previousPoints[i];
                    float age_delta = point.createTime - lastTime;
                    float time = age_delta / 1000.0f; // Age is microseconds, time is seconds
                    float pos_delta = (point.location - lastPos).Length;
                    float vel = pos_delta / time; // Inches per second
                    Vector3 atpoint = new Vector3(point.location.X * 1000, point.location.Y * 1000, point.location.Z * 1000);
                    if (lastIsGood)
                    {
                        GL.LineWidth(1);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Color3(Color.LightGray);
                        for (int j = 0; j < 5; j++)
                        {
                            GL.Vertex3(lastpoint + new Vector3(0, 0, j * 10));
                            GL.Vertex3(lastpoint + new Vector3(0, 0, j * 10 + 10));
            
                            GL.Vertex3(lastpoint + new Vector3(0, 0, j * 10));
                            GL.Vertex3(atpoint + new Vector3(0, 0, j * 10));
                        }
                        GL.End();
                        GL.LineWidth(2);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Color3(Color.Orange);
                        GL.Vertex3(lastpoint + new Vector3(0, 0, lastVel * lastVel * 200));
                        GL.Vertex3(atpoint + new Vector3(0, 0, vel * vel * 200));
                        GL.End();
                    }
                    lastVel = vel;
                    lastpoint = atpoint;
            
                    lastPos = point.location;
                    lastTime = point.createTime;
                    lastIsGood = true;
                }
            }
            GL.Enable(EnableCap.Lighting);
            GL.LineWidth(1);

        }


        private List<PreviousPoint> previousPoints = new List<PreviousPoint>();
        public class PreviousPoint
        {
            public PreviousPoint(float time, Vector3 location)
            {
                this.createTime = time;
                this.location = location;
            }
            public float createTime;
            public Vector3 location;
        }

        Vector3 lastPosition;
        void RouterPositionUpdate(object o, EventArgs e)
        {
            IRobotCommandWithStatus status = o as IRobotCommandWithStatus;
            if (status != null)
            {
                Vector3 position = status.CurrentPosition;
                float time = status.Time;
                float distance = (lastPosition - position).Length;
        
                if ((lastPosition - position).Length > 0.0001f)
                {
                    lock (previousPoints)
                    {
                        //Console.WriteLine("{0},{1}", time, distance);
                        while (previousPoints.Count > 1000)
                        {
                            previousPoints.RemoveAt(0);
                        }
                        previousPoints.Add(new PreviousPoint(time, position));
                        lastPosition = position;
                    }
                }
            }
        }

    }
}

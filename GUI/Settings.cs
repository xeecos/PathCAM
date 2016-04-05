using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;

namespace GUI
{
    /// <summary>
    /// Collects all settings and exposes them through one class.
    /// </summary>
    public class Settings
    {
        private Robot.Robot robot;
        private Router.Router router;
        public Settings(Robot.Robot robot, Router.Router router)
        {
            this.router = router;
            this.robot = robot;
        }

        [DisplayName("连接层高度")]
        [Description("工件与材料连接处厚度")]
        public float TabHeight
        {
            get { return router.TabHeight; }
            set { router.TabHeight = value; }
        }

        ///
        /// Accessors for the property grid
        ///
        [DisplayName("最后一刀的高度")]
        [Description("Height of the last pass in inches")]
        public float LastPassHeight
        {
            get { return router.LastPassHeight; }
            set { router.LastPassHeight = value; }
        }

        [DisplayName("刀具直径")]
        [Description("Tool Diameter in inches")]
        public float ToolDiameter
        {
            get { return router.ToolDiameter; }
            set { router.ToolDiameter = value; }
        }

        [DisplayName("安全高度")]
        [Description("Safe travel height")]
        public float MoveHeight
        {
            get { return router.MoveHeight; }
            set { router.MoveHeight = value; }
        }

        [DisplayName("层厚")]
        [Description("Maximum Cut Depth")]
        public float MaxCutDepth
        {
            get { return router.MaxCutDepth; }
            set { router.MaxCutDepth = value; }
        }



        [DisplayName("切割速度")]
        [Description("Cutting Speed (inches per minute)")]
        public float RoutSpeed
        {
            get { return robot.MaxCutSpeed; }
            set { robot.MaxCutSpeed = value; }
        }

        [DisplayName("移动速度")]
        [Description("Rapid movement speed (inches per minute)")]
        public float MoveSpeed
        {
            get { return robot.MaxRapidSpeed; }
            set { robot.MaxRapidSpeed = value; }
        }

        [DisplayName("最大Z轴速度")]
        [Description("Maximum possible Z axis speed (inches per minute)")]
        public float MaxAxisSpeeds
        {
            get { return robot.MaxZSpeed; }
            set { robot.MaxZSpeed = value; }
        }
    }
}

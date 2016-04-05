﻿/*
 * PathCAM - Toolpath generation software for CNC manufacturing machines
 * Copyright (C) 2013  Benjamin R. Porter https://github.com/xenovacivus
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see [http://www.gnu.org/licenses/].
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using OpenTK;
using Commands;
using System.IO;

namespace Router
{
    public class GCodeLoader
    {
        public static List<ICommand> Load(string filename)
        {
            string[] lines = System.IO.File.ReadAllLines(filename);
            var commands = new List<ICommand>();
            float scale = 1.0f, speed = 0, x = 0, y = 0, z = 0f;
            foreach (string s in lines)
            {
                Regex r = new Regex("^G(?<G_VALUE>\\d+)");
                if (r.IsMatch (s))
                {
                    Match m = r.Match(s);
                    Int32 g_value = Int32.Parse (m.Groups["G_VALUE"].Value);

                    if (g_value == 0 || g_value == 1)
                    {
                        // Rapid positioning or linear interpolation
                        // Go to X, Y, Z at feedrate F.
                        Vector3 fromPoint = new Vector3(x, y, z);
                        GetFloat(s, "F", ref speed);
                        GetFloat(s, "X", ref x);
                        GetFloat(s, "Y", ref y);
                        GetFloat(s, "Z", ref z);
                        Vector3 toPoint = new Vector3(x, y, z);

                        // If the XYZ params weren't specified or didn't change, don't do a move.
                        // If only speed changed, that's ok; it has been updated for the next move.
                        if ((toPoint - fromPoint).Length > 0)
                        {
                            var speedType = g_value == 0 ? MoveTool.SpeedType.Rapid : MoveTool.SpeedType.Cutting;
                            commands.Add(new MoveTool(toPoint * scale, speedType));
                        }
                        
                    }
                    else if (g_value == 4)
                    {
                        // Dwell Time (X, U, or P): dwell time in milliseconds
                    }
                    else if (g_value == 20)
                    {
                        // Inch Mode
                        scale = 1.0f;
                    }
                    else if (g_value == 21)
                    {
                        // Metric Mode
                        scale = 1.0f / (25.4f);
                    }
                    else if (g_value == 90)
                    {
                        // Absolute Programming
                        //Console.WriteLine("Absolute Programming");
                    }
                    else
                    {
                        Console.WriteLine("G code is not understood: " + s);
                    }
                }
            }
            return commands;
        }

        public static void ExportGCode(List<ICommand> commands, string filename,float zOffset)
        {
            using (var file = File.CreateText(filename))
            {
                file.WriteLine("G20 (Units are Inches)");
                file.WriteLine("G90 (Absolute Positioning)");
                file.WriteLine("G94 (Units per Minute feed rate)");
                file.WriteLine("M4 (Enable Motor)");
                // TODO: paramaterize these
                float rapidSpeed = 50.0f;
                float cuttingSpeed = 25.0f;
                float plungeSpeed = 10.0f;

                float lastSpeed = -1;
                float lastHeight = 0;
                float scale = 1.0f;
                foreach (ICommand command in commands)
                {
                    if (command is MoveTool)
                    {
                        MoveTool m = command as MoveTool;
                        float speed = m.Speed == MoveTool.SpeedType.Cutting ? cuttingSpeed * scale : rapidSpeed * scale;
                        var gCommand = m.Speed == MoveTool.SpeedType.Cutting ? "G1" : "G0";
                        
                        Vector3 target = m.Target * scale;
                        float height = target.Z + zOffset;
                        if ((height + 0.001f) < lastHeight + zOffset)
                        {
                            speed = Math.Min(plungeSpeed, speed);
                        }
                        lastHeight = height + zOffset;
                        if (lastSpeed != speed)
                        {
                            lastSpeed = speed;
                            file.WriteLine("{0} F{1:F4}", gCommand, speed);
                        }
                        
                        file.WriteLine("{0} X{1:F4} Y{2:F4} Z{3:F4}", gCommand, target.X, target.Y, target.Z + zOffset);
                        
                    }
                }
                file.WriteLine("M5 (Disable Motor)");
            }
        }

        /// <summary>
        /// Get a float in the format of "G01 Z-0.0100 F2.00", where string is Z, F, or other preceding character
        /// </summary>
        /// <param name="s"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        private static bool GetFloat(string input, string find, ref float f)
        {
            Regex r = new Regex(find + @"(?<VALUE>-?[\d\.]+)");
            if (r.IsMatch(input))
            {
                Match m = r.Match(input);
                string value_string = m.Groups["VALUE"].Value;
                
                f = float.Parse(value_string);
                //Console.WriteLine("Value for " + find + " is " + f);
                return true;
            }
            return false;
        }


    }
}
